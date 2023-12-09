using Microsoft.Extensions.Options;
using MultiTenancy.Settings;

namespace MultiTenancy.Services
{
    public class TenantServices : ITenantServices
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext? _httpcontext;
        private Tenant? _currentTenant;
        public TenantServices(IHttpContextAccessor ContextAccessor,IOptions<TenantSettings> tenantSettings)
        {
            _httpcontext = ContextAccessor.HttpContext;
            _tenantSettings=tenantSettings.Value;
            if(_httpcontext is not null)
            {
                if(_httpcontext.Request.Headers.TryGetValue("tenant",out var tenantId))
                {
                    SetCurrenTenant(tenantId!);
                }
                else
                {
                    throw new Exception("No tenant provieded");
                }
            }

        }

        public string? GetConnectionString()
        {
            
            if (_currentTenant is null)
            {
                var connectionString = _tenantSettings.Defults.ConnectionString;
                return connectionString;
            }
            else
            {
                return _currentTenant.ConnectionString;
            }
          
        }

        public Tenant? GetCurrentTenant()
        {
           return _currentTenant;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defults.DbProvider;
        }
        private void SetCurrenTenant(string tenantid)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TId == tenantid);
            if (_currentTenant is null)
            {
                throw new Exception("Invalid tenant Id");
            }
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defults.ConnectionString;
            }
        }
    }
}
