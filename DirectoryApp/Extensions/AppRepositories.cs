using DirectoryApp.Repository;


namespace DirectoryApp.Extensions
{
    public static class AppRepositories
    {
        public static IServiceCollection AddAppRepositories(this IServiceCollection services)
        {
            services.AddScoped<ContactRepo>();
            return services;
        }
    }
}
