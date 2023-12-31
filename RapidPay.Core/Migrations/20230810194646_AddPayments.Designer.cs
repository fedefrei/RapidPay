﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RapidPay.Core;

#nullable disable

namespace RapidPay.Core.Migrations
{
    [DbContext(typeof(RapidPayContext))]
    [Migration("20230810194646_AddPayments")]
    partial class AddPayments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RapidPay.Core.Entities.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("CCV")
                        .HasColumnType("int");

                    b.Property<double>("CardLimit")
                        .HasColumnType("float");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateLastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameOnCard")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ValidUntilMonth")
                        .HasColumnType("int");

                    b.Property<int>("ValidUntilYear")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("RapidPay.Core.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateLastUpdate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
