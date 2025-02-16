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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using ASC.Common;
using ASC.Common.Logging;
using ASC.CRM.Core.Dao;
using ASC.Data.Storage;
using ASC.Web.Core;
using ASC.Web.Core.Utility.Skins;
using ASC.Web.CRM.Configuration;

using Microsoft.Extensions.Options;

namespace ASC.Web.CRM.Classes
{
    [Scope]
    public class OrganisationLogoManager
    {
        private DaoFactory _daoFactory;
        private ILog _logger;
        private Global _global;
        private WebImageSupplier _webImageSupplier;

        public OrganisationLogoManager(WebImageSupplier webImageSupplier,
                                       Global global,
                                       IOptionsMonitor<ILog> logger,
                                       DaoFactory daoFactory)
        {
            _webImageSupplier = webImageSupplier;
            _global = global;
            _logger = logger.Get("ASC.CRM");
            _daoFactory = daoFactory;
        }


        #region Members

        public readonly String OrganisationLogoBaseDirName = "organisationlogo";

        public readonly String OrganisationLogoImgName = "logo";

        public readonly String OrganisationLogoSrcFormat = "data:image/jpeg;base64,{0}";

        public readonly Size OrganisationLogoSize = new Size(200, 150);

        private readonly Object _synchronizedObj = new Object();

        #endregion

        #region Private Methods

        private String BuildFileDirectory()
        {
            return String.Concat(OrganisationLogoBaseDirName, "/");
        }

        private String BuildFilePath(String imageExtension)
        {
            return String.Concat(BuildFileDirectory(), OrganisationLogoImgName, imageExtension);
        }

        private String ExecResizeImage(byte[] imageData, Size fotoSize, IDataStore dataStore, String photoPath)
        {
            var data = imageData;
            using (var stream = new MemoryStream(data))
            using (var img = new Bitmap(stream))
            {
                var imgFormat = img.RawFormat;
                if (fotoSize != img.Size)
                {
                    using (var img2 = CommonPhotoManager.DoThumbnail(img, fotoSize, false, false, false))
                    {
                        data = CommonPhotoManager.SaveToBytes(img2, Global.GetImgFormatName(imgFormat));
                    }
                }
                else
                {
                    data = Global.SaveToBytes(img);
                }

                using (var fileStream = new MemoryStream(data))
                {
                    var photoUri = dataStore.Save(photoPath, fileStream).ToString();
                    photoUri = String.Format("{0}?cd={1}", photoUri, DateTime.UtcNow.Ticks);
                    return photoUri;
                }
            }
        }

        #endregion

        public String GetDefaultLogoUrl()
        {
            return _webImageSupplier.GetAbsoluteWebPath("org_logo_default.png", ProductEntryPoint.ID);
        }

        public String GetOrganisationLogoBase64(int logoID)
        {
            if (logoID <= 0) { return ""; }

            return _daoFactory.GetInvoiceDao().GetOrganisationLogoBase64(logoID);


        }

        public String GetOrganisationLogoSrc(int logoID)
        {
            var bytestring = GetOrganisationLogoBase64(logoID);
            return String.IsNullOrEmpty(bytestring) ? "" : String.Format(OrganisationLogoSrcFormat, bytestring);
        }

        public void DeletePhoto(bool recursive)
        {
            var photoDirectory = BuildFileDirectory();
            var store = _global.GetStore();

            lock (_synchronizedObj)
            {
                if (store.IsDirectory(photoDirectory))
                {
                    store.DeleteFiles(photoDirectory, "*", recursive);
                    if (recursive)
                    {
                        store.DeleteDirectory(photoDirectory);
                    }
                }
            }
        }

        public int TryUploadOrganisationLogoFromTmp(DaoFactory factory)
        {
            var directoryPath = BuildFileDirectory();
            var dataStore = _global.GetStore();

            if (!dataStore.IsDirectory(directoryPath))
                return 0;

            try
            {
                var photoPaths = _global.GetStore().ListFilesRelative("", directoryPath, OrganisationLogoImgName + "*", false);
                if (photoPaths.Length == 0)
                    return 0;

                byte[] bytes;
                using (var photoTmpStream = dataStore.GetReadStream(Path.Combine(directoryPath, photoPaths[0])))
                {
                    bytes = Global.ToByteArray(photoTmpStream);
                }

                var logoID = factory.GetInvoiceDao().SaveOrganisationLogo(bytes);
                dataStore.DeleteFiles(directoryPath, "*", false);
                return logoID;
            }

            catch (Exception ex)
            {
                _logger.ErrorFormat("TryUploadOrganisationLogoFromTmp failed with error: {0}", ex);

                return 0;
            }
        }

        public String UploadLogo(byte[] imageData, ImageFormat imageFormat)
        {
            var photoPath = BuildFilePath("." + Global.GetImgFormatName(imageFormat));

            return ExecResizeImage(imageData, OrganisationLogoSize, _global.GetStore(), photoPath);
        }
    }
}
