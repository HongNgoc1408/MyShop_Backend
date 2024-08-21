using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Mappers;
using MyShop_Backend.Repositories.CategoryRepositories;
using MyShop_Backend.Services.CategoryService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ket noi csdl
builder.Services.AddDbContext<MyShopDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("MyShop_Backend")));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Mapper));

// Register services
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


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

app.Run();
