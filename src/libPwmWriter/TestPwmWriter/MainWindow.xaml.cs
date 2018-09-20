using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using lachina.hardware.PwmWriterLibrary;

namespace lachina.hardware.TestPwmWriter
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PwmWriterLibrary.PwmWriterLibrary pwm = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDevices();
        }

        private void InitializeDevices()
        {
            try
            {
                ComboBoxDevices.ItemsSource = PwmWriterLibrary.PwmWriterLibrary.Devices;
            }
            catch(Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (MainWindow:InitializeDevices) " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void ComboBoxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string device = ComboBoxDevices.SelectedItem.ToString();
                if(pwm != null)
                {
                    pwm.Dispose();
                }
                pwm = new PwmWriterLibrary.PwmWriterLibrary(device);
                pwm.eventReceive += pwm_eventReceive;
                pwm.Open();
            }
            catch(Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (MainWindow:ComboBoxDevices_SelectionChanged) " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void pwm_eventReceive(object sender, string e)
        {
            try
            {
                Trace.TraceInformation(e);
                this.Dispatcher.Invoke(() =>
                {
                    ListBoxLog.Items.Add(e);
                    ListBoxLog.ScrollIntoView(e);
                });
            }
            catch(Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (MainWindow:pwm_eventReceive) " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            InitializeDevices();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Slider slider = sender as Slider;
                string id = slider.DataContext.ToString();
                int pos = (int)slider.Value;
                pwm.Send(id[0], pos);
            }
            catch(Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (MainWindow:Slider_ValueChanged) " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
