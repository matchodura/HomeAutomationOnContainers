﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPI.API.Utilities
{
    public static class PythonRunner
    {

        public static string RunScript(string scriptPath)
        {
            Console.WriteLine("Running python script!");
            //ProcessStartInfo start = new ProcessStartInfo();
            //start.FileName = "python3";

            //start.Arguments = scriptPath;



            //start.UseShellExecute = false;// Do not use OS shell
            //start.CreateNoWindow = false; // We don't need new window
            //start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            //start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)


            Console.WriteLine($"Script path: {scriptPath}");
            Console.WriteLine("Starting python process");
            //using var process = new Process
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        RedirectStandardOutput = true,
            //        UseShellExecute = true,
            //        CreateNoWindow = true,
            //        WindowStyle = ProcessWindowStyle.Hidden,
            //        FileName = "/bin/bash",
            //        Arguments = @$"{scriptPath}"
            //    }
            //};

            var startInfo = new ProcessStartInfo
            {
                FileName = @"python3",
                //Arguments = @$"-c ""{scriptPath}""",
                Arguments = @$"{scriptPath}",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine(result);

                Console.Read();
            }

            Console.WriteLine("Ended python process");


            return "Completed";
        }

    }
}
