using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopNet.API.Errors;
using ShopNet.BLL.Interfaces;
using ShopNet.BLL.MappingProfiles;
using ShopNet.BLL.Services;
using ShopNet.DAL.Data;
using StackExchange.Redis;
using System.Reflection;

namespace ShopNet.API.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle         
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"),
                    m => m.MigrationsAssembly(typeof(StoreContext).Assembly.FullName));
            });
            services.AddSingleton<IConnectionMultiplexer>(conf =>
                ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(config.GetConnectionString("Redis"))));

            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
            services.AddHttpContextAccessor();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IProductsRepository, ProductsService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",
                    policy => { policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"); });
            });

            return services;
        }
    }
}