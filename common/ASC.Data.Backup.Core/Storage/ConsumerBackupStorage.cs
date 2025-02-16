/*
 *
 * (c) Copyright Ascensio System Limited 2010-2020
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
using System.IO;
using System.Linq;

using ASC.Common;
using ASC.Data.Storage;
using ASC.Data.Storage.Configuration;

namespace ASC.Data.Backup.Storage
{
    [Scope]
    public class ConsumerBackupStorage : IBackupStorage
    {
        private IDataStore Store { get; set; }
        private const string Domain = "backup";
        private StorageSettingsHelper StorageSettingsHelper { get; set; }
        public ConsumerBackupStorage(StorageSettingsHelper storageSettingsHelper)
        {
            StorageSettingsHelper = storageSettingsHelper;
        }
        public void Init(IReadOnlyDictionary<string, string> storageParams)
        {
            var settings = new StorageSettings { Module = storageParams["module"], Props = storageParams.Where(r => r.Key != "module").ToDictionary(r => r.Key, r => r.Value) };
            Store = StorageSettingsHelper.DataStore(settings);
        }
        public string Upload(string storageBasePath, string localPath, Guid userId)
        {
            using var stream = File.OpenRead(localPath);
            var storagePath = Path.GetFileName(localPath);
            Store.Save(Domain, storagePath, stream, ACL.Private);
            return storagePath;
        }

        public void Download(string storagePath, string targetLocalPath)
        {
            using var source = Store.GetReadStream(Domain, storagePath);
            using var destination = File.OpenWrite(targetLocalPath);
            source.CopyTo(destination);
        }

        public void Delete(string storagePath)
        {
            if (Store.IsFile(Domain, storagePath))
            {
                Store.Delete(Domain, storagePath);
            }
        }

        public bool IsExists(string storagePath)
        {
            return Store.IsFile(Domain, storagePath);
        }

        public string GetPublicLink(string storagePath)
        {
            return Store.GetInternalUri(Domain, storagePath, TimeSpan.FromDays(1), null).AbsoluteUri;
        }
    }
}
