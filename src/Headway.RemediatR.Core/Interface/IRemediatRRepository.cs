using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RemediatR.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RemediatR.Core.Interface
{
    public interface IRemediatRRepository : IRepository
    {
        Task<Product> AddCustomerProductAsync(Customer customer, Product product);
        Task<int> DeleteCustomerProductAsync(Customer customer, Product product);
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<IEnumerable<Customer>> GetCustomersAsync(SearchCriteria searchCriteria);
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int id);
        Task<IEnumerable<Program>> GetProgramsAsync();
        Task<Program> GetProgramAsync(int id);
        Task<Program> AddProgramAsync(Program program);
        Task<Program> UpdateProgramAsync(Program program);
        Task<int> DeleteProgramAsync(int id);
        Task<IEnumerable<RedressCase>> GetRedressesAsync();
        Task<Redress> GetRedressAsync(int id);
        Task<Redress> AddRedressAsync(Redress redress);
        Task<Redress> UpdateRedressAsync(Redress redress);
        Task<int> DeleteRedressAsync(int id);
    }
}
