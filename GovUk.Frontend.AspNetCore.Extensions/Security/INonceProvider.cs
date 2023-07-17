namespace GovUk.Frontend.AspNetCore.Extensions.Security
{
    public interface INonceProvider
    {
        string GetNonce();
    }
}
