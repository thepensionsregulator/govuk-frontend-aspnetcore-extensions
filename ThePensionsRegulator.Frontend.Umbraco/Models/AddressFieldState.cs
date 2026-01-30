namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public record AddressFieldState(
        string ModelPropertyName,
        string Label,
        string InvalidAriaLabel,
        bool HasErrorMessage,
        string? AttemptedValue,
        string? ErrorMessage
  );
}
