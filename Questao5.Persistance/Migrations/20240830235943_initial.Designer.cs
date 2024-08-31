﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Questao5.Persistance.Data;

namespace Questao5.Persistance.Migrations
{
    [DbContext(typeof(Questao5Context))]
    [Migration("20240830235943_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("Questao5.Domain.Entites.ContaCorrente", b =>
                {
                    b.Property<Guid>("IdContaCorrente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Numero")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdContaCorrente");

                    b.ToTable("ContaCorrente");
                });

            modelBuilder.Entity("Questao5.Domain.Entites.IdEmpotencia", b =>
                {
                    b.Property<Guid>("ChaveIdempotencia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Requisicao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Resultado")
                        .HasColumnType("TEXT");

                    b.HasKey("ChaveIdempotencia");

                    b.ToTable("IdEmpotencia");
                });

            modelBuilder.Entity("Questao5.Domain.Entites.Movimento", b =>
                {
                    b.Property<Guid>("IdMovimento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataMovimento")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("IdContaCorrente")
                        .HasColumnType("TEXT");

                    b.Property<char>("TipoMovimento")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT");

                    b.HasKey("IdMovimento");

                    b.HasIndex("IdContaCorrente");

                    b.ToTable("Movimento");
                });

            modelBuilder.Entity("Questao5.Domain.Entites.Movimento", b =>
                {
                    b.HasOne("Questao5.Domain.Entites.ContaCorrente", "ContaCorrente")
                        .WithMany("Movimentos")
                        .HasForeignKey("IdContaCorrente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContaCorrente");
                });

            modelBuilder.Entity("Questao5.Domain.Entites.ContaCorrente", b =>
                {
                    b.Navigation("Movimentos");
                });
#pragma warning restore 612, 618
        }
    }
}
