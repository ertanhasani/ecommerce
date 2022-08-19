namespace WebApp.Data;

public class OrderDetails
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? OrderId { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public decimal? Total { get; set; }

    public string CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public string LasUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedOnDate { get; set; }

    public bool? IsDeleted { get; set; }

    public Order Order { get; set; }

    public Product Product { get; set; }
}