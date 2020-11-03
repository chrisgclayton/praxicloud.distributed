// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes
{
    #region Using Clauses
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// An index manager that handles strings
    /// </summary>
    public interface IStringIndexManager : IIndexManager<string>
    {
        #region Properties
        /// <summary>
        /// The complete list of indexes owned by the index manager
        /// </summary>
        string[] OwnedIndexes { get; }

        /// <summary>
        /// The complete index range
        /// </summary>
        string[] Indexes { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Updates the indexes
        /// </summary>
        /// <param name="indexes">The complete list of indexes</param>
        /// <param name="cancellationToken">A token to monitor for abort requests</param>
        /// <returns>True if the range was updated successfully</returns>
        Task<bool> UpdateIndexesAsync(string[] indexes, CancellationToken cancellationToken);
        #endregion
    }
}
