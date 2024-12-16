public class EditOrderViewModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public List<EditOrderDetailViewModel> OrderDetails { get; set; }
}

public class EditOrderDetailViewModel
{
    public int OrderDetailId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
