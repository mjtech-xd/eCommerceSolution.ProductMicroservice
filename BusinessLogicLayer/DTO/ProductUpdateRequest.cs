namespace BusinessLogicLayer.DTO;

public record ProductUpdateRequest(Guid ProductID, string? ProductName, CategoryOptions Category, double UnitPrice, int? QuantityInStock)
{
    public ProductUpdateRequest(): this(Guid.Empty, null, default, 0, null)
    {
    }
}