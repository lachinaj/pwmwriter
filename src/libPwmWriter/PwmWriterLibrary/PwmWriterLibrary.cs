using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lachina.hardware.PwmWriterLibrary
{
    public class PwmWriterLibrary : IDisposable
    {
        private SerialPort sp = null;
        private string Device = null;

        public event EventHandler<string> eventReceive;

        public static string[] Devices
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }

        public PwmWriterLibrary(string Device)
        {
            this.Device = Device;
        }

        public void Open()
        {
            try
            {
                sp = new SerialPort(Device, 115200);
                sp.DataReceived += Sp_DataReceived;
                sp.Open();
            }
            catch(Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (RobotControlLibrary:Open) " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                eventReceive?.Invoke(this, sp.ReadLine());
            }
            catch (Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (RobotControlLibrary:Sp_DataReceived) " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void Send(char id, int pos)
        {
            try
            {
                String Send = id + ":" + pos + "\n";
                sp?.Write(Send.ToCharArray(), 0, Send.Length);
            }
            catch (Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (RobotControlLibrary:Send) " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void Close()
        {
            try
            {
                sp?.Dispose();
                sp = null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(DateTime.Now + " : (RobotControlLibrary:Close) " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
