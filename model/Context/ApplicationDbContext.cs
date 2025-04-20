using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace Technical_Challenge.model.Context;

public class ApplicationDbContext : IdentityDbContext<User, UserRole, string>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }



    public DbSet<AddressBook> AddressBooks { get; set; } = null!;
    public DbSet<user_address_book> User_Address_Books { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }




    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
          //  entity.Property(e => e.Email).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity

           .OwnsMany(x => x.RefreshTokens)
           .ToTable("RefreshTokens")
           .WithOwner()
           .HasForeignKey("UserId");

        });
        modelBuilder.Entity<AddressBook>(entity =>
        {
            entity.ToTable("AddressBooks");
            entity.HasKey(e => e.Id);
            
        });
        modelBuilder.Entity<user_address_book>(entity =>
        {
            entity.ToTable("User_Address_Books");
            entity.HasKey(e => new { e.UserId, e.AddressBookId });
            entity.HasOne(e => e.User)
                .WithMany(e => e.User_Address_Books)
                .HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.AddressBook)
                .WithMany(e => e.User_Address_Books)
                .HasForeignKey(e => e.AddressBookId);
        });

        // Configure IdentityUserLogin<TKey> (AspNetUserLogins)
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            entity.ToTable("IdentityUserLogin");

        });

        // Configure IdentityUserRole<TKey> (AspNetUserRoles)
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.ToTable("IdentityUserRole");


        });

        // Configure IdentityUserToken<TKey> (AspNetUserTokens)
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            entity.ToTable("IdentityUserToken");

        });

        // Configure IdentityRoleClaim<TKey> (AspNetRoleClaims)
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id); // Primary key is already defined, but adding explicitly for consistency
            entity.ToTable("IdentityRoleClaim");

        });

        // Configure IdentityUserClaim<TKey> (AspNetUserClaims)
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id); // Primary key is already defined, but adding explicitly for consistency
            entity.ToTable("IdentityUserClaim");

        });

    }

}
