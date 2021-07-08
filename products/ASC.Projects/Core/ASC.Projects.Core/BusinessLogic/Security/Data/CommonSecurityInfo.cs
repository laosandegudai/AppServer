//#region License agreement statement

///*
// *
// * (c) Copyright Ascensio System Limited 2010-2018
// *
// * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
// * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
// * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
// * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
// *
// * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
// * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
// *
// * You can contact Ascensio System SIA by email at sales@onlyoffice.com
// *
// * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
// * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
// *
// * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
// * relevant author attributions when distributing the software. If the display of the logo in its graphic 
// * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
// * in every copy of the program you distribute. 
// * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
// *
//*/

//#endregion License agreement statement

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;
//using ASC.Common;
//using ASC.Projects.Core.Data.Domain.Enums;
//using Microsoft.CodeAnalysis;

//namespace ASC.Projects.Core.BusinessLogic.Security.Data
//{
//    [DataContract(Name = "common_security", Namespace = "")]
//    public class CommonSecurityInfo
//    {
//        [DataMember]
//        public bool CanCreateProject { get; set; }

//        [DataMember]
//        public bool CanCreateTask { get; set; }

//        [DataMember]
//        public bool CanCreateMilestone { get; set; }

//        [DataMember]
//        public bool CanCreateMessage { get; set; }

//        [DataMember]
//        public bool CanCreateTimeSpend { get; set; }

//        // ToDo: impelment this later.
//        //public CommonSecurityInfo()
//        //{
//        //    var filter = new TaskFilter
//        //    {
//        //        SortBy = "title",
//        //        SortOrder = true,
//        //        ProjectStatuses = new List<ProjectStatus> { ProjectStatus.Open }
//        //    };

//        //    using (var scope = DIHelper.Resolve())
//        //    {
//        //        var projectSecurity = scope.Resolve<ProjectSecurity>();
//        //        var engineFactory = scope.Resolve<EngineFactory>();
//        //        var projects = engineFactory.ProjectEngine.GetByFilter(filter).ToList();

//        //        CanCreateProject = projectSecurity.CanCreate<Project>(null);
//        //        CanCreateTask = projects.Any(projectSecurity.CanCreate<Task>);
//        //        CanCreateMilestone = projects.Any(projectSecurity.CanCreate<Milestone>);
//        //        CanCreateMessage = projects.Any(projectSecurity.CanCreate<Message>);
//        //        CanCreateTimeSpend = projects.Any(projectSecurity.CanCreate<TimeSpend>);
//        //    }
//        //}

//    }
//}
