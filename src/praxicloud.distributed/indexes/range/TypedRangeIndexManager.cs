// Copyright (c) Christopher Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes.range
{
    #region Using Clauses
    using praxicloud.core.security;
    #endregion

    /// <summary>
    /// Range index manager for signed 64 bit integers
    /// </summary>
    public sealed class Int64RangeIndexManager : RangeIndexManager<long>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public Int64RangeIndexManager(int managerQuantity, int id, long minimum, long maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Range index manager for signed 32 bit integers
    /// </summary>
    public sealed class Int32RangeIndexManager : RangeIndexManager<int>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public Int32RangeIndexManager(int managerQuantity, int id, int minimum, int maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Range index manager for signed 16 bit integers
    /// </summary>
    public sealed class Int16RangeIndexManager : RangeIndexManager<short>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public Int16RangeIndexManager(int managerQuantity, int id, short minimum, short maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Range index manager for byte range
    /// </summary>
    public sealed class ByteRangeIndexManager : RangeIndexManager<byte>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public ByteRangeIndexManager(int managerQuantity, int id, byte minimum, byte maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Range index manager for unsigned 64 bit integers
    /// </summary>
    public sealed class UnsignedInt64RangeIndexManager : RangeIndexManager<ulong>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public UnsignedInt64RangeIndexManager(int managerQuantity, int id, ulong minimum, ulong maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Range index manager for unsigned 32 bit integers
    /// </summary>
    public sealed class UnsignedInt32RangeIndexManager : RangeIndexManager<uint>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public UnsignedInt32RangeIndexManager(int managerQuantity, int id, uint minimum, uint maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Range index manager for unsigned 16 bit integers
    /// </summary>
    public sealed class UnsignedInt16RangeIndexManager : RangeIndexManager<ushort>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public UnsignedInt16RangeIndexManager(int managerQuantity, int id, ushort minimum, ushort maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }
}
