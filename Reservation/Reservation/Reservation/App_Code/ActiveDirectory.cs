using System;
using System.DirectoryServices.AccountManagement;

/// <summary>
/// Summary description for ActiveDirectory
/// </summary>

namespace Reservation
{
    public class ActiveDirectory
    {
        #region Variables

        private const string SDomain = "SERVER1.INFRA-S80.local";
        private const string SDefaultOu = "DC=INFRA-S80,DC=local";
        private string SDefaultRootOu = "DC=INFRA-S80,DC=local";
        private const string SServiceUser = "CN=Administrator,CN=Users,DC=INFRA-S80,DC=local";
        private const string SServicePassword = "rickenjules";

        #endregion

        /// <summary>
        /// Validates a users' credentials.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateCredentials(string username, string password)
        {
            PrincipalContext pContext = GetPrincipalContext();
            return pContext.ValidateCredentials(username, password);
        }

        /// <summary>
        /// Gets info over the base principal context
        /// </summary>
        /// <returns></returns>
        public PrincipalContext GetPrincipalContext()
        {
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, SDomain, SDefaultOu, ContextOptions.SimpleBind, SServiceUser, SServicePassword);
            return oPrincipalContext;
        }

        /// <summary>
        /// Checks if the users' account is expired
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserExpired(string username)
        {
            UserPrincipal oPrincipal = GetUser(username);
            if (oPrincipal.AccountExpirationDate == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds the right user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserPrincipal GetUser(string username)
        {
            PrincipalContext pContext = GetPrincipalContext();

            UserPrincipal oUserPrincipal = UserPrincipal.FindByIdentity(pContext, username);
            return oUserPrincipal;
        }

        /// <summary>
        /// Finds info about the given group.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public GroupPrincipal GetGroup(string groupName)
        {
            PrincipalContext pContext = GetPrincipalContext();

            GroupPrincipal oGroupPrincipal = GroupPrincipal.FindByIdentity(pContext, groupName);
            return oGroupPrincipal;
        }

        /// <summary>
        /// Sets a users' password to the new one.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        public void SetUserPassword(string username, string password, out string message)
        {
            try
            {
                UserPrincipal oUserPrincipal = GetUser(username);
                oUserPrincipal.SetPassword(password);
                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

        }

        /// <summary>
        /// Enables a users' account.
        /// </summary>
        /// <param name="username"></param>
        public void EnableUserAccount(string username)
        {
            UserPrincipal oUserPrincipal = GetUser(username);
            oUserPrincipal.Enabled = true;
            oUserPrincipal.Save();
        }

        /// <summary>
        /// Disables a users' account.
        /// </summary>
        /// <param name="username"></param>
        public void DisableUserAccount(string username)
        {
            UserPrincipal oUserPrincipal = GetUser(username);
            oUserPrincipal.Enabled = false;
            oUserPrincipal.Save();
        }

        /// <summary>
        /// Creates a new user in the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="givename"></param>
        /// <param name="surname"></param>
        /// <returns></returns>
        public UserPrincipal CreateNewUser(/*string sOu, */string username, string password, string givename, string surname)
        {
            if (!IsUserExisting(username))
            {
                PrincipalContext oPrincipalContext = GetPrincipalContext();

                UserPrincipal oUserPrincipal = new UserPrincipal(oPrincipalContext, username, password, true
                    /*Enabled or not*/) { UserPrincipalName = username, GivenName = givename, Surname = surname };

                //User Log on Name
                oUserPrincipal.Save();

                return oUserPrincipal;
            }
            else
            {
                return GetUser(username);
            }
        }

        /// <summary>
        /// Checks if the user exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserExisting(string username)
        {
            return GetUser(username) != null;
        }

        /// <summary>
        /// Gets info over the specified principal context.
        /// </summary>
        /// <param name="sOu"></param>
        /// <returns></returns>
        public PrincipalContext GetPrincipalContext(string sOu)
        {
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, SDomain, sOu, ContextOptions.SimpleBind, SServiceUser, SServicePassword);
            return oPrincipalContext;
        }

        /// <summary>
        /// Deletes the given user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool DeleteUser(string username)
        {
            try
            {
                UserPrincipal oUserPrincipal = GetUser(username);

                oUserPrincipal.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
