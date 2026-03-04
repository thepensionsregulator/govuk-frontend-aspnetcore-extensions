namespace ThePensionsRegulator.GovUk.Frontend.Security
{
    public interface INonceProvider
    {
        string GetNonce();
    }
}
