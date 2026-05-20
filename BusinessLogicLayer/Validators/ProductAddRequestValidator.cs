using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product Name is required");
        
        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category is required");
        
        RuleFor(x => x.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage($"Unit Price should between 0 and {double.MaxValue}");
        
        RuleFor(x => x.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity In Stock should between 0 and {int.MaxValue}");
    }
}