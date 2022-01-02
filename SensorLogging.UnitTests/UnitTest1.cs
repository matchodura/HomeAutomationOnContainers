using SensorLogging.API;
using System;
using Xunit;

namespace SensorLogging.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void PythonRunnerTest()
        {
            {
                var mainLocation = @"C:\skrypt.py";
                var scriptPath = mainLocation + @"mijia/MiTemperature2/LYWSD03MMC.py";
                var dataLocation = mainLocation + @"Results/data.txt";

                string[] scriptParams = new string[10];

                scriptParams[0] = "-d";
                scriptParams[1] = "A4:C1:38:48:31:DF";
                scriptParams[2] = "-r";
                scriptParams[3] = "-b";
                scriptParams[4] = "-c";
                scriptParams[5] = "1";
                scriptParams[6] = "--name";
                scriptParams[7] = "MySensor";
                scriptParams[8] = "--callback";
                scriptParams[9] = "sendToFile.sh";

                string pythonVersion = "python3";

                Console.WriteLine(scriptPath);
                Console.WriteLine(scriptParams);
               // var result = PythonRunner.RunScript(pythonVersion, scriptPath, dataLocation, scriptParams);
            }
        }
    }
}
