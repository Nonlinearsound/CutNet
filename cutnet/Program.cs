using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace cutnet
{
    class Program
    {
        

        static void Main(string[] args)
        {
            var arrConnStates = new[] { "Disconnected", 
                "Connecting", 
                "--> Connected <--", 
                "Disconnecting ", 
                "Hardware Not Present", 
                "Hardware Disabled",
                "Hardware Malfunction",
                "Media Disconnected" };

            string mode = "disable";
            if(args.Length >= 1)
            {
                if (args[0] == "info")
                {
                    mode = "info";
                }
                if (args[0] == "enable")
                {
                    mode = "enable";

                }
                SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
                ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    if( item["NetConnectionId"]!=null)
                    {
                        string connId = (string)item["NetConnectionId"];
                        if(mode == "info")
                        {
                            try
                            {
                                Console.WriteLine("Adapter Info:");
                                Console.WriteLine("NetConnectionId: " + item["NetConnectionId"]);
                                if( ((UInt16)item["NetConnectionStatus"]) <= 7)
                                {
                                    Console.WriteLine("Connection State: " + arrConnStates[((UInt16)item["NetConnectionStatus"])] );
                                }
                                else
                                {
                                    Console.WriteLine("Connection State: " + item["NetConnectionStatus"]);
                                }
                                Console.WriteLine("ProductName: " + item["ProductName"]);
                                Console.WriteLine("ServiceName: " + item["ServiceName"]);

                                Console.WriteLine("NetConnectionStatus: "+  item["NetConnectionStatus"]);
                                Console.WriteLine("PhysicalAdapter: " + item["PhysicalAdapter"]);
                                Console.WriteLine("SystemName: " + item["SystemName"]);
                                Console.WriteLine("AdapterType: " + item["AdapterType"]);
                                Console.WriteLine("AdapterTypeID: " + item["AdapterTypeID"]);
                                Console.WriteLine("NetworkAddresses: " + item["NetworkAddresses"]);
                                Console.WriteLine("CreationClassName: " + item["CreationClassName"]);
                                Console.WriteLine("---------\n");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        else
                        {
                            if (connId.Contains("Ethernet") || connId.Contains("WLAN"))
                            {
                                UInt16 ret = 0;
                                if (item["NetConnectionStatus"] != null)
                                {
                                    ret = ((UInt16)item["NetConnectionStatus"]);
                                }

                                if (ret == 2)   // Adapter is Connected
                                {
                                    Console.WriteLine("Network adapter " + item["NetConnectionId"] + " Product(" + item["ProductName"] + ")" + " has been " + mode + "d!");
                                    item.InvokeMethod(mode, null);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Done.");
            }
        }
    }
}

/*
  		item["NetConnectionStatus"]	2	object {ushort}
		item["PhysicalAdapter"]	true	object {bool}
		item["NetConnectionId"]	"WLAN"	object {string}
		item["SystemName"]	"DESKTOP-HSUI89H"	object {string}
		item["AdapterType"]	"Ethernet 802.3"	object {string}
		item["AdapterTypeID"]	0	object {ushort}
		item["NetworkAddresses"]	null	object
		item["CreationClassName"]	"Win32_NetworkAdapter"	object {string}
		item["ProductName"]	"Intel(R) Wi-Fi 6 AX200 160MHz"	object {string}
		item["ServiceName"]	"Netwtw10"	object {string}
*/
