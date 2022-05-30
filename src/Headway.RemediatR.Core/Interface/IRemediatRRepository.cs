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
    }
}
