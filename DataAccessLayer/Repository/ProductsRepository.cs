using System.Linq.Expressions;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class ProductsRepository(ApplicationDbContext dbContext) : IProductRepository
{
    public async Task<IEnumerable<Product?>> GetProducts()
    {
        return await dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        var products = await dbContext.Products.ToListAsync();
        return products.AsEnumerable().Where(conditionExpression.Compile());
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await dbContext.Products.FirstOrDefaultAsync(conditionExpression);
    }

    public async Task<Product?> AddProduct(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == product.ProductID);
        if (existingProduct == null)
            return null;
        existingProduct.ProductName = product.ProductName;
        existingProduct.Category = product.Category;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.QuantityInStock = product.QuantityInStock;
        await dbContext.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? existingProduct = await dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productId);
        if (existingProduct == null)
            return false;
        dbContext.Remove(existingProduct);
        int affectedRowsCount = await dbContext.SaveChangesAsync();
        return affectedRowsCount > 1;
    }
}