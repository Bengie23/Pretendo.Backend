namespace Pretendo.Backend.Scripting
{
    /// <summary>
    /// Invokes a powershell script that register a given domain in the local host file.
    /// It should auto elevate rights
    /// Inspired by https://github.com/TomChantler/EditHosts
    /// </summary>
    public static class DomainCreator
    {
        /// <summary>
        /// Attempts to create a new record in the local host file.
        /// </summary>
        /// <param name="domainName"></param>
        public static void CreateDomain(string domainName)
        {
            "Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted; Get-ExecutionPolicy".ExecutePowerShell();

            var script = String.Format(@".\Scripting\echo.ps1 -Hostname {0}", domainName);
            script.ExecutePowerShell();

            "Set-ExecutionPolicy -Scope Process -ExecutionPolicy Restricted; Get-ExecutionPolicy".ExecutePowerShell();
        }
    }
}
