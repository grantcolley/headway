using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IModuleRepository : IRepository
    {
        Task<IEnumerable<Module>> GetModulesAsync(string claim);
        Task<IEnumerable<Module>> GetModulesAsync();
        Task<Module> GetModuleAsync(int id);
        Task<Module> AddModuleAsync(Module module);
        Task<Module> UpdateModuleAsync(Module module);
        Task<int> DeleteModuleAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<int> DeleteCategoryAsync(int id);
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
        Task<MenuItem> GetMenuItemAsync(int id);
        Task<MenuItem> AddMenuItemAsync(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
        Task<int> DeleteMenuItemAsync(int id);
    }
}
