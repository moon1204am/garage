using Newtonsoft.Json;
using System.Collections;

namespace Garage.Model
{
    public class Garage<T> : IEnumerable<T> where T : IVehicle
    {
        
        private T[] vehicles;
        private int currentIndex;
        private int count;
        public int GetFreeSpaces => Capacity - GetCount;
        public int GetCount => count;
        public bool IsFull => GetFreeSpaces == 0;
        public int Capacity { get; }

        public Garage(int capacity)
        {
            if(capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            vehicles = new T[capacity];
            currentIndex = 0;
            count = 0;
        }

        public bool Insert(T vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            if(IsFull) return false;
            else if (currentIndex == (Capacity - 1) && vehicles[currentIndex] != null)
            {
                int index = SearchForFreeSpace();
                if (index > -1) vehicles[index] = vehicle;
            } 
            else
            {
                vehicles[currentIndex] = vehicle;
                if (currentIndex < Capacity - 1) currentIndex++;
            }
            count++;
            return true;
        }

        public bool Remove(IVehicle vehicleToRemove)
        {
            int index = Array.IndexOf(vehicles, vehicleToRemove);
            if (index == -1) return false;
            vehicles[index] = default(T)!;
            count--;
            return true;
        }

        private int SearchForFreeSpace()
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i] != null)
                    return i;
            }
            return -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T v in vehicles)
            {
             if(v is not null)   
                yield return v;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
