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
using ASC.Core;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// Repository working with <see cref="DbProjectParticipant"/> entity.
    /// </summary>
    internal class ParticipantRepository : BaseTenantRepository<DbProjectParticipant, int>, IParticipantRepository
    {
        #region .ctor

        public ParticipantRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        #endregion .ctor

        /// <summary>
        /// Receives ids of projects following by participant with specified id.
        /// </summary>
        /// <param name="participantId">Id of needed participant.</param>
        /// <returns>List of ids <see cref="List{int}"/> which contains ids of projects following by participant with specified id.</returns>
        public List<int> GetFollowingProjectIds(Guid participantId)
        {
            var result = GetAll()
                .Include(p => p.FollowingProjects)
                .Where(p => p.ParticipantId == participantId)
                .SelectMany(p => p.FollowingProjects.Select(fp => fp.Id))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives ids of current participant projects.
        /// </summary>
        /// <param name="participantId">Current participant projects.</param>
        /// <returns>List of ids <see cref="List{int}"/> which contains ids of current participant projects.</returns>
        public List<int> GetMyProjects(Guid participantId)
        {
            var result = GetAll()
                .Where(p => p.ParticipantId == participantId)
                .Select(p => p.ProjectId)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives ids of projects which are could be interesting for current participant.
        /// </summary>
        /// <param name="participantId">Id of needed participant.</param>
        /// <returns>List of ids <see cref="List{int}"/> which contains ids of projects which are could be interesting for current participant.</returns>
        public List<int> GetObservableProjectIds(Guid participantId)
        {
            var result = GetFollowingProjectIds(participantId)
                .Union(GetMyProjects(participantId))
                .Distinct()
                .ToList();

            return result;
        }

        /// <summary>
        /// Adds participant with specified id to list of followers of project with specified id.
        /// </summary>
        /// <param name="project">Id of following project.</param>
        /// <param name="participantId">Id of participant, who follows project.</param>
        public void AddToFollowingProjects(int project, Guid participantId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes participant with specified id from followers of project with specified id.
        /// </summary>
        /// <param name="projectId">Id of unfollowing project.</param>
        /// <param name="participantId">Id of participant, who unfollowings from project.</param>
        public void RemoveFromFollowingProjects(int projectId, Guid participantId)
        {
            throw new NotImplementedException();
        }

        public bool IsFollowingProject(int projectId, Guid participantId)
        {
            var result = GetAll()
                .Include(p => p.FollowingProjects)
                .Where(p => p.ParticipantId == participantId)
                .SelectMany(p => p.FollowingProjects.Select(pp => pp.Id))
                .Contains(projectId);

            return result;
        }

        public List<DbProjectParticipant> GetTeam(int projectId, bool withExcluded = false)
        {
            var predicate = GetAll()
                .Include(pp => pp.FollowingProjects)
                .Where(pp => pp.ProjectId == projectId);

            if (!withExcluded)
            {
                predicate = predicate.Where(pp => !pp.IsRemoved);
            }

            var result = predicate
                .ToList();

            return result;
        }

        public List<DbProjectParticipant> GetTeam(List<int> projectIds)
        {
            var result = GetAll()
                .Include(p => p.FollowingProjects)
                .ThenInclude(p => p.Participants)
                .SelectMany(p => p.FollowingProjects.Select(fp => fp))
                .Where(fp => projectIds.Contains(fp.Id))
                .SelectMany(p => p.Participants)
                .ToList();

            return result;
        }

        public void SetTeamSecurity(int projectId, Guid participantId, ProjectTeamSecurity teamSecurity)
        {
            var participant = GetAll()
                .SingleOrDefault(p => p.ProjectId == projectId && p.ParticipantId == participantId);

            if (participant == null)
            {
                throw new InvalidOperationException($"A participant with ID = {participantId} does not exists.");
            }

            participant.LastModificationDate = DateTime.UtcNow;
            participant.Security = (int)teamSecurity;

            base.Update(participant);
        }
    }
}
