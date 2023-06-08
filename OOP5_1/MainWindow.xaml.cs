using MeasuringDevice;
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
namespace MeasuringDevice
{
    public interface IMeasuringDevice
    {
        /// <summary>/// Converts the raw data collected by the measuring device into a metric value
        /// </summary>
        ///<returns>The latest measurement from the device converted to metric units.</returns>
        decimal MetricValue();
        /// <summary>
        /// Converts the raw data collected by the measuring device into an imperial value.
        /// </summary>
        ///<returns>The latest measurement from the device converted to imperial units.</returns>
        decimal ImperialValue();
        /// <summary>
        /// Starts the measuring device.
        /// </summary>
        void StartCollecting();
        /// <summary>
        /// Stops the measuring device. 
        /// </summary>
        void StopCollecting();
        /// <summary>
        /// Enables access to the raw data from the device in whatever units are native to the device
        /// </summary>
        /// <returns>The raw data from the device in native format.</returns>
        int[] GetRawData();
    }
}


namespace OOP5_1
{

    public class DeviceController
    {
        public DeviceType measurementType;
        public DeviceController(DeviceType measurementType)
        {
            this.measurementType = measurementType;
        }

        public void StopDevice()
        {

        }


        public int TakeMeasurement()
        {
            Random rnd = new Random();
            return rnd.Next(1,11);
        }

        public static DeviceController StartDevice(DeviceType measurementType)
        {
            return new DeviceController(measurementType);
        }
    }
    public enum Units {Metric, Imperial };
    public enum DeviceType {LENGTH,MASS}

    public class MeasureLengthDevice: IMeasur   ingDevice
    {
        private Units unitsToUse;
        private int[] dataCaptured;
        private int mostRecentMeasure;
        private DeviceController controller;
        private const DeviceType measurementType = DeviceType.LENGTH;

        public MeasureLengthDevice(Units unitsToUse) {
            this.unitsToUse = unitsToUse;
        }

        public decimal MetricValue()
        {
            if (unitsToUse == Units.Metric)
            {
                return mostRecentMeasure;
            }
            else
            {
                return Convert.ToDecimal(mostRecentMeasure * 25.4);
            }
        }
        public decimal ImperialValue() {
            if (unitsToUse == Units.Imperial)
            {
                return mostRecentMeasure;
            }
            else
            {
                return Convert.ToDecimal(mostRecentMeasure * 0.03937);
            }
        }
        
        public void StopCollecting() {
            if (controller!= null)
            {
                controller.StopDevice();
                controller = null;
            }
        }
        public int[] GetRawData() {
            return dataCaptured;
        }
        public int[] GetMostRecentMeasure() {
            int[] ints  = new int[1];
            return ints;
        }
        private void GetMeasurements()
        {
            dataCaptured = new int[10];
            System.Threading.ThreadPool.QueueUserWorkItem((dummy) =>
            {
                int x = 0;
                Random timer = new Random();

                while (controller != null)
                {
                    System.Threading.Thread.Sleep(timer.Next(1000, 5000));
                    dataCaptured[x] = controller != null ?
                        controller.TakeMeasurement() : dataCaptured[x];
                    mostRecentMeasure = dataCaptured[x];
                    x++;
                    if (x == 10)
                    {
                        x = 0;
                    }
                }
            });

        }

        public void StartCollecting()
        {
            controller = new DeviceController(measurementType);
            GetMeasurements();
        }
    }
    public partial class MainWindow : Window
    {
        MeasureLengthDevice mydevice;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mydevice = new MeasureLengthDevice((Units)Enum.Parse(typeof(Units), Units_select.Text));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mydevice.StartCollecting();
        }

        private void get_raw_data_btn_Click(object sender, RoutedEventArgs e)
        {
            int[] mydata = mydevice.GetRawData();
            data_captured.Items.Clear();
            for (int i = 0; i < mydata.Length; i++)
            {
                data_captured.Items.Add(mydata[i].ToString());
            }
        }

        private void stop_collecting_btn_Click(object sender, RoutedEventArgs e)
        {
            mydevice.StopCollecting();
        }

        private void get_imperial_value_btn_Click(object sender, RoutedEventArgs e)
        {

            decimal mydata = mydevice.ImperialValue();
            MessageBox.Show((mydata.ToString()));
        }

        private void get_metric_value_btn_Click(object sender, RoutedEventArgs e)
        {
            decimal mydata = mydevice.MetricValue();
           MessageBox.Show((mydata.ToString()));
        }
    }
}

