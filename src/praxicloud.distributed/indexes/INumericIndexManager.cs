// Copyright (c) Christopher Clayton. All rights reserved.
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
    /// <typeparam name="T">A numeric type</typeparam>
    public interface INumericIndexManager<T> : IIndexManager<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        #region Properties
        /// <summary>
        /// The minimum value in the range
        /// </summary>
        T Minimum { get; }

        /// <summary>
        /// The maximum value in the range
        /// </summary>
        T Maximum { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Updates the range of the partition
        /// </summary>
        /// <param name="minimum">The minimum value in the range</param>
        /// <param name="maximum">The maximum value in the range</param>
        /// <param name="cancellationToken">A token to monitor for abort requests</param>
        /// <returns>True if the range was updated successfully</returns>
        Task<bool> UpdateIndexesAsync(T minimum, T maximum, CancellationToken cancellationToken);
        #endregion
    }
}
