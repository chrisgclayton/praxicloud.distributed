// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes.strings
{
    #region Using Clauses
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Nito.AsyncEx;
    using praxicloud.core.security;
    #endregion

    /// <summary>
    /// A round robin index distribution tool 
    /// </summary>
    public sealed class StringIndexManager : IStringIndexManager
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
        /// <param name="indexes">The indexes being managed</param>
        public StringIndexManager(int managerQuantity, int id, string[] indexes)
        {
            Guard.NotLessThan(nameof(managerQuantity), managerQuantity, 1);
            Guard.NotLessThan(nameof(id), id, 0);
            Guard.NotMoreThan(nameof(id), id, managerQuantity - 1);

            Indexes = indexes.OrderBy(item => item).ToArray();
            Id = id;
            ManagerQuantity = managerQuantity;
        }
        #endregion
        #region Properties
        /// <inheritdoc />
        public string[] OwnedIndexes { get; private set; }

        /// <inheritdoc />
        public int Id { get; private set; }

        /// <inheritdoc />
        public bool AreIndexesUpdatable => true;

        /// <inheritdoc />
        public bool IsIdUpdatable => true;

        /// <inheritdoc />
        public int ManagerQuantity { get; private set; }

        /// <inheritdoc />
        public bool IsManagerQuantityUpdatable => true;

        /// <inheritdoc />
        public string[] Indexes { get; private set; }

        /// <inheritdoc />
        public IIndexManager<string>.UpdateNotificationHandler NotificationHandler { get; set; }
        #endregion
        #region Methods
        /// <inheritdoc />
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            using (await _control.LockAsync())
            {
                OwnedIndexes = CreateOwnedIndexCollection(ManagerQuantity, Id, Indexes);
            }
        }

        /// <inheritdoc />
        public bool IsOwner(string index)
        {
            return OwnedIndexes.Contains(index);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateIdAsync(int id, CancellationToken cancellationToken)
        {
            var success = false;
            var notify = false;

            if (IsIdUpdatable)
            {
                using (await _control.LockAsync())
                {
                    if (IsIdUpdatable)
                    {
                        var ownedIndexes = CreateOwnedIndexCollection(ManagerQuantity, id, Indexes);

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
        public async Task<bool> UpdateManagerQuantityAsync(int quantity, CancellationToken cancellationToken)
        {
            var success = false;
            var notify = false;

            if (IsManagerQuantityUpdatable)
            {
                using (await _control.LockAsync())
                {
                    if (IsManagerQuantityUpdatable)
                    {
                        var ownedIndexes = CreateOwnedIndexCollection(quantity, Id, Indexes);

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
        public async Task<bool> UpdateIndexesAsync(string[] indexes, CancellationToken cancellationToken)
        {
            var success = false;
            var notify = false;

            if (AreIndexesUpdatable)
            {
                using (await _control.LockAsync())
                {
                    if (AreIndexesUpdatable)
                    {
                        var ownedIndexes = CreateOwnedIndexCollection(ManagerQuantity, Id, indexes);

                        Indexes = indexes;
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
        /// <param name="indexes">The complete list of indexes</param>
        /// <returns>The array of owned indexes</returns>
        private string[] CreateOwnedIndexCollection(int managerQuantity, int id, string[] indexes)
        {
            var values = new List<string>(indexes.Length / managerQuantity);

            for (var index = 0; index < indexes.Length; index++)
            {
                if ((index % managerQuantity) == id)
                {
                    values.Add(indexes[index]);
                }
            }

            return values.ToArray();
        }
        #endregion
    }
}
