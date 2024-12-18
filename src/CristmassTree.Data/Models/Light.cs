namespace CristmassTree.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Light
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public float X { get; set; }

    [Required]
    public float Y { get; set; }

    [Required]
    [Range(3f, 6f)]
    public float Radius { get; set; }

    [Required]
    public Color Color { get; set; }
}