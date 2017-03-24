using CapsLockIndicator.Properties;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace CapsLockIndicator
{
    public class CustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem exitAppMenuItem;
        private BackgroundWorker backgroundWorker;

        public CustomApplicationContext()
        {
            InitializeComponents();
            backgroundWorker.RunWorkerAsync();
        }

        private void InitializeComponents()
        {
            trayIcon = new NotifyIcon();
            contextMenuStrip = new ContextMenuStrip();
            exitAppMenuItem = new ToolStripMenuItem();
            backgroundWorker = new BackgroundWorker();
            // 
            // notifyIcon
            // 
            trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            trayIcon.BalloonTipText = "Test";
            trayIcon.BalloonTipTitle = "Caps Lock";
            trayIcon.ContextMenuStrip = contextMenuStrip;
            trayIcon.Icon = Resources.CapsLockOn;
            trayIcon.Text = "Caps Lock";
            trayIcon.Visible = true;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] {exitAppMenuItem});
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(104, 26);
            // 
            // exitAppToolStripMenuItem
            // 
            exitAppMenuItem.Name = "exitAppToolStripMenuItem";
            exitAppMenuItem.Size = new System.Drawing.Size(103, 22);
            exitAppMenuItem.Text = "Close";
            exitAppMenuItem.Click += new System.EventHandler(exitAppToolStripMenuItem_Click);
            // 
            // backgroundWorker
            // 
            backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_DoWork);
        }

        private bool CapsLockActive()
        {
            return Control.IsKeyLocked(Keys.CapsLock);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool capsLockStatus = CapsLockActive();
            int timeout = 3000;
            while (true)
            {
                // the status has changed
                if (capsLockStatus != CapsLockActive())
                {
                    string message = string.Format("Caps Lock: {0}", capsLockStatus ? "OFF" : "ON");
                    trayIcon.Text = message;
                    trayIcon.BalloonTipText = message;
                    trayIcon.ShowBalloonTip(timeout);
                }
                capsLockStatus = CapsLockActive();

                if (backgroundWorker.CancellationPending)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        private void exitAppToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            backgroundWorker.CancelAsync();
            Application.Exit();
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            backgroundWorker.CancelAsync();

            Application.Exit();
        }
    }
}
