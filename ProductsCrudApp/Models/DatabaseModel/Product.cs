using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Range(100000, 999999)]
    public int ProductId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    [Required]
    public int StockAvailable { get; set; }
}
