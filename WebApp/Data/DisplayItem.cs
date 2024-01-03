namespace WebApp.Data;

public class DisplayItem
{
    public int Id { get; set; }
    
    public string Path { get; set; }
    
    public string FileName { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string ShortDescription { get; set; }
    
    public string PageName { get; set; }
    
    public string SectionName { get; set; }
    
    public string ClassName { get; set; }
    
    public int Order { get; set; }
    
    public bool? IsDeleted { get; set; }
}