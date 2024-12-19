namespace GovUk.Frontend.Umbraco.Blocks
{
    /// <summary>
    /// Intercept a block after its rendering settings have been set but before it is rendered.
    /// </summary>
    public interface IBlockViewInterceptor
    {
        /// <summary>
        /// Intercept a block after its rendering settings have been set but before it is rendered.
        /// </summary>
        /// <param name="blockViewModel"></param>
        void InterceptBlockView(BlockViewModel blockViewModel);
    }
}
