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
using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;

namespace ASC.Projects.Core.DataAccess.Domain.Entities
{
    /// <summary>
    /// Represents basic database entity with Id having specified type.
    /// </summary>
    /// <typeparam name="TKey">Type of Id.</typeparam>
    public abstract class BaseDbEntity<TKey> : IBaseDbEntity<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Id of entity.
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Unique Id of entity.
        /// </summary>
        public virtual string UniqueId => GetUniqueId(GetType(), Id);

        public virtual string ItemPath => string.Empty;

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return $"{GetType().FullName}|{Id.GetHashCode()}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var result = obj is BaseDbEntity<TKey> comparingObject
                && ((!IsTransient()
                    && !comparingObject.IsTransient()
                    && Id.Equals(comparingObject.Id))
                    || ((IsTransient() 
                    || comparingObject.IsTransient()) 
                    && GetHashCode().Equals(comparingObject.GetHashCode())));

            return result;
        }

        public static string GetUniqueId(Type type, TKey id)
        {
            return $"{type.Name}_{id}";
        }

        private bool IsTransient()
        {
            return Id.Equals(default(TKey));
        }
    }
}
