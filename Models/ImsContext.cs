using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models;

public partial class ImsContext : DbContext
{
    public ImsContext()
    {
    }

    public ImsContext(DbContextOptions<ImsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceList> InvoiceLists { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC074A4A5B0D");

            entity.ToTable("Customer");

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contact)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoice__3214EC0706AD69F7");

            entity.ToTable("Invoice");

            entity.Property(e => e.GrandTotal).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.TotalDiscount).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK__Invoice__Cid__49C3F6B7");
        });

        modelBuilder.Entity<InvoiceList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceL__3214EC07D7A08425");

            entity.ToTable("InvoiceList");

            entity.Property(e => e.Discount).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLists)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__InvoiceLi__Invoi__4CA06362");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07994ADD5F");

            entity.ToTable("Product");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(20, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
