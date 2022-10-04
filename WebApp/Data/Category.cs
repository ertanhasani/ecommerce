using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Data;

public class Category
{
    public Category()
    {
        ProductCategory = new HashSet<ProductCategory>();
    }

    public int Id { get; set; }

    public int? ParentId { get; set; }

    [Required (ErrorMessage = "نام دسته بندی باید وارد گردد")]
    public string Name { get; set; }

    public string CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public string LasUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedOnDate { get; set; }

    public bool? IsDeleted { get; set; }

    [ForeignKey("ParentId")] public virtual Category Parent { get; set; }

    public ICollection<ProductCategory> ProductCategory { get; set; }

    public ICollection<Upload> Upload { get; set; }
}