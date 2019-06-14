using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Server
{
    public static class LabOff
    {
        static void Main(string[] args)
        {
            List<IPAddress> broadcastAddresses = getIPAddress();

            //IPAddress broadcastIPAddress = IPAddress.Broadcast;

            if(args.Length > 0)
            {
                byte[] sendbuf = Encoding.ASCII.GetBytes(args[0]);

                foreach(var broadcastIPAddress in broadcastAddresses)
                {
                    UdpClient udpClient = new UdpClient();
                    IPEndPoint ipEndPoint = new IPEndPoint(broadcastIPAddress, 40123);
                    Socket socket = new Socket(ipEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                    udpClient.Connect(broadcastIPAddress, 40123);

                    udpClient.EnableBroadcast = true;
                    udpClient.Send(sendbuf, sendbuf.Length);

                    udpClient.Close();
                }
                //socket.SendTo(sendbuf, SocketFlags.Broadcast, ipEndPoint);
            }
            else
            {
                Console.WriteLine("Please enter data to send");
            }
        }

        static List<IPAddress> getIPAddress()
        {
            List<IPAddress> ipAddresses = new List<IPAddress>();

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach(var nic in networkInterfaces)
            {
                UnicastIPAddressInformationCollection unicastIPPool = nic.GetIPProperties().UnicastAddresses;
                foreach(var ip in unicastIPPool)
                {
                    if(ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    { 
                        IPAddress temp = GetBroadcastAddress(ip.Address, ip.IPv4Mask);
                        if(!ipAddresses.Exists(address => address.Address.Equals(temp.Address)))
                            ipAddresses.Add(temp);
                    }
                }
            }

            //string hostName = Dns.GetHostName();
            //ipAddresses = Dns.GetHostByName(hostName).AddressList.ToList<IPAddress>();

            return ipAddresses;
        }

        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }
    }
}
