using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(40123);
            while(true)
            {
                try
                {
                    IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Any, 40123);
                    Byte[] commandReceive = udpClient.Receive(ref ipEndpoint);
                    string command = Encoding.ASCII.GetString(commandReceive);
                    switch(command)
                    {
                        case "Shutdown":
                            Console.WriteLine("Going to LockDown Workstation");
                            Thread.Sleep(1000);
                            System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                            break;
                        case "Lockdown":
                            Console.WriteLine("Going to LockDown Workstation");
                            Thread.Sleep(1000);
                            System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                            break;
                        default:
                            Console.WriteLine($"{command}");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            udpClient.Close();
            Console.WriteLine("Ending Process");
            Console.ReadLine();
        }
    }
}
