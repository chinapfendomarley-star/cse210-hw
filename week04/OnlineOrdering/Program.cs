using System;
using System.Collections.Generic;

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