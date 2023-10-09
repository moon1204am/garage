using System.Collections;

namespace Garage.Model
{
    /// <summary>
    /// Generic class responsible for holding the garage and it's data.
    /// </summary>
    /// <typeparam name="T">must be IVehicle or implement IVehicle</typeparam>
    public class Garage<T> : IEnumerable<T> where T : IVehicle
    {
        private T[] vehicles;
        private int currentIndex;
        private int count;
        public int GetFreeSpaces => Capacity - Count;
        public int Count => count;
        public bool IsFull => GetFreeSpaces == 0;
        public int Capacity { get; }

        public Garage(int capacity)
        {
            if(capacity < GarageSettings.MinimumCapacity) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            vehicles = new T[capacity];
            currentIndex = 0;
            count = 0;
        }

        /// <summary>
        /// Inserts a vehicle to the garage.
        /// </summary>
        /// <param name="vehicle">The vehicle to be inserted.</param>
        /// <returns>True if vehicle was inserted, otherwise false.</returns>
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

        /// <summary>
        /// Removes a vehicle from the garage.
        /// </summary>
        /// <param name="vehicleToRemove">The vehicle to remove.</param>
        /// <returns>True if the vehicle was removed, otherwise false.</returns>
        public bool Remove(IVehicle vehicleToRemove)
        {
            int index = Array.IndexOf(vehicles, vehicleToRemove);
            if (index == -1) return false;
            vehicles[index] = default(T)!;
            count--;
            return true;
        }

        /// <summary>
        /// Search for a free space in the garage array.
        /// </summary>
        /// <returns>The found free index of array, otherwise -1.</returns>
        private int SearchForFreeSpace()
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i] != null)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Implementation called when foreach looping through the garage.
        /// </summary>
        /// <returns></returns>
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
