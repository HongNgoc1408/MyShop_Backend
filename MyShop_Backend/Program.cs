﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyShop_Backend.Data;
using MyShop_Backend.Mappers;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.BrandRepositories;
using MyShop_Backend.Repositories.CategoryRepositories;
using MyShop_Backend.Repositories.ImageRepositories;
using MyShop_Backend.Repositories.ProductColorRepositories;
using MyShop_Backend.Repositories.ProductSizeRepositories;
using MyShop_Backend.Repositories.SizeRepositories;
using MyShop_Backend.Repositories.TransactionRepositories;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.Repository.ProductRepository;
using MyShop_Backend.Services.AuthServices;
using MyShop_Backend.Services.BrandServices;
using MyShop_Backend.Services.CachingServices;
using MyShop_Backend.Services.CategoryService;
using MyShop_Backend.Services.Products;
using MyShop_Backend.Services.SendMailServices;
using MyShop_Backend.Services.Sizes;
using MyShop_Backend.Services.UserServices;
using MyShop_Backend.Storages;
using MyStore.Repository.ProductRepository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
builder.Services.AddDbContext<MyShopDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("MyShop_Backend")));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Mapping));

// Auth and Identity
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
	opt.Password.RequireNonAlphanumeric = false;
	opt.Password.RequiredLength = 6;
	opt.User.RequireUniqueEmail = true;
})
	.AddEntityFrameworkStores<MyShopDbContext>()
	.AddTokenProvider("MyShop_Backend", typeof(DataProtectorTokenProvider<User>));

// Email
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSingleton<ISendMailService, SendMailService>();



// JWT Authentication
builder.Services.AddAuthentication(option =>
{
	option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
	option.RequireHttpsMetadata = false;
	option.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidAudience = builder.Configuration["JWT:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? "")),
		ClockSkew = TimeSpan.Zero
	};
});

// Register services
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICachingService, CachingService>();
builder.Services.AddScoped<IFileStorage, FileStorage>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ISizeService, SizeService>();





// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
builder.Services.AddScoped<IProductColorRepository, ProductColorRepository>();

// CORS
builder.Services.AddCors(opt =>
{
	opt.AddPolicy("MyCors", opt =>
	{
		opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyCors");

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
