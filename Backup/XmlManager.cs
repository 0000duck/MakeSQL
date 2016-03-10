using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MakeSQL
{
    public class XmlManager
    {
        /// <summary>
        /// 创建xml
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="item">节点集</param>
        /// <returns></returns>
        public static int CreateXML(string filePath,string items)
        {
            try
            {
                XmlTextWriter writer = new XmlTextWriter(filePath, null);
                //使用自动缩进便于阅读
                writer.Formatting = Formatting.Indented;
                //写入根元素
                writer.WriteStartElement("items");
                writer.WriteStartElement("item");
                ////写入属性及属性的名字
                //writer.WriteAttributeString("类别", "文学");
                //writer.WriteAttributeString("品质", "优");
                string[] itemArray = items.Split(new char[] { ',' });
                //加入子元素
                for (int i = 0; i < itemArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(itemArray[i]))
                    {
                        continue;
                    }

                    writer.WriteElementString(itemArray[i], "");
                    //writer.WriteElementString("author", "");
                    //writer.WriteElementString("price", "");
                }
                //关闭根元素，并书写结束标签
                writer.WriteEndElement();
                writer.WriteEndElement();
                //将XML写入文件并且关闭XmlTextWriter
                writer.Close();
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// 更新xml
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">节点值</param>
        /// <returns></returns>
        public static int UpdateXML(string filePath, string nodeName, string nodeValue)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode xns = xmlDoc.SelectSingleNode("Items");
                XmlNodeList xnl = xns.ChildNodes;
                foreach (XmlNode xn in xnl)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Name == nodeName)
                    {
                        xe.InnerText = nodeValue;
                        break;
                    }
                }

                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">节点值</param>
        /// <returns></returns>
        public static int ReadNodeValue(string filePath, string nodeName, ref string nodeValue)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode xns = xmlDoc.SelectSingleNode("Items");
                XmlNodeList xnl = xns.ChildNodes;
                foreach (XmlNode xn in xnl)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Name == nodeName)
                    {
                        nodeValue = xe.InnerText;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// 获取xml值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetXmlString(string filePath)
        {
            string xml = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            xml = xmlDoc.InnerXml;

            return xml;
        }

    }
}
