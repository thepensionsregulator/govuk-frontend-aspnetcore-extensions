using System.ComponentModel.DataAnnotations;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprAddress : ITprAddress
    {
        public virtual string? Uprn { get; set; }

        [Required(ErrorMessage = "Enter address line 1")]
        [MaxLength(100, ErrorMessage = "Address line 1 must be 100 characters or fewer")]
        public virtual string? AddressLine1 { get; set; }

        [MaxLength(100, ErrorMessage = "Address line 2 must be 100 characters or fewer")]
        public virtual string? AddressLine2 { get; set; }

        [MaxLength(100, ErrorMessage = "Address line 3 must be 100 characters or fewer")]
        public virtual string? AddressLine3 { get; set; }

        [Required(ErrorMessage = "Enter a town or city")]
        [MaxLength(100, ErrorMessage = "Town or city must be 100 characters or fewer")]
        public virtual string? PostTown { get; set; }

        [MaxLength(100, ErrorMessage = "County must be 100 characters or fewer")]
        public virtual string? PostCounty { get; set; }

        [Required(ErrorMessage = "Enter a postcode")]
        [MaxLength(10, ErrorMessage = "Postcode must be 10 characters or fewer")]
        public virtual string? Postcode { get; set; }

        public virtual int? CountryId { get; set; }

        public void PopulateAddress(ITprAddress address)
        {
            AddressLine1 = address.AddressLine1?.Trim();
            AddressLine2 = address.AddressLine2?.Trim();
            AddressLine3 = address.AddressLine3?.Trim();
            PostTown = address.PostTown?.Trim();
            PostCounty = address.PostCounty?.Trim();
            Postcode = address.Postcode?.Trim().ToUpper();
            CountryId = address.CountryId;
            Uprn = address.Uprn?.Trim();
        }
    }
}