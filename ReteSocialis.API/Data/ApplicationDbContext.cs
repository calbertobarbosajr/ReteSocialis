using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReteSocialis.API.Models;

namespace ReteSocialis.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tabelas
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<UserProfile> UserProfiles { get; set; } = default!;
        public DbSet<Friend> Friends { get; set; } = default!;
        public DbSet<FriendInvitation> FriendInvitations { get; set; } = default!;
        public DbSet<Message> Messages { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuração da tabela Post
            builder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.PostId);
                entity.Property(p => p.Content)
                    .IsRequired()
                    .HasMaxLength(2000);
                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configuração da tabela Friends
            builder.Entity<Friend>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // evita múltiplos caminhos em cascata

                entity.HasOne(f => f.FriendUser)
                    .WithMany()
                    .HasForeignKey(f => f.FriendId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(f => f.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configuração da tabela FriendInvitations
            builder.Entity<FriendInvitation>(entity =>
            {
                entity.HasKey(fi => fi.Id);
                entity.Property(fi => fi.InvitationKey)
                    .IsRequired();
                entity.Property(fi => fi.ReceiverEmail)
                    .IsRequired();
                entity.Property(fi => fi.SentAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configuração da tabela Messages
            builder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Content)
                    .IsRequired()
                    .HasMaxLength(1000);
                entity.Property(m => m.SentAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configuração opcional para UserProfile (1:1 com ApplicationUser usando UserId como PK)
            builder.Entity<UserProfile>(entity =>
            {
                // chave primária é UserId (string)
                entity.HasKey(up => up.UserId);

                // garante compatibilidade com o comprimento do IdentityUser.Id (nvarchar(450))
                entity.Property(up => up.UserId).HasMaxLength(450);

                // relação 1:1 com ApplicationUser
                entity.HasOne(up => up.User)
                    .WithOne(u => u.Profile)
                    .HasForeignKey<UserProfile>(up => up.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // ou Restrict se preferir evitar cascade
            });
        }
    }
}