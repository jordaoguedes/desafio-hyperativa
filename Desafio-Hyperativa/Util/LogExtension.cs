using Serilog;

namespace DesafioHyperativa.Util
{
    public static class LogExtension
    {
        public static IServiceCollection AddSeriLog(this IServiceCollection services, WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

            builder.Host.UseSerilog(
                (hostBuilderContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
                }
            );

            return services;
        }
    }
}
