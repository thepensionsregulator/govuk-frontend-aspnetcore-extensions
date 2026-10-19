using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Models;

public class UmbracoTprAddress : ITprAddress
{
    public string? UPRNReference { get; set; }

    [Required]
    [MaxLength(100)]
    public string? AddressLine1 { get; set; }
    [MaxLength(100)]
    public string? AddressLine2 { get; set; }
    [MaxLength(100)]
    public string? AddressLine3 { get; set; }
    [Required]
    [MaxLength(100)]
    public string? PostTown { get; set; }
    [MaxLength(100)]
    public string? PostCounty { get; set; }
    public string? Postcode { get; set; }
    [Required]
    public int? CountryId { get; set; }
}
