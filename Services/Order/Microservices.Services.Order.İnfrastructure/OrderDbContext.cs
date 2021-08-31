using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.Order.İnfrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";


        public OrderDbContext(DbContextOptions<OrderDbContext> opts):base(opts)
        {
            
        }


        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

        //ADDRESS BİR VALUEOBJECT. BIR OWNED TYPE OLACAĞI İÇİN BURADA TANIMLAMADIK. 
        //DBD BİR TABLO OLARAK KARŞILIĞI YOK.


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);

            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(x=>x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(x => x.Address).WithOwner();
        }
    }
}