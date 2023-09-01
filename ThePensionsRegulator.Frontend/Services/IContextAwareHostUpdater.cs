namespace ThePensionsRegulator.Frontend.Services
{
    public interface IContextAwareHostUpdater
    {
        string UpdateHost(string destinationUrl, string requestHost);
    }
}