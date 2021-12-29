using SensorLogging.API.Utilities;
using System;
using Xunit;

namespace SensorLogging.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void PythonRunnerTest()
        {
            var scriptPath = @"C:\skrypt.py";
            //var scriptPath = @"/home/pi/mijia/MiTemperature2/LYWSD03MMC.py";
            //ar scriptParams = "-d " + macAdress + " -r -b -c " + retries;

            string pythonVersion = "python";
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

            var result = PythonRunner.RunScript(pythonVersion, scriptPath, scriptParams);
            Console.WriteLine(result);
        }
    }
}
