﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoListApp.WebApi.Data;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    [Migration("20240616090718_AddTodoListUsersRoles")]
    partial class AddTodoListUsersRoles
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TagEntityTaskItemEntity", b =>
                {
                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.Property<int>("TasksId")
                        .HasColumnType("int");

                    b.HasKey("TagsId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("TagEntityTaskItemEntity");
                });

            modelBuilder.Entity("TodoListApp.WebApi.Data.Entities.TagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("TodoListApp.WebApi.Data.Entities.TaskItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AssignedTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOverDue")
                        .HasColumnType("bit");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TodoListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TodoListApp.WebApi.Data.Entities.TodoListEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("TodoListApp.WebApi.Data.Entities.TodoListsUsersEntity", b =>
                {
                    b.Property<int>("TodoListId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("TodoListId", "UserName");

                    b.ToTable("TodoListsUsers");
                });

            modelBuilder.Entity("TagEntityTaskItemEntity", b =>
                {
                    b.HasOne("TodoListApp.WebApi.Data.Entities.TagEntity", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoListApp.WebApi.Data.Entities.TaskItemEntity", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TodoListApp.WebApi.Data.Entities.TaskItemEntity", b =>
                {
                    b.HasOne("TodoListApp.WebApi.Data.Entities.TodoListEntity", "TodoList")
                        .WithMany("Tasks")
                        .HasForeignKey("TodoListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TodoList");
                });

            modelBuilder.Entity("TodoListApp.WebApi.Data.Entities.TodoListsUsersEntity", b =>
                {
                    b.HasOne("TodoListApp.WebApi.Data.Entities.TodoListEntity", "TodoList")
                        .WithMany("Users")
                        .HasForeignKey("TodoListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TodoList");
                });

            modelBuilder.Entity("TodoListApp.WebApi.Data.Entities.TodoListEntity", b =>
                {
                    b.Navigation("Tasks");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
