namespace ThePensionsRegulator.GovUk.Frontend.Security
{
    public interface IConsentCookieReader
    {
        /// <summary>
        /// Checks whether the user has granted their consent to all cookies.
        /// </summary>
        /// <returns><c>true</c> if consent granted. <c>false</c> if consent refused, or no response recorded.</returns>
        bool HasConsent();

        /// <summary>
        /// Checks whether the user has granted their consent to a specific category of cookies.
        /// </summary>
        /// <returns><c>true</c> if consent granted. <c>false</c> if consent refused, or no response recorded.</returns>
        bool HasConsent(string category);

        /// <summary>
        /// Checks whether the consent cookie is present in the user browser.
        /// </summary>
        /// <returns><c>true</c> if cookie is present. <c>false</c> if cookie is not present.</returns>
        bool IsConsentCookiePresent();
    }
}
