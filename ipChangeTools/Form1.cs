﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ipChangeTools.Tools;

namespace ipChangeTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //获取当前所有可用网卡，并绑定到下拉框
            List<String> parm = new List<string>();
            //参数为true会把所有的网卡列出来,参数为false会只列出有效的网卡
            parm = IpManager.GetNICNameList(false);
            foreach (String p in parm)
            {
                comboBox1.Items.Add(p);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("未检测到有效的网卡,请检查是否网卡是否启用");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nic = comboBox1.Text;
            string[] ip = { "192.168.0.233" };
            string[] submask = { "255.255.255.0" };
            string[] getway = { "192.168.0.1" };
            string[] dns = null;
            IpManager.SetIpAddress(nic,ip,submask,getway,dns);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string xmlFilePath = Application.StartupPath;
            //String msg = XmlMaster.GenerateXMLFile(xmlFilePath);
            String msg = XmlMaster.GenerateXMLFile("./ipAddress.xml");
            if (msg != "")
            {
                MessageBox.Show(msg);
            }
            MessageBox.Show("创建完成，开始添加");
            msg = XmlMaster.AddXmlInformation("./ipAddress.xml","测试添加","192.168.0.110","255.255.255.0","192.168.0.1");
            if (msg != "")
            {
                MessageBox.Show(msg);
            }
        }
    }
}