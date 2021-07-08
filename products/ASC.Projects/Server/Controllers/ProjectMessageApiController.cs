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
using System.Linq;
using System.Threading.Tasks;
using ASC.Common;
using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Projects.Configuration;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Validators;
using ASC.Projects.ViewModels;
using ASC.Web.Api.Routing;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Projects.Controllers
{
    [Scope]
    [DefaultRoute]
    [ApiController]
    public class ProjectMessageApiController : BaseApiController
    {
        private readonly IMessageManager _messageManager;

        private readonly IMapper _mapper;

        public ProjectMessageApiController(ProductEntryPoint productEntryPoint,
            SecurityContext securityContext,
            IMessageManager messageManager,
            IMapper mapper) : base(productEntryPoint, securityContext)
        {
            _messageManager = messageManager.NotNull(nameof(messageManager));
            _mapper = mapper.NotNull(nameof(mapper));
        }


        /// <summary>
        /// Receives all messages of project.
        /// </summary>
        /// <param name="projectId">Id of project.</param>
        /// <returns>List of messages <see cref="MessageViewModel"/> related to project with specified id.</returns>
        [Read(@"{projectId:[0-9]+}/message")]
        public List<MessageViewModel> GetProjectMessages(int projectId)
        {
            var messages = _messageManager
                .GetProjectMessages(projectId)
                .Select(m => _mapper.Map<MessageData, MessageViewModel>(m))
                .ToList();

            return messages;
        }

        [Create(@"{projectid:[0-9]+}/message")]
        public IActionResult AddProjectMessage(CreateMessageViewModel newMessage)
        {
            var validator = new CreateMessageItemValidator();

            var validationResult = validator.Validate(newMessage);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(_ => _.ErrorMessage));

                return BadRequest($"There is an invalid request for message creation: {errors}");
            }

            var discussion = _mapper.Map<CreateMessageViewModel, MessageData>(newMessage)

            _messageManager.SaveOrUpdate(discussion)
        }
    }
}
