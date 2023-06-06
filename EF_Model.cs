using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace LinQ_EF_Project
{
    public partial class EF_Model : DbContext
    {
        public EF_Model()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Order_Quantity> Order_Quantity { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_MeasuringUnit> Product_MeasuringUnit { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
        public virtual DbSet<Supply_Quantity> Supply_Quantity { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .Property(e => e.Client_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Mail)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Website)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Order_Quantity)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Product_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Order_Quantity)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Product_MeasuringUnit)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Supply_Quantity)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product_MeasuringUnit>()
                .Property(e => e.Measuring_Unit)
                .IsFixedLength();

            modelBuilder.Entity<Store>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Store>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Store>()
                .Property(e => e.Store_Manager)
                .IsUnicode(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Supplies)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supply>()
                .HasMany(e => e.Supply_Quantity)
                .WithRequired(e => e.Supply)
                .HasForeignKey(e => e.Supply_Num)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Vendor_Name)
                .IsFixedLength();

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Mail)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Website)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .HasMany(e => e.Supplies)
                .WithRequired(e => e.Vendor)
                .WillCascadeOnDelete(false);
        }
    }
}
