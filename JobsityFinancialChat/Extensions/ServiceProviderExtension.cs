using JobsityFinancialChat.Data;
using Microsoft.EntityFrameworkCore;

namespace JobsityFinancialChat.Extensions
{
    public static class ServiceProviderExtension
    {
        public static IServiceProvider SetupDataBase(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            MigrateDataBase<JobsityContext>(scope);
            return services;
        }

        private static void MigrateDataBase<TContext>(IServiceScope scope)
            where TContext : DbContext
        {
            var context = scope.ServiceProvider.GetService<TContext>();
            if (context is not null)
            {
                context.Database.Migrate();
            }
        }
    }
}
