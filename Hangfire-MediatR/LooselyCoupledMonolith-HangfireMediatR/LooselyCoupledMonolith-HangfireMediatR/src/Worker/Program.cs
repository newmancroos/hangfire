using Hangfire;
using Hangfire.MediatR;
using Microsoft.Extensions.Hosting;
using Sales;
using Shipping;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSales();
                    services.AddShipping();

                    services.AddHangfire(configuration =>
                    {
                        configuration.UseSqlServerStorage("data source=.;User id=sa;password=Vennai_1;initial catalog=TestHangfire;MultipleActiveResultSets=True");
                        configuration.UseMediatR();
                    });

                    services.AddHangfireServer();
                });
    }
}
