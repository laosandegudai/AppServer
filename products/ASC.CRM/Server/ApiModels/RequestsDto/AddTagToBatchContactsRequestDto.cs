﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ASC.Api.Core;
using ASC.CRM.Core.Enums;

using Microsoft.AspNetCore.Mvc;

namespace ASC.CRM.ApiModels
{
    public class AddTagToBatchContactsRequestDto
    {
        public IEnumerable<string> Tags { get; set; }
        public int ContactStage { get; set; }
        public int ContactType { get; set; }
        public ContactListViewType ContactListView { get; set; }
        public ApiDateTime FromDate { get; set; }
        public ApiDateTime ToDate { get; set; }
        public string TagName { get; set; }
    }
}
