using System;
using System.Collections.Generic;
using System.Text;

// Customer's products, shipping cost, and printable order labels.
class Order
{
    private List<Product> _products;
    private Customer _customer;
    private Promotion _promotion;
    private decimal _savings;
    private List<string> _restockAlerts;

    public Order(Customer customer, List<Product> products, Promotion promotion = null)
    {
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer));
        }
        if (products == null)
        {
            throw new ArgumentNullException(nameof(products));
        }
        if (products.Count == 0)
        {
            throw new ArgumentException("An order must contain at least one product.", nameof(products));
        }

        _customer = customer;
        _products = new List<Product>(products);
        foreach (Product product in _products)
        {
            if (product == null)
            {
                throw new ArgumentException("An order cannot contain a null product.", nameof(products));
            }
        }

        _promotion = promotion;
        _savings = promotion == null ? 0m : promotion.CalculateSavings(_products);

        // Validate every line before reducing stock so a rejected order leaves inventory unchanged.
        Dictionary<Product, int> requestedQuantities = new Dictionary<Product, int>();
        foreach (Product product in _products)
        {
            if (requestedQuantities.ContainsKey(product))
            {
                requestedQuantities[product] += product.GetQuantity();
            }
            else
            {
                requestedQuantities.Add(product, product.GetQuantity());
            }
        }

        foreach (KeyValuePair<Product, int> item in requestedQuantities)
        {
            item.Key.EnsureStockAvailable(item.Value);
        }
        foreach (KeyValuePair<Product, int> item in requestedQuantities)
        {
            item.Key.DecreaseStock(item.Value);
        }

        // Restock alerts after this order changes inventory.
        _restockAlerts = new List<string>();
        HashSet<string> alertedProductIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Product product in _products)
        {
            if (product.IsBelowReorderThreshold() && alertedProductIds.Add(product.GetProductId()))
            {
                _restockAlerts.Add(product.GetRestockAlert());
            }
        }
    }

    public decimal CalculateSubtotal()
    {
        decimal productsTotal = 0m;
        foreach (Product product in _products)
        {
            productsTotal += product.CalculateTotalCost();
        }

        return productsTotal;
    }

    public decimal CalculateSavings() => _savings;

    public string GetPromotionCode() => _promotion == null ? "None" : _promotion.GetCode();

    public decimal CalculateShippingCost()
    {
        return _customer.IsInUSA() ? 5.00m : 35.00m;
    }

    public decimal CalculateTotalCost()
    {
        // Savings to products before adding the customer's shipping charge.
        return CalculateSubtotal() - CalculateSavings() + CalculateShippingCost();
    }

    public List<string> GetRestockAlerts()
    {
        return new List<string>(_restockAlerts);
    }

    public string GetPackingLabel()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== PACKING LABEL ===");
        foreach (Product product in _products)
        {
            sb.AppendLine($"Product: {product.GetName()} (ID: {product.GetProductId()})");
        }
        return sb.ToString().TrimEnd();
    }

    public string GetShippingLabel()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== SHIPPING LABEL ===");
        sb.AppendLine(_customer.GetCustomerDetails());
        return sb.ToString().TrimEnd();
    }
}