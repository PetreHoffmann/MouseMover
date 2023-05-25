using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MouseMover
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public NotifyIcon _notifyIcon;
        public bool _isExit;
        public string appCulture;
        public MouseMover.MainWindow mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string appCulture = MouseMover.Properties.Settings.Default.appCulture;

            Console.WriteLine(appCulture);

            if (appCulture.Length != 0)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(appCulture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(appCulture);
            }
            else
            {
                MouseMover.Properties.Settings.Default.appCulture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            }
            mainWindow = new MouseMover.MainWindow();
            mainWindow.Closing += MainWindow_Closing;
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = MouseMover.Properties.Resources.MouseMover;
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Visible = true;

            CreateContextMenu();
            mainWindow.Show();
        }

        private void CreateContextMenu()
        {
            Image exitImage = MouseMover.Properties.Resources.exit;
            Image showImage = MouseMover.Properties.Resources.show;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add(MouseMover.Properties.Resources.trayShow, showImage).Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("-");
            _notifyIcon.ContextMenuStrip.Items.Add(MouseMover.Properties.Resources.trayExit, exitImage).Click += (s, e) => ExitApplication();
        }

        public void ShowMainWindow()
        {
            if (mainWindow.IsVisible)
            {
                if (mainWindow.WindowState == WindowState.Minimized)
                {
                    mainWindow.WindowState = WindowState.Normal;
                }
                mainWindow.Activate();
            }
            else
            {
                //mainWindow.Show();
                App.Current.MainWindow.Show();
            }
        }

        public void ExitApplication()
        {
            _isExit = true;
            mainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                mainWindow.Hide();
            }
        }
    }
}
