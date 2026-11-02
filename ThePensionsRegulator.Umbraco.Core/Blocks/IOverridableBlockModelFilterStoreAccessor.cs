namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <summary>
    /// Resolves the current request-scoped block filter store for cached block models.
    /// </summary>
    public interface IOverridableBlockModelFilterStoreAccessor
    {
        IOverridableBlockModelFilterStore Get();
    }
}
