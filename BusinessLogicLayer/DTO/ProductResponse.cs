namespace BusinessLogicLayer.DTO;

public record ProductResponse(Guid ProductID, string? ProductName, CategoryOptions Category, double UnitPrice, int? QuantityInStock)
{
    public ProductResponse(): this(Guid.Empty, null, default, 0, null)
    {
    }
}