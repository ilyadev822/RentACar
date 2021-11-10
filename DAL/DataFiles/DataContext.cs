using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataFiles
{
  public partial class DataContext : DbContext
    {
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=RentACar;Integrated Security=True");
            }
        }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarModel> CarModels { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");
            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasIndex(car => car.CarLicense)
                  .IsUnique(true);


                entity.HasOne(c => c.BranchNavigation)
                    .WithMany(b => b.Cars)
                    .HasPrincipalKey(b => b.BranchID)
                    .HasForeignKey(c => c.BranchID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Cars_Branches");
                entity.HasOne(car => car.CarModelNavigation)
                   .WithMany(CarModel => CarModel.Cars)
                   .HasPrincipalKey(CarModel => CarModel.CarModelID)
                   .HasForeignKey(car => car.CarModelID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Cars_CarModels");
            });

            modelBuilder.Entity<CarModel>(entity =>
            {
                entity.HasIndex(carModel => new { carModel.CarModelName, carModel.CarVendor,carModel.YearOfManufacture })
                  .IsUnique(true);
            });

            modelBuilder.Entity<Order>(entity =>
            {

                entity.HasOne(o => o.CarNavigation)
                 .WithMany(c => c.Orders)
                 .HasPrincipalKey(c => c.CarLicense)
                 .HasForeignKey(o => o.CarLicense)
                 .OnDelete(DeleteBehavior.Restrict)
                 .HasConstraintName("fk_orders_carsLisence");

                entity.HasOne(o => o.CarNavigation)
                .WithMany(c => c.Orders)
                .HasPrincipalKey(c => c.CarID)
                .HasForeignKey(o => o.CarID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_CarID");



                entity.HasOne(o => o.UserNavigation)
                   .WithMany(u => u.Orders)
                   .HasPrincipalKey(u => u.UserID)
                   .HasForeignKey(o => o.UserID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Orders_Users");

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(user => user.Tz)
                   .IsUnique(true);
                entity.Property(p => p.BirthDate).IsRequired(false);
            });

        }

        }
}
