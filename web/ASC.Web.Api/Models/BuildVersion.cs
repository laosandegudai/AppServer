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


using System.Text.Json.Serialization;


using ASC.Common;

using Microsoft.Extensions.Configuration;

namespace ASC.Api.Settings
{
    [Singletone]
    public class BuildVersion
    {
        public string CommunityServer { get; set; }

        public string DocumentServer { get; set; }

        public string MailServer { get; set; }

        public string XmppServer { get; set; }

        [JsonIgnore]
        private IConfiguration Configuration { get; }

        public BuildVersion(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public BuildVersion GetCurrentBuildVersion()
        {
            CommunityServer = GetCommunityVersion();
            DocumentServer = GetDocumentVersion();
            MailServer = GetMailServerVersion();
            XmppServer = GetXmppServerVersion();

            return this;
        }

        private string GetCommunityVersion()
        {
            return Configuration["version:number"] ?? "8.5.0";
        }

        private static string GetDocumentVersion()
        {
            return "";
            //TODO
            /*
            if (string.IsNullOrEmpty(FilesLinkUtility.DocServiceApiUrl))
                return null;

            return DocumentServiceConnector.GetVersion();*/
        }

        private static string GetMailServerVersion()
        {
            //TODO
            return "";
            /*
            try
            {
               
                var engineFactory = new EngineFactory(
                    CoreContext.TenantManager.GetCurrentTenant().TenantId,
                    SecurityContext.CurrentAccount.ID.ToString());

                var version = engineFactory.ServerEngine.GetServerVersion();
                return version;
                }
            catch (Exception e)
            {
                LogManager.GetLogger("ASC").Warn(e.Message, e);
            }

            return null;*/
        }

        private static string GetXmppServerVersion()
        {
            //try
            //{
            //    if (ConfigurationManagerExtension.AppSettings["web.talk"] != "true")
            //        return null;

            //    return new JabberServiceClient().GetVersion();
            //}
            //catch (Exception e)
            //{
            //    LogManager.GetLogger("ASC").Warn(e.Message, e);
            //}

            return null;
        }
    }
}
