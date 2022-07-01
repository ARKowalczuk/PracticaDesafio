﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Practica.Infraestructure.DataAccess;

#nullable disable

namespace Practica.API.Migrations
{
    [DbContext(typeof(PedidosContext))]
    partial class PedidosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Practica.Domain.Entities.Pedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CicloDelPedido")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CodigoDeContratoInterno")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Cuando")
                        .HasColumnType("datetime2");

                    b.Property<long>("CuentaCorriente")
                        .HasColumnType("bigint");

                    b.Property<string>("EstadoDelPedido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("NumeroDePedido")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Pedidos");
                });
#pragma warning restore 612, 618
        }
    }
}
