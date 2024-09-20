using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;

namespace Test.Domain.Services;

[TestClass]
public class VehicleServiceTest
{
    private DataBaseContext CreateTestContext() 
    {
        //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Usa o diretório base do app
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DataBaseContext(configuration);
    }

    [TestMethod]
    public void TestAdd()
    {
        //arrange
        var context = CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle
        {
            Id = 1,
            Name = "The TestCar",
            Mark = "test",
            Year = 2000
        };
        var vehicleService = new VehicleService(context);
        
        //act
        vehicleService.Add(vehicle);

        //assert
        Assert.AreEqual(1, vehicleService.GetAll(1).Count);
    }

    [TestMethod]
    public void TestGetById()
    {
        //arrange
        var context = CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle
        {
            Id = 1,
            Name = "The TestCar",
            Mark = "test",
            Year = 2000
        };
        var vehicleService = new VehicleService(context);
        vehicleService.Add(vehicle);
        
        //act
        var vehicleId = vehicleService.GetById(vehicle.Id);

        //assert
        Assert.AreEqual(1, vehicleId.Id);
    }

    [TestMethod]
    public void TestDelete()
    {
        //arrange
        var context = CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle
        {
            Id = 1,
            Name = "The TestCar",
            Mark = "test",
            Year = 2000
        };
        var vehicleService = new VehicleService(context);
        vehicleService.Add(vehicle);
        
        //act
        int vehicleId = vehicleService.Delete(vehicle);

        //assert
        Assert.AreEqual(1, vehicleId);
    }

}