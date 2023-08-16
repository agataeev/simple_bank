using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Models.Entities;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<User> User { get; set; }

    public DbSet<Account> Account { get; set; }

    public DbSet<RefreshToken> RefreshToken { get; set; }

    public DbSet<Currency> Currency { get; set; }
    public DbSet<UserRight> UserRights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Username)
                .HasColumnName("username");
            entity.Property(e => e.Pin)
                .HasColumnName("pin");
            entity.Property(e => e.Rights)
                .HasColumnName("rights");
            entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");
        });


        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("account");
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasColumnName("type");
            entity.Property(e => e.Balance)
                .HasColumnName("balance");
            entity.Property(e => e.Number)
                .HasColumnName("number");
            entity.Property(e => e.Active)
                .HasColumnName("active");
            entity.Property(e => e.Currency)
                .HasColumnName("currency");
            entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");
        });


        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_token");
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Value)
                .HasColumnName("value");
            entity.Property(e => e.Created)
                .HasColumnName("created");
            entity.Property(e => e.Revoked)
                .HasColumnName("revoked");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.AccessToken)
                .HasColumnName("access_token");
            entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("currency");
            entity.HasNoKey();
            entity.Property(e => e.Code)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserRight>(entity =>
        {
            entity.ToTable("user_right");
            entity.HasNoKey();
            entity.Property(e => e.Code)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });
    }
}