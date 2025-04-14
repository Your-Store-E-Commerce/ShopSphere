using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopSphere.Data.Entities.Order;

namespace ShopSphere.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
     
            builder.Property(o => o.Subtotal).HasColumnType("decimal(12,2)");
            builder.Property(o => o.orderStatus)
                .HasConversion(
                (OStatus) => (OStatus).ToString(),
                 (OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), (OStatus))
                    );
            builder.HasMany(o => o.Items)
                .WithOne().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
