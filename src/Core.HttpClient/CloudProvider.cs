namespace Core.HttpClient
{
    /// <summary>
    /// Supported cloud providers.
    /// </summary>
    public enum CloudProvider
    {
        /// <summary>
        /// Unsupported Provider
        /// </summary>
        None,
        /// <summary>
        /// Microsoft Azure
        /// </summary>
        Azure,
        /// <summary>
        /// Amazon AWS
        /// </summary>
        Aws,
        /// <summary>
        /// Google
        /// </summary>
        Google,
        /// <summary>
        /// IBM
        /// </summary>
        Ibm,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle
    }
}
