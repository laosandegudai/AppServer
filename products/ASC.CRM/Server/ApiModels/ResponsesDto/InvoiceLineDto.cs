﻿/*
 *
 * (c) Copyright Ascensio System Limited 2010-2018
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/



using ASC.Common.Mapping;
using ASC.CRM.Core.Entities;

namespace ASC.CRM.ApiModels
{
    /// <summary>
    ///  Invoice Line
    /// </summary>
    public class InvoiceLineDto : IMapFrom<InvoiceLine>
    {
        public int Id { get; set; }
        public int InvoiceID { get; set; }
        public int InvoiceItemID { get; set; }
        public int InvoiceTax1ID { get; set; }
        public int InvoiceTax2ID { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public static InvoiceLineDto GetSample()
        {
            return new InvoiceLineDto
            {
                Description = string.Empty,
                Discount = (decimal)0.00,
                InvoiceID = 0,
                InvoiceItemID = 0,
                InvoiceTax1ID = 0,
                InvoiceTax2ID = 0,
                Price = (decimal)0.00,
                Quantity = 0
            };
        }
    }
}
