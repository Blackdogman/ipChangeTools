using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

                ////创建第一层节点
                //XmlElement topElement = xmlDoc.CreateElement("data");
                ////给第一层节点写入属性值
                //topElement.SetAttribute("local","本地测试");
                //topElement.SetAttribute("ip","192.168.0.233");
                //topElement.SetAttribute("mask","255.255.255.0");
                //topElement.SetAttribute("gateway","192.168.0.1");
                //rootElement.AppendChild(topElement);

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
                bool localSame = false;
                bool ipSame = false;
                bool maskSame = false;
                bool gatewaySame = false;
                if (GetXMLSomeOne(xmlFilePath, "local", local).Count!=0)
                    localSame = true;
                if (GetXMLSomeOne(xmlFilePath, "ip", ip).Count != 0)
                    ipSame = true;
                if (GetXMLSomeOne(xmlFilePath, "mask", mask).Count != 0)
                    maskSame = true;
                if (GetXMLSomeOne(xmlFilePath, "gateway", gateway).Count != 0)
                    gatewaySame = true;
                if (localSame == true && ipSame == true && maskSame == true && gatewaySame == true)
                {
                    //这里要执行的是修改操作
                }
                else
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
            }
            catch (Exception ex)
            {
                throw new Exception("添加出错啦！\n"+ex);
            }
        }

        /// <summary>
        /// 得到某个XML的所有数据
        /// </summary>
        /// <param name="xmlFilePath">XML的文件路径</param>
        /// <returns></returns>
        public static List<Hashtable> GetXMLInformation(String xmlFilePath)
        {
            List<Hashtable> hsList = new List<Hashtable>();
            Hashtable hs = new Hashtable();
            try
            {
                if(!File.Exists(xmlFilePath))
                {
                    GenerateXMLFile(xmlFilePath);
                }
                xmlDoc.Load(xmlFilePath);
                XmlNode rootNode = xmlDoc.FirstChild;
                XmlNodeList nodeList = rootNode.ChildNodes;
                if (nodeList == null)
                {
                    return null;
                }
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

        /// <summary>
        /// 得到某个XML某个字段对应的某个值的详细信息
        /// </summary>
        /// <param name="xmlFilePath">要访问的路径</param>
        /// <param name="key">查询的字段</param>
        /// <param name="value">查询字段对应的值</param>
        /// <returns>对应的Hashtable</returns>
        public static Hashtable GetXMLSomeOne(String xmlFilePath,String key,String value)
        {
            Hashtable hs = new Hashtable();
            try
            {
                xmlDoc.Load(xmlFilePath);
                XmlNode rootNode = xmlDoc.FirstChild;
                XmlNodeList nodeList = rootNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    switch (key)
                    {
                        case "local":
                            if (node.Attributes["local"].Value == value)
                            {
                                hs.Add("local", node.Attributes["local"].Value);
                                hs.Add("ip", node.Attributes["ip"].Value);
                                hs.Add("mask", node.Attributes["mask"].Value);
                                hs.Add("gateway", node.Attributes["gateway"].Value);
                            }
                            break;
                        case "ip":
                            if (node.Attributes["ip"].Value == value)
                            {
                                hs.Add("local", node.Attributes["local"].Value);
                                hs.Add("ip", node.Attributes["ip"].Value);
                                hs.Add("mask", node.Attributes["mask"].Value);
                                hs.Add("gateway", node.Attributes["gateway"].Value);
                            }
                            break;
                        case "mask":
                            if (node.Attributes["mask"].Value == value)
                            {
                                hs.Add("local", node.Attributes["local"].Value);
                                hs.Add("ip", node.Attributes["ip"].Value);
                                hs.Add("mask", node.Attributes["mask"].Value);
                                hs.Add("gateway", node.Attributes["gateway"].Value);
                            }
                            break;
                        case "gateway":
                            if (node.Attributes["gateway"].Value == value)
                            {
                                hs.Add("local", node.Attributes["local"].Value);
                                hs.Add("ip", node.Attributes["ip"].Value);
                                hs.Add("mask", node.Attributes["mask"].Value);
                                hs.Add("gateway", node.Attributes["gateway"].Value);
                            }
                            break;
                        default:
                            throw new Exception("未找到相应字段！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("取得XML信息！！！" + ex);
            }
            return hs;
        }
    }
}
