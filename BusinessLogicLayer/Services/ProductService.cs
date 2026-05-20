using System.Linq.Expressions;
using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessLogicLayer.Services;

public class ProductService(
    IValidator<ProductAddRequest> productAddRequestValidator,
    IValidator<ProductUpdateRequest> productUpdateRequestValidator,
    IProductRepository productRepository,
    IMapper mapper) : IProductService
{
    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await productRepository.GetProducts();
        IEnumerable<ProductResponse?> responses = mapper.Map<IEnumerable<ProductResponse>>(products);
        return responses.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(
        Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product?> products = await productRepository.GetProductsByCondition(conditionExpression);
        IEnumerable<ProductResponse?> responses = mapper.Map<IEnumerable<ProductResponse>>(products);
        return responses.ToList();
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await productRepository.GetProductByCondition(conditionExpression);
        ProductResponse response = mapper.Map<ProductResponse>(product);
        return response;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
            throw new ArgumentNullException(nameof(productAddRequest));

        //Validator
        ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }

        var product = mapper.Map<Product>(productAddRequest);
        Product? addedProduct = await productRepository.AddProduct(product);
        if (addedProduct == null)
            return null;
        var response = mapper.Map<ProductResponse>(addedProduct);
        return response;
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        var existingProduct =
            await productRepository.GetProductByCondition(x => x.ProductID == productUpdateRequest.ProductID);
        if (existingProduct == null)
            throw new ArgumentNullException(nameof(existingProduct));
        //Validation
        var validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }

        var product = mapper.Map<Product>(productUpdateRequest);
        var updatedProduct = await productRepository.UpdateProduct(product);
        var response = mapper.Map<ProductResponse>(updatedProduct);
        return response;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? existingProduct = await productRepository.GetProductByCondition(x => x.ProductID == productId);
        if (existingProduct == null)
            return false;
        bool isDeleted = await productRepository.DeleteProduct(productId);
        return isDeleted;
    }
}