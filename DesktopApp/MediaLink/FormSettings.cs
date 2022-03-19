using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace MediaLink
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            cboAddress.Items.Add("0.0.0.0");
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            cboAddress.Items.Add(ip.Address.ToString());
                        }
                    }
                }
            }

            cboAddress.Text = Properties.Settings.Default.ListenAddress;
            numPort.Value = Properties.Settings.Default.ListenPort;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ListenAddress = cboAddress.Text;
            Properties.Settings.Default.ListenPort = decimal.ToInt32(numPort.Value);

            LinkManager.WebSocketManager.Restart();
            Close();
        }
    }
}
