// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes
{
    #region Using Clauses
    using System;
    #endregion

    /// <summary>
    /// A range of index values
    /// </summary>
    /// <typeparam name="T">A numeric value</typeparam>
    public sealed class IndexRange<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="minimum">The minimum value in the range</param>
        /// <param name="maximum">The maximum value in the range</param>
        public IndexRange(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }
        #endregion
        #region Properties
        /// <summary>
        /// The minimum value in the range
        /// </summary>
        public T Minimum { get; }

        /// <summary>
        /// The maximum value in the ramge
        /// </summary>
        public T Maximum { get; }
        #endregion
    }
}
