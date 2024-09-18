using minimal_api.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class VehicleTest
{
    [TestMethod]
    public void TestGetSetProperties()
    {
        //arrange
        var vehicle = new Vehicle();
        
        //act
        vehicle.Id = 1;
        vehicle.Name = "test";
        vehicle.Mark = "test";

        //assert
        Assert.AreEqual(1, vehicle.Id);
        Assert.AreEqual("test", vehicle.Name);
        Assert.AreEqual("test", vehicle.Mark);
    }
}
