using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using SapCo2.Samples.Core.Extensions;

namespace SapCo2.Samples.Web.Mvc.Net5
{
    public class Startup
    {
        #region Constants

        private const string AppSettingsFileName = "appSettings.json";
        private const string UserSecretId = "6EE22606-D56C-4FC3-A363-5C58E3ED1371";

        #endregion

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        public Startup()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(AppSettingsFileName, true, true)
                .AddUserSecrets(UserSecretId, true)
                .AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSapCo2SampleCore(Configuration);
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
