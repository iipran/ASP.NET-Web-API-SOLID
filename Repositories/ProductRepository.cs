// Copyright (c) 2023 Irfan Kertianto. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace MySolidAPI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MySolidAPI.Data;
    using MySolidAPI.Models;

    /// <summary>
    /// Represents a repository for managing <see cref="Product"/> entities.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly AppDataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
        public ProductRepository(AppDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<Product?> GetProductById(int productId)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _context.Add(product);
            return await SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _context.Update(product);
            await SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await GetProductById(productId);
            if (product == null)
            {
                return false;
            }

            _context.Remove(product);
            return await SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> ProductExists(int productId)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == productId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<dynamic>> ExecuteSQLQuery(string query, params object[] parameters)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _context.Products
                .FromSqlRaw(query, parameters)
                .ToListAsync<dynamic>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<dynamic>> ExecuteStoredProcedure(string procedureName, params object[] parameters)
        {
            if (string.IsNullOrEmpty(procedureName))
            {
                throw new ArgumentNullException(nameof(procedureName));
            }

            var sql = $"EXEC {procedureName}";

            if (parameters?.Length > 0)
            {
                var parameterNames = Enumerable.Range(0, parameters.Length)
                    .Select(i => $"@p{i}")
                    .ToArray();

                sql += " " + string.Join(", ", parameterNames);
            }

            return await _context.Products
                .FromSqlRaw(sql, parameters ?? new object[0])
                .ToListAsync<dynamic>();
        }

        private async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

