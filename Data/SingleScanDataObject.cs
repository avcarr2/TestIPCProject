using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System; 
namespace Data
{
    [Serializable]
    public class SingleScanDataObject
    {
        public double[] Xarray { get; set; }
        public double[] Yarray { get; set; }
        public double TIC { get; set; }

        public SingleScanDataObject(double[] xarray, double[] yarray, double tic)
        {
            Xarray = xarray;
            Yarray = yarray;
            TIC = tic;
        }        
    }
    public class MemoryMappedFileIO
    {
        public string MemoryMapName { get; set; }
        public string MutexName { get; }
        // Raised when MMF is written and mutex is released.

        public event EventHandler<MMFWrittenEventArgs> MMFWritten; 
        public MemoryMappedFileIO(string name)
        {
            MemoryMapName = name;
            MutexName = name + "mutex"; 
        }
        public void CreateMemoryMappedFile<T>(T data, string name, long capacity)
        {
            if (!data.GetType().IsSerializable)
            {
                throw new ArgumentException("Type is not serializable", nameof(data));
            }
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew(name, capacity))
            {
                bool mutexCreated;
                Mutex mutex = new Mutex(true, name + "mutex", out mutexCreated);
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryFormatter writer = new BinaryFormatter();
                    if (typeof(T) == typeof(SingleScanDataObject))
                    {
                        try
                        {
                            writer.Serialize(stream, data);
                        }
                        catch (SerializationException e)
                        {
                            Console.WriteLine("Failed to serialize. " + e.Message);
                        }
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        throw new NotImplementedException();
                    }
                }
                mutex.ReleaseMutex();
                OnMMFWritten(); 
            }
        }
        protected virtual void OnMMFWritten()
        {
            MMFWrittenEventArgs e = new MMFWrittenEventArgs
            {
                MutexName = MutexName,
                MemoryMapName = MemoryMapName
            };

            EventHandler<MMFWrittenEventArgs> handler = MMFWritten; 
            if(handler != null)
            {
                handler(this, e); 
            }
        }
    }
    public class MMFWrittenEventArgs : EventArgs
    {
        public string MemoryMapName { get; set; }
        public string MutexName { get; set; }
    }
    public delegate void MMFWrittenEventHandler(Object sender, MMFWrittenEventArgs e);

    // NEXT: Create a memory mapped file in the ConsoleApp48 and create a listener in the TestIPCProject app. 
    // Create a client and listener setup as well so you can pass the name of the string. 
}
