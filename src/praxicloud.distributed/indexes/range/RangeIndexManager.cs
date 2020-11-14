// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes.range
{
    #region Using Clauses
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Nito.AsyncEx;
    using praxicloud.core.security;
    #endregion

    /// <summary>
    /// A index distribution tool for ranges 
    /// </summary>
    /// <typeparam name="T">A numeric value</typeparam>
    public abstract class RangeIndexManager<T> : IRangeIndexManager<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
        public RangeIndexManager(int managerQuantity, int id, T minimum, T maximum)
        {
            Guard.NotLessThan(nameof(managerQuantity), managerQuantity, 1);
            Guard.NotLessThan(nameof(id), id, 0);
            Guard.NotMoreThan(nameof(id), id, managerQuantity - 1);

            Minimum = minimum;
            Maximum = maximum;
            Id = id;
            ManagerQuantity = managerQuantity;
        }
        #endregion
        #region Properties
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
        public virtual bool IsManagerQuantityUpdatable => true;

        /// <inheritdoc />
        public IndexRange<T> OwnedIndexRange { get; protected set; }

        /// <inheritdoc />
        public IIndexManager<T>.UpdateNotificationHandler NotificationHandler { get; set; }
        #endregion
        #region Methods
        /// <inheritdoc />
        public virtual async Task InitializeAsync(CancellationToken cancellationToken)
        {
            using (await _control.LockAsync())
            {
                OwnedIndexRange = await CreateOwnedIndexCollectionAsync(ManagerQuantity, Id, Minimum, Maximum, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public virtual bool IsOwner(T index)
        {
            return OwnedIndexRange != null && index.CompareTo(OwnedIndexRange.Minimum) <= 0 && index.CompareTo(OwnedIndexRange.Maximum) >= 0;
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
                    if (IsIdUpdatable)
                    {
                        var ownedIndexRanges = await CreateOwnedIndexCollectionAsync(ManagerQuantity, id, Minimum, Maximum, cancellationToken).ConfigureAwait(false);

                        Id = id;
                        OwnedIndexRange = ownedIndexRanges;

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
                        OwnedIndexRange = ownedIndexes;

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
                        OwnedIndexRange = ownedIndexes;

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
        /// <returns>The owned index range</returns>
        protected virtual Task<IndexRange<T>> CreateOwnedIndexCollectionAsync(int managerQuantity, int id, dynamic minimum, dynamic maximum, CancellationToken cancellationToken)
        {
            IndexRange<T> range;
            var totalItems = maximum - minimum + 1;

            if (totalItems > id)
            {
                var baseItemCount = totalItems / managerQuantity;
                var remainingItemCount = totalItems % managerQuantity;

                var start = (id * baseItemCount + (remainingItemCount > 0 && id > 0 ? Math.Min(id, remainingItemCount) : 0)) + minimum;
                var end = start + baseItemCount - 1 + (remainingItemCount > id ? 1 : 0);

                range = new IndexRange<T>(start, end);
            }
            else
            {
                range = null;
            }

            return Task.FromResult(range);
        }
        #endregion
    }
}
