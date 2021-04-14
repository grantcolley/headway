// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Headway.IdentityProvider.Data;
using Headway.IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Headway.IdentityProvider
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlite(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "AliceSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("alice created");
                    }
                    else
                    {
                        Log.Debug("alice already exists");
                    }

                    var bob = userMgr.FindByNameAsync("bob").Result;
                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Email = "BobSmith@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("bob created");
                    }
                    else
                    {
                        Log.Debug("bob already exists");
                    }

                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    var weatherUser = roleMgr.FindByNameAsync("weatheruser").Result;
                    if (weatherUser == null)
                    {
                        weatherUser = new IdentityRole
                        {
                            Id = "weatheruser",
                            Name = "weatheruser",
                            NormalizedName = "weatheruser"
                        };

                        var weatherUserResult = roleMgr.CreateAsync(weatherUser).Result;
                        if (!weatherUserResult.Succeeded)
                        {
                            throw new Exception(weatherUserResult.Errors.First().Description);
                        }

                        var aliceRoleResult = userMgr.AddToRoleAsync(alice, weatherUser.Name).Result;
                        if (!aliceRoleResult.Succeeded)
                        {
                            throw new Exception(aliceRoleResult.Errors.First().Description);
                        }

                        Log.Debug("weatheruser created");
                    }
                    else
                    {
                        Log.Debug("weatheruser already exists");
                    }

                    var headwayAdmin = roleMgr.FindByNameAsync("headwayadmin").Result;
                    if (headwayAdmin == null)
                    {
                        headwayAdmin = new IdentityRole
                        {
                            Id = "headwayadmin",
                            Name = "headwayadmin",
                            NormalizedName = "headwayadmin"
                        };

                        var headwayAdminResult = roleMgr.CreateAsync(headwayAdmin).Result;
                        if (!headwayAdminResult.Succeeded)
                        {
                            throw new Exception(headwayAdminResult.Errors.First().Description);
                        }

                        var aliceRoleResult = userMgr.AddToRoleAsync(alice, headwayAdmin.Name).Result;
                        if (!aliceRoleResult.Succeeded)
                        {
                            throw new Exception(aliceRoleResult.Errors.First().Description);
                        }

                        Log.Debug("headwayadmin created");
                    }
                    else
                    {
                        Log.Debug("headwayadmin already exists");
                    }

                    var blazorUser = roleMgr.FindByNameAsync("headwayuser").Result;
                    if (blazorUser == null)
                    {
                        blazorUser = new IdentityRole
                        {
                            Id = "headwayuser",
                            Name = "headwayuser",
                            NormalizedName = "headwayuser"
                        };

                        var blazorUserResult = roleMgr.CreateAsync(blazorUser).Result;
                        if (!blazorUserResult.Succeeded)
                        {
                            throw new Exception(blazorUserResult.Errors.First().Description);
                        }

                        var aliceRoleResult = userMgr.AddToRoleAsync(alice, blazorUser.Name).Result;
                        if (!aliceRoleResult.Succeeded)
                        {
                            throw new Exception(aliceRoleResult.Errors.First().Description);
                        }

                        var bobRoleResult = userMgr.AddToRoleAsync(bob, blazorUser.Name).Result;
                        if (!bobRoleResult.Succeeded)
                        {
                            throw new Exception(bobRoleResult.Errors.First().Description);
                        }

                        Log.Debug("headwayuser created");
                    }
                    else
                    {
                        Log.Debug("headwayuser already exists");
                    }
                }
            }
        }
    }
}
