﻿// <auto-generated />
using ASC.Core.Common.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ASC.Core.Common.Migrations.Npgsql.FilesDbContextNpgsql
{
    [DbContext(typeof(PostgreSqlFilesDbContext))]
    partial class FilesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ASC.Core.Common.EF.Model.FilesConverts", b =>
                {
                    b.Property<string>("Input")
                        .HasColumnName("input")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Ouput")
                        .HasColumnName("output")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Input", "Ouput")
                        .HasName("files_converts_pkey");

                    b.ToTable("files_converts","onlyoffice");

                    b.HasData(
                        new
                        {
                            Input = ".csv",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".csv",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".csv",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".doc",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".doc",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".doc",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".doc",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".doc",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".docm",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".docm",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".docm",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".docm",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".docm",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".doct",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".docx",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".docx",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".docx",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".docx",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".dot",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".dot",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".dot",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".dot",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".dot",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".dotm",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".dotm",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".dotm",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".dotm",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".dotm",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".dotx",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".dotx",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".dotx",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".dotx",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".dotx",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".epub",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".epub",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".epub",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".epub",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".epub",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".fb2",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".fb2",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".fb2",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".fb2",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".fb2",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".fodp",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".fodp",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".fodp",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".fods",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".fods",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".fods",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".fods",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".fodt",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".fodt",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".fodt",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".fodt",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".fodt",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".html",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".html",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".html",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".html",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".html",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".mht",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".mht",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".mht",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".mht",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".mht",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".odp",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".odp",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".otp",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".otp",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".otp",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".ods",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".ods",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".ods",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".ots",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".ots",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".ots",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".ots",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".odt",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".odt",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".odt",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".odt",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".ott",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".ott",
                            Ouput = ".odt"
                        },
                        new
                        {
                            Input = ".ott",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".ott",
                            Ouput = ".rtf"
                        },
                        new
                        {
                            Input = ".ott",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".pot",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".pot",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".pot",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".potm",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".potm",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".potm",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".potx",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".potx",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".potx",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".pps",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".pps",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".pps",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".ppsm",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".ppsm",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".ppsm",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".ppsx",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".ppsx",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".ppsx",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".ppt",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".ppt",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".ppt",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".pptm",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".pptm",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".pptm",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".pptt",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".pptt",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".pptt",
                            Ouput = ".pptx"
                        },
                        new
                        {
                            Input = ".pptx",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".pptx",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".rtf",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".rtf",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".rtf",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".rtf",
                            Ouput = ".txt"
                        },
                        new
                        {
                            Input = ".txt",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".txt",
                            Ouput = ".docx"
                        },
                        new
                        {
                            Input = ".txt",
                            Ouput = ".odp"
                        },
                        new
                        {
                            Input = ".txt",
                            Ouput = ".rtx"
                        },
                        new
                        {
                            Input = ".xls",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".xls",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".xls",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".xls",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".xlsm",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".xlsm",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".xlsm",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".xlsm",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".xlst",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".xlst",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".xlst",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".xlst",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".xlt",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".xlt",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".xlt",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".xlt",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".xltm",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".xltm",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".xltm",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".xltm",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".xltx",
                            Ouput = ".pdf"
                        },
                        new
                        {
                            Input = ".xltx",
                            Ouput = ".csv"
                        },
                        new
                        {
                            Input = ".xltx",
                            Ouput = ".ods"
                        },
                        new
                        {
                            Input = ".xltx",
                            Ouput = ".xlsx"
                        },
                        new
                        {
                            Input = ".xps",
                            Ouput = ".pdf"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
