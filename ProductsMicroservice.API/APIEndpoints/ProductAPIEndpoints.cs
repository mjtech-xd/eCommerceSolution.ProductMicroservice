using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;

namespace ProductsMicroservice.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //Get /api/products
        app.MapGet("/api/products", async (IProductService productsService) =>
        {
            List<ProductResponse?> products = await productsService.GetProducts();
            return Results.Ok(products);
        });
        
        //Get /api/products/search/productId
        app.MapGet("/api/products/search/product-id/{productId:guid}", async (IProductService productsService, Guid productId) =>
        {
            ProductResponse? product = await productsService.GetProductByCondition(x => x.ProductID == productId);
            return Results.Ok(product);
        });
        
        //Get /api/products/search/
        app.MapGet("/api/products/search/{searchString}", async (IProductService productsService, string searchString) =>
        {
            List<ProductResponse?> productsByProductName = await productsService.GetProductsByCondition(x => x.ProductName != null && x.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            List<ProductResponse?> productsByCategory = await productsService.GetProductsByCondition(x => x.Category != null && x.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            var products = productsByProductName.Union(productsByCategory);
            
            return Results.Ok(products);
        });
        
        //Delete /api/products/xxxxxxxxxxxxxxxx
        app.MapDelete("/api/products/{productId:guid}", async (IProductService productService, Guid productId) =>
        {
            bool isDeleted = await productService.DeleteProduct(productId);
            if(!isDeleted)
                return Results.Problem("An error occurred while deleting the product. Please try again later.");
            return Results.Ok(true);
        });
        
        //Post /api/products
        app.MapPost("/api/products", async (IProductService productService, IValidator<ProductAddRequest> productAddRequestValidator, ProductAddRequest productAddRequest) =>
        {
            ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);
            if(!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(grp => grp.Key, grp => grp
                        .Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }
            var response = await productService.AddProduct(productAddRequest);
            if(response == null)
                return Results.Problem("An error occurred while adding the product. Please try again later.");
            return Results.Created($"/api/products/search/productId/{response.ProductID}", response);
        });
        
        //PUT /api/products
        app.MapPut("/api/products", async (IProductService productService, IValidator<ProductUpdateRequest> productUpdateRequestValidator, ProductUpdateRequest productUpdateRequest) =>
        {
            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
            if(!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(grp => grp.Key, grp => grp
                        .Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }
            var updatedProductResponse = await productService.UpdateProduct(productUpdateRequest);
            if(updatedProductResponse == null)
                return Results.Problem("An error occurred while updating the product. Please try again later.");
            return Results.Ok(updatedProductResponse);
        });
        return app;
    }
}