using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace CapsLockIndicator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Hide(); // I don't think this works when TopMost Set
            backgroundWorker.RunWorkerAsync();
        }

        private bool CapsLockActive()
        {
            return IsKeyLocked(Keys.CapsLock);
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
                    string message;
                    if (capsLockStatus)
                    {
                        message = "OFF";
                    }
                    else
                    {
                        message = "ON";
                    }
                    message = string.Format("Caps Lock: {0}", message);
                    notifyIcon.Text = message;
                    notifyIcon.BalloonTipText = message;
                    notifyIcon.ShowBalloonTip(timeout);
                }
                capsLockStatus = CapsLockActive();
                Thread.Sleep(1000);
            }
        }
    }
}
