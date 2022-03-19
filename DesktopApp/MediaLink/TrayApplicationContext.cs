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
        public static ToolStripMenuItem StatusMenuItem { get; private set; }

        private NotifyIcon notifyIcon;
        private FormSettings formSettings;
        private AboutBox formAbout;

        public TrayApplicationContext()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.Icon;
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem titleMenuItem = new ToolStripMenuItem(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version, notifyIcon.Icon.ToBitmap());
            titleMenuItem.Enabled = false;

            StatusMenuItem = new ToolStripMenuItem("Status: Stopped", Properties.Resources.Stopped);

            notifyIcon.ContextMenuStrip.Items.Add(titleMenuItem);
            notifyIcon.ContextMenuStrip.Items.Add("-");
            notifyIcon.ContextMenuStrip.Items.Add(StatusMenuItem);
            notifyIcon.ContextMenuStrip.Items.Add("-");
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Settings", null, new EventHandler(ShowSettings)));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("About", null, new EventHandler(ShowAbout)));
            notifyIcon.ContextMenuStrip.Items.Add("-");
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, new EventHandler(Exit)));

            notifyIcon.Visible = true;

            LinkManager.Start();
            formSettings = new FormSettings();
            formAbout = new AboutBox();
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
            if (formAbout.Visible)
            {
                formAbout.Activate();
            }
            else
            {
                formAbout.ShowDialog();
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
