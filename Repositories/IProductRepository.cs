using MySolidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySolidAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product?> GetProductById(int productId);
        Task<bool> CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
        Task<bool> ProductExists(int productId);
        Task<IEnumerable<dynamic>> ExecuteSQLQuery(string query, params object[] parameters);
        Task<IEnumerable<dynamic>> ExecuteStoredProcedure(string procedureName, params object[] parameters);
    }
}

