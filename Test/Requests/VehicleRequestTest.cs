using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.ModelViews;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class VehicleRequestTest
    {
        
        private static string? _token;
        private static HttpClient? _client;
        private static int vehicleId; 

        [ClassInitialize]
        public static async Task ClassInit(TestContext testContext)
        {
            // Inicializa o cliente HTTP e obtém o token
            Setup.ClassInit(testContext);
            _client = Setup.client;

            var loginDTO = new LoginDTO
            {
                Email = "adm@test.com",
                Password = "123",
            };
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/admin/login", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsByteArrayAsync();
            var admLogged = JsonSerializer.Deserialize<AdminLogged>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _token = admLogged?.Token;

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }

        private void SetAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }
        }

        [TestMethod]
        public async Task TestGetAll()
        {
            // Arrange
            int page = 1; // Página a ser testada

            // Configura o cabeçalho de autorização
            SetAuthorizationHeader();

            // Act
            var response = await _client.GetAsync($"/vehicles?page={page}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Resposta JSON: " + result); 
            
            var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(vehicles);
            Assert.IsTrue(vehicles.Count > 0, "A lista de veículos não deve estar vazia.");
        }

        [TestMethod]
        public async Task TestCreate()
        {
            // Arrange
            var vehicle = new Vehicle
            {
                Name = "Test Car",
                Mark = "Test Mark",
                Year = 2024
            };
            var content = new StringContent(JsonSerializer.Serialize(vehicle), Encoding.UTF8, "application/json");

            // Configura o cabeçalho de autorização
            SetAuthorizationHeader();

            // Act
            var response = await _client.PostAsync($"/vehicle", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Resposta JSON: " + result); 
            
            var vehicles = JsonSerializer.Deserialize<Vehicle>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(vehicles);

            vehicleId = vehicles.Id;
        }
    }
}
