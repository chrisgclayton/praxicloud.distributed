// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.indexes.discrete
{
    #region Using Clauses
    using praxicloud.core.security;
    #endregion

    /// <summary>
    /// Discrete round robin index manager for signed 64 bit integers
    /// </summary>
    public sealed class Int64IndexManager : DiscreteIndexManager<long>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public Int64IndexManager(int managerQuantity, int id, long minimum, long maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Discrete round robin index manager for signed 32 bit integers
    /// </summary>
    public sealed class Int32IndexManager : DiscreteIndexManager<int>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public Int32IndexManager(int managerQuantity, int id, int minimum, int maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Discrete round robin index manager for signed 16 bit integers
    /// </summary>
    public sealed class Int16IndexManager : DiscreteIndexManager<short>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public Int16IndexManager(int managerQuantity, int id, short minimum, short maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Discrete round robin index manager for byte range
    /// </summary>
    public sealed class ByteIndexManager : DiscreteIndexManager<byte>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public ByteIndexManager(int managerQuantity, int id, byte minimum, byte maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Discrete round robin index manager for unsigned 64 bit integers
    /// </summary>
    public sealed class UnsignedInt64IndexManager : DiscreteIndexManager<ulong>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public UnsignedInt64IndexManager(int managerQuantity, int id, ulong minimum, ulong maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Discrete round robin index manager for unsigned 32 bit integers
    /// </summary>
    public sealed class UnsignedInt32IndexManager : DiscreteIndexManager<uint>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public UnsignedInt32IndexManager(int managerQuantity, int id, uint minimum, uint maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }

    /// <summary>
    /// Discrete round robin index manager for unsigned 16 bit integers
    /// </summary>
    public sealed class UnsignedInt16IndexManager : DiscreteIndexManager<ushort>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the type
        /// </summary>
        /// <param name="managerQuantity">The number of managers that will take part in the processing</param>
        /// <param name="id">The 0 based index of the manager that owns this instance</param>
        /// <param name="minimum">The minimum value in the entire range</param>
        /// <param name="maximum">The maximum value in the entire range</param>
        public UnsignedInt16IndexManager(int managerQuantity, int id, ushort minimum, ushort maximum) : base(managerQuantity, id, minimum, maximum)
        {
            Guard.NotLessThan(nameof(maximum), maximum, minimum);
        }
        #endregion
    }
}
