﻿using Common.Enums;
using Network.API.NetworkScanner.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Network.API.NetworkScanner
{
    public static class Scanner
    {
        public static List<ScannedDevice> TotalScan()
        {
            int start = BitConverter.ToInt32(new byte[] { 0, 0, 168, 192 }, 0);
            int end = BitConverter.ToInt32(new byte[] { 255, 0, 168, 192 }, 0);
            List<ScannedDevice> devices = new List<ScannedDevice>();

            for (int i = start; i <= end; i++)
            {
                byte[] bytes = BitConverter.GetBytes(i);
                int timeout = 1000;
                var ipAddress = new IPAddress(new[] { bytes[3], bytes[2], bytes[1], bytes[0] });

                Ping ping = new Ping();
                PingReply pingresult = ping.Send(ipAddress.ToString(), timeout);

                if (pingresult.Status.ToString() == "Success")
                {
                    var hostname = GetHostName(ipAddress.ToString());
                    var macAddress = GetMacAddress(ipAddress.ToString());
                    devices.Add(new ScannedDevice() { HostName = hostname, IP = ipAddress.ToString(), MAC = macAddress, TimeOfScan = DateTime.UtcNow });
                }

            }

            return devices;
        }

        public static List<KnownDevices> ScanOfKnownDevices(string[] addresses)
        {
            int start = BitConverter.ToInt32(new byte[] { 0, 0, 168, 192 }, 0);
            int end = BitConverter.ToInt32(new byte[] { 255, 0, 168, 192 }, 0);
            List<KnownDevices> devices = new List<KnownDevices>();


            foreach(var address in addresses)
            {
                int timeout = 500;

                Ping ping = new Ping();
                PingReply pingresult = ping.Send(address.ToString(), timeout);

                if (pingresult.Status.ToString() == "Success")
                {           
                
                    devices.Add(new KnownDevices() {IP = address.ToString(), TimeOfScan = DateTime.UtcNow, Status = DeviceStatus.Online });
                }
                else
                {
                    devices.Add(new KnownDevices() {IP = address.ToString(), TimeOfScan = DateTime.UtcNow, Status = DeviceStatus.Offline });
                }
            }
               

            return devices;
        }
        public static KnownDevices ScanOfKnownDevices(string address)
        {
            int start = BitConverter.ToInt32(new byte[] { 0, 0, 168, 192 }, 0);
            int end = BitConverter.ToInt32(new byte[] { 255, 0, 168, 192 }, 0);
            KnownDevices device = new KnownDevices();

            int timeout = 500;

            Ping ping = new Ping();
            PingReply pingresult = ping.Send(address.ToString(), timeout);

            if (pingresult.Status.ToString() == "Success")
            {
                device = new KnownDevices() { IP = address.ToString(), TimeOfScan = DateTime.UtcNow, Status = DeviceStatus.Online };

            }
            else
            {
                device = new KnownDevices() { IP = address.ToString(), TimeOfScan = DateTime.UtcNow, Status = DeviceStatus.Offline };
            }
            
            return device;
        }


        public static List<ScannedDevice> ScanNewDevices(string[] addressesToOmit)
        {
            int start = BitConverter.ToInt32(new byte[] { 0, 0, 168, 192 }, 0);
            int end = BitConverter.ToInt32(new byte[] { 255, 0, 168, 192 }, 0);
            List<ScannedDevice> devices = new List<ScannedDevice>();

            for (int i = start; i <= end; i++)
            {
                byte[] bytes = BitConverter.GetBytes(i);
                int timeout = 250;
                var ipAddress = new IPAddress(new[] { bytes[3], bytes[2], bytes[1], bytes[0] });

                //check if currently running ip adress already exist
                if (!addressesToOmit.Contains(ipAddress.ToString()))
                {
                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(ipAddress.ToString(), timeout);

                    if (pingresult.Status.ToString() == "Success")
                    {
                        var hostname = GetHostName(ipAddress.ToString());
                        var macAddress = GetMacAddress(ipAddress.ToString());
                        devices.Add(new ScannedDevice() { HostName = hostname, IP = ipAddress.ToString(), MAC = macAddress, TimeOfScan = DateTime.UtcNow });
                    }
                }


            }

            return devices;
        }


        private static string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException ex)
            {
                //unknown host or
                //not every IP has a name
                //log exception (manage it)
            }

            return null;
        }

        private static string GetMacAddress(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a " + ipAddress;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + ":" + substrings[4] + ":" + substrings[5] + ":" + substrings[6]
                         + ":" + substrings[7] + ":"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "not found";
            }
        }
    }       
}