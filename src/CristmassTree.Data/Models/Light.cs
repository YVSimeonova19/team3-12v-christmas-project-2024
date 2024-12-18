namespace CristmassTree.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Light
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; } = 1;

    [Required]
    public double X { get; set; }

    [Required]
    public double Y { get; set; }

    [Required]
    [Range(3f, 6f)]
    public double? Radius { get; set; }

    [Required]
    public string? Color { get; set; }

    [Required]
    public string? Effect { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public string? CT { get; set; }
}