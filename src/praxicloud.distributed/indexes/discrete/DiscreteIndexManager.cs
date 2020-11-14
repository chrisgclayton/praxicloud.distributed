// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes.discrete
{
    #region Using Clauses
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Nito.AsyncEx;
    using praxicloud.core.security;
    #endregion

    /// <summary>
    /// A round robin index distribution tool 
    /// </summary>
    /// <typeparam name="T">A numeric value</typeparam>
    public abstract class DiscreteIndexManager<T> : IDiscreteIndexManager<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        #region Variables
        /// <summary>
        /// An asynchronous control for access to reload the collection
        /// </summary>
        private readonly AsyncLock _control = new AsyncLock();
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public DiscreteIndexManager(int managerQuantity, int id, T minimum, T maximum)
        {
            Guard.NotLessThan(nameof(managerQuantity), managerQuantity, 1);
            Guard.NotLessThan(nameof(id), id, 0);
            Guard.NotMoreThan(nameof(id), id, managerQuantity - 1);
            
            Minimum = minimum;
            Maximum = maximum;
            Id = id;
            ManagerQuantity = managerQuantity;
            IsManagerQuantityUpdatable = true;
        }
        #endregion
        #region Properties
        /// <inheritdoc />
        public T[] OwnedIndexes { get; protected set; }

        /// <inheritdoc />
        public int Id { get; protected set; }

        /// <inheritdoc />
        public T Minimum { get; protected set; }

        /// <inheritdoc />
        public T Maximum { get; protected set; }

        /// <inheritdoc />
        public virtual bool AreIndexesUpdatable => true;

        /// <inheritdoc />
        public virtual bool IsIdUpdatable => true;

        /// <inheritdoc />
        public int ManagerQuantity { get; protected set; }

        /// <inheritdoc />
        public virtual bool IsManagerQuantityUpdatable { get; protected set; }

        /// <inheritdoc />
        public IIndexManager<T>.UpdateNotificationHandler NotificationHandler { get; set; }
        #endregion
        #region Methods
        /// <inheritdoc />
        public virtual async Task InitializeAsync(CancellationToken cancellationToken)
        {
            using (await _control.LockAsync())
            {
                OwnedIndexes = await CreateOwnedIndexCollectionAsync(ManagerQuantity, Id, Minimum, Maximum, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public virtual bool IsOwner(T index)
        {
            return OwnedIndexes.Contains(index);
        }

        /// <inheritdoc />
        public virtual async Task<bool> UpdateIdAsync(int id, CancellationToken cancellationToken)
        {
            var notify = false;
            var success = false;

            if (IsIdUpdatable)
            {
                using (await _control.LockAsync())
                {
                    if(IsIdUpdatable)
                    {
                        var ownedIndexes = await CreateOwnedIndexCollectionAsync(ManagerQuantity, id, Minimum, Maximum, cancellationToken).ConfigureAwait(false);

                        Id = id;
                        OwnedIndexes = ownedIndexes;

                        notify = NotificationHandler != null;
                    }
                }
            }

            if (notify) await NotificationHandler(this, cancellationToken).ConfigureAwait(false);

            return success;
        }

        /// <inheritdoc />
        public virtual async Task<bool> UpdateManagerQuantityAsync(int quantity, CancellationToken cancellationToken)
        {
            var notify = false;
            var success = false;

            if (IsManagerQuantityUpdatable)
            {
                using (await _control.LockAsync())
                {
                    if (IsManagerQuantityUpdatable)
                    {
                        var ownedIndexes = await CreateOwnedIndexCollectionAsync(quantity, Id, Minimum, Maximum, cancellationToken).ConfigureAwait(false);

                        ManagerQuantity = quantity;
                        OwnedIndexes = ownedIndexes;

                        notify = NotificationHandler != null;
                    }
                }
            }

            if (notify) await NotificationHandler(this, cancellationToken).ConfigureAwait(false);

            return success;
        }

        /// <inheritdoc />
        public virtual async Task<bool> UpdateIndexesAsync(T minimum, T maximum, CancellationToken cancellationToken)
        {
            var notify = false;
            var success = false;

            if (AreIndexesUpdatable)
            {
                using (await _control.LockAsync())
                {
                    if (AreIndexesUpdatable)
                    {
                        var ownedIndexes = await CreateOwnedIndexCollectionAsync(ManagerQuantity, Id, minimum, maximum, cancellationToken).ConfigureAwait(false);

                        Minimum = minimum;
                        Maximum = maximum;
                        OwnedIndexes = ownedIndexes;

                        notify = NotificationHandler != null;
                    }
                }
            }

            if (notify) await NotificationHandler(this, cancellationToken).ConfigureAwait(false);

            return success;
        }

        /// <summary>
        /// Creates a collection of owned indexes from the range provided
        /// </summary>
        /// <param name="managerQuantity">The number of managers handling the index</param>
        /// <param name="id">The 0 based id of the manager</param>
        /// <param name="minimum">The minimum value in the range</param>
        /// <param name="maximum">The maximum value in the range</param>
        /// <param name="cancellationToken">A token to monitor for abort requests</param>
        /// <returns>The array of owned indexes</returns>
        protected virtual Task<T[]> CreateOwnedIndexCollectionAsync(int managerQuantity, int id, dynamic minimum, dynamic maximum, CancellationToken cancellationToken)
        {
            var values = new List<T>((int)((maximum - minimum) / (managerQuantity)));

            for (var index = minimum; index <= maximum; index++)
            {
                if ((index % managerQuantity) == id)
                {
                    values.Add(index);
                }
            }

            return Task.FromResult(values.ToArray());
        }
        #endregion
    }
}
