using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections;

namespace Login.Models.Security
{
    public class HiwiBoerseRoleProvider : RoleProvider
    {
        private ArrayList rollen = new ArrayList();

        public HiwiBoerseRoleProvider() {
            rollen.Add("Bewerber");
            rollen.Add("Anbieter");
            rollen.Add("Bearbeiter");
            rollen.Add("Admin");
        }
        
        public override string ApplicationName { get; set; }

        public override string[] GetRolesForUser(string email)
        {
            DBManager db = DBManager.getInstanz();

            int rechte = db.rechteFuerBenutzer(email);
            
            if (rechte != -1) 
            {
                return new string[] { (string)rollen[rechte] }; ;
            }
            return new string[] { }; ;

        }


        public override bool IsUserInRole(string email, string rolle)
        {
            string userRolle;
            
            DBManager db = DBManager.getInstanz();

            int rechte = db.rechteFuerBenutzer(email);

            if (rechte != -1) 
            {
                userRolle = (string)rollen[rechte] ;
                if (userRolle.Equals(rolle))
                {
                    return true;
                }
            }
            return false;
            
        }

        public override void CreateRole(string roleName)
        {
            rollen.Add(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            

        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return false;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return null;
        }

        public override string[] GetAllRoles()
        {
            return (string[])rollen.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return null;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {

        }

        public override bool RoleExists(string roleName)
        {
            return false;
        }

    }
}