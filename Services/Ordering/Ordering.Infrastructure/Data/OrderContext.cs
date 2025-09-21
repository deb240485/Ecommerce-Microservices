using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options): base(options)
        {
                
        }
        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<Order>())
            {
                switch(entry.State)
                {
                    case EntityState.Added: 
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "benspark"; // TO DO: replace with auth server.
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "benspark"; // TO DO: replace with auth server.
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
