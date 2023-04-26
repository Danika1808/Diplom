using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebVer.Configurations;
using WebVer.Domain.Blockchain;
using WebVer.Domain.Documents;
using WebVer.Domain.Identity;
using WebVer.Domain.VerificationCenter;

namespace WebVer;

public sealed class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>,
    UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        var autoMigrate = configuration.GetValue<bool>("AutoMigrate");
        if(autoMigrate)
            Database.Migrate();
    }

    public DbSet<CertificateInfo> CertificateInfo { get; set; }
    
    public DbSet<Document> Documents { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<AppointSingerDocument> AppointSingerDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        
        modelBuilder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
    }
}