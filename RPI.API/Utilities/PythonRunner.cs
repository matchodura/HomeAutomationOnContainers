using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPI.API.Utilities
{
    public static class PythonRunner
    {
        public async static Task<string> RunScript(string scriptPath)
        {
            string result = string.Empty;

            await Task.Run(() =>
            {

                Console.WriteLine("Starting python process");


                var startInfo = new ProcessStartInfo
                {
                    FileName = @"python3",
                    Arguments = @$"{scriptPath}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }

                Console.WriteLine("Ended python process");

            });

            return result;
        }

    }
}
