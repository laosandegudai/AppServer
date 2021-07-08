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

using ASC.Notify.Recipients;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Interfaces
{
    /// <summary>
    /// An interface of notifications recipients builder.
    /// </summary>
    public interface IDirectRecipientBuilder
    {
        /// <summary>
        /// Sets up a specified of recipient.
        /// </summary>
        /// <param name="id">Id of recipient.</param>
        /// <returns>A current instance of <see cref="IDirectRecipientBuilder"/>.</returns>
        IDirectRecipientBuilder WithId(string id);

        /// <summary>
        /// Sets up a name of recipient.
        /// </summary>
        /// <param name="name">Name of recipient.</param>
        /// <returns>A current instance of <see cref="IDirectRecipientBuilder"/>.</returns>
        IDirectRecipientBuilder WithName(string name);

        /// <summary>
        /// Sets up an addresses of recipients.
        /// </summary>
        /// <param name="addresses">Addresses of recipients.</param>
        /// <returns>A current instance of <see cref="IDirectRecipientBuilder"/>.</returns>
        IDirectRecipientBuilder WithAddresses(string[] addresses);

        /// <summary>
        /// Sets up a need of activation check.
        /// </summary>
        /// <param name="isActivationCheckRequired">Determines need for activation check.</param>
        /// <returns>A current instance of <see cref="IDirectRecipientBuilder"/>.</returns>
        IDirectRecipientBuilder WithCheckActivation(bool isActivationCheckRequired);

        /// <summary>
        /// Constructs an instance of <see cref="DirectRecipient"/> having specified parameters.
        /// </summary>
        /// <returns>Just constructed recipient <see cref="DirectRecipient"/>.</returns>
        DirectRecipient Build();
    }
}
