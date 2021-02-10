using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using V2Sharp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using V2Sharp.Repository;
using V2Sharp.IRepository;

namespace V2Sharp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var ConnectionString = Configuration.GetConnectionString("V2SharpContext");
            services.AddDbContext<V2SharpContext>(opt 
                => opt.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));
            services.AddSingleton<IStatus, Status>();
            services.AddScoped<IUserInfo, UserInfo>();
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
