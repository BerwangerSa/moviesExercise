﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistencia.Repositorio;

namespace Persistencia.Migrations
{
    [DbContext(typeof(MovieContext))]
    [Migration("20210830230323_bd com casting")]
    partial class bdcomcasting
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Persistencia.Entidades.Actor", b =>
                {
                    b.Property<int>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActorId");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("Persistencia.Entidades.ActorMovie", b =>
                {
                    b.Property<int>("ActorMovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActorId")
                        .HasColumnType("int");

                    b.Property<string>("Character")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.HasKey("ActorMovieId");

                    b.HasIndex("ActorId");

                    b.HasIndex("MovieId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Persistencia.Entidades.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Persistencia.Entidades.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Director")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenreID")
                        .HasColumnType("int");

                    b.Property<decimal>("Gross")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MovieId");

                    b.HasIndex("GenreID");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Persistencia.Entidades.ActorMovie", b =>
                {
                    b.HasOne("Persistencia.Entidades.Actor", "Actor")
                        .WithMany("Characters")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Persistencia.Entidades.Movie", "Movie")
                        .WithMany("Characters")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Persistencia.Entidades.Movie", b =>
                {
                    b.HasOne("Persistencia.Entidades.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Persistencia.Entidades.Actor", b =>
                {
                    b.Navigation("Characters");
                });

            modelBuilder.Entity("Persistencia.Entidades.Genre", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("Persistencia.Entidades.Movie", b =>
                {
                    b.Navigation("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}
