using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Win32;

namespace ipChangeTools.Tools
{
    /// <summary>
    ///  网络设置类，设置网络的各种参数（DNS、网关、子网掩码、IP）
    /// </summary>
    class IpManager
    {
        public IpManager()
        {

        }

        /// <summary>  
        /// 修改IP地址  
        /// </summary>  
        /// <param name="nic"></param>  
        /// <param name="ip"></param>  
        /// <param name="mask"></param>  
        /// <param name="way"></param>  
        /// <param name="dns"></param>  
        public static void SetIpAddress(string nic, string[] ip, string[] mask, string[] way, string[] dns)
        {
            nic = GetNICId(nic);
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            foreach (ManagementObject mo in moc)
            {
                string nicid = mo.GetPropertyValue("SettingID").ToString();
                //当前网卡设置IP，其他网卡设置自动获取  
                if (nicid.Equals(nic))
                {
                    //IP 地址设置  
                    if (ip != null && mask != null && way != null)
                    {
                        //设置IP地址  
                        inPar = mo.GetMethodParameters("EnableStatic");
                        inPar["IPAddress"] = ip;
                        inPar["SubnetMask"] = mask;
                        outPar=mo.InvokeMethod("EnableStatic", inPar, null);

                        //设置网关地址   
                        inPar = mo.GetMethodParameters("SetGateways");
                        inPar["DefaultIPGateway"] = way;
                        outPar=mo.InvokeMethod("SetGateways", inPar, null);

                        //设置DNS   
                        inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        inPar["DNSServerSearchOrder"] = dns;
                        outPar=mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                        return;
                    }
                }
            }
        }

        ///<summary>
        /// 获取本地IP地址
        /// </summary>
        public List<String> getLocalIpAddress()
        {
            List<String> ipAddress = new List<String>(); ;
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();//获取本机所有网卡对象
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.Description.Contains("ASIX"))//枚举条件：描述中包含"ASIX" ASIX为有线网卡的品牌，与无线网卡品牌有区别
                {
                    IPInterfaceProperties ipProperties = adapter.GetIPProperties();//获取IP配置
                    UnicastIPAddressInformationCollection ipCollection = ipProperties.UnicastAddresses;//获取单播地址集
                    foreach (UnicastIPAddressInformation ip in ipCollection)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)//只要ipv4的
                            ipAddress.Add(ip.Address.ToString()); //获取ip
                    }
                }
            }
            return ipAddress;
        }

        /// <summary>  
        /// 获取当前系统中所有网卡的名称  
        /// </summary>  
        /// <param name="showNotEnable" type="bool">是否显示不可用网卡</param>
        /// <returns></returns>  
        public static List<string> GetNICNameList(bool showNotEnable)
        {
            List<string> list = new List<string>();
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            //获取当前系统的所有NIC  
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    list.Add(mo.GetPropertyValue("SettingID").ToString());
                    continue;
                }
            }
            return GetNICNameList(list, showNotEnable);
        }

        /// <summary>  
        /// 加载网卡名称  
        /// </summary>  
        /// <param name="names"></param>  
        /// <param name="showNotEnable" type="boolean">是否显示不可用网卡</param>
        /// <returns></returns>  
        public static List<string> GetNICNameList(List<string> names, bool showNotEnable)
        {
            List<string> list = new List<string>();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            if (showNotEnable == true)
            {
                foreach (NetworkInterface adapter in nics)
                {
                    if (names.Contains(adapter.Id))
                    {
                        list.Insert(0, adapter.Description);
                    }
                    else
                    {
                        list.Add("(不可用)" + adapter.Description);
                    }
                }
            }else
            {
                foreach (NetworkInterface adapter in nics)
                {
                    if (names.Contains(adapter.Id))
                    {
                        list.Insert(0, adapter.Description);
                    }
                }
            }
            return list;
        }

        /// <summary>  
        /// 加载网卡ID  
        /// </summary>  
        /// <param name="names"></param>  
        /// <returns></returns>  
        public static string GetNICId(string nicname)
        {
            string name = null;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.Description.Equals(nicname))
                {
                    name = adapter.Id;
                }
            }
            return name;
        }
    }
}
