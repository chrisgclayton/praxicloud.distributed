# PraxiCloud Distributed
PraxiCloud Libraries are a set of common utilities and tools for general software development that simplify common development efforts for software development. The distributed library contains features and functions that are targeted at solving distributed computing problems. This library attempts to maintain a low number of dependencies.



# Installing via NuGet

Install-Package PraxiCloud-Distributed



# Indexes



## Key Types and Interfaces

|Class| Description | Notes |
| ------------- | ------------- | ------------- |
|**DiscreteIndexManager**|Manages a range of numeric values, assigning the contained indexes across multiple processing instances (potentially on different machines). This manager does not try and maintain "ranges" of values, instead it balances as the managers sees as appropriate (e.g. round robin).<br />***IsOwner*** returns true if the number was assigned to the current instance of manager.<br />***UpdateManagerQuantityAsync*** changes the number of known index manager instances. This may occur due to scale events etc. Adjusting this may result in ownership changes.<br />|  |
|**Int64IndexManager**|A discrete index manager that manages 64 bit signed integers.|  |
|**Int32IndexManager**|A discrete index manager that manages 32 bit signed integers.|  |
|**Int16IndexManager**|A discrete index manager that manages 16 bit signed integers.|  |
|**ByteIndexManager**|A discrete index manager that manages byte values.|  |
|**UnsignedInt64IndexManager**|A discrete index manager that manages 64 bit unsigned integers.|  |
|**UnsignedInt32IndexManager**|A discrete index manager that manages 32 bit unsigned integers.|  |
|**UnsignedInt16IndexManager**|A discrete index manager that manages 16 bit unsigned integers.|  |
|**StringIndexManager**|An index manager that manages an array of strings.| In the constructor the manager orders the strings. |
|**Int64RangeIndexManager**|A range index manager that manages 64 bit signed integers and distributes them in ranges consecutive values.| Unless managers must operate over a consecutive set of values it is recommended to use the discrete functionality. |
|**Int32RangeIndexManager**|A range index manager that manages 32 bit signed integers and distributes them in ranges consecutive values.| Unless managers must operate over a consecutive set of values it is recommended to use the discrete functionality. |
|**Int16RangeIndexManager**|A range index manager that manages 16 bit signed integers and distributes them in ranges consecutive values.| Unless managers must operate over a consecutive set of values it is recommended to use the discrete functionality. |
|**ByteRangeIndexManager**|A range index manager that manages bytes values and distributes them in ranges consecutive values.| Unless managers must operate over a consecutive set of values it is recommended to use the discrete functionality. |
|**UnsignedInt64RangeIndexManager**|A range index manager that manages 64 bit unsigned integers and distributes them in ranges consecutive values.| Unless managers must operate over a consecutive set of values it is recommended to use the discrete functionality. |
|**UnsignedInt32RangeIndexManager**|A range index manager that manages 32 bit unsigned integers and distributes them in ranges consecutive values.| Unless managers must operate over a consecutive set of values it is recommended to use the discrete functionality. |
|**UnsignedInt16RangeIndexManager**|A range index manager that manages 16 bit unsigned integers and distributes them in ranges consecutive values.| Unless managers must operate over a consecutive set of values it is recommended to use the discrete functionality. |

## Sample Usage

### Implement a Custom Discrete Numeric Index Manager

```csharp
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
            
        protected override Task<T[]> CreateOwnedIndexCollectionAsync(int managerQuantity, int id, dynamic minimum, dynamic maximum, CancellationToken cancellationToken)
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
    }
```

### Use a 32 Bit Signed Integer Index Manager

```csharp
var manager = new Int32IndexManager(1, 0, minimum, maximum)
{
    NotificationHandler = (manager, cancellationToken) =>
    {
	return Task.CompletedTask;
    }
};

await manager.InitializeAsync(CancellationToken.None).ConfigureAwait(false);

var myOwnedIndexes = manager.OwnedIndexes;

if(manager.IsOwner(4))
{
    Console.WriteLine("I own the number 4");
}

await manager.UpdateManagerQuantityAsync(2, CancellationToken.None).ConfigureAwait(false);

if(manager.IsOwner(4))
{
    Console.WriteLine("I own the number 4");
}
```

### Use a String Integer Index Manager

```csharp
var indexKeys = Enumerable.Range(minimum, maximum - minimum + 1).Select(item => item.ToString()).ToArray();

var manager = new Int32IndexManager(1, 0, indexKeys)
{
    NotificationHandler = (manager, cancellationToken) =>
    {
	return Task.CompletedTask;
    }
};

await manager.InitializeAsync(CancellationToken.None).ConfigureAwait(false);

var myOwnedIndexes = manager.OwnedIndexes;

if(manager.IsOwner("4"))
{
    Console.WriteLine("I own the string 4");
}

await manager.UpdateManagerQuantityAsync(2, CancellationToken.None).ConfigureAwait(false);

if(manager.IsOwner("4"))
{
    Console.WriteLine("I own the string 4");
}
```

### Use a 32 Bit Signed Integer Range Index Manager

```csharp
var manager = new Int32IndexManager(1, 0, minimum, maximum)
{
    NotificationHandler = (manager, cancellationToken) =>
    {
	return Task.CompletedTask;
    }
};

await manager.InitializeAsync(CancellationToken.None).ConfigureAwait(false);

var myOwnedRange = manager.OwnedIndexRange;

if(manager.IsOwner(4))
{
    Console.WriteLine("I own the string 4");
}

await manager.UpdateManagerQuantityAsync(2, CancellationToken.None).ConfigureAwait(false);

if(manager.IsOwner(4))
{
    Console.WriteLine("I own the string 4");
}
```

## Additional Information

For additional information the Visual Studio generated documentation found [here](./documents/praxicloud.distributed/praxicloud.distributed.xml), can be viewed using your favorite documentation viewer.




