// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes
{
    #region Using Clauses
    using System;
    #endregion

    /// <summary>
    /// An index manager that handles discrete ownership vs. ranges
    /// </summary>
    /// <typeparam name="T">A numeric type</typeparam>
    interface IRangeIndexManager<T> : INumericIndexManager<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        #region Properties
        /// <summary>
        /// The range of indexes that are owned by the manager
        /// </summary>
        IndexRange<T> OwnedIndexRange { get; }
        #endregion
    }
}
