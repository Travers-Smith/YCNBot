using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;

namespace YCNBot.Data;

public partial class YCNBotContext : DbContext
{
    public YCNBotContext()
    {
    }

    public YCNBotContext(DbContextOptions<YCNBotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<CommunityPrompt> CommunityPrompts { get; set; }

    public virtual DbSet<CommunityPromptComment> CommunityPromptComments { get; set; }

    public virtual DbSet<CommunityPromptLike> CommunityPromptLikes { get; set; }

    public virtual DbSet<FeedbackType> FeedbackTypes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<UserAgreedTerm> UserAgreedTerms { get; set; }

    public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3214EC07319C749B");

            entity.ToTable("Chat");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CommunityPrompt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Communit__3214EC07F3527F9B");

            entity.ToTable("CommunityPrompt");

            entity.Property(e => e.DateAdded).HasColumnType("datetime");
        });

        modelBuilder.Entity<CommunityPromptComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Communit__3214EC07AC7B38C8");

            entity.ToTable("CommunityPromptComment");

            entity.HasOne(d => d.CommunityPrompt).WithMany(p => p.CommunityPromptComments)
                .HasForeignKey(d => d.CommunityPromptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommunityPrompt_CommunityQuestionComments");
        });

        modelBuilder.Entity<CommunityPromptLike>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Communit__3214EC077CFC0BA3");

            entity.HasOne(d => d.CommunityPrompt).WithMany(p => p.CommunityPromptLikes)
                .HasForeignKey(d => d.CommunityPromptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommunityPrompt_CommunityPromptLikes");
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

            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.UniqueIdentifier).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_Message");
        });

        modelBuilder.Entity<UserAgreedTerm>(entity =>
        {
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
