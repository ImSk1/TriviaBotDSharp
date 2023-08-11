using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TriviaBotDSharp.Core.Services.AnswersServices;
using TriviaBotDSharp.Core.Services.AnswersServices.Contracts;
using TriviaBotDSharp.Core.Services.ProfileServices;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.DAL;
using TriviaBotDSharp.API.Services;
using Microsoft.Extensions.Configuration;
using TriviaBotDSharp.Core.MappingProfiles;

namespace TriviaBotDSharp
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<TriviaContext>(options =>
            {
                var conString = _config.GetConnectionString("DefaultConnection");
                options.UseSqlServer(conString,
                    x => x.MigrationsAssembly("TriviaBotDSharp.DAL.Migrations"));
                

            });
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAnswersService, AnswersService>();
            services.AddScoped<IAPIService, APIService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<PlayerProfileProfile>();
            });
            var serviceProvider = services.BuildServiceProvider();
            var bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
