using Backend_BankingApp.Model;
using BankingAppBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.DAL
{
    public class BankDbContext : DbContext
    {
          public DbSet<Card> Cards { get; set; }
          public DbSet<User> Users { get; set; }
          public DbSet<Service> Services { get; set; }
          public DbSet<TransactionBuy> TransactionsBuy { get; set; }
          public DbSet<TransactionSupply> TransactionsSupply { get; set; }
          public DbSet<TransactionTransfer> TransactionsTransfer { get; set; }
          public DbSet<RequestModel> RequestsMoney { get; set; }

          public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
          {
          }

          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
               base.OnModelCreating(modelBuilder);

               modelBuilder.Entity<Card>().HasKey(c => c.Id);
               modelBuilder.Entity<User>().HasKey(u => u.Id);
               modelBuilder.Entity<Service>().HasKey(s => s.IdService);
               modelBuilder.Entity<TransactionBuy>().HasKey(t => t.Id);
               modelBuilder.Entity<TransactionSupply>().HasKey(t => t.Id);
               modelBuilder.Entity<TransactionTransfer>().HasKey(t => t.Id);
               modelBuilder.Entity<RequestModel>().HasKey(t => t.Id);


               modelBuilder.Entity<TransactionTransfer>()
                    .HasOne(t => t.Sender)
                    .WithMany()
                    .HasForeignKey(t => t.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionTransfer>()
                    .HasOne(t => t.Card)
                    .WithMany()
                    .HasForeignKey(t => t.CardId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionTransfer>()
                    .HasOne(t => t.Receiver)
                    .WithMany()
                    .HasForeignKey(t => t.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionTransfer>()
                    .Property(t => t.Valute)
                    .IsRequired()
                    .HasMaxLength(10);

               modelBuilder.Entity<TransactionTransfer>()
                    .Property(t => t.Date)
                    .IsRequired();

               modelBuilder.Entity<TransactionBuy>()
                    .HasOne(t => t.Sender)
                    .WithMany()
                    .HasForeignKey(t => t.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionBuy>()
                    .HasOne(t => t.Card)
                    .WithMany()
                    .HasForeignKey(t => t.CardId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionBuy>()
                    .HasOne(t => t.Service)
                    .WithMany()
                    .HasForeignKey(t => t.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionBuy>()
                    .Property(t => t.Valute)
                    .IsRequired()
                    .HasMaxLength(10);

               modelBuilder.Entity<TransactionBuy>()
                    .Property(t => t.Date)
                    .IsRequired();

               modelBuilder.Entity<TransactionSupply>()
                    .HasOne(t => t.Sender)
                    .WithMany()
                    .HasForeignKey(t => t.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionSupply>()
                    .HasOne(t => t.Card)
                    .WithMany()
                    .HasForeignKey(t => t.CardId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TransactionSupply>()
                    .Property(t => t.Valute)
                    .IsRequired()
                    .HasMaxLength(10);

               modelBuilder.Entity<TransactionSupply>()
                    .Property(t => t.Date)
                    .IsRequired();

               modelBuilder.Entity<Card>()
                    .HasOne(c => c.User)
                    .WithMany()
                    .HasForeignKey(c => c.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);

               modelBuilder.Entity<Card>()
                    .Property(c => c.CardNumber)
                    .IsRequired()
                    .HasMaxLength(16);

               modelBuilder.Entity<Card>()
                    .Property(c => c.Valute)
                    .IsRequired()
                    .HasMaxLength(10);

               modelBuilder.Entity<Card>()
                    .Property(c => c.NameHolder)
                    .IsRequired()
                    .HasMaxLength(100);

               modelBuilder.Entity<Card>()
                    .Property(c => c.Type)
                    .IsRequired()
                    .HasMaxLength(50);

               modelBuilder.Entity<RequestModel>()
                    .HasOne(r => r.Sender)
                    .WithMany()
                    .HasForeignKey(r => r.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<RequestModel>()
                    .Property(r => r.Valute)
                    .IsRequired()
                    .HasMaxLength(10);

               modelBuilder.Entity<RequestModel>()
                    .Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(100);

               modelBuilder.Entity<Service>()
                    .Property(s => s.Title)
                    .IsRequired()
                    .HasMaxLength(100);

               modelBuilder.Entity<Service>()
                    .Property(s => s.Subtitle)
                    .HasMaxLength(200);

               modelBuilder.Entity<Service>().HasData(
         new Service
         {
              IdService = "1",
              Title = "APPLE",
              Subtitle = "SHOPPING",
              Emblem = "./assets/images/transactions/apple.png"
         },
         new Service
         {
              IdService = "2",
              Title = "SPOTIFY",
              Subtitle = "ENTERTAIMENT",
              Emblem = "./assets/images/transactions/spotify.png"
         },
         new Service
         {
              IdService = "3",
              Title = "GROCERY",
              Subtitle = "FOOD",
              Emblem = "./assets/images/transactions/grocery.png"
         },
         new Service
         {
              IdService = "4",
              Title = "NETFLIX",
              Subtitle = "ENTERTAIMENT",
              Emblem = "./assets/images/transactions/netflix.png"
         },
         new Service
         {
              IdService = "5",
              Title = "GAMES",
              Subtitle = "ENTERTAIMENT",
              Emblem = "./assets/images/transactions/games.png"
         },
         new Service
         {
              IdService = "6",
              Title = "COMUNAL",
              Subtitle = "SERVICES",
              Emblem = "./assets/images/transactions/comunal.png"
         },
         new Service
         {
              IdService = "7",
              Title = "CAR",
              Subtitle = "AUTOSERVICE",
              Emblem = "./assets/images/transactions/car.png"
         },
         new Service
         {
              IdService = "8",
              Title = "VACATION",
              Subtitle = "TRAVEL",
              Emblem = "./assets/images/transactions/travel-icon.png"
         }
     );

          }
     }
}

