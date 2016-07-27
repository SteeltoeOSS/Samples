
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System;
using System.Data.Entity;

namespace MusicStoreUI.Models
{
    public class ApplicationUser : IdentityUser {
   
        public virtual string NormalizedUserName { get; set; }

        public virtual string NormalizedEmail { get; set; }

        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole() : base()
        {

        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public virtual string NormalizedName { get; set; }
    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class AccountsContext : IdentityDbContext<ApplicationUser>
    {
        public AccountsContext(string connection) : base(connection)
        {

        }
    }

    //public class AccountsContext :
    //    BaseIdentityContext<ApplicationUser, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    //{
    //    private AccountsEF6Context Context;
    //    public AccountsContext(DbContextOptions options, AccountsEF6Context del)
    //        : base(options)
    //    {
    //        Context = del;
    //    }
    //}
    //public class BaseIdentityContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : DbContext
    //   where TUser : IdentityUser<TKey, TUserClaim, TUserRole, TUserLogin>
    //   where TRole : IdentityRole<TKey, TUserRole, TRoleClaim>
    //   where TKey : IEquatable<TKey>
    //   where TUserClaim : IdentityUserClaim<TKey>
    //   where TUserRole : IdentityUserRole<TKey>
    //   where TUserLogin : IdentityUserLogin<TKey>
    //   where TRoleClaim : IdentityRoleClaim<TKey>
    //   where TUserToken : IdentityUserToken<TKey>
    //{
    //    public BaseIdentityContext(DbContextOptions options) : base(options)
    //    {

    //    }
    //    /// <summary>
    //    /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
    //    /// </summary>
    //    public DbSet<TUser> Users { get; set; }

    //    /// <summary>
    //    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
    //    /// </summary>
    //    public DbSet<TUserClaim> UserClaims { get; set; }

    //    /// <summary>
    //    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
    //    /// </summary>
    //    public DbSet<TUserLogin> UserLogins { get; set; }

    //    /// <summary>
    //    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User roles.
    //    /// </summary>
    //    public DbSet<TUserRole> UserRoles { get; set; }

    //    /// <summary>
    //    /// Gets or sets the <see cref="DbSet{TEntity}"/> of User tokens.
    //    /// </summary>
    //    public DbSet<TUserToken> UserTokens { get; set; }

    //    /// <summary>
    //    /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
    //    /// </summary>
    //    public DbSet<TRole> Roles { get; set; }

    //    /// <summary>
    //    /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
    //    /// </summary>
    //    public DbSet<TRoleClaim> RoleClaims { get; set; }

    //    /// <summary>
    //    /// Configures the schema needed for the identity framework.
    //    /// </summary>
    //    /// <param name="builder">
    //    /// The builder being used to construct the model for this context.
    //    /// </param>
    //    //protected override void OnModelCreating(DbModelBuilder builder)
    //    //{

    //    //    builder.Entity<TUser>(b =>
    //    //    {
    //    //        b.HasKey(u => u.Id);
    //    //        b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
    //    //        b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
    //    //        b.ToTable("AspNetUsers");
    //    //        b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

    //    //        b.Property(u => u.UserName).HasMaxLength(256);
    //    //        b.Property(u => u.NormalizedUserName).HasMaxLength(256);
    //    //        b.Property(u => u.Email).HasMaxLength(256);
    //    //        b.Property(u => u.NormalizedEmail).HasMaxLength(256);
    //    //        b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
    //    //        b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
    //    //        b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
    //    //    });

    //    //    builder.Entity<TRole>(b =>
    //    //    {
    //    //        b.HasKey(r => r.Id);
    //    //        b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex");
    //    //        b.ToTable("AspNetRoles");
    //    //        b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

    //    //        b.Property(u => u.Name).HasMaxLength(256);
    //    //        b.Property(u => u.NormalizedName).HasMaxLength(256);

    //    //        b.HasMany(r => r.Users).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
    //    //        b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
    //    //    });

    //    //    builder.Entity<TUserClaim>(b =>
    //    //    {
    //    //        b.HasKey(uc => uc.Id);
    //    //        b.ToTable("AspNetUserClaims");
    //    //    });

    //    //    builder.Entity<TRoleClaim>(b =>
    //    //    {
    //    //        b.HasKey(rc => rc.Id);
    //    //        b.ToTable("AspNetRoleClaims");
    //    //    });

    //    //    builder.Entity<TUserRole>(b =>
    //    //    {
    //    //        b.HasKey(r => new { r.UserId, r.RoleId });
    //    //        b.ToTable("AspNetUserRoles");
    //    //    });

    //    //    builder.Entity<TUserLogin>(b =>
    //    //    {
    //    //        b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
    //    //        b.ToTable("AspNetUserLogins");
    //    //    });

    //    //    builder.Entity<TUserToken>(b =>
    //    //    {
    //    //        b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });
    //    //        b.ToTable("AspNetUserTokens");
    //    //    });
    //}


}
