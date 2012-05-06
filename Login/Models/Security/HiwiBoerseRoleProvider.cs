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

            String query = "SELECT rechte FROM benutzer WHERE email='"+ email + "'";
            ArrayList daten = db.auslesen(query);
            
            if (daten.Count > 0) 
            {
                ArrayList zeile = (ArrayList)daten[0];
                int rolle = (int)zeile[0];
                return new string[] { (string)rollen[rolle] }; ;
            }
            return new string[] { }; ;

        }


        public override bool IsUserInRole(string email, string rolle)
        {
            string userRolle;
            
            DBManager db = DBManager.getInstanz();

            String query = "SELECT rechte FROM benutzer WHERE email='"+ email + "'";
            ArrayList daten = db.auslesen(query);
            
            if (daten.Count > 0) 
            {
                ArrayList zeile = (ArrayList)daten[0];
                userRolle = (string)rollen[(int)zeile[0]] ;
                return rollen.Contains(userRolle);
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