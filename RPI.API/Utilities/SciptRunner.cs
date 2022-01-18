using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPI.API.Utilities
{
    public static class SciptRunner
    {
        public async static Task<string> RunPythonScript(string scriptPath)
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

        public async static Task<string> RunBashScript(string scriptPath)
        {
            string result = string.Empty;

            await Task.Run(() =>
            {

                Console.WriteLine($"Starting bash script - {scriptPath}");


                var startInfo = new ProcessStartInfo
                {
                    FileName = @"/bin/bash",
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

                Console.WriteLine($"Ended bash script - {scriptPath}");

            });

            return result;
        }

    }
}
