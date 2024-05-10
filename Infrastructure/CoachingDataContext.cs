using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Policy;

namespace Infrastructure
{
    public class PlayerDataContext : IdentityDbContext<ApplicationUsers>
    {

        public PlayerDataContext(DbContextOptions<PlayerDataContext> options) : base(options)
        {

        }
        public DbSet<PlayerMemberShip> PlayerMemberShip { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<UserAttendance> UserAttendance { get; set; }
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }
        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Actions> Actions { get; set; }
        //public DbSet<Staff> Staff { get; set; }
        public DbSet<ItemPurchaseOrder> ItemPurchaseOrder { get; set; }
        public DbSet<NegatePurchaseOrder> NegatePurchaseOrders { get; set; }
       
        public DbSet<VendorPayment> VendorPayments { get; set; }
        public DbSet<VendorReceipt> VendorReceipts { get; set; }
        //public DbSet<AdditionalFeatures> AdditionalFeatures { get; set; }
        //public DbSet<Designations> Designations { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<NavigationMenu> NavigationMenu { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }
        public DbSet<PackageHistory> PackageHistory { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<CashReceipt> CashReceipt { get; set; }
        public DbSet<CashReceiptGoods> CashReceiptGoods { get; set; }
        public DbSet<AccountHead> AccountHeads { get; set; }
        public DbSet<AccountCodes> AccountCodes { get; set; }       
        public DbSet<PaymentVoucherDetail> PaymentVoucherDetail { get; set; }
        public DbSet<ReceiptVoucher> ReceipVoucher { get; set; }
        public DbSet<ReceiptVoucherDetail> ReceiptVoucherDetail { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=DESKTOP-61JNP5Q\\SQL2019;Database=AQAcademy;Trusted_Connection=True;MultipleActiveResultSets=true;");
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountHead>()
                .HasData
                (
                new AccountHead { id = 1, AccountHeadName = "Liability", AccountHeadDescription = "Liability or Payables", CreatedAt = new DateTime(), CreatedBy = "2", IsActive = true },
                new AccountHead { id = 2, AccountHeadName = "Asset", AccountHeadDescription = "Asset or Receivables or Income", CreatedAt = new DateTime(), CreatedBy = "2", IsActive = true }
                );
        }
    }
}
