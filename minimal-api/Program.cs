using Microsoft.EntityFrameworkCore.Storage;
using minimal_api.Infrastructure.Db;
using minimal_api.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Enums;


#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataBaseContext>(options => {
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")));
});
var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");//WithTags → separa as rotas no swagger pelo nome indicado
#endregion

#region Admin
app.MapPost("admin/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) => {
    if (adminService.Login(loginDTO) != null) {
        return Results.Ok("Login realizado com sucesso!");
    } else {
        return Results.Unauthorized();
    }
}).WithTags("Admin");

app.MapGet("/admin", ([FromQuery] int? page, IAdminService adminService) => {
    var admins = new List<AdminModelView>();
    var adminsAll = adminService.GetAll(page);

    foreach (var adm in adminsAll) {
        admins.Add(new AdminModelView {
            Id = adm.Id,
            Email = adm.Email,
            Rule = adm.Rule
        });
    }
    return Results.Ok(admins);
}).WithTags("Admin");

app.MapGet("/admin/{id:int}", ([FromRoute] int id, IAdminService adminService) => {
    var admin = adminService.GetById(id);
    if (admin == null) {return Results.NotFound();}
    return Results.Ok(new AdminModelView {
            Id = admin.Id,
            Email = admin.Email,
            Rule = admin.Rule
        });
}).WithTags("Admin");

app.MapPost("admin", ([FromBody] AdminDTO adminDTO, IAdminService adminService) => {
    var validation = new ValidationErrors{
        Messages = new List<string>()
    };
    if (string.IsNullOrEmpty(adminDTO.Email)) {
        validation.Messages.Add("Email can't be empty!");
    }
    if (string.IsNullOrEmpty(adminDTO.Password)) {
        validation.Messages.Add("Password can't be empty!");
    }
    if (adminDTO.Rule == null) {
        validation.Messages.Add("Rule can't be empty!");
    }

    if (validation.Messages.Count > 0) {
        return Results.BadRequest(validation);
    }

    var admin = new Admin {
        Email = adminDTO.Email,
        Password = adminDTO.Password,
        Rule = adminDTO.Rule.ToString() ?? Rule.Editor.ToString(),
    };
    adminService.Add(admin);
    return Results.Created($"/admin/{admin.Id}", new AdminModelView {
        Id = admin.Id,
        Email = admin.Email,
        Rule = admin.Rule
    });
}).WithTags("Admin");

#endregion

#region Vehicle
ValidationErrors ValidationDTO(VehicleDTO vehicleDTO)
{
    var validation = new ValidationErrors{
        Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(vehicleDTO.Name)) {
        validation.Messages.Add("Name can't be empty!");
    }
    if (string.IsNullOrEmpty(vehicleDTO.Mark)) {
        validation.Messages.Add("Mark can't be empty!");
    }
    if (vehicleDTO.Year < 1950) {
        validation.Messages.Add("year cannot be less than 1950!");
    }
    return validation;
}


app.MapGet("/vehicles", ([FromQuery] int? page, IVehicleService vehicleService) => {
    var vehicles = vehicleService.GetAll(page);
    return Results.Ok(vehicles);
}).WithTags("Vehicle");

app.MapGet("/vehicles/{id:int}", ([FromRoute] int id, IVehicleService vehicleService) => {
    var vehicle = vehicleService.GetById(id);
    if (vehicle == null) {return Results.NotFound();}
    return Results.Ok(vehicle);
}).WithTags("Vehicle");

app.MapPost("/vehicle", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) => {
    var validation = ValidationDTO(vehicleDTO);
    if (validation.Messages.Count > 0) {
        return Results.BadRequest(validation);
    }

    var vehicle = new Vehicle {
        Name = vehicleDTO.Name,
        Mark = vehicleDTO.Mark,
        Year = vehicleDTO.Year
    };
    vehicleService.Add(vehicle);
    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
}).WithTags("Vehicle");

app.MapPut("/vehicles/{id:int}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) => {
    var validation = ValidationDTO(vehicleDTO);
    if (validation.Messages.Count > 0) {
        return Results.BadRequest(validation);
    }

    var vehicle = vehicleService.GetById(id);
    if (vehicle == null) {return Results.NotFound();}

    vehicle.Name = vehicleDTO.Name;
    vehicle.Mark = vehicleDTO.Mark;
    vehicle.Year = vehicleDTO.Year;
    vehicleService.Update(vehicle);

    return Results.Ok(vehicle);
}).WithTags("Vehicle");

app.MapDelete("/vehicles/{id:int}", ([FromRoute] int id, IVehicleService vehicleService) => {
    var vehicle = vehicleService.GetById(id);
    if (vehicle == null) {return Results.NotFound();}
    vehicleService.Delete(vehicle);
    return Results.NoContent();
}).WithTags("Vehicle");

#endregion


app.UseSwagger();
app.UseSwaggerUI();

app.Run();
