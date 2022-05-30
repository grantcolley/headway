using Headway.RemediatR.Core.Interface;
using Headway.RemediatR.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RemediatR.Repository
{
    public class RemediatRRepository : IRemediatRRepository
    {
        public Task<Product> AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsAuthorisedAsync(string claim, string permission)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}