using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TriviaBotDSharp.DAL;
using Microsoft.EntityFrameworkCore.Design;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.Core.Services.ProfileServices;

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

            var serviceProvider = services.BuildServiceProvider();
            var bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
