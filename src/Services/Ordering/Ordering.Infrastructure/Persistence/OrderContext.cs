﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domains.Common;
using Ordering.Domains.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext:DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options):base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "KingKong";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "KingKong";
                        break;
                }
                
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
