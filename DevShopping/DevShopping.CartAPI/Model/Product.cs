﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShopping.CartAPI.Model;


[Table("product")]
public class Product 
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Column("price")]
    [Required]
    [Range(1, 10000000)]
    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Column("decription")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Column("category_name")]
    [StringLength(50)]
    public string? CategoryName { get; set; }

    [Column("image_url")]
    [StringLength(300)]
    public string? ImageURL { get; set; }
}
