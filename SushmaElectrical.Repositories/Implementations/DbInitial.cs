using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using SushmaElectrical.Entities;
using SushmaElectrical.Infrastructure.GlobalConfiguration;
using SushmaElectrical.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Implementations
{
    public class DbInitial : IDbInitial
    {


        private UserManager<ApplicationUser> _userManager;


        private RoleManager<IdentityRole> _roleManager;


        public DbInitial(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public Task Seed()
        {
            try
            {
                if (!_roleManager.RoleExistsAsync(Role.Admin_Role).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(Role.Admin_Role)).GetAwaiter().GetResult();
                    var user = new ApplicationUser
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                        Mobile = "888888888",
                        Name = "Admin",
                        EmailConfirmed = true
                    };
                    _userManager.CreateAsync(user, "Admin@123").GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(user, (Role.Admin_Role)).GetAwaiter().GetResult();
                }
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
           
        }



    }
}
