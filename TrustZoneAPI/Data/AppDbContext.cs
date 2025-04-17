using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Models;

namespace TrustZoneAPI.Data;

public partial class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessibilityEvent> AccessibilityEvents { get; set; }

    public virtual DbSet<AccessibilityFeature> AccessibilityFeatures { get; set; }

    public virtual DbSet<AichatConversation> AichatConversations { get; set; }

    public virtual DbSet<AichatMessage> AichatMessages { get; set; }

    //public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    //public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    //public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    //public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<BookingType> BookingTypes { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<BranchBooking> BranchBookings { get; set; }

    public virtual DbSet<BranchOpeningHour> BranchOpeningHours { get; set; }

    public virtual DbSet<BranchPhoto> BranchPhotos { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<DisabilityType> DisabilityTypes { get; set; }

    public virtual DbSet<FavoritePlace> FavoritePlaces { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<PlaceFeature> PlaceFeatures { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<SavedPlace> SavedPlaces { get; set; }

    public virtual DbSet<TMessage> TMessages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDisability> UserDisabilities { get; set; }

    public virtual DbSet<UserLocation> UserLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.UseCollation("Arabic_CI_AI");

        modelBuilder.Entity<AccessibilityEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accessib__3214EC070198F051");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.EventName).HasMaxLength(100);
            entity.Property(e => e.OrganizerId).HasMaxLength(450);
            entity.Property(e => e.SpecialFeaturesAvailable).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Branch).WithMany(p => p.AccessibilityEvents)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccessibilityEvents_Branches");

            entity.HasOne(d => d.Organizer).WithMany(p => p.AccessibilityEvents)
                .HasForeignKey(d => d.OrganizerId)
                .HasConstraintName("FK_AccessibilityEvents_Users");
        });

        modelBuilder.Entity<AccessibilityFeature>(entity =>
        {
            entity.HasKey(e => e.FeatureId).HasName("PK__Accessib__82230BC9EBEED709");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FeatureName).HasMaxLength(100);
        });

        modelBuilder.Entity<AichatConversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AIChatCo__3214EC070E6E19D6");

            entity.ToTable("AIChatConversations");

            entity.Property(e => e.ConversationTopic).HasMaxLength(100);
            entity.Property(e => e.Language)
                .HasMaxLength(10)
                .HasDefaultValue("en");
            entity.Property(e => e.LastActivityAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AichatConversations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIChatConversations_Users");
        });

        modelBuilder.Entity<AichatMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AIChatMe__3214EC072CD5E998");

            entity.ToTable("AIChatMessages");

            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Conversation).WithMany(p => p.AichatMessages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("FK_AIChatMessages_Conversations");

            entity.HasOne(d => d.User).WithMany(p => p.AichatMessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AIChatMessages_Users");
        });

        //modelBuilder.Entity<AspNetRole>(entity =>
        //{
        //    entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
        //        .IsUnique()
        //        .HasFilter("([NormalizedName] IS NOT NULL)");

        //    entity.Property(e => e.Name).HasMaxLength(256);
        //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
        //});

        //modelBuilder.Entity<AspNetRoleClaim>(entity =>
        //{
        //    entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

        //    entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        //});

        //modelBuilder.Entity<AspNetUserClaim>(entity =>
        //{
        //    entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

        //    entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims)
        //        .HasForeignKey(d => d.UserId)
        //        .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
        //});

        //modelBuilder.Entity<AspNetUserLogin>(entity =>
        //{
        //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

        //    entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

        //    entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
        //        .HasForeignKey(d => d.UserId)
        //        .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
        //});

        //modelBuilder.Entity<AspNetUserToken>(entity =>
        //{
        //    entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        //    entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens)
        //        .HasForeignKey(d => d.UserId)
        //        .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
        //});

        modelBuilder.Entity<BookingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingT__3214EC074C0015CD");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Branches__3214EC07F1B4D88A");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(200);

            entity.HasOne(d => d.Place).WithMany(p => p.Branches)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branches_Places");
        });

        modelBuilder.Entity<BranchBooking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BranchBo__3214EC07A7DAF350");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.SpecialRequirements).HasMaxLength(500);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.BookingType).WithMany(p => p.BranchBookings)
                .HasForeignKey(d => d.BookingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchBookings_BookingTypes");

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchBookings)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchBookings_Branches");

            entity.HasOne(d => d.User).WithMany(p => p.BranchBookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchBookings_Users");
        });

        modelBuilder.Entity<BranchOpeningHour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BranchOp__3214EC0723840DA8");

            entity.Property(e => e.ClosingTime).HasPrecision(0);
            entity.Property(e => e.OpeningTime).HasPrecision(0);
            entity.Property(e => e.SpecialNotes).HasMaxLength(200);

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchOpeningHours)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchOpeningHours_Branches");
        });

        modelBuilder.Entity<BranchPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BranchPh__3214EC073946367E");

            entity.Property(e => e.Caption).HasMaxLength(200);
            entity.Property(e => e.IsApproved).HasDefaultValue(true);
            entity.Property(e => e.PhotoUrl).HasMaxLength(255);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UploadedByUserId).HasMaxLength(450);

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchPhotos)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchPhotos_Branches");

            entity.HasOne(d => d.UploadedByUser).WithMany(p => p.BranchPhotos)
                .HasForeignKey(d => d.UploadedByUserId)
                .HasConstraintName("FK_BranchPhotos_Users");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC079F32EABD");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC07CACA2BA2");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastMessageAt).HasColumnType("datetime");
            entity.Property(e => e.User1Id).HasMaxLength(450);
            entity.Property(e => e.User2Id).HasMaxLength(450);

            entity.HasOne(d => d.User1).WithMany(p => p.ConversationUser1s)
                .HasForeignKey(d => d.User1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectConversations_User1");

            entity.HasOne(d => d.User2).WithMany(p => p.ConversationUser2s)
                .HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectConversations_User2");
        });

        modelBuilder.Entity<DisabilityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Disabili__3214EC0700CA92FC");

            entity.ToTable("DisabilityType");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<FavoritePlace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favorite__3214EC0703461086");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Branch).WithMany(p => p.FavoritePlaces)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoritePlaces_Branches");

            entity.HasOne(d => d.User).WithMany(p => p.FavoritePlaces)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoritePlaces_AspNetUsers");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Places__3214EC0770C6295B");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Details)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Places)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Places_Categories");
        });

        modelBuilder.Entity<PlaceFeature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlaceFea__3214EC07BB43D1CB");

            entity.HasOne(d => d.Feature).WithMany(p => p.PlaceFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlaceFeatures_AccessibilityFeatures");

            entity.HasOne(d => d.Place).WithMany(p => p.PlaceFeatures)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlaceFeatures_Places");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3214EC07722C547B");

            entity.ToTable("Review");

            entity.Property(e => e.ContentUrl)
                .HasMaxLength(200)
                .HasColumnName("ContentURL");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Branch).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Branches");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_AspNetUsers");
        });

        modelBuilder.Entity<SavedPlace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SavedPla__3214EC07CE4107FC");

            entity.ToTable("SavedPlace");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Branch).WithMany(p => p.SavedPlaces)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SavedPlace_Branches");

            entity.HasOne(d => d.User).WithMany(p => p.SavedPlaces)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SavedPlace_AspNetUsers");
        });

        modelBuilder.Entity<TMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tMessage__3214EC0743D0AF5E");

            entity.ToTable("tMessages");

            entity.Property(e => e.ReadAt).HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasMaxLength(450);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Conversation).WithMany(p => p.TMessages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectMessages_Conversation");

            entity.HasOne(d => d.Sender).WithMany(p => p.TMessages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectMessages_Sender");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id).HasName("PK_AspNetUsers");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            //entity.HasMany(d => d.Roles).WithMany(p => p.Users)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "AspNetUserRole",
            //        r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
            //        l => l.HasOne<User>().WithMany()
            //            .HasForeignKey("UserId")
            //            .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId"),
            //        j =>
            //        {
            //            j.HasKey("UserId", "RoleId");
            //            j.ToTable("AspNetUserRoles");
            //            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
            //        });
        });

        modelBuilder.Entity<UserDisability>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserDisa__3214EC07E688D591");

            entity.ToTable("UserDisability");

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.DisabilityType).WithMany(p => p.UserDisabilities)
                .HasForeignKey(d => d.DisabilityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDisability_DisabilityType");

            entity.HasOne(d => d.User).WithMany(p => p.UserDisabilities)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDisability_AspNetUsers");
        });

        modelBuilder.Entity<UserLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLoca__3214EC07103293FA");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
