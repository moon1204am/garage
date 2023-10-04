using Garage.Controller;
using Garage.Model;

namespace Garage.Test
{
    public class UnitTest1
    {
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
        }

        [Fact]
        public void AddVehicles_SameLicenceNumber_ShouldFail()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            IVehicle vehicle1 = new Airplane("abc123", "white", 3, 3);
            IVehicle vehicle2 = new Car("ABC123", "white", 3, 2.2);

            //Act
            garage.Insert(vehicle1);

            //Assert
            Assert.False(garage.Insert(vehicle2));
        }
    }
}