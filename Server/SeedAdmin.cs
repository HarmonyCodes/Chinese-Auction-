// using Microsoft.AspNetCore.Identity;
// using Project.models;

// public static class SeedAdmin
// {
//     public static async Task SeedAdminAsync(UserManager<User> userManager, Roles<Admin> roleManager)
//     {
//         var roleExists = await roleManager.RoleExistsAsync("Admin");
//         if (!roleExists)
//         {
//             await roleManager.CreateAsync(new IdentityRole("Admin"));
//         }

//         var adminUser = await userManager.FindByEmailAsync("admin@example.com");
//         if (adminUser == null)
//         {
//             var user = new ApplicationUser
//             {
//                 UserName = "admin@example.com",
//                 Email = "admin@example.com",
//                 EmailConfirmed = true
//             };

//             var result = await userManager.CreateAsync(user, "YourStrongPassword!1");
//             if (result.Succeeded)
//             {
//                 await userManager.AddToRoleAsync(user, "Admin");
//                 Console.WriteLine("✅ Admin user created successfully!");
//             }
//             else
//             {
//                 foreach (var error in result.Errors)
//                 {
//                     Console.WriteLine($"❌ {error.Description}");
//                 }
//             }
//         }
//         else
//         {
//             Console.WriteLine("ℹ️ Admin user already exists.");
//         }
//     }
// }
