using Amazon.SimpleNotificationService;
using Amazon.SimpleSystemsManagement;
using Banking.Onboarding.Domain;
using Banking.Onboarding.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Onboarding
{
    [Amazon.Lambda.Annotations.LambdaStartup]
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            // Build configuration and include SSM Parameter Store
            var builder = new ConfigurationBuilder()
                .AddSystemsManager("/onboarding",false, TimeSpan.FromMinutes(5));

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAWSService<IAmazonSimpleNotificationService>();
            services.AddAWSService<IAmazonSimpleSystemsManagement>();
            services.AddSingleton(Configuration);
            services.InjectDomain();
            services.InjectInfrastructure();
        }
    }
}
