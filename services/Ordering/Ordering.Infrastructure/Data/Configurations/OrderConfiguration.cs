using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Constants;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .UseIdentityColumn();

            builder.Property(o => o.UserName)
                .HasMaxLength(OrderConstants.UserNameMaxLength);

            builder.Property(o => o.TotalPrice)
                .HasColumnType(OrderConstants.PriceColumnType);

            builder.Property(o => o.FirstName)
                .HasMaxLength(OrderConstants.NameMaxLength);

            builder.Property(o => o.LastName)
                .HasMaxLength(OrderConstants.NameMaxLength);

            builder.Property(o => o.EmailAddress)
                .HasMaxLength(OrderConstants.EmailMaxLength);

            builder.Property(o => o.AddressLine)
                .HasMaxLength(OrderConstants.AddressMaxLength);

            builder.Property(o => o.Country)
                .HasMaxLength(OrderConstants.CountryMaxLength);

            builder.Property(o => o.State)
                .HasMaxLength(OrderConstants.StateMaxLength);

            builder.Property(o => o.ZipCode)
                .HasMaxLength(OrderConstants.ZipCodeMaxLength);

            builder.Property(o => o.CardName)
                .HasMaxLength(OrderConstants.CardNameMaxLength);

            builder.Property(o => o.CardNumber)
                .HasMaxLength(OrderConstants.CardNumberMaxLength);

            builder.Property(o => o.Expiration)
                .HasMaxLength(OrderConstants.ExpirationMaxLength);

            builder.Property(o => o.Cvv)
                .HasMaxLength(OrderConstants.CvvMaxLength);

            builder.Property(o => o.CreatedBy)
                .HasMaxLength(OrderConstants.AuditFieldMaxLength);

            builder.Property(o => o.LastModifiedBy)
                .HasMaxLength(OrderConstants.AuditFieldMaxLength);
        }
    }
}

