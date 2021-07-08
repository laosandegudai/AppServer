#region License agreement statement

/*
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

#endregion License agreement statement

using System;
using System.Runtime.Serialization;

using ASC.Api.Core;

namespace ASC.Projects.ViewModels
{
    /// <summary>
    /// Base view model containing detailed information about API response item.
    /// </summary>
    public class BaseFullViewModel : BaseViewModel
    {
        /// <summary>
        /// Date when item was created.
        /// </summary>
        public ApiDateTime Created { get; set; }

        /// <summary>
        /// Person, who created an item.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public EmployeeViewModel CreatedBy { get; set; }

        /// <summary>
        /// Id of person who created an item.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Guid CreatedById { get; set; }

        /// <summary>
        /// Date when item was updated.
        /// </summary>
        private ApiDateTime _updated;

        /// <summary>
        /// Date when item was updated.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public ApiDateTime Updated
        {
            get => _updated < Created
                ? Created
                : _updated;

            set => _updated = value;
        }

        /// <summary>
        /// Person who updated an item.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public EmployeeViewModel UpdatedBy { get; set; }

        /// <summary>
        /// Id of person who updated an item.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Guid UpdatedById { get; set; }
    }
}
