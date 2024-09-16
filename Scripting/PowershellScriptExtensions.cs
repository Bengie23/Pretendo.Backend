using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Pretendo.Backend.Scripting
{
    /// <summary>
    /// Extension methods for powershell scripts
    /// </summary>
    public static class PowershellScriptExtensions
    {
        /// <summary>
        /// Executes a given powershell script
        /// </summary>
        /// <param name="script"></param>
        public static void ExecutePowerShell(this string script)
        {
            using (PowerShell PS = PowerShell.Create())
            {
                PS.AddScript(script);
                Collection<PSObject> results = PS.Invoke();
                foreach (PSObject obj in results)
                {
                    Console.WriteLine(obj.ToString());
                }
            }
        }
    }
}
