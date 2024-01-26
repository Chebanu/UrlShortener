using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Contracts.Models;

[Table("url_shortener")]
public class UrlShortenerModel
{
    [Key]
    [Column("modified_url")]
    public string ModifiedUrl { get; set; }

    [Required]
    [Column("origin_url")]
    public string OriginUrl { get; set; }

    [Required]
    [Column("create_date")]
    public DateTime CreateDate { get; set; }
}