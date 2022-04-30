using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TriviaBotDSharp.DAL;
using Microsoft.EntityFrameworkCore.Design;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.Core.Services.ProfileServices;
using TriviaBotDSharp.Core.Services.AnswersServices.Contracts;
using TriviaBotDSharp.Core.Services.AnswersServices;
using TriviaBotDSharp.Core.Services.APIServices.Contracts;
using TriviaBotDSharp.Core.Services.APIServices;

namespace TriviaBotDSharp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<TriviaContext>(options =>
            {
                options.UseSqlServer("Data Source=DESKTOP-26JMI7A\\MSSQLSERVER01;Initial Catalog=TriviaContext;Integrated Security=True",
                    x => x.MigrationsAssembly("TriviaBotDSharp.DAL.Migrations"));
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            });
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAnswersService, AnswersService>();
            services.AddScoped<IAPIService, APIService>();

            var serviceProvider = services.BuildServiceProvider();
            var bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
