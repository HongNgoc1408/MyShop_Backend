using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyShop_Backend.Data;
using MyShop_Backend.Mappers;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.BrandRepositories;
using MyShop_Backend.Repositories.CartItemRepositories;
using MyShop_Backend.Repositories.CategoryRepositories;
using MyShop_Backend.Repositories.DeliveryAddressRepositories;
using MyShop_Backend.Repositories.ImageRepositories;
using MyShop_Backend.Repositories.ProductColorRepositories;
using MyShop_Backend.Repositories.ProductSizeRepositories;
using MyShop_Backend.Repositories.SizeRepositories;
using MyShop_Backend.Repositories.TransactionRepositories;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.CommonRepository.ProductRepository;
using MyShop_Backend.Services.AuthServices;
using MyShop_Backend.Services.BrandServices;
using MyShop_Backend.Services.CachingServices;
using MyShop_Backend.Services.Carts;
using MyShop_Backend.Services.CategoryService;
using MyShop_Backend.Services.Products;
using MyShop_Backend.Services.SendMailServices;
using MyShop_Backend.Services.Sizes;
using MyShop_Backend.Services.UserServices;
using MyShop_Backend.Storages;
using MyStore.Repository.ProductRepository;
using System.Text;
using MyShop_Backend.Services.Orders;
using MyShop_Backend.Repositories.OrderRepositories;
using MyShop_Backend.Repositories.OrderDetailRepositories;
using MyShop_Backend.Repositories.PaymentMethodRepositories;
using MyShop_Backend.Services.Payments;
using MyShop_Backend.Library;
using MyShop_Backend.Repositories.ProductFavoriteRepositories;
using MyShop_Backend.Repositories.ImportDetailRepositories;
using MyShop_Backend.Repositories.ImportRepositories;
using MyShop_Backend.Services.Imports;
using MyShop_Backend.Repositories.ProductReviewRepositories;
using MyShop_Backend.Services.Statistices;
using MyShop_Backend.Repositories.LogRepositories;
using MyShop_Backend.Repositories.LogDetailRepositories;
using MyShop_Backend.Services.Log;
using MyShop_Backend.Services.LogImports;
using MyShop_Backend.DataSeeding;
using MyShop_Backend.Services.Reviews;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(e =>
{
	e.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer abcdef12345\""
	});
	e.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				new string[] { }
			}
		});
});
// Database connection
builder.Services.AddDbContext<MyShopDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("MyShop_Backend")));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Mapping));

// Auth and Identity
builder.Services.AddIdentity<User, Role>(opt =>
{
	opt.Password.RequireNonAlphanumeric = false;
	opt.Password.RequiredLength = 6;
	opt.User.RequireUniqueEmail = true;
})
	.AddEntityFrameworkStores<MyShopDbContext>()
	.AddDefaultTokenProviders();

// Email
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSingleton<ISendMailService, SendMailService>();



// JWT Authenticationvar
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
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
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
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVNPayLibrary, VNPayLibrary>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<ILogImportService, LogService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IReviewService, ReviewService>();


// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
builder.Services.AddScoped<IProductColorRepository, ProductColorRepository>();
builder.Services.AddScoped<IProductFavoriteRepository, ProductFavoriteRepository>();
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IDeliveryAddressRepository, DeliveryAddressRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IImportRepository, ImportRepository>();
builder.Services.AddScoped<IImportDetailRepository, ImportDetailRepository>();
builder.Services.AddScoped<ILogDetailRepository, LogDetailRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();


// CORS
builder.Services.AddCors(opt =>
{
	opt.AddPolicy("MyCors", opt =>
	{
		opt.WithOrigins("http://localhost:3000", "http://localhost:3001").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
		//opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
	});
});



builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; 
});

var app = builder.Build();

DataSeeding.Initialize(app.Services).Wait();


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
