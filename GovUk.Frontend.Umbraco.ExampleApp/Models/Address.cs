
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class Address : IAddress
    {
        public Address()
        {
        }


        public int? AddressReference { get; set; }

        [MaxLength(100)]
        public string? AddressLine1 { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AddressLine2 { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AddressLine3 { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AddressLine4 { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AddressLine5 { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? PostCode { get; set; } = string.Empty;

        public int? CountryId { get; set; }


        public string? Country { get; set; } = string.Empty;


        public string? CountryCode { get; set; } = string.Empty;
    }
}