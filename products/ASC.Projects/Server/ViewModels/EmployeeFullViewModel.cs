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
using System.Collections.Generic;
using System.Runtime.Serialization;
using ASC.Core.Users;
using ASC.Web.Api.Models;

namespace ASC.Projects.ViewModels
{
    [DataContract(Name = "person", Namespace = "")]
    public class EmployeeFullViewModel : EmployeeViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public string Sex { get; set; }

        public EmployeeStatus Status { get; set; }

        public EmployeeActivationStatus ActivationStatus { get; set; }

        public DateTime Terminated { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime WorkFrom { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<GroupWrapperSummary> Groups { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Location { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Notes { get; set; }

        public string AvatarMedium { get; set; }

        public string Avatar { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsLDAP { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<string> ListAdminModules { get; set; }

        public bool IsOwner { get; set; }

        public bool IsVisitor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CultureName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        protected string MobilePhone { get; set; }

        [DataMember(EmitDefaultValue = false)]
        protected MobilePhoneActivationStatus MobilePhoneActivationStatus { get; set; }

        public bool IsSSO { get; set; }
    }
}
