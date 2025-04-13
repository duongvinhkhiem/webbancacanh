using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebBanCaCanh.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            modelBuilder.Entity<Promotion>();
            modelBuilder.Entity<Order>()
                .HasOptional(o => o.User) 
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .WillCascadeOnDelete(false);
  

            modelBuilder.Entity<OrderDetail>()
                .HasRequired(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<OrderDetail>()
                .HasRequired(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<ProductPromotion>()
                .HasRequired(pp => pp.Product)
                .WithMany()
                .HasForeignKey(pp => pp.ProductId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<ProductPromotion>()
                .HasRequired(pp => pp.Promotion)
                .WithMany()
                .HasForeignKey(pp => pp.PromotionId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Banner>();
            modelBuilder.Entity<News>();

            modelBuilder.Entity<ProductImage>()
              .HasRequired(pi => pi.Product)
              .WithMany(p => p.ProductImages)
              .HasForeignKey(pi => pi.ProductId)
              .WillCascadeOnDelete(true);
        }
    }
}