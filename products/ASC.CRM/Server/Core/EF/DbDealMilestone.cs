﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ASC.CRM.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASC.CRM.Core.EF
{
    [Table("crm_deal_milestone")]
    public partial class DbDealMilestone : IDbCrm
    {
        [Key]
        [Column("id", TypeName = "int(10)")]
        public int Id { get; set; }
        
        [Required]
        [Column("color", TypeName = "varchar(50)")]
        public string Color { get; set; }
        
        [Column("sort_order", TypeName = "int(10)")]
        public int SortOrder { get; set; }
        
        [Required]
        [Column("title", TypeName = "varchar(250)")]
        public string Title { get; set; }
        
        [Column("description", TypeName = "text")]
        public string Description { get; set; }
        
        [Column("probability", TypeName = "int(10)")]
        public int Probability { get; set; }
        
        [Column("status", TypeName = "int(10)")]
        public DealMilestoneStatus Status { get; set; }
                
        [Column("tenant_id", TypeName = "int(10)")]
        public int TenantId { get; set; }
    }
}