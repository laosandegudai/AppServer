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


using System;
using System.Collections.Generic;

using ASC.Common;
using ASC.Common.Caching;
using ASC.Core.Common.Settings;

namespace ASC.Web.Core.Users
{
    [Serializable]
    public class UserHelpTourSettings : ISettings, ICacheWrapped<CachedUserHelpTourSettings>
    {
        public Guid ID
        {
            get { return new Guid("{DF4B94B7-42C8-4fce-AAE2-D479F3B39BDD}"); }
        }

        public Dictionary<Guid, int> ModuleHelpTour { get; set; }

        public bool IsNewUser { get; set; }

        public ISettings GetDefault(IServiceProvider serviceProvider)
        {
            return new UserHelpTourSettings
            {
                ModuleHelpTour = new Dictionary<Guid, int>(),
                IsNewUser = false
            };
        }

        public CachedUserHelpTourSettings WrapIn()
        {
            return new CachedUserHelpTourSettings
            {
                IsNewUser = IsNewUser,
                ModuleHelpTour = this.ModuleHelpTour
            };
        }
    }

    public partial class CachedUserHelpTourSettings : ICustomSer<CachedUserHelpTourSettings>,
        ICacheWrapped<UserHelpTourSettings>
    {
        public Dictionary<Guid, int> ModuleHelpTour { get; set; } = new Dictionary<Guid, int>();
        public void CustomDeSer()
        {
            foreach (var pair in ModuleHelpTourProto)
            {
                ModuleHelpTour.Add(Guid.Parse(pair.Key), pair.Value);
            }
        }

        public void CustomSer()
        {
            foreach (var pair in ModuleHelpTour)
            {
                ModuleHelpTourProto.Add(pair.Key.ToString(), pair.Value);
            }
        }

        public UserHelpTourSettings WrapIn()
        {
            return new UserHelpTourSettings
            {
                ModuleHelpTour = this.ModuleHelpTour,
                IsNewUser = this.IsNewUser
            };
        }
    }

    [Scope]
    public class UserHelpTourHelper
    {
        private SettingsManager SettingsManager { get; }

        public UserHelpTourHelper(SettingsManager settingsManager)
        {
            SettingsManager = settingsManager;
        }

        public bool IsNewUser
        {
            get { return SettingsManager.LoadForCurrentUser<UserHelpTourSettings, CachedUserHelpTourSettings>().IsNewUser; }
            set
            {
                var settings = SettingsManager.LoadForCurrentUser<UserHelpTourSettings, CachedUserHelpTourSettings>();
                settings.IsNewUser = value;
                SettingsManager.SaveForCurrentUser<UserHelpTourSettings, CachedUserHelpTourSettings>(settings);
            }
        }
    }
}