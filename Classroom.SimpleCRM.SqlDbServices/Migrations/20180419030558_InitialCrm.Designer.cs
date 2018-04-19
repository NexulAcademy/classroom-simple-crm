﻿// <auto-generated />
using Classroom.SimpleCRM;
using Classroom.SimpleCRM.SqlDbServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Classroom.SimpleCRM.SqlDbServices.Migrations
{
    [DbContext(typeof(CrmDbContext))]
    [Migration("20180419030558_InitialCrm")]
    partial class InitialCrm
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Classroom.SimpleCRM.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress");

                    b.Property<string>("FirstName");

                    b.Property<DateTime>("LastContactDate");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("PreferredContactMethod");

                    b.Property<int>("Status");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });
#pragma warning restore 612, 618
        }
    }
}