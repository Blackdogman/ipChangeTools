using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ipChangeTools.Tools
{
    class XmlMaster
    {
        //初始化一个xml实例
        static XmlDocument xmlDoc = new XmlDocument();
        /// <summary>
        /// 用于第一次使用的时候初始化一个XML文件
        /// </summary>
        /// <param name="xmlFilePath">XML文件生成的位置</param>
        public static void GenerateXMLFile(String xmlFilePath)
        {
            try
            {
                //创建一个节点作为根节点
                XmlElement rootElement = xmlDoc.CreateElement("DATAS");
                //添加根节点到xml中
                xmlDoc.AppendChild(rootElement);

                //创建第一层节点
                XmlElement topElement = xmlDoc.CreateElement("data");
                //给第一层节点写入属性值
                topElement.SetAttribute("local","本地测试");
                topElement.SetAttribute("ip","192.168.0.233");
                topElement.SetAttribute("mask","255.255.255.0");
                topElement.SetAttribute("gateway","192.168.0.1");
                rootElement.AppendChild(topElement);

                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("哎呀！创建XML文件的时候出错了\n"+ex);
            }
        }

        /// <summary>
        /// 向XML添加子节点
        /// </summary>
        /// <param name="xmlFilePath">添加的XML文件位置</param>
        /// <param name="local">表示IP的地点</param>
        /// <param name="ip">添加的IP</param>
        /// <param name="mask">添加的子网掩码</param>
        /// <param name="gateway">添加的网关</param>
        public static void AddXmlInformation(String xmlFilePath,String local,String ip,String mask,String gateway)
        {
            try
            {
                xmlDoc.Load(xmlFilePath);
                XmlElement newElement = xmlDoc.CreateElement("data");
                newElement.SetAttribute("local", local);
                newElement.SetAttribute("ip", ip);
                newElement.SetAttribute("mask", mask);
                newElement.SetAttribute("gateway", gateway);
                xmlDoc.FirstChild.AppendChild(newElement);

                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("添加出错啦！\n"+ex);
            }
        }

        public static List<Hashtable> GetXMLInformation(String xmlFilePath)
        {
            List<Hashtable> hsList = new List<Hashtable>();
            Hashtable hs = new Hashtable();
            try
            {
                xmlDoc.Load(xmlFilePath);
                XmlNode rootNode = xmlDoc.FirstChild;
                XmlNodeList nodeList = rootNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    hs = new Hashtable();
                    hs.Add("local", node.Attributes["local"].Value);
                    hs.Add("ip", node.Attributes["ip"].Value);
                    hs.Add("mask", node.Attributes["mask"].Value);
                    hs.Add("gateway", node.Attributes["gateway"].Value);
                    hsList.Add(hs);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("取得XML信息！！！"+ex);
            }
            return hsList;
        }
    }
}
