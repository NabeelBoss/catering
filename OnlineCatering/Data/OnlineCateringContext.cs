using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlineCatering.Models;

namespace OnlineCatering.Data
{
    public partial class OnlineCateringContext : DbContext
    {
        public OnlineCateringContext()
        {
        }

        public OnlineCateringContext(DbContextOptions<OnlineCateringContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<CardInfo> CardInfos { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;
        public virtual DbSet<Venue> Venues { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.BookingDate).HasColumnType("date");

                entity.HasOne(d => d.BookingCatererNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingCaterer)
                    .HasConstraintName("FK__Booking__Booking__571DF1D5");

                entity.HasOne(d => d.BookingMenuNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingMenu)
                    .HasConstraintName("FK__Booking__Booking__59063A47");

                entity.HasOne(d => d.BookingVenueNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingVenue)
                    .HasConstraintName("FK__Booking__Booking__5812160E");
            });

            modelBuilder.Entity<CardInfo>(entity =>
            {
                entity.HasKey(e => e.CardId)
                    .HasName("PK__CardInfo__55FECD8E8611E81C");

                entity.ToTable("CardInfo");

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardCvv).HasColumnName("CardCVV");

                entity.Property(e => e.CardExMm).HasColumnName("CardEx_MM");

                entity.Property(e => e.CardExYy).HasColumnName("CardEx_YY");

                entity.Property(e => e.CardName)
                    .HasMaxLength(55)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryImage).IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(55)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.MenuDescription)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.MenuImage).IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.HasOne(d => d.CatererMenuNavigation)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.CatererMenu)
                    .HasConstraintName("FK__Menu__CatererMen__534D60F1");

                entity.HasOne(d => d.MenuCatNavigation)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.MenuCat)
                    .HasConstraintName("FK__Menu__MenuCat__5441852A");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__1788CC4CD70884CC");

                entity.ToTable("UserInfo");

                entity.HasIndex(e => e.UserEmail, "UQ__UserInfo__08638DF896A14269")
                    .IsUnique();

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.UserImage).IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole)
                    .HasMaxLength(55)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.ToTable("Venue");

                entity.Property(e => e.VenueId).HasColumnName("VenueID");

                entity.Property(e => e.VenueName)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.HasOne(d => d.CatererVenueNavigation)
                    .WithMany(p => p.Venues)
                    .HasForeignKey(d => d.CatererVenue)
                    .HasConstraintName("FK__Venue__CatererVe__5070F446");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
