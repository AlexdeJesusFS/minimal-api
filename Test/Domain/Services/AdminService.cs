using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;

namespace Test.Domain.Services;

[TestClass]
public class AdminServiceTest
{
    private DataBaseContext CreateTestContext() 
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Usa o diretório base do app
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DataBaseContext(configuration);
    }

    [TestMethod]
    public void TestSalveAdmin()
    {
        //arrange
        var context = CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Adimins");

        var adm = new Admin
        {
            Id = 1,
            Email = "test@test.com",
            Password = "test",
            Rule = "Adm"
        };
        var adminService = new AdminService(context);
        
        //act
        adminService.Add(adm);
        

        //assert
        Assert.AreEqual(1, adminService.GetAll(1).Count);
        
    }
}
