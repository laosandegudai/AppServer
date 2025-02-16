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


using System.Collections.Generic;

using ASC.Api.Core;
using ASC.Api.Documents;
using ASC.Common;
using ASC.CRM.ApiModels;
using ASC.CRM.Core;
using ASC.CRM.Core.Dao;
using ASC.CRM.Core.Entities;
using ASC.Web.Api.Models;

using AutoMapper;

namespace ASC.CRM.Mapping
{
    [Scope]
    public class RelationshipEventDtoTypeConverter : ITypeConverter<RelationshipEvent, RelationshipEventDto>
    {
        private readonly FileWrapperHelper _fileWrapperHelper;
        private readonly DaoFactory _daoFactory;
        private readonly CrmSecurity _crmSecurity;
        private readonly ApiDateTimeHelper _apiDateTimeHelper;
        private readonly EmployeeWraperHelper _employeeWraperHelper;
        private readonly EntityDtoHelper _entityDtoHelper;

        public RelationshipEventDtoTypeConverter(
                           ApiDateTimeHelper apiDateTimeHelper,
                           EmployeeWraperHelper employeeWraperHelper,
                           FileWrapperHelper fileWrapperHelper,
                           CrmSecurity crmSecurity,
                           DaoFactory daoFactory,
                           EntityDtoHelper entityDtoHelper)
        {
            _apiDateTimeHelper = apiDateTimeHelper;
            _employeeWraperHelper = employeeWraperHelper;
            _crmSecurity = crmSecurity;
            _daoFactory = daoFactory;
            _fileWrapperHelper = fileWrapperHelper;
            _entityDtoHelper = entityDtoHelper;
        }

        public RelationshipEventDto Convert(RelationshipEvent source, RelationshipEventDto destination, ResolutionContext context)
        {
            var result = new RelationshipEventDto
            {
                Id = source.ID,
                CreateBy = _employeeWraperHelper.Get(source.CreateBy),
                Created = _apiDateTimeHelper.Get(source.CreateOn),
                Content = source.Content,
                Files = new List<FileWrapper<int>>(),
                CanEdit = _crmSecurity.CanEdit(source)
            };


            var historyCategory = _daoFactory.GetListItemDao().GetByID(source.CategoryID);

            if (historyCategory != null)
            {
                result.Category = (HistoryCategoryBaseDto)context.Mapper.Map<HistoryCategoryDto>(historyCategory);
            }

            if (source.EntityID > 0)
            {
                result.Entity = _entityDtoHelper.Get(source.EntityType, source.EntityID);
            }

            result.Files = _daoFactory.GetRelationshipEventDao().GetFiles(source.ID).ConvertAll(file => _fileWrapperHelper.Get<int>(file));

            if (source.ContactID > 0)
            {
                var relativeContact = _daoFactory.GetContactDao().GetByID(source.ContactID);

                if (relativeContact != null)
                {
                    result.Contact = context.Mapper.Map<ContactBaseDto>(relativeContact);
                }
            }

            result.CanEdit = _crmSecurity.CanAccessTo(source);

            return result;

        }
    }

}