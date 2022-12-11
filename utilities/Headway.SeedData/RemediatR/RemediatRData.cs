using Headway.Core.Constants;
using Headway.Core.Model;
using Microsoft.EntityFrameworkCore;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRData
    {
        public readonly static Dictionary<string, Permission> Permissions = new();
        public readonly static Dictionary<string, Role> Roles = new();
        public readonly static Dictionary<string, User> Users = new();

        public static List<Country> CountriesGet()
        {
            List<Country> countries = new();

            var lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RemediatR", "_countries.csv"));

            foreach (var line in lines.Skip(1))
            {
                var c = line.Split(',');

                countries.Add(new Country
                {
                    Code = c[0],
                    Latitude = string.IsNullOrWhiteSpace(c[1]) ? default(decimal?) : decimal.Parse(c[1]),
                    Longitude = string.IsNullOrWhiteSpace(c[2]) ? default(decimal?) : decimal.Parse(c[2]),
                    Name = c[3]
                });
            }

            return countries;
        }

        public static void PermissionsCreate()
        {
            Permissions.Add(RemediatRAuthorisation.CUSTOMER_READ, new Permission { Name = RemediatRAuthorisation.CUSTOMER_READ, Description = "RemediatR Customer Read" });
            Permissions.Add(RemediatRAuthorisation.CUSTOMER_WRITE, new Permission { Name = RemediatRAuthorisation.CUSTOMER_WRITE, Description = "RemediatR Customer Write" });

            Permissions.Add(RemediatRAuthorisation.REDRESS_READ, new Permission { Name = RemediatRAuthorisation.REDRESS_READ, Description = "RemediatR Redress Read" });
            Permissions.Add(RemediatRAuthorisation.REDRESS_WRITE, new Permission { Name = RemediatRAuthorisation.REDRESS_WRITE, Description = "RemediatR Redress Write" });
            Permissions.Add(RemediatRAuthorisation.REDRESS_TRANSITION, new Permission { Name = RemediatRAuthorisation.REDRESS_TRANSITION, Description = "RemediatR Redress Transition" });
            Permissions.Add(RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION, new Permission { Name = RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION, Description = "RemediatR Communication Dispatch Transition" });
            Permissions.Add(RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION, new Permission { Name = RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION, Description = "RemediatR Awaiting Response Transition" });

            Permissions.Add(RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION, new Permission { Name = RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION, Description = "RemediatR Redress Refund Review Transition" });
            Permissions.Add(RemediatRAuthorisation.REDRESS_COMPLETE, new Permission { Name = RemediatRAuthorisation.REDRESS_COMPLETE, Description = "RemediatR Redress Complete" });

            Permissions.Add(RemediatRAuthorisation.REFUND_READ, new Permission { Name = RemediatRAuthorisation.REFUND_READ, Description = "RemediatR Refund Read" });
            Permissions.Add(RemediatRAuthorisation.REFUND_WRITE, new Permission { Name = RemediatRAuthorisation.REFUND_WRITE, Description = "RemediatR Refund Write" });
            Permissions.Add(RemediatRAuthorisation.REFUND_CACULATION_COMPLETE, new Permission { Name = RemediatRAuthorisation.REFUND_CACULATION_COMPLETE, Description = "RemediatR Refund Calculation Complete" });
            Permissions.Add(RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE, new Permission { Name = RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE, Description = "RemediatR Refund Varification Complete" });

            Permissions.Add(RemediatRAuthorisation.REFUND_REVIEW_TRANSITION, new Permission { Name = RemediatRAuthorisation.REFUND_REVIEW_TRANSITION, Description = "RemediatR Refund Review Transition" });
        }

        public static void RolesCreate()
        {
            Roles.Add(RemediatRAuthorisation.REDRESS_CASE_OWNER, new Role { Name = RemediatRAuthorisation.REDRESS_CASE_OWNER, Description = "RemediatR Redress Case Owner" });
            Roles.Add(RemediatRAuthorisation.REDRESS_REVIEWER, new Role { Name = RemediatRAuthorisation.REDRESS_REVIEWER, Description = "RemediatR Redress Reviewer" });
            Roles.Add(RemediatRAuthorisation.REFUND_ASSESSOR, new Role { Name = RemediatRAuthorisation.REFUND_ASSESSOR, Description = "RemediatR Refund Assessor" });
            Roles.Add(RemediatRAuthorisation.REFUND_REVIEWER, new Role { Name = RemediatRAuthorisation.REFUND_REVIEWER, Description = "RemediatR Refund Reviewer" });
        }

        public static void RolesAssignPermissions()
        {
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.CUSTOMER_READ]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.CUSTOMER_WRITE]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_WRITE]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_TRANSITION]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION]);

            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.CUSTOMER_READ]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_COMPLETE]);

            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_WRITE]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_CACULATION_COMPLETE]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE]);

            Roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_REVIEW_TRANSITION]);
        }

        public static void UsersCreate()
        {
            Users.Add("grace", new User { UserName = "grace", Email = "grace@email.com" });
            Users.Add("mel", new User { UserName = "mel", Email = "mel@email.com" });
            Users.Add("bill", new User { UserName = "bill", Email = "bill@email.com" });
            Users.Add("will", new User { UserName = "will", Email = "will@email.com" });
            Users.Add("mary", new User { UserName = "mary", Email = "mary@email.com" });
        }

        public static void UsersAssignRoles(bool assignAllUsersHeadwayUserRole = true)
        {
            if (assignAllUsersHeadwayUserRole)
            {
                foreach (var user in Users.Values)
                {
                    user.Roles.Add(Roles[HeadwayAuthorisation.USER]);
                }
            }

            Users["grace"].Roles.Add(Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER]);
            Users["mel"].Roles.Add(Roles[RemediatRAuthorisation.REDRESS_REVIEWER]);
            Users["bill"].Roles.Add(Roles[RemediatRAuthorisation.REFUND_ASSESSOR]);
            Users["will"].Roles.Add(Roles[RemediatRAuthorisation.REFUND_ASSESSOR]);
            Users["mary"].Roles.Add(Roles[RemediatRAuthorisation.REFUND_REVIEWER]);
        }
    }
}