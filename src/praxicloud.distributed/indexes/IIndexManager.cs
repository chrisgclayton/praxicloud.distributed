// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes
{
    #region Using Clauses
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// An index manager that handles 
    /// </summary>
    public interface IIndexManager<T>
    {
        #region Delegates
        /// <summary>
        /// The signature of a handler to receive notifications of index, id or manager count change events.
        /// </summary>
        /// <param name="manager">The manager that the change occurred on</param>
        /// <param name="cancellationToken">A token to monitor for abort requests</param>
        delegate Task UpdateNotificationHandler(IIndexManager<T> manager, CancellationToken cancellationToken);
        #endregion
        #region Properties
        /// <summary>
        /// The 0 based id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The number of managers that are in existance
        /// </summary>
        int ManagerQuantity { get; }

        /// <summary>
        /// True if the range supports updating
        /// </summary>
        bool AreIndexesUpdatable { get; }

        /// <summary>
        /// True if the id supports updating
        /// </summary>
        bool IsIdUpdatable { get; }

        /// <summary>
        /// True if the manager quantity is updatable
        /// </summary>
        bool IsManagerQuantityUpdatable { get; }

        /// <summary>
        /// A handler that can be assigned to be notified of changes to the id, manager quantity or range
        /// </summary>
        UpdateNotificationHandler NotificationHandler { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Checks if the specified index is own
        /// </summary>
        /// <param name="index">The value that is being checked</param>
        /// <returns>True if the current id owns the index value</returns>
        bool IsOwner(T index);

        /// <summary>
        /// Updates the id
        /// </summary>
        /// <param name="id">The new value of the id</param>
        /// <param name="cancellationToken">A token to monitor for abort requests</param>
        /// <returns>True if it was updated successfully</returns>
        Task<bool> UpdateIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the manager quantity
        /// </summary>
        /// <param name="quantity">The new manager quantity</param>
        /// <param name="cancellationToken">A token to monitor for abort requests</param>
        /// <returns>True if it was updated successfully</returns>
        Task<bool> UpdateManagerQuantityAsync(int quantity, CancellationToken cancellationToken);

        /// <summary>
        /// Initializes the manager before first use
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for abort requests</param>
        Task InitializeAsync(CancellationToken cancellationToken);
        #endregion
    }
}
