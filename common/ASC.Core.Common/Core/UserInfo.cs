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
using System.Globalization;
using System.Text;

using ASC.Common.Caching;
using ASC.Notify.Recipients;

using Google.Protobuf;


namespace ASC.Core.Users
{
    public partial class UserInfoStore : ProtoSerializeMap<UserInfoStore, string, UserInfo>
    {
        public UserInfoStore(IEnumerable<UserInfo> userInfos)
        {
            foreach (var userInfo in userInfos)
            {
                ByGuid.Add(userInfo.ID.ToString(), userInfo);
            }
        }

        public override void CustomDeSer()
        {
            foreach (var pair in ByGuid)
            {
                pair.Value.CustomDeSer();
            }
        }

        public override void CustomSer()
        {
            foreach (var pair in ByGuid)
            {
                pair.Value.CustomSer();
            }
        }

        protected override IDictionary<string, UserInfo> GetMap()
        {
            return ByGuid;
        }
    }


    [Serializable]
    public sealed partial class UserInfo : IDirectRecipient, ICustomSer<UserInfo>
    {
        partial void OnConstruction()
        {
            Status = EmployeeStatus.Active;
            ActivationStatus = (int)EmployeeActivationStatus.NotActivated;
            LastModified = DateTime.UtcNow;
        }

        public Guid ID { get; set; }
        public DateTime? BirthDate { get; set; }

        private bool? sex;
        public bool? Sex
        {
            get
            {
                return sex;
            }
            set
            {
                SexIsNull = !value.HasValue;
                SexProto = value ?? default;
                sex = value;
            }
        }

        public EmployeeStatus Status
        {
            get
            {
                return (EmployeeStatus)StatusProto;
            }
            set
            {
                StatusProto = (int)value;
            }
        }

        public EmployeeActivationStatus ActivationStatus
        {
            get
            {
                return (EmployeeActivationStatus)ActivationStatusProto;
            }
            set
            {
                ActivationStatusProto = (int)value;
            }
        }

        private DateTime? terminatedDate;
        public DateTime? TerminatedDate
        {
            get
            {
                return terminatedDate;
            }
            set
            {
                terminatedDate = value;
            }
        }

        private DateTime? workFromDate;
        public DateTime? WorkFromDate
        {
            get
            {
                return workFromDate;
            }
            set
            {
                workFromDate = value;
            }
        }

        private DateTime lastModified;
        public DateTime LastModified
        {
            get
            {
                return lastModified;
            }
            set
            {
                lastModified = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return ((EmployeeActivationStatus)ActivationStatus).HasFlag(EmployeeActivationStatus.Activated);
            }
        }

        public MobilePhoneActivationStatus MobilePhoneActivationStatus
        {
            get
            {
                return (MobilePhoneActivationStatus)MobilePhoneActivationStatusProto;
            }
            set
            {
                MobilePhoneActivationStatusProto = (int)value;
            }
        }

        private DateTime createDate;
        public DateTime CreateDate
        {
            get
            {
                return createDate;
            }
            set
            {
                createDate = value;
            }
        }

        public CultureInfo GetCulture()
        {
            return string.IsNullOrEmpty(CultureName) ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(CultureName);
        }

        string[] IDirectRecipient.Addresses
        {
            get { return !string.IsNullOrEmpty(Email) ? new[] { Email } : new string[0]; }
        }

        public bool CheckActivation
        {
            get { return !IsActive; /*if user already active we don't need activation*/ }
        }

        string IRecipient.ID
        {
            get { return ID.ToString(); }
        }

        string IRecipient.Name
        {
            get { return ToString(); }
        }

        internal string ContactsToString()
        {
            if (ContactsList == null || ContactsList.Count == 0) return null;
            var sBuilder = new StringBuilder();
            foreach (var contact in ContactsList)
            {
                sBuilder.AppendFormat("{0}|", contact);
            }
            return sBuilder.ToString();
        }

        internal UserInfo ContactsFromString(string contacts)
        {
            if (string.IsNullOrEmpty(contacts)) return this;

            ContactsList.Clear();

            ContactsList.AddRange(contacts.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));

            return this;
        }

        public void CustomSer()
        {
            IDProto = ID.ToByteString();

            BirthDateProto = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime());

            TerminatedDateProto = (TerminatedDate == null) ?
                Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
                : Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(TerminatedDate.Value.ToUniversalTime());

            CreateDateProto = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(CreateDate.ToUniversalTime());

            WorkFromDateProto = (WorkFromDate == null) ?
                Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
                : Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(WorkFromDate.Value.ToUniversalTime());

            LastModifiedProto = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(LastModified.ToUniversalTime());
        }

        public void CustomDeSer()
        {
            ID = IDProto.FromByteString();

            CreateDate = CreateDateProto.ToDateTime();
            WorkFromDate = WorkFromDateProto.ToDateTime();
            LastModified = LastModifiedProto.ToDateTime();

            if (BirthDateProto != null)
            {
                var result = BirthDateProto.ToDateTime();

                if (result != DateTime.MinValue)
                {
                    BirthDate = result;
                }
            }

            if (TerminatedDateProto != null)
            {
                var result = TerminatedDateProto.ToDateTime();

                if (result != DateTime.MinValue)
                {
                    TerminatedDate = result;
                }
            }
        }
    }
}