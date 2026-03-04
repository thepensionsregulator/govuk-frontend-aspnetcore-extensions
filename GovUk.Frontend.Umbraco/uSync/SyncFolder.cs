using uSync.BackOffice.Configuration;

namespace GovUk.Frontend.Umbraco.uSync
{
    public class SyncFolder : ISyncFolder
    {
        public string Path => "uSync/ThePensionsRegulator.GovUk.Frontend.Umbraco/";

        public int Weight => -10000;
    }
}