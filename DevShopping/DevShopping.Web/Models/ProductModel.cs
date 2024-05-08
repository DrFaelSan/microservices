using System.ComponentModel.DataAnnotations;

namespace DevShopping.Web.Models;

public class ProductModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public string ImageURL { get; set; }

    [Range(1,100)]
    public int Count { get; set; } = 1;
    
    public string SubStringName()
        =>  (Name.Length < 24) ? Name : $"{Name[..21]} ...";

    public string SubStringDescription()
        =>  (Description.Length < 355) ?  Description : $"{Description[..352]} ...";
}
