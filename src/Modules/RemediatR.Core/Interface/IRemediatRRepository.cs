using Headway.Core.Args;
using Headway.Core.Interface;
using Headway.Core.Model;
using RemediatR.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemediatR.Core.Interface
{
    public interface IRemediatRRepository : IRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<IEnumerable<Customer>> GetCustomersAsync(SearchArgs searchArgs);
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int id);
        Task<IEnumerable<Program>> GetProgramsAsync();
        Task<Program> GetProgramAsync(int id);
        Task<Program> AddProgramAsync(Program program);
        Task<Program> UpdateProgramAsync(Program program);
        Task<int> DeleteProgramAsync(int id);
        Task<IEnumerable<RedressCase>> GetRedressCasesAsync(SearchArgs searchArgs);
        Task<IEnumerable<NewRedressCase>> SearchNewRedressCasesAsync(SearchArgs searchArgs);
        Task<Redress> CreateRedressAsync(DataArgs dataArgs);
        Task<Redress> GetRedressAsync(int id);
        Task<Redress> AddRedressAsync(Redress redress);
        Task<Redress> UpdateRedressAsync(Redress redress);
        Task<int> DeleteRedressAsync(int id);
    }
}
