using Webbandothethao.Models;

public class UserOrdersViewModel
{
    public List<tblOrder> Orders { get; set; }  // List of orders
    public List<OrderDetailViewModel> OrderDetails { get; set; }  // List of order details
}


public class OrderDetailViewModel
{
    // Order details properties
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }

    // The order itself
    public tblOrder Orders { get; set; }  // This should be the order associated with the details
    public List<OrderDetailViewModel> OrderDetails { get; set; }  // List of order details
}

