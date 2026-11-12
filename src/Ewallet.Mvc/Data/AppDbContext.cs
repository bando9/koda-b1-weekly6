using Ewallet.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Mvc.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>(); 
    public DbSet<PaymentMethod> PaymentMethod => Set<PaymentMethod>(); 
    public DbSet<TopUpRequest> TopUpRequest => Set<TopUpRequest>();
    public DbSet<Transaction> Transaction => Set<Transaction>();
    public DbSet<User> User => Set<User>();
    public DbSet<Wallet> Wallet => Set<Wallet>();
    public DbSet<WalletMutation> WalletMutation => Set<WalletMutation>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Users_Status", "[Status] IN ('ACTIVE', 'SUSPENDED')");
            });

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).UseIdentityColumn();

            entity.Property(u => u.Username).HasMaxLength(50).IsRequired();
            entity.HasIndex(u => u.Username).IsUnique();
            
            entity.Property(u => u.FullName).HasMaxLength(100).IsRequired();
            
            entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.Property(u => u.Phone).HasMaxLength(20).IsRequired();
            entity.HasIndex(u => u.Phone).IsUnique();

            entity.Property(u => u.PasswordHash).HasMaxLength(500);
            
            entity.Property(u => u.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            
            entity.Property(u => u.CreatedAtUtc).HasColumnType("DATETIME2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
            entity.Property(u => u.UpdatedAtUtc).HasColumnType("DATETIME2").IsRequired(false);

        });
    
        // Wallets
        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.ToTable("Wallets", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Wallets_Balance", "[Balance] >= 0");
                table.HasCheckConstraint("CK_Wallets_Status", "[Status] IN ('ACTIVE', 'FROZEN', 'CLOSED')");
            });

            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id).UseIdentityColumn();

            entity.Property(w => w.WalletNumber).HasMaxLength(30).IsRequired();
            entity.HasIndex(w => w.WalletNumber).IsUnique();

            entity.Property(w => w.Balance).HasPrecision(18,2).IsRequired();

            entity.Property(w => w.Status).HasConversion<string>().HasMaxLength(20).IsRequired();

            entity.Property(w => w.RowVersion).IsRowVersion().IsRequired();

            entity.Property(w => w.CreatedAtUtc).HasColumnType("DATETIME2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
            entity.Property(w => w.UpdatedAtUtc).HasColumnType("DATETIME2").IsRequired(false);

            entity.HasOne(w => w.User).WithOne(u => u.Wallet).HasForeignKey<Wallet>(u=>u.UserId).IsRequired();
        });

        // Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transactions", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Transactions_Amount", "[Amount] > 0");
                table.HasCheckConstraint("CK_Transactions_Status", "[Type] IN ('TOPUP', 'TRANSFER')");
                table.HasCheckConstraint("CK_Transactions_Type", "[Status] IN ('PENDING', 'SUCCESS', 'FAILED')");
            });

            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).UseIdentityColumn();

            entity.Property(t => t.ReferenceNo).HasMaxLength(50).IsRequired();
            entity.HasIndex(t => t.ReferenceNo).IsUnique();

            entity.Property(t => t.SenderWalletId).IsRequired(false);
            entity.Property(t => t.ReceiverWalletId).IsRequired();

            entity.Property(t => t.Type).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(t => t.Amount).HasPrecision(18,2).IsRequired();
            entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(t => t.Description).HasMaxLength(255).IsRequired(false);
            
            entity.Property(t => t.CreatedAtUtc).HasColumnType("DATETIME2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
            entity.Property(t => t.CompletedAtUtc).HasColumnType("DATETIME2").IsRequired(false);

            entity.HasIndex(t => new{t.SenderWalletId,t.CreatedAtUtc});
            entity.HasIndex(t => new{t.ReceiverWalletId,t.CreatedAtUtc});


            entity.HasOne(t => t.SenderWallet)
                .WithMany()
                .HasForeignKey(t => t.SenderWalletId)
                // .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(t => t.ReceiverWallet)
                .WithMany()
                .HasForeignKey(t => t.ReceiverWalletId)
                // .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        });


    }
}