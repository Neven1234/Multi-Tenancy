using MultiTenancy.Settings;

namespace MultiTenancy.Services
{
    public interface ITenantServices
    {
        public string? GetDatabaseProvider();
        public string? GetConnectionString();
        public Tenant? GetCurrentTenant();
    }
}
