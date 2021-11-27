﻿// <auto-generated />
using System;
using ASMR.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASMR.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("ASMR.Core.Entities.Bean", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InventoryId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId")
                        .IsUnique();

                    b.ToTable("Beans");
                });

            modelBuilder.Entity("ASMR.Core.Entities.BeanInventory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CurrentGreenBeanWeight")
                        .HasColumnType("decimal(8,3)");

                    b.Property<decimal>("CurrentRoastedBeanWeight")
                        .HasColumnType("decimal(8,3)");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BeanInventories");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Configuration", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Key")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("ASMR.Core.Entities.IncomingGreenBean", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BeanId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("WeightAdded")
                        .HasColumnType("decimal(8,3)");

                    b.HasKey("Id");

                    b.HasIndex("BeanId");

                    b.HasIndex("UserId");

                    b.ToTable("IncomingGreenBeans");
                });

            modelBuilder.Entity("ASMR.Core.Entities.MediaFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("MediaFiles");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Packaging", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BeanId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BeanId");

                    b.HasIndex("UserId");

                    b.ToTable("Packagings");
                });

            modelBuilder.Entity("ASMR.Core.Entities.PackagingResult", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PackagingId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PackagingId");

                    b.HasIndex("ProductId");

                    b.ToTable("PackagingResults");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(22,2)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Method")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BeanId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentInventoryQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(22,2)");

                    b.Property<decimal>("WeightPerPackaging")
                        .HasColumnType("decimal(8,3)");

                    b.HasKey("Id");

                    b.HasIndex("BeanId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ASMR.Core.Entities.RoastingSession", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BeanId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CancellationReason")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("CancelledAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("FinishedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("GreenBeanWeight")
                        .HasColumnType("decimal(8,3)");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("RoastedBeanWeight")
                        .HasColumnType("decimal(8,3)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BeanId");

                    b.HasIndex("UserId");

                    b.ToTable("RoastingSessions");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Transaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("ASMR.Core.Entities.TransactionItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(22,2)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(8,3)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("TransactionId");

                    b.ToTable("TransactionItems");
                });

            modelBuilder.Entity("ASMR.Core.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsWaitingForApproval")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ASMR.Core.Entities.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FriendlyName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Xml")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ASMR.Core.Entities.Bean", b =>
                {
                    b.HasOne("ASMR.Core.Entities.BeanInventory", "Inventory")
                        .WithOne("Bean")
                        .HasForeignKey("ASMR.Core.Entities.Bean", "InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("ASMR.Core.Entities.IncomingGreenBean", b =>
                {
                    b.HasOne("ASMR.Core.Entities.Bean", "Bean")
                        .WithMany("IncomingGreenBeans")
                        .HasForeignKey("BeanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASMR.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bean");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASMR.Core.Entities.MediaFile", b =>
                {
                    b.HasOne("ASMR.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Packaging", b =>
                {
                    b.HasOne("ASMR.Core.Entities.Bean", "Bean")
                        .WithMany()
                        .HasForeignKey("BeanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASMR.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bean");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASMR.Core.Entities.PackagingResult", b =>
                {
                    b.HasOne("ASMR.Core.Entities.Packaging", "Packaging")
                        .WithMany("Results")
                        .HasForeignKey("PackagingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASMR.Core.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Packaging");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Product", b =>
                {
                    b.HasOne("ASMR.Core.Entities.Bean", "Bean")
                        .WithMany("Products")
                        .HasForeignKey("BeanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bean");
                });

            modelBuilder.Entity("ASMR.Core.Entities.RoastingSession", b =>
                {
                    b.HasOne("ASMR.Core.Entities.Bean", "Bean")
                        .WithMany("RoastingSessions")
                        .HasForeignKey("BeanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASMR.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bean");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Transaction", b =>
                {
                    b.HasOne("ASMR.Core.Entities.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASMR.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASMR.Core.Entities.TransactionItem", b =>
                {
                    b.HasOne("ASMR.Core.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASMR.Core.Entities.Transaction", "Transaction")
                        .WithMany("Items")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("ASMR.Core.Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ASMR.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ASMR.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("ASMR.Core.Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASMR.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ASMR.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ASMR.Core.Entities.Bean", b =>
                {
                    b.Navigation("IncomingGreenBeans");

                    b.Navigation("Products");

                    b.Navigation("RoastingSessions");
                });

            modelBuilder.Entity("ASMR.Core.Entities.BeanInventory", b =>
                {
                    b.Navigation("Bean");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Packaging", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("ASMR.Core.Entities.Transaction", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
