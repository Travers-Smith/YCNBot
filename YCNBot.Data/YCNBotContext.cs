using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;

namespace YCNBot.Data;

public partial class YCNBotContext : DbContext
{
    public YCNBotContext(DbContextOptions<YCNBotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<FeedbackType> FeedbackTypes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<UserAgreedTerms> UserAgreedTerms { get; set; }
    public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3214EC07319C749B");

            entity.ToTable("Chat");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<FeedbackType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC079EB28446");

            entity.ToTable("FeedbackType");

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

        modelBuilder.Entity<UserFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserFeed__3214EC07A26182F4");

            entity.ToTable("UserFeedback");

            entity.HasOne(d => d.FeedbackType).WithMany(p => p.UserFeedbacks)
                .HasForeignKey(d => d.FeedbackTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeedbackType_UserFeedback");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
