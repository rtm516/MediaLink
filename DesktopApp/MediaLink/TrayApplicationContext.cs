using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MediaLink
{
    internal class TrayApplicationContext : ApplicationContext
    {
        private NotifyIcon notifyIcon;
        private FormSettings formSettings;

        public TrayApplicationContext()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = MediaLink.Properties.Resources.Icon;
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem titleMenuItem = new ToolStripMenuItem(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version, notifyIcon.Icon.ToBitmap());
            titleMenuItem.Enabled = false;

            notifyIcon.ContextMenuStrip.Items.Add(titleMenuItem);
            notifyIcon.ContextMenuStrip.Items.Add("-");
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Settings", null, new EventHandler(ShowSettings)));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("About", null, new EventHandler(ShowAbout)));
            notifyIcon.ContextMenuStrip.Items.Add("-");
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, new EventHandler(Exit)));

            notifyIcon.Visible = true;

            LinkManager.Start();
            formSettings = new FormSettings();
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            if (formSettings.Visible)
            {
                formSettings.Activate();
            }
            else
            {
                formSettings.ShowDialog();
            }
        }

        private void ShowAbout(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
