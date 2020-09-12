using System;
using System.Collections.Generic;
using System.Linq;
using GreenPOS.Context;
using GreenPOS.Entity;

namespace GreenPos.Model
{
    public static class Seed
    {
        public static void EnsureSeeded(this GreenPOSDBContext context)
        {
            var currentDate = DateTime.UtcNow;
            long userId = 1;

            if (!context.Screen.Any())
            {
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 1, Name = "Admin Dashboard", Description = "Admin Dashboard", CreatedBy = userId, CreatedOn = currentDate });
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 2, Name = "User Dashboard", Description = "User Dashboard", CreatedBy = userId, CreatedOn = currentDate });
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 3, Name = "Manage Users", Description = "Manage Users", CreatedBy = userId, CreatedOn = currentDate });
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 4, Name = "Manage Roles", Description = "Manage Roles", CreatedBy = userId, CreatedOn = currentDate });
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 5, Name = "Manage Contacts", Description = "Manage Contacts", CreatedBy = userId, CreatedOn = currentDate });
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 6, Name = "Manage Projects", Description = "Manage Projects", CreatedBy = userId, CreatedOn = currentDate });
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 7, Name = "Manage Jobs", Description = "Manage Projects", CreatedBy = userId, CreatedOn = currentDate });
                context.Screen.Add(new Screen { IsActive = true, UniqueId = 8, Name = "Manage Customers", Description = "Manage Projects", CreatedBy = userId, CreatedOn = currentDate });
            }

            if (!context.ScreenPermission.Any())
            {
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 7, Name = "Create Jobs", Description = "Create Jobs", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 7, Name = "Delete Jobs", Description = "Delete Jobs", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 7, Name = "Readonly Jobs", Description = "Readonly Jobs", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 8, Name = "Create Customer", Description = "Create Customer", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 8, Name = "Delete Customer", Description = "Delete Customer", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 8, Name = "Readonly Customer", Description = "Readonly Customer", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 3, Name = "Create User", Description = "Create User", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 3, Name = "Delete User", Description = "Delete User", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 4, Name = "Create Role", Description = "Create Role", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 4, Name = "Delete Role", Description = "Delete Role", CreatedBy = userId, CreatedOn = currentDate });
                context.ScreenPermission.Add(new ScreenPermission { IsActive = true, ScreenId = 1, Name = "View Dashboard", Description = "View Dashboard", CreatedBy = userId, CreatedOn = currentDate });
            }

            context.SaveChanges();

            if (!context.Role.Any())
            {
                var allPermissions = context.ScreenPermission.Any() ?
                    string.Join(",", context.ScreenPermission.Where(a => a.IsActive).Select(a => a.Id.ToString()).ToList()) :
                    "0";

                var userScreens = new List<string>() { "user", "role", "view dashboard", "customer" };
                var userPermissions = context.ScreenPermission.Any() ?
                    string.Join(",", context.ScreenPermission.Where(a => a.IsActive && userScreens.Contains(a.Name.ToLower().Trim()))
                    .Select(a => a.Id.ToString()).ToList()) :
                    string.Empty;
                context.Role.Add(new Role { IsActive = true, PermissionIds = allPermissions, Name = "Super Admin", CreatedBy = userId, CreatedOn = currentDate });
                context.Role.Add(new Role { IsActive = true, PermissionIds = allPermissions, Name = "Company Admin", CreatedBy = userId, CreatedOn = currentDate });
                context.Role.Add(new Role { IsActive = true, PermissionIds = userPermissions, Name = "Member", CreatedBy = userId, CreatedOn = currentDate });

                context.SaveChanges();
            }

            //Ensure we create initial Threat List
            if (!context.User.Any())
            {
                var u = new User
                {
                    UserName = "superadmin",
                    Email = "superadmin@greenpos.com",
                    CompanyName = string.Empty,
                    CreatedBy = userId,
                    CreatedOn = currentDate,
                    Password = "S_p@r000",
                    IsActive = true
                };
                context.User.Add(u);

                u = new User
                {
                    UserName = "admin",
                    Email = "admin03@greenpos.com",
                    CompanyName = "Green POS",
                    CreatedBy = userId,
                    CreatedOn = currentDate,
                    Password = "Admin_12345",
                    IsActive = true
                };
                context.User.Add(u);

                u = new User
                {
                    UserName = "userone",
                    Email = "userone@greenpos.com",
                    CompanyName = "Green POS",
                    CreatedBy = userId,
                    CreatedOn = currentDate,
                    IsActive = true,
                    Password = "User_12345"
                };
                context.User.Add(u);


                context.SaveChanges();
            }


            if (!context.UserRole.Any())
            {
                userId = context.User.FirstOrDefault(a => a.UserName.Equals("superadmin")).Id;
                var roleId = context.Role.FirstOrDefault(a => a.Name.Equals("Super Admin")).Id;
                context.UserRole.Add(new UserRole { UserId = userId, RoleId = roleId, IsActive = true, CreatedBy = userId, CreatedOn = currentDate });

                userId = context.User.FirstOrDefault(a => a.UserName.Equals("admin")).Id;
                roleId = context.Role.FirstOrDefault(a => a.Name.Equals("Company Admin")).Id;
                context.UserRole.Add(new UserRole { UserId = userId, RoleId = roleId, IsActive = true, CreatedBy = userId, CreatedOn = currentDate });

                userId = context.User.FirstOrDefault(a => a.UserName.Equals("userone")).Id;
                roleId = context.Role.FirstOrDefault(a => a.Name.Equals("Member")).Id;
                context.UserRole.Add(new UserRole { IsActive = true, CreatedBy = userId, CreatedOn = currentDate });

                context.SaveChanges();
            }
        }
    }
}
