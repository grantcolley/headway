using Headway.Core.Interface;
using Headway.RemediatR.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RemediatR.Core.Interface
{
    public interface IRemediatRRepository : IRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<int> DeleteProductAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int id);
        Task<IEnumerable<Program>> GetProgramsAsync();
        Task<Program> GetProgramAsync(int id);
        Task<Program> AddProgramAsync(Program program);
        Task<Program> UpdateProgramAsync(Program program);
        Task<int> DeleteProgramAsync(int id);
        Task<IEnumerable<Redress>> GetRedressesAsync();
        Task<Redress> GetRedressAsync(int id);
        Task<Redress> AddRedressAsync(Redress redress);
        Task<Redress> UpdateRedressAsync(Redress redress);
        Task<int> DeleteRedressAsync(int id);
    }
}
