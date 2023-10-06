using Garage.Controller;
using Garage.Model;

namespace Garage.Test
{
    public class GarageTest
    {
        private Airplane airplane;
        private Car car;
        Garage<IVehicle> garage;

        public GarageTest()
        {
            airplane = new Airplane("abc123", "white", 3, 3);
            car = new Car("abc124", "black", 3, 2.2);
        }

        [Fact]
        public void AddAirplane_ShouldSucceed()
        {
            //Arrange
            garage = new Garage<IVehicle>(1);

            //Act
            bool res = garage.Insert(airplane);

            //Assert
            Assert.True(res);
            Assert.Equal(garage.First(), airplane);
        }

        [Fact]
        public void AddVehicles_GarageIsFull_ShouldFail()
        {
            //Arrange
            garage = new Garage<IVehicle>(1);

            //Act
            garage.Insert(airplane);

            //Assert
            Assert.False(garage.Insert(car));
        }

        [Fact]
        public void RemoveVehicle_GarageHasVehicle_ShouldSucceed()
        {
            //Arrange
            garage = new Garage<IVehicle>(1);
            garage.Insert(airplane);

            //Act
            bool res = garage.Remove(airplane);

            //Assert
            Assert.True(res);
        }
        [Fact]
        public void RemoveVehicle_VehicleNotInGarage_ShouldFail()
        {
            //Arrange
            garage = new Garage<IVehicle>(1);

            //Act
            bool res = garage.Remove(car);

            //Assert
            Assert.False(res);
        }

        [Fact]
        public void GetNrOfVehicles_ShouldReturnCorrect()
        {
            //Arrange
            garage = new Garage<IVehicle>(2);
            garage.Insert(airplane);
            garage.Insert(car);
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
            garage = new Garage<IVehicle>(2);
            garage.Insert(airplane);
            garage.Insert(car);
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
            garage = new Garage<IVehicle>(50);
            garage.Insert(airplane);
            garage.Insert(car);
            int expected = 48;

            //Act
            int actual = garage.GetFreeSpaces;

            //Assert
            Assert.Equal(expected, actual);
        }
    }


}