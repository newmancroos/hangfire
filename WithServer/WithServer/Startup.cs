using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonClassLib;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WithServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        //private readonly IRecurringJobManager _recurringJobManager;
        public Startup(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHangfire((provider, configs) => configs
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(_configuration.GetConnectionString("HangfireDb"), new Hangfire.SqlServer.SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,
                PrepareSchemaIfNecessary = true
            }));

            services.AddSingleton<IDisplayMessage, DisplayMessage>();
            services.AddHangfireServer(options=> {
                //options.ServerName = "MyServer";
                //options.Queues = new string[] { "WithServer" };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Test Hangfire"
            });

            //var options = new BackgroundJobServerOptions { ServerName="MyServer" };
            //app.UseHangfireServer(options);
            //backgroundJobClient.Enqueue(() => serviceProvider.GetService<IDisplayMessage>().DisplyText("From With-Server"));
            //BackgroundJob.Enqueue(() => serviceProvider.GetService<IDisplayMessage>().DisplyText("From With-Server"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
