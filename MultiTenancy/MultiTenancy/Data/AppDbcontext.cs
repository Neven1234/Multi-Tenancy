using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MultiTenancy.Contract;
using MultiTenancy.Models;
using MultiTenancy.Services;
using MultiTenancy.Settings;

namespace MultiTenancy.Data
{
    public class AppDbcontext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantServices _tenantServices;
        public AppDbcontext(DbContextOptions options, ITenantServices tenantServices) : base(options)
        {
            _tenantServices = tenantServices;
            TenantId=_tenantServices.GetCurrentTenant()?.TId;
        }

        public AppDbcontext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(e=>e.TenantId == TenantId);
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString=_tenantServices.GetConnectionString();
            if(!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DbProvider=_tenantServices.GetDatabaseProvider();
                if(DbProvider.ToLower()== "mssql")
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }
        }
        // when add new product
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entity in ChangeTracker.Entries<IMustHaveTenant>().Where(e=>e.State==EntityState.Added))
            {
                entity.Entity.TenantId = TenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Product> Products { get; set; }
    }

  
}
