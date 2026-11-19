using System;

// Product and calculating its cost for the ordered quantity.
class Product
{
    private string _name;
    private string _productId;
    private decimal _pricePerUnit;
    private int _quantity;
    private int _stockQuantity;
    private int _reorderThreshold;

    public Product(string name, string productId, decimal pricePerUnit, int quantity,
        int stockQuantity, int reorderThreshold)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A product name is required.", nameof(name));
        }
        if (string.IsNullOrWhiteSpace(productId))
        {
            throw new ArgumentException("A product ID is required.", nameof(productId));
        }
        if (pricePerUnit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pricePerUnit), "Price cannot be negative.");
        }
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Order quantity must be greater than zero.");
        }
        if (stockQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock quantity cannot be negative.");
        }
        if (reorderThreshold < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(reorderThreshold), "Reorder threshold cannot be negative.");
        }

        _name = name;
        _productId = productId;
        _pricePerUnit = pricePerUnit;
        _quantity = quantity;
        _stockQuantity = stockQuantity;
        _reorderThreshold = reorderThreshold;
    }

    public string GetName() => _name;
    public string GetProductId() => _productId;
    public int GetQuantity() => _quantity;
    public int GetStockQuantity() => _stockQuantity;

    public decimal CalculateTotalCost()
    {
        return _pricePerUnit * _quantity;
    }

    internal void EnsureStockAvailable(int requestedQuantity)
    {
        if (requestedQuantity > _stockQuantity)
        {
            throw new InvalidOperationException(
                $"Insufficient stock for {_name}: requested {requestedQuantity}, available {_stockQuantity}.");
        }
    }

    internal void DecreaseStock(int quantity)
    {
        _stockQuantity -= quantity;
    }

    public bool IsBelowReorderThreshold()
    {
        return _stockQuantity < _reorderThreshold;
    }

    public string GetRestockAlert()
    {
        return $"RESTOCK ALERT: {_name} (ID: {_productId}) has {_stockQuantity} in stock; " +
            $"reorder threshold is {_reorderThreshold}.";
    }
}