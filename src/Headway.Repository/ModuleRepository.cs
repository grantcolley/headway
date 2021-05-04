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
                            Rights = new List<string>{ "headwayuser-read" }
                        }
                    },
                    Rights = new List<string>{ "headwayuser-read" }
                }
            },
            Roles = new List<string> { "headwayuser" }
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
                            Rights = new List<string>{ "headwayuser-read", "headwayuser-write" }
                        }
                    },
                    Rights = new List<string>{ "headwayuser-read", "headwayuser-write" }
                }
            },
            Roles = new List<string> { "headwayuser" }
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
                            Rights = new List<string>{ "headwayuser-read", "headwayuser-write" }
                        }
                    },
                    Rights = new List<string>{ "headwayuser-read", "headwayuser-write" }
                }
            },
            Roles = new List<string> { "headwayuser" }
        };

        private Module configuration = new Module
        {
            Name = "Configuration",
            Order = 4,
            Categories = new List<Category>
            {
                new Category
                {
                    Name = "Users",
                    Order = 1,
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 5,
                            Name = "User",
                            ImageClass = "oi oi-person",
                            Path = "user",
                            Order = 1,
                            Rights = new List<string>{ "headwayadmin-write" }
                        }
                    },
                    Rights = new List<string>{ "headwayadmin-write" }
                }
            },
            Roles = new List<string> { "headwayadmin" }
        };

        public Task<IEnumerable<Module>> GetModulesAsync(string userName)
        {
            var modules = new List<Module> { home, counter, weather, configuration };
            return Task.FromResult<IEnumerable<Module>>(modules);
        }
    }
}