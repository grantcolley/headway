using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        private Module home = new Module
        {
            Name = "Home",
            Order = 1,
            Categories = new List<Category>
            {
                new Category
                {
                    Name = "Home Category",
                    Order = 1,
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 1,
                            Name = "Home",
                            ImageClass = "oi oi-home",
                            Path = "/",
                            Order = 1,
                            Authorised = new List<string>{ "User" }
                        }
                    },
                    Authorised = new List<string>{ "User" }
                }
            },
            Roles = new List<string> { "User" }
        };

        private Module counter = new Module
        {
            Name = "Counter",
            Order = 2,
            Categories = new List<Category>
            {
                new Category
                {
                    Name = "Counter Category",
                    Order = 1,
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 2,
                            Name = "Counter",
                            ImageClass = "oi oi-plus",
                            Path = "counter",
                            Order = 1,
                            Authorised = new List<string>{ "User" }
                        }
                    },
                    Authorised = new List<string>{ "User" }
                }
            },
            Roles = new List<string> { "User" }
        };

        private Module weather = new Module
        {
            Name = "Weather",
            Order = 3,
            Categories = new List<Category>
            {
                new Category
                {
                    Name = "Weather Category",
                    Order = 1,
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 3,
                            Name = "Fetch data",
                            ImageClass = "oi oi-list-rich",
                            Path = "fetchdata",
                            Order = 1,
                            Authorised = new List<string>{ "User" }
                        }
                    },
                    Authorised = new List<string>{ "User" }
                }
            },
            Roles = new List<string> { "User" }
        };

        private Module configuration = new Module
        {
            Name = "Administration",
            Order = 4,
            Categories = new List<Category>
            {
                new Category
                {
                    Name = "Authorisation",
                    Order = 1,
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 5,
                            Name = "Users",
                            ImageClass = "oi oi-person",
                            Path = "users",
                            Order = 1,
                            Authorised = new List<string>{ "Admin" }
                        },
                        new MenuItem
                        {
                            Id = 6,
                            Name = "Permissions",
                            ImageClass = "oi oi-key",
                            Path = "permissions",
                            Order = 1,
                            Authorised = new List<string>{ "Admin" }
                        },
                        new MenuItem
                        {
                            Id = 6,
                            Name = "Roles",
                            ImageClass = "oi oi-lock-locked",
                            Path = "roles",
                            Order = 1,
                            Authorised = new List<string>{ "Admin" }
                        }
                    },
                    Authorised = new List<string>{ "Admin" }
                }
            },
            Roles = new List<string> { "Admin" }
        };

        public Task<IEnumerable<Module>> GetModulesAsync(string userName)
        {
            var modules = new List<Module> { home, counter, weather, configuration };
            return Task.FromResult<IEnumerable<Module>>(modules);
        }
    }
}