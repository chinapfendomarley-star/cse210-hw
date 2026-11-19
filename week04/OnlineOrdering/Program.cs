using System;
using System.Collections.Generic;
using System.Text;

// Address and country in formatted-address form.
class Address
{
    private string _street;
    private string _city;
    private string _stateProvince;
    private string _country;

    public Address(string street, string city, string stateProvince, string country)
    {
        _street = street;
        _city = city;
        _stateProvince = stateProvince;
        _country = country;
    }

    public bool IsInUSA()
    {
        string normalized = _country.Trim();
        return normalized.Equals("USA", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("UNITED STATES", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("UNITED STATES OF AMERICA", StringComparison.OrdinalIgnoreCase);
    }

    public string GetFormattedAddress()
    {
        return $"{_street}\n{_city}, {_stateProvince}\n{_country}";
    }
}

// Connecting customers to their address for order details and shipping rates.
class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public bool IsInUSA()
    {
        return _address.IsInUSA();
    }

    public string GetCustomerDetails()
    {
        return $"{_name}\n{_address.GetFormattedAddress()}";
    }
}

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

enum DiscountType
{
    Percentage,
    FixedAmount
}

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

class Program
{
    static void Main()
    {
        // Domestic order to demonstrate the lower U.S. shipping rate.
        Address address1 = new Address("324 Evergreen Terrace", "Springfield", "IL", "USA");
        Customer customer1 = new Customer("Hank Voight", address1);
        List<Product> products1 = new List<Product>
        {
            new Product("Books", "BK-001", 29.99m, 1, 10, 2),
            new Product("Laptops", "LT-002", 899.99m, 1, 10, 2),
            new Product("Cameras", "CM-003", 499.00m, 1, 10, 2)
        };
        Promotion linePromotion = new Promotion("BOOK3", DiscountType.FixedAmount, 3.00m, "BK-001");
        Order order1 = new Order(customer1, products1, linePromotion);

        // Build an international order to demonstrate the higher shipping rate.
        Address address2 = new Address("18 Park Road", "Mutare", "Manicaland", "Zimbabwe");
        Customer customer2 = new Customer("Rumbidzai Moyo", address2);
        List<Product> products2 = new List<Product>
        {
            new Product("Furniture", "FR-004", 249.00m, 1, 5, 2),
            new Product("Shoes", "SH-005", 79.50m, 2, 10, 3)
        };
        Promotion orderPromotion = new Promotion("SAVE10", DiscountType.Percentage, 10m);
        Order order2 = new Order(customer2, products2, orderPromotion);

        // Orders imported from the first three entries in Giftlist.mdb (each with 2+ products).
        Address address3 = new Address("1280 Oakridge Drive", "Austin", "TX 78701", "USA");
        Customer customer3 = new Customer("Antoinette Goodchild", address3);
        List<Product> products3 = new List<Product>
        {
            new Product("Lamp", "GIFT-001", 29.99m, 1, 10, 2),
            new Product("Desk Shade", "GIFT-004", 14.50m, 1, 8, 2)
        };
        Order order3 = new Order(customer3, products3);

        Address address4 = new Address("459 Evergreen Terrace", "Springfield", "IL 23451", "USA");
        Customer customer4 = new Customer("Elenor Rastova", address4);
        List<Product> products4 = new List<Product>
        {
            new Product("Frame Print", "GIFT-002", 19.99m, 1, 10, 2),
            new Product("Matte Board", "GIFT-005", 9.99m, 1, 8, 2)
        };
        Order order4 = new Order(customer4, products4);

        Address address5 = new Address("315 Birchwood Court", "Boston", "MA 32176", "USA");
        Customer customer5 = new Customer("Talent Svongwa", address5);
        List<Product> products5 = new List<Product>
        {
            new Product("Laptop", "GIFT-003", 899.99m, 1, 10, 2),
            new Product("Laptop Sleeve", "GIFT-006", 24.99m, 1, 12, 2)
        };
        Order order5 = new Order(customer5, products5);

        // Printing each order's labels and total, including the applicable shipping cost.
        List<Order> orders = new List<Order> { order1, order2, order3, order4, order5 };

        for (int i = 0; i < orders.Count; i++)
        {
            Console.WriteLine($"*** ORDER #{i + 1} ***");
            Console.WriteLine(orders[i].GetPackingLabel());
            Console.WriteLine();
            Console.WriteLine(orders[i].GetShippingLabel());
            Console.WriteLine($"\nSubtotal: ${orders[i].CalculateSubtotal():F2}");
            Console.WriteLine(
                $"Promotion ({orders[i].GetPromotionCode()}): -${orders[i].CalculateSavings():F2}");
            Console.WriteLine($"Shipping: ${orders[i].CalculateShippingCost():F2}");
            Console.WriteLine($"Total Cost: ${orders[i].CalculateTotalCost():F2}");
            foreach (string alert in orders[i].GetRestockAlerts())
            {
                Console.WriteLine(alert);
            }
            Console.WriteLine(new string('=', 42));
        }
    }
}