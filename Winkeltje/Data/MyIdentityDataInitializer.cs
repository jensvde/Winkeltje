using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using System.Web.Providers.Entities;
using Winkeltje.Data;

public class MyIdentityDataInitializer
{
    private ApplicationDbContext _context;

    public MyIdentityDataInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async void SeedAdminUser()
    {
        var user = new IdentityUser
        {
            UserName = "peggy.bouwen@telenet.be",
            NormalizedUserName = "peggy.bouwen@telenet.be",
            Email = "peggy.bouwen@telenet.be",
            NormalizedEmail = "peggy.bouwen@telenet.be",
            EmailConfirmed = true,
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var roleStore = new RoleStore<IdentityRole>(_context);

        if (!_context.Roles.Any(r => r.Name == "admin"))
        {
            await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });
        }

        if (!_context.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<IdentityUser>();
            var hashed = password.HashPassword(user, "password");
            user.PasswordHash = hashed;
            var userStore = new UserStore<IdentityUser>(_context);
            await userStore.CreateAsync(user);
            await userStore.AddToRoleAsync(user, "admin");
        }

        await _context.SaveChangesAsync();
    }
}