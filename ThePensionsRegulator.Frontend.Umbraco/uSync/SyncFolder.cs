using uSync.BackOffice.Configuration;

namespace ThePensionsRegulator.Frontend.Umbraco.uSync
{
    public class SyncFolder : ISyncFolder
    {
        public string Path => "uSync/ThePensionsRegulator.Frontend.Umbraco/";

        public int Weight => -9999;
    }
}