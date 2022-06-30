namespace GovUk.Frontend.Umbraco.Services
{
    public interface IContextAwareHostUpdater
    {
        string UpdateHost(string destinationUrl, string requestHost);
    }
}