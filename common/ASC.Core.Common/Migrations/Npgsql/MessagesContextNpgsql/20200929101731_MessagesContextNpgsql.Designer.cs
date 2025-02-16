﻿// <auto-generated />
using System;
using ASC.Core.Common.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ASC.Core.Common.Migrations.Npgsql.MessagesContextNpgsql
{
    [DbContext(typeof(PostgreSqlMessagesContext))]
    [Migration("20200929101731_MessagesContextNpgsql")]
    partial class MessagesContextNpgsql
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ASC.Core.Common.EF.Model.AuditEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Action")
                        .HasColumnName("action")
                        .HasColumnType("integer");

                    b.Property<string>("Browser")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("browser")
                        .HasColumnType("character varying(200)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(200);

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("description")
                        .HasColumnType("character varying(20000)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(20000);

                    b.Property<string>("Initiator")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("initiator")
                        .HasColumnType("character varying(200)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(200);

                    b.Property<string>("Ip")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ip")
                        .HasColumnType("character varying(50)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(50);

                    b.Property<string>("Page")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("page")
                        .HasColumnType("character varying(300)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(300);

                    b.Property<string>("Platform")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("platform")
                        .HasColumnType("character varying(200)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(200);

                    b.Property<string>("Target")
                        .HasColumnName("target")
                        .HasColumnType("text");

                    b.Property<int>("TenantId")
                        .HasColumnName("tenant_id")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("user_id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("NULL")
                        .IsFixedLength(true)
                        .HasMaxLength(38);

                    b.HasKey("Id");

                    b.HasIndex("TenantId", "Date")
                        .HasName("date");

                    b.ToTable("audit_events","onlyoffice");
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.DbTenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnName("alias")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("Calls")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("calls")
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("1");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnName("creationdatetime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("Industry")
                        .HasColumnName("industry")
                        .HasColumnType("integer");

                    b.Property<string>("Language")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("language")
                        .HasColumnType("character(10)")
                        .HasDefaultValueSql("'en-US'")
                        .IsFixedLength(true)
                        .HasMaxLength(10);

                    b.Property<DateTime>("LastModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_modified")
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("MappedDomain")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("mappeddomain")
                        .HasColumnType("character varying(100)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<Guid>("OwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("owner_id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(38);

                    b.Property<string>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("payment_id")
                        .HasColumnType("character varying(38)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(38);

                    b.Property<bool>("Spam")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("spam")
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("1");

                    b.Property<int>("Status")
                        .HasColumnName("status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("StatusChanged")
                        .HasColumnName("statuschanged")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TimeZone")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("timezone")
                        .HasColumnType("character varying(50)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(50);

                    b.Property<string>("TrustedDomains")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("trusteddomains")
                        .HasColumnType("character varying(1024)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(1024);

                    b.Property<int>("TrustedDomainsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("trusteddomainsenabled")
                        .HasColumnType("integer")
                        .HasDefaultValueSql("1");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("version")
                        .HasColumnType("integer")
                        .HasDefaultValueSql("2");

                    b.Property<DateTime>("VersionChanged")
                        .HasColumnName("version_changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("Version_Changed")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .IsUnique()
                        .HasName("alias");

                    b.HasIndex("LastModified")
                        .HasName("last_modified_tenants_tenants");

                    b.HasIndex("MappedDomain")
                        .HasName("mappeddomain");

                    b.HasIndex("Version")
                        .HasName("version");

                    b.ToTable("tenants_tenants","onlyoffice");
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.DbTenantPartner", b =>
                {
                    b.Property<int>("TenantId")
                        .HasColumnName("tenant_id")
                        .HasColumnType("integer");

                    b.Property<string>("AffiliateId")
                        .HasColumnName("affiliate_id")
                        .HasColumnType("text");

                    b.Property<string>("Campaign")
                        .HasColumnName("campaign")
                        .HasColumnType("text");

                    b.Property<string>("PartnerId")
                        .HasColumnName("partner_id")
                        .HasColumnType("text");

                    b.HasKey("TenantId");

                    b.ToTable("tenants_partners");
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.LoginEvents", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Action")
                        .HasColumnName("action")
                        .HasColumnType("integer");

                    b.Property<string>("Browser")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("browser")
                        .HasColumnType("character varying(200)")
                        .HasDefaultValueSql("NULL::character varying")
                        .HasMaxLength(200);

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("description")
                        .HasColumnType("character varying(500)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(500);

                    b.Property<string>("Ip")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ip")
                        .HasColumnType("character varying(50)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(50);

                    b.Property<string>("Login")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("login")
                        .HasColumnType("character varying(200)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(200);

                    b.Property<string>("Page")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("page")
                        .HasColumnType("character varying(300)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(300);

                    b.Property<string>("Platform")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("platform")
                        .HasColumnType("character varying(200)")
                        .HasDefaultValueSql("NULL")
                        .HasMaxLength(200);

                    b.Property<int>("TenantId")
                        .HasColumnName("tenant_id")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("uuid")
                        .IsFixedLength(true)
                        .HasMaxLength(38);

                    b.HasKey("Id");

                    b.HasIndex("Date")
                        .HasName("date_login_events");

                    b.HasIndex("UserId", "TenantId")
                        .HasName("tenant_id_login_events");

                    b.ToTable("login_events","onlyoffice");
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.DbTenantPartner", b =>
                {
                    b.HasOne("ASC.Core.Common.EF.Model.DbTenant", "Tenant")
                        .WithOne("Partner")
                        .HasForeignKey("ASC.Core.Common.EF.Model.DbTenantPartner", "TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
