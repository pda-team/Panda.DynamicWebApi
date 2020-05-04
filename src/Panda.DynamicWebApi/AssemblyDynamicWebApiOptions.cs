namespace Panda.DynamicWebApi
{
    /// <summary>
    /// Specifies the dynamic webapi options for the assembly.
    /// </summary>
    public class AssemblyDynamicWebApiOptions
    {
        /// <summary>
        /// Routing prefix for all APIs
        /// <para></para>
        /// Default value is "api".
        /// </summary>
        public string ApiPrefix { get; set; }

        /// <summary>
        /// API HTTP Verb.
        /// <para></para>
        /// Default value is "POST".
        /// </summary>
        public string HttpVerb { get; set; }
    }
}