namespace IISAuthen.Helpers
{
    public class SaapClient
    {
        /// <summary>
        /// Dummy implementation for SaapClient.
        /// In company project, this decrypts or fetches secrets.
        /// In personal project, it simply returns the connection string as-is.
        /// </summary>
        /// <param name="connectionString">The connection string from configuration.</param>
        /// <returns>The original connection string.</returns>
        public static string GetConnection(string connectionString)
        {
            // In personal project, just return as-is (no decryption).
            return connectionString;
        }
    }
}