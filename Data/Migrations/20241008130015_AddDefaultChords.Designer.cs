﻿// <auto-generated />
using System;
using CDR_Worship.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CDR_Worship.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241008130015_AddDefaultChords")]
    partial class AddDefaultChords
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CDR_Worship.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<byte[]>("ImageFileData")
                        .HasColumnType("bytea");

                    b.Property<string>("ImageFileType")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CDR_Worship.Models.Chord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ChordName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Chords");
                });

            modelBuilder.Entity("CDR_Worship.Models.ChordAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<int>("ChordDocumentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<string>("FileContentType")
                        .HasColumnType("text");

                    b.Property<byte[]>("FileData")
                        .HasColumnType("bytea");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("FileType")
                        .HasColumnType("text");

                    b.Property<string>("MusicName")
                        .HasColumnType("text");

                    b.Property<int?>("SongDocumentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ChordDocumentId");

                    b.HasIndex("SongDocumentId");

                    b.ToTable("ChordAttachments");
                });

            modelBuilder.Entity("CDR_Worship.Models.ChordDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ChordId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PdfData")
                        .HasColumnType("bytea");

                    b.Property<int?>("ScheduledSongId")
                        .HasColumnType("integer");

                    b.Property<int>("SongDocumentId")
                        .HasColumnType("integer");

                    b.Property<string>("SongName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Tempo")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ChordId");

                    b.HasIndex("ScheduledSongId");

                    b.ToTable("ChordDocuments");
                });

            modelBuilder.Entity("CDR_Worship.Models.Instrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("InstrumentName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Instruments");
                });

            modelBuilder.Entity("CDR_Worship.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("MemberName")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("CDR_Worship.Models.ScheduledSong", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BackingVocalistId")
                        .HasColumnType("integer");

                    b.Property<int?>("BackingVocalistTwoId")
                        .HasColumnType("integer");

                    b.Property<int?>("BassistId")
                        .HasColumnType("integer");

                    b.Property<int?>("ChordId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("DrummerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("LeadGuitaristId")
                        .HasColumnType("integer");

                    b.Property<int?>("LeadSingerId")
                        .HasColumnType("integer");

                    b.Property<int?>("MemberId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SecondGuitaristId")
                        .HasColumnType("integer");

                    b.Property<string>("SongName")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BackingVocalistId");

                    b.HasIndex("BackingVocalistTwoId");

                    b.HasIndex("BassistId");

                    b.HasIndex("ChordId");

                    b.HasIndex("DrummerId");

                    b.HasIndex("LeadGuitaristId");

                    b.HasIndex("LeadSingerId");

                    b.HasIndex("MemberId");

                    b.HasIndex("SecondGuitaristId");

                    b.ToTable("ScheduledSongs");
                });

            modelBuilder.Entity("CDR_Worship.Models.SongAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<int?>("ChordDocumentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<string>("FileContentType")
                        .HasColumnType("text");

                    b.Property<byte[]>("FileData")
                        .HasColumnType("bytea");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("FileType")
                        .HasColumnType("text");

                    b.Property<string>("MusicName")
                        .HasColumnType("text");

                    b.Property<int>("SongDocumentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ChordDocumentId");

                    b.HasIndex("SongDocumentId");

                    b.ToTable("SongAttachments");
                });

            modelBuilder.Entity("CDR_Worship.Models.SongDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ChordDocumentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<int?>("ScheduledSongId")
                        .HasColumnType("integer");

                    b.Property<string>("SongName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ScheduledSongId");

                    b.ToTable("SongDocuments");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CDR_Worship.Models.ChordAttachment", b =>
                {
                    b.HasOne("CDR_Worship.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("CDR_Worship.Models.ChordDocument", "ChordDocument")
                        .WithMany("ChordAttachments")
                        .HasForeignKey("ChordDocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CDR_Worship.Models.SongDocument", null)
                        .WithMany("ChordAttachments")
                        .HasForeignKey("SongDocumentId");

                    b.Navigation("AppUser");

                    b.Navigation("ChordDocument");
                });

            modelBuilder.Entity("CDR_Worship.Models.ChordDocument", b =>
                {
                    b.HasOne("CDR_Worship.Models.Chord", "Chord")
                        .WithMany()
                        .HasForeignKey("ChordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CDR_Worship.Models.ScheduledSong", null)
                        .WithMany("ChordDocuments")
                        .HasForeignKey("ScheduledSongId");

                    b.Navigation("Chord");
                });

            modelBuilder.Entity("CDR_Worship.Models.ScheduledSong", b =>
                {
                    b.HasOne("CDR_Worship.Models.Member", "BackingVocalist")
                        .WithMany()
                        .HasForeignKey("BackingVocalistId");

                    b.HasOne("CDR_Worship.Models.Member", "BackingVocalistTwo")
                        .WithMany()
                        .HasForeignKey("BackingVocalistTwoId");

                    b.HasOne("CDR_Worship.Models.Member", "Bassist")
                        .WithMany()
                        .HasForeignKey("BassistId");

                    b.HasOne("CDR_Worship.Models.Chord", "Chord")
                        .WithMany()
                        .HasForeignKey("ChordId");

                    b.HasOne("CDR_Worship.Models.Member", "Drummer")
                        .WithMany()
                        .HasForeignKey("DrummerId");

                    b.HasOne("CDR_Worship.Models.Member", "LeadGuitarist")
                        .WithMany()
                        .HasForeignKey("LeadGuitaristId");

                    b.HasOne("CDR_Worship.Models.Member", "LeadSinger")
                        .WithMany()
                        .HasForeignKey("LeadSingerId");

                    b.HasOne("CDR_Worship.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId");

                    b.HasOne("CDR_Worship.Models.Member", "SecondGuitarist")
                        .WithMany()
                        .HasForeignKey("SecondGuitaristId");

                    b.Navigation("BackingVocalist");

                    b.Navigation("BackingVocalistTwo");

                    b.Navigation("Bassist");

                    b.Navigation("Chord");

                    b.Navigation("Drummer");

                    b.Navigation("LeadGuitarist");

                    b.Navigation("LeadSinger");

                    b.Navigation("Member");

                    b.Navigation("SecondGuitarist");
                });

            modelBuilder.Entity("CDR_Worship.Models.SongAttachment", b =>
                {
                    b.HasOne("CDR_Worship.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("CDR_Worship.Models.ChordDocument", null)
                        .WithMany("SongAttachments")
                        .HasForeignKey("ChordDocumentId");

                    b.HasOne("CDR_Worship.Models.SongDocument", "Song")
                        .WithMany("SongAttachments")
                        .HasForeignKey("SongDocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("CDR_Worship.Models.SongDocument", b =>
                {
                    b.HasOne("CDR_Worship.Models.ScheduledSong", null)
                        .WithMany("Songs")
                        .HasForeignKey("ScheduledSongId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CDR_Worship.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CDR_Worship.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CDR_Worship.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CDR_Worship.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CDR_Worship.Models.ChordDocument", b =>
                {
                    b.Navigation("ChordAttachments");

                    b.Navigation("SongAttachments");
                });

            modelBuilder.Entity("CDR_Worship.Models.ScheduledSong", b =>
                {
                    b.Navigation("ChordDocuments");

                    b.Navigation("Songs");
                });

            modelBuilder.Entity("CDR_Worship.Models.SongDocument", b =>
                {
                    b.Navigation("ChordAttachments");

                    b.Navigation("SongAttachments");
                });
#pragma warning restore 612, 618
        }
    }
}
