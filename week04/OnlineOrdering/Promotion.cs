using System;
using System.Collections.Generic;

// Representing a coupon applied to the full order or to a product ID.
class Promotion
{
    private string _code;
    private DiscountType _discountType;
    private decimal _amount;
    private string _productId;

    public Promotion(string code, DiscountType discountType, decimal amount, string productId = null)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("A promotion code is required.", nameof(code));
        }
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Discount amount must be greater than zero.");
        }
        if (discountType == DiscountType.Percentage && amount > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "A percentage discount cannot exceed 100.");
        }
        if (discountType != DiscountType.Percentage && discountType != DiscountType.FixedAmount)
        {
            throw new ArgumentOutOfRangeException(nameof(discountType));
        }

        _code = code;
        _discountType = discountType;
        _amount = amount;
        _productId = string.IsNullOrWhiteSpace(productId) ? null : productId;
    }

    public string GetCode() => _code;

    public decimal CalculateSavings(List<Product> products)
    {
        decimal eligibleSubtotal = 0m;
        int eligibleQuantity = 0;

        foreach (Product product in products)
        {
            if (_productId == null ||
                string.Equals(product.GetProductId(), _productId, StringComparison.OrdinalIgnoreCase))
            {
                eligibleSubtotal += product.CalculateTotalCost();
                eligibleQuantity += product.GetQuantity();
            }
        }

        if (_productId != null && eligibleQuantity == 0)
        {
            throw new InvalidOperationException(
                $"Promotion {_code} does not apply to a product in this order.");
        }

        if (_discountType == DiscountType.Percentage)
        {
            decimal percentageSavings = eligibleSubtotal * _amount / 100m;
            return decimal.Round(percentageSavings, 2, MidpointRounding.AwayFromZero);
        }

        // A fixed product discount applies once per unit; an order discount applies once.
        decimal fixedSavings = _productId == null ? _amount : _amount * eligibleQuantity;
        return decimal.Round(Math.Min(fixedSavings, eligibleSubtotal), 2, MidpointRounding.AwayFromZero);
    }
}