﻿// <auto-generated />
using System;
using DesafioHyperativa.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DesafioHyperativa.Migrations
{
    [DbContext(typeof(ContextDb))]
    [Migration("20230812191121_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DesafioHyperativa.Entities.Cartao", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int8")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("DataRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(1)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<DateTime?>("DataUpdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(2)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<string>("NumeroCartao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NumeroCartao")
                        .IsUnique();

                    b.ToTable("cartao", (string)null);
                });

            modelBuilder.Entity("DesafioHyperativa.Entities.CartaoLote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int8")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CartaoId")
                        .HasColumnType("int8");

                    b.Property<DateTime?>("DataRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(1)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<DateTime?>("DataUpdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(2)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<long>("LoteId")
                        .HasColumnType("int8");

                    b.HasKey("Id");

                    b.HasIndex("CartaoId");

                    b.HasIndex("LoteId");

                    b.ToTable("cartao_lote", (string)null);
                });

            modelBuilder.Entity("DesafioHyperativa.Entities.Lote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int8")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Data")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<DateTime?>("DataRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(1)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<DateTime?>("DataUpdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(2)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("QuantidadeRegistro")
                        .HasColumnType("integer");

                    b.Property<string>("RegistroLote")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("lote", (string)null);
                });

            modelBuilder.Entity("DesafioHyperativa.Entities.LoteStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int8")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int?>("CodigoErro")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DataRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(1)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<DateTime?>("DataUpdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnOrder(2)
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<string>("Erro")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("File")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.ToTable("lote_status", (string)null);
                });

            modelBuilder.Entity("DesafioHyperativa.Entities.CartaoLote", b =>
                {
                    b.HasOne("DesafioHyperativa.Entities.Cartao", "Cartao")
                        .WithMany("ListCartao")
                        .HasForeignKey("CartaoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DesafioHyperativa.Entities.Lote", "Lote")
                        .WithMany("ListLote")
                        .HasForeignKey("LoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cartao");

                    b.Navigation("Lote");
                });

            modelBuilder.Entity("DesafioHyperativa.Entities.Cartao", b =>
                {
                    b.Navigation("ListCartao");
                });

            modelBuilder.Entity("DesafioHyperativa.Entities.Lote", b =>
                {
                    b.Navigation("ListLote");
                });
#pragma warning restore 612, 618
        }
    }
}
