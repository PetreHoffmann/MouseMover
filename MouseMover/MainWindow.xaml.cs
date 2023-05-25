using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using System.IO;

namespace MouseMover
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>ffffffff
    public partial class MainWindow : MetroWindow
    {
        //Access Mouse Library
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        //variables
        private Timer timer1;
        private double screenWidth;
        private double screenHeight;
        public MainWindow()
        {
            //Initializer of the Main Window
            InitializeComponent();

            // Get Version for Window and Window Title
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Title = Properties.Resources.appName + " - " + Properties.Resources.version + " " + version; 
            txtVersion.Text = Properties.Resources.version + " " + version;
            stepMinutes.Value = Properties.Settings.Default.tickMinutes;


            // Set Theme based on Settings Value
            var theme = Properties.Settings.Default.theme;
            if (theme == "Dark.Blue")
            {
                toggleTheme.IsOn = true;
            }
            else
            {
                toggleTheme.IsOn = false;
            }

            // Set Theme based on Settings Value
            var sleepQuestion = Properties.Settings.Default.sleep;
            if (sleepQuestion == true)
            {
                toggleSleep.IsOn = true;
            }
            else
            {
                toggleSleep.IsOn = false;
            }

            // Get Screen Size
            screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            // Start Timer
            InitTimer();

            // Check if Timer is started
            if (timer1.Enabled)
            {
                // If started disable Start button and enable Stop button
                btnStart.IsEnabled = false;
                btnStop.IsEnabled = true;
            }
            else
            {
                // If stopped enable Start button and disable Stop button
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = false;
            }
        }

        // Initialize Timer Method
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        // Move Mouse Pointer randomly on the screen
        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rx = new Random();
            Random ry = new Random();
            SetCursorPos(rx.Next((int)screenWidth-50), ry.Next((int)screenHeight-50));
            if (Properties.Settings.Default.sleep)
            {
                NativeMethods.PreventSleep();
            }
            else
            {
                NativeMethods.AllowSleep();
            }
        }

        // What happens if I click Start
        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.tickMinutes = (int)stepMinutes.Value;
            Properties.Settings.Default.sleep = toggleSleep.IsOn;
            Properties.Settings.Default.Save();
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = Properties.Resources.confirmYes,
                NegativeButtonText = Properties.Resources.confirmNo
            };

            MessageDialogResult result = await this.ShowMessageAsync(Properties.Resources.confirmTitle, Properties.Resources.confirmText, MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                // Set Interval to the defined minutes, start timer, disable Start button, enable Stop button and hide the Main Window
                timer1.Interval = (int)stepMinutes.Value * 1000 * 60;
                timer1.Start();
                btnStart.IsEnabled = false;
                btnStop.IsEnabled = true;
                if(Properties.Settings.Default.sleep)
                {
                    NativeMethods.PreventSleep();
                } else
                {
                    NativeMethods.AllowSleep();
                }
                this.Hide();
                ((App)System.Windows.Application.Current).mainWindow.Hide();
            }
        }

        // What happens if I click Stop
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // Stop timer, remove timer variable value, enable Start button and disable Stop button
            timer1.Stop();
            timer1.Dispose();
            NativeMethods.AllowSleep();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        // Toggle Theme between Light and Dark Mode, als save the Setting
        private void toggleTheme_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleTheme.IsOn)
            {
                ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
                Properties.Settings.Default.theme = "Dark.Blue";
                Properties.Settings.Default.Save();
            }
            else
            {
                ThemeManager.Current.ChangeTheme(this, "Light.Blue");
                Properties.Settings.Default.theme = "Light.Blue";
                Properties.Settings.Default.Save();
            }
        }

        private async void btnen_Clicked(object sender, RoutedEventArgs e)
        {
            MouseMover.Properties.Settings.Default.appCulture = "en-US";
            Properties.Settings.Default.Save();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            MessageDialogResult result = await this.ShowMessageAsync(Properties.Resources.langChangeTitle, Properties.Resources.langChangeText);
        }

        private async void btnde_Clicked(object sender, RoutedEventArgs e)
        {
            MouseMover.Properties.Settings.Default.appCulture = "de-DE";
            Properties.Settings.Default.Save();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");

            MessageDialogResult result = await this.ShowMessageAsync(Properties.Resources.langChangeTitle, Properties.Resources.langChangeText);
        }

        private async void btnro_Clicked(object sender, RoutedEventArgs e)
        {
            MouseMover.Properties.Settings.Default.appCulture = "ro-RO";
            Properties.Settings.Default.Save();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ro-RO");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ro-RO");

            MessageDialogResult result = await this.ShowMessageAsync(Properties.Resources.langChangeTitle, Properties.Resources.langChangeText);
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            ((App)System.Windows.Application.Current).ExitApplication();
        }

        private async void btnes_Click(object sender, RoutedEventArgs e)
        {
            MouseMover.Properties.Settings.Default.appCulture = "es-ES";
            Properties.Settings.Default.Save();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-ES");

            MessageDialogResult result = await this.ShowMessageAsync(Properties.Resources.langChangeTitle, Properties.Resources.langChangeText);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ((App)System.Windows.Application.Current).mainWindow.Hide();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string pDF = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "userguide.pdf");
            File.WriteAllBytes(pDF, MouseMover.Properties.Resources.userguide);

            System.Diagnostics.Process.Start(pDF);
        }

        private void toggleSleep_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSleep.IsOn)
            {
                Properties.Settings.Default.sleep = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.sleep = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
