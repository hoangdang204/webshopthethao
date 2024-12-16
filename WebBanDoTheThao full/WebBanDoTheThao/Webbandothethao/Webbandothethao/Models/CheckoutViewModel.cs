public class CheckoutViewModel
{
    public List<CartItemViewModel> CartItems { get; set; }
    public List<AddressViewModel> Addresses { get; set; }
    public List<PaymentMethodViewModel> PaymentMethods { get; set; }
    public decimal TotalPrice { get; set; }
}

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class AddressViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public bool IsDefault { get; set; }
}

public class PaymentMethodViewModel
{
    public int Id { get; set; }
    public string MethodName { get; set; }
    public bool IsDefault { get; set; }
}
