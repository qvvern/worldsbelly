using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities
{
    public class Notification : IEntity
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public NotificationTemplate Template { get; set; }
        public int? SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public string ActionUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ReadAt { get; set; }
        public bool IsRead { get; set; }
        public Notification()
        {
            this.CreatedAt = DateTime.UtcNow;
        }
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Notification>().ToTable("Notifications");

            builder.Entity<Notification>()
                .HasOne(c => c.Template)
                .WithMany()
                .HasForeignKey("TemplateId")
                .IsRequired();

            builder.Entity<Notification>()
                .HasOne(c => c.Sender)
                .WithMany()
                .HasForeignKey("SenderId");

            builder.Entity<Notification>()
                .HasOne(c => c.Receiver)
                .WithMany()
                .HasForeignKey("ReceiverId")
                .IsRequired();

            builder.Entity<NotificationTemplateTranslation>().ToTable("NotificationTemplateTranslations");
            builder.Entity<NotificationTemplateTranslation>().HasKey(t => new { t.TemplateId, t.LanguageId });
            builder.Entity<NotificationTemplateTranslation>()
                .HasOne(t => t.Template)
                .WithMany(t => t.Translations)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NotificationType>().ToTable("NotificationTypes");

            builder.Entity<NotificationTemplate>().ToTable("NotificationTemplates");
            builder.Entity<NotificationTemplate>()
                .HasOne(c => c.Type)
                .WithMany()
                .HasForeignKey("TypeId")
                .IsRequired();
        }
    }
    public class NotificationType : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<NotificationTemplate> Templates { get; set; }
    }
    public class NotificationTemplate : IEntity
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public NotificationType Type { get; set; }
        public string EnglishTitle { get; set; }
        public string EnglishContent { get; set; }
        public List<NotificationTemplateTranslation> Translations { get; set; }
    }
    public class NotificationTemplateTranslation
    {
        public int TemplateId { get; set; }
        public NotificationTemplate Template { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
