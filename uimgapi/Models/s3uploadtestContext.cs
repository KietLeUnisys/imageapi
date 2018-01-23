using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace uimgapi.Models
{
    public partial class s3uploadtestContext : DbContext
    {
        public s3uploadtestContext(DbContextOptions<s3uploadtestContext> options)
        : base(options)
        { }

        public virtual DbSet<AwsS3> AwsS3 { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AwsS3>(entity =>
            {
                entity.ToTable("aws_s3");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApprovalDecisionNotes)
                    .HasColumnName("approval_decision_notes")
                    .HasMaxLength(120);

                entity.Property(e => e.ApprovalStatus)
                    .HasColumnName("approval_status")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.EmailStatus)
                    .HasColumnName("email_status")
                    .HasMaxLength(45);

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasMaxLength(45);

                entity.Property(e => e.ImageLink)
                    .HasColumnName("image_link")
                    .HasMaxLength(60);

                entity.Property(e => e.Name).HasMaxLength(60);

                entity.Property(e => e.UniqueCode)
                    .HasColumnName("unique_code")
                    .HasMaxLength(45);

                entity.Property(e => e.UploadedDate)
                    .HasColumnName("uploaded_date")
                    .HasMaxLength(45);
            });
        }
    }
}
