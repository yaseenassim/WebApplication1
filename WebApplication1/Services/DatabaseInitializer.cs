using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Services
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync(UserManager<IdentityUser>? userManager, RoleManager<IdentityRole>? roleManager)
        {
            if(userManager == null || roleManager == null)
            {
                Console.WriteLine("userManager or roleManager is null => exit");
                return;
            }

            //Check if there is an admin role or not
            var exists = await roleManager.RoleExistsAsync("admin");
            if(!exists)
            {
                Console.WriteLine("Admin role not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            //Check if there is a seller role or not
            exists = await roleManager.RoleExistsAsync("seller");
            if(!exists)
            {
                Console.WriteLine("Seller role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }

            //Check if there is a client role
            exists = await roleManager.RoleExistsAsync("client");
            if(!exists)
            {
                Console.WriteLine("client role not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("client"));
            }

            //Check if we have an admin user
            var adminUsers = await userManager.GetUsersInRoleAsync("admin");
            if(adminUsers.Any())
            {
                //Admin user exists => exit
                Console.WriteLine("Admin user already exists => exit");
                return;
            }

            //Create an admin user
            var user = new IdentityUser()
            {
                UserName = "yaseenassim5@gmail.com", //Used to authenticate user
                Email = "yaseenassim5@gmail.com",
            };

            string initialPassword = "Admin123*";

            var result = await userManager.CreateAsync(user, initialPassword);
            if (result.Succeeded)
            {
                //set user role
                await userManager.AddToRoleAsync(user, "admin");
                Console.WriteLine("Admin role created! Please update the initial password!");
                Console.WriteLine("Email: ", user.Email);
                Console.WriteLine("Initial Password: ", initialPassword);
            }
            else
            {
                Console.WriteLine("Unable to create user admin account: " + result.Errors.First().Description);
            }
        }
    }
}
