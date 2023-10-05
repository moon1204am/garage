using Garage.Controller;
using Garage.Model;

namespace Garage.Test
{
    public class GarageTest
    {
        private Airplane vehicle;

        public GarageTest()
        {
            vehicle = new Airplane("", "", 4, 5);
        }

        [Fact]
        public void AddAirplane_ShouldSucceed()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            IVehicle vehicle = new Airplane("abc123", "white", 3, 3);

            //Act
            bool res = garage.Insert(vehicle);

            //Assert
            Assert.True(res);
            Assert.Equal(garage.First(), vehicle);
        }

        [Fact]
        public void AddVehicles_GarageIsFull_ShouldFail()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            IVehicle vehicle1 = new Airplane("abc123", "white", 3, 3);
            IVehicle vehicle2 = new Car("abc124", "black", 3, 2.2);

            //Act
            garage.Insert(vehicle1);

            //Assert
            Assert.False(garage.Insert(vehicle2));
        }

        [Fact]
        public void RemoveVehicle_GarageHasVehicle_ShouldSucceed()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            IVehicle vehicle = new Airplane("abc123", "white", 3, 3);
            garage.Insert(vehicle);

            //Act
            bool res = garage.Remove(vehicle);

            //Assert
            Assert.True(res);
        }
        [Fact]
        public void RemoveVehicle_VehicleNotInGarage_ShouldFail()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);

            //Act
            bool res = garage.Remove(new Car("abc123", "black", 4, 5.4));

            //Assert
            Assert.False(res);
        }

        [Fact]
        public void GetNrOfVehicles_ShouldReturnCorrect()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(2);
            IVehicle vehicle1 = new Airplane("abc123", "white", 3, 3);
            IVehicle vehicle2 = new Car("abc124", "black", 3, 2.2);
            garage.Insert(vehicle1);
            garage.Insert(vehicle2);
            int expected = 2;

            //Act
            int actual = garage.GetCount;

            //Assert
            Assert.Equal(expected, actual);
           
        }

        [Fact]
        public void CheckIfGarageIsFull_ShouldReturnTrue()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(2);
            IVehicle vehicle1 = new Airplane("abc123", "white", 3, 3);
            IVehicle vehicle2 = new Car("abc124", "black", 3, 2.2);
            garage.Insert(vehicle1);
            garage.Insert(vehicle2);
            bool expected = true;

            //Act
            bool actual = garage.IsFull;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckNrOfFreeSpots_ShouldReturnCorrect()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(50);
            IVehicle vehicle1 = new Airplane("abc123", "white", 3, 3);
            IVehicle vehicle2 = new Car("abc124", "black", 3, 2.2);
            garage.Insert(vehicle1);
            garage.Insert(vehicle2);
            int expected = 48;

            //Act
            int actual = garage.GetFreeSpaces;

            //Assert
            Assert.Equal(expected, actual);
        }
    }


}