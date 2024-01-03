namespace WebApp.Data;

public class Price
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public decimal? Price1 { get; set; }

    public bool? IsSale { get; set; }

    public bool? IsActive { get; set; }

    public Product Product { get; set; }
}