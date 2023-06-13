using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VillaNumber>()
                .HasOne(x => x.Villa)
                .WithMany()
                .HasForeignKey(x => x.VillaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Villa>().HasData(new Villa
            {
                Id = 1,
                Name = "Royal Villa",
                Details = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes.",
                ImageUrl = "",
                Occupancy = 4,
                Rate = 200,
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.UtcNow
            },
            new Villa
            {
                Id = 2,
                Name = "Premium Pool Villa",
                Details = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes.",
                ImageUrl = "",
                Occupancy = 4,
                Rate = 300,
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.UtcNow
            },
            new Villa
            {
                Id = 3,
                Name = "Luxury Pool Villa",
                Details = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes.",
                ImageUrl = "",
                Occupancy = 4,
                Rate = 400,
                Sqft = 750,
                Amenity = "",
                CreatedDate = DateTime.UtcNow
            }, new Villa
            {
                Id = 4,
                Name = "Diamond Villa",
                Details = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes.",
                ImageUrl = "",
                Occupancy = 4,
                Rate = 500,
                Sqft = 900,
                Amenity = "",
                CreatedDate = DateTime.UtcNow
            },
            new Villa
            {
                Id = 5,
                Name = "Diamond Pool Villa",
                Details = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes.",
                ImageUrl = "",
                Occupancy = 4,
                Rate = 600,
                Sqft = 1100,
                Amenity = "",
                CreatedDate = DateTime.UtcNow
            });
        }
    }
}
