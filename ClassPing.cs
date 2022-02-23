using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace LC_Service
{
    public class ClassPing
    {
        public bool GetHostInformation(out string pHostName, out string pMacAddress, out string pErrorMessage)
        {
            pMacAddress = pErrorMessage = string.Empty;

            try
            {
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] netInterface = NetworkInterface.GetAllNetworkInterfaces();

                pHostName = ipProperties.HostName;

                // No Network Card Error
                if (netInterface == null || netInterface.Length < 1)
                {
                    pHostName = pMacAddress = string.Empty;
                    pErrorMessage = " There is no Network Card";
                    return false; 
                }

                foreach (NetworkInterface adapter in netInterface)
                {
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        PhysicalAddress address = adapter.GetPhysicalAddress();
                        pMacAddress = address.ToString();
                    }
                }

                return true;
            }
            catch (Exception Exp)
            {
                pHostName = pMacAddress = string.Empty;
                pErrorMessage = Exp.Message;

                return false;
            }
        }

        public string GetDNSName(out string pErrorMessage)
        {
            try
            {
                string sDnsName = Dns.GetHostName();
                pErrorMessage = string.Empty;
                return sDnsName;
            }
            catch (SocketException sockExp)
            {
                pErrorMessage = sockExp.Message;
                return string.Empty;
            }
            catch (ArgumentNullException nullExp)
            {
                pErrorMessage = nullExp.Message;
                return string.Empty;
            }
            catch (ArgumentOutOfRangeException outExp)
            {
                pErrorMessage = outExp.Message;
                return string.Empty;
            }
            catch (ArgumentException argExp)
            {
                pErrorMessage = argExp.Message;
                return string.Empty;
            }
        }

        public string GetIPAddress(string sDnsName, out string pErrorMessage)
        {
            string sIPAddress = string.Empty;

            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(sDnsName);

                foreach (IPAddress ipAddress in ipHost.AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        sIPAddress = ipAddress.ToString();
                        break;
                    }
                }

                pErrorMessage = string.Empty;
                return sIPAddress;
            }
            catch (SocketException sockExp)
            {
                pErrorMessage = sockExp.Message;
                return string.Empty;
            }
            catch (ArgumentNullException nullExp)
            {
                pErrorMessage = nullExp.Message;
                return string.Empty;
            }
            catch (ArgumentOutOfRangeException outExp)
            {
                pErrorMessage = outExp.Message;
                return string.Empty;
            }
            catch (ArgumentException argExp)
            {
                pErrorMessage = argExp.Message;
                return string.Empty;
            }
        }

        public bool CheckNetwork(out string pErrorMessage)
        {
            try
            {
                string sHostName = Dns.GetHostName();
                IPHostEntry ipHost = Dns.GetHostEntry(sHostName);

                foreach (IPAddress ipAddress in ipHost.AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        break;
                    }
                }

                pErrorMessage = string.Empty;
                return true;
            }
            catch (SocketException sockExp)
            {
                pErrorMessage = sockExp.Message;
                return false;
            }
            catch (ArgumentNullException nullExp)
            {
                pErrorMessage = nullExp.Message;
                return false;
            }
            catch (ArgumentOutOfRangeException outExp)
            {
                pErrorMessage = outExp.Message;
                return false;
            }
            catch (ArgumentException argExp)
            {
                pErrorMessage = argExp.Message;
                return false;
            }
        }

        public bool PingTest(string pIP)
        {

            bool result = IPAddress.TryParse(pIP, out IPAddress ip);
            if (!result) return false;

            return PingTest(ip);
        }

        public bool PingTestDNS(string pDNS)
        {
            try
            {
                IPAddress[] addressList = Dns.GetHostAddresses(pDNS);
                return PingTest(addressList[0]);
            }
            catch
            {
                return false;
            }
        }

        private bool PingTest(IPAddress pIP)
        {
            bool result = false;

            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions
                    {
                        // Use the default Ttl value which is 128,
                        // but change the fragmentation behavior.
                        DontFragment = true
                    };

                    // Create a buffer of 32 bytes of data to be transmitted.
                    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    int timeout = 120;
                    PingReply reply = pingSender.Send(pIP, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        result = true;
                        break;
                    }

                    System.Threading.Thread.Sleep(10);
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        //private string GetDNSFromURL(string pURL)
        //{
        //    string sURL = pURL;

        //    int index = pURL.IndexOf("://");
        //    if ( index > 0)
        //    {
        //        sURL = pURL.Substring(index + 3);
        //    }

        //    index = sURL.IndexOf("/");
        //    if (index > 0)
        //        sURL = sURL.Substring(0, index);

        //    return sURL;
        //}
    }
}
