using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdminRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestGetSetProperties()
    {
        //arrange
        var loginDTO = new LoginDTO{
            Email = "adm@test.com",
            Password = "123",
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

        //act
        var response = await Setup.client.PostAsync("/admin/login", content);

        //assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsByteArrayAsync();
        var admLogged = JsonSerializer.Deserialize<AdminLogged>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogged);
        Assert.IsNotNull(admLogged?.Email ?? "");
        Assert.IsNotNull(admLogged?.Rule ?? "");
        Assert.IsNotNull(admLogged?.Token ?? "");
    }
}