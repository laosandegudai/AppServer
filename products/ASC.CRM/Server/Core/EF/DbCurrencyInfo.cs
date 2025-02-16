﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ASC.Core.Common.EF;
using ASC.Core.Common.EF.Model;

using Microsoft.EntityFrameworkCore;

using Nest;

namespace ASC.CRM.Core.EF
{
    [Table("crm_currency_info")]
    public sealed class DbCurrencyInfo 
    {
        [Required]
        [Column("resource_key", TypeName = "varchar(255)")]
        public string ResourceKey { get; set; }
        
        [Key]
        [Column("abbreviation", TypeName = "varchar(255)")]
        public string Abbreviation { get; set; }
        
        [Required]
        [Column("symbol", TypeName = "varchar(255)")]
        public string Symbol { get; set; }
        
        [Required]
        [Column("culture_name", TypeName = "varchar(255)")]
        public string CultureName { get; set; }
        
        [Column("is_convertable", TypeName = "tinyint(4)")]
        public bool IsConvertable { get; set; }

        [Column("is_basic", TypeName = "tinyint(4)")]
        public bool IsBasic { get; set; }
    }
}