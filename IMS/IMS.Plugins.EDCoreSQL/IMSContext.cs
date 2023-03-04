using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSQL
{
    public class IMSContext : DbContext
    {
        public IMSContext(DbContextOptions<IMSContext> options):base(options)
        {

        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInventory> ProductsInventory { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ProductTransaction> ProductTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //fluent API
            modelBuilder.Entity<ProductInventory>().HasKey(pi => new { pi.ProductId,pi.InventoryId});
            
            modelBuilder.Entity<ProductInventory>().HasOne(pi => pi.Product).WithMany(p => p.ProductInventories).HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductInventory>().HasOne(pi => pi.Inventory).WithMany(i => i.ProductInventories).HasForeignKey(pi => pi.InventoryId);


            //seeding data
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory { InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 20 },
                new Inventory { InventoryId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 100 },
                new Inventory { InventoryId = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 70 },
                new Inventory { InventoryId = 4, InventoryName = "Bike Padels", Quantity = 20, Price = 10 }         
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Bicycle", Quantity = 10, Price = 350 },
                new Product { ProductId = 2, ProductName = "Honda Civic", Quantity = 5, Price = 30000 }
            );
            modelBuilder.Entity<ProductInventory>().HasData(
                new ProductInventory { ProductId = 1, InventoryId = 1, InventoryQuantity = 1},  //Seat
                new ProductInventory { ProductId = 1, InventoryId = 2, InventoryQuantity = 1 }, //body
                new ProductInventory { ProductId = 1, InventoryId = 3, InventoryQuantity = 2 }, //wheels 
                new ProductInventory { ProductId = 1, InventoryId = 4, InventoryQuantity = 2 } //pedals

            );
        }
    }
}