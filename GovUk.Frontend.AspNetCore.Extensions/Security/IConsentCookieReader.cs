namespace GovUk.Frontend.AspNetCore.Extensions.Security
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
    }
}
