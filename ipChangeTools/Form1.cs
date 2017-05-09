using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ipChangeTools.Tools;
using System.Collections;

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

            //读取XML完成初始化
            List<Hashtable> hsList = XmlMaster.GetXMLInformation("./ipAddress.xml");
            if (hsList != null && hsList.Count>0)
            {
                foreach (Hashtable hs in hsList)
                {
                    comboBox2.Items.Add(hs["local"]);
                }
                comboBox2.SelectedIndex = 0;
                Hashtable hs1 = hsList.First<Hashtable>();
                label9.Text = hs1["ip"].ToString();
                label10.Text = hs1["mask"].ToString();
                label11.Text = hs1["gateway"].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nic = comboBox1.Text;
            string[] ip = { label9.Text };
            string[] submask = { label10.Text };
            string[] getway = { label11.Text };
            string[] dns = null;
            IpManager.SetIpAddress(nic,ip,submask,getway,dns);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hashtable hs = XmlMaster.GetXMLSomeOne("./ipAddress.xml", "local", comboBox2.Text);
            label9.Text = hs["ip"].ToString();
            label10.Text = hs["mask"].ToString();
            label11.Text = hs["gateway"].ToString();
        }

        private void 修改XML配置文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String path = System.Environment.CurrentDirectory;
            System.Diagnostics.Process.Start(path+"/ipAddress.xml");
        }

        private void 恢复自动获取IPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //IpManager.SetAutoIP(comboBox1.Text);
            IpManager.EnableDHCP(comboBox1.Text);
        }
    }
}
