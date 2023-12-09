namespace MultiTenancy.Settings
{
    public class TenantSettings
    {
        public Configuration Defults { get; set; } = default!;
        public List<Tenant> Tenants { get; set; } = new();
    }
}
