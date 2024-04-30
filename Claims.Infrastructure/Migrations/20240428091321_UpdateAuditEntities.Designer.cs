﻿// <auto-generated />
using System;
using Claims.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Claims.Infrastructure.Migrations
{
    [DbContext(typeof(AuditContext))]
    [Migration("20240428091321_UpdateAuditEntities")]
    partial class UpdateAuditEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Claims.Infrastructure.Persistence.Entities.ClaimAuditEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("ClaimId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("HttpRequestType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ClaimAudits");
                });

            modelBuilder.Entity("Claims.Infrastructure.Persistence.Entities.CoverAuditEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CoverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("HttpRequestType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CoverAudits");
                });
#pragma warning restore 612, 618
        }
    }
}
