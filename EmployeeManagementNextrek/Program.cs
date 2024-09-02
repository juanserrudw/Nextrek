using EmployeeManagementNextrek;
using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models.Dtos;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementNextrek.Repositories.IRepository;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Configuracion conexion a DB.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingConfig));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository >();
builder.Services.AddScoped<IEmployeeAddressRepository, EmployeeAddressRepository >();
builder.Services.AddScoped<IEmployeeDocumentRepository, EmployeeDocumentRepository >();
builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository >();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository >();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Asegurar que la base de datos esté creada y aplicar las migraciones pendientes.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    context.Database.Migrate();
}

app.Run();
