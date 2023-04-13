using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;
using YCNBot.Entities;

namespace YCNBot.Data;

public partial class YCNBotContext : DbContext
{
    public YCNBotContext(DbContextOptions<YCNBotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<UserAgreedTerms> UserAgreedTerms { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3214EC07319C749B");

            entity.ToTable("Chat");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC07474A5C6B");

            entity.ToTable("Message");

            entity.Property(e => e.UniqueIdentifier).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_Message");
        });

        modelBuilder.Entity<UserAgreedTerms>(entity =>
        {
            entity.ToTable("UserAgreedTerms");

            entity.HasKey(e => e.Id).HasName("PK__UserAgre__3214EC07282EF798");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
