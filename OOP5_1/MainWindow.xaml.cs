using MeasuringDevice;
using OOP5_1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        /// Converts the raw data collected by the measuring device into a metric value.
        /// </summary>
        /// <returns>The latest measurement from the device converted to metric units.</returns>
        decimal MetricValue();
        /// <summary>
        /// Converts the raw data collected by the measuring device into an imperial value.
        /// </summary>
        /// <returns>The latest measurement from the device converted to imperial units.</returns>
        decimal ImperialValue();
        /// <summary>
        /// Starts the measuring device.
        /// </summary>
        void StartCollecting();
        /// <summary>
        /// Stops the measuring device. 
        /// </summary>
        void StopCollecting();
        /// <summary
        /// Enables access to the raw data from the device in whatever units are native to the device
        /// </summary>
        /// <returns>The raw data from the device in native format.</returns>
        int[] GetRawData();
        /// <summary>
        /// Returns the file name of the logging file for the device.
        /// </summary>
        /// <returns>The file name of the logging file.</returns>
        string GetLoggingFile();
        /// <summary>
        /// Gets the Units used natively by the device.
        /// </summary>
        Units UnitsToUse { get; }
        /// <summary>
        /// Gets an array of the measurements taken by the device.
        /// </summary>
        int[] DataCaptured { get; }
        /// <summary>
        /// Gets the most recent measurement taken by the device.
        /// </summary>
        int MostRecentMeasure { get; }
        /// <summary>
        /// Gets or sets the name of the logging file used. 
        /// If the logging file changes this closes the current file and creates the new file
        /// </summary>
        string LoggingFileName { get; set; }
    }
    interface IEventEnabledMeasuringDevice : IMeasuringDevice
    {
        event EventHandler NewMeasurementTaken;
        // Event that fires every heartbeat.
        event HeartBeatEventHandler HeartBeat;
        // Read only heartbeat interval - set in constructor.
        int HeartBeatInterval { get; }
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
    public delegate void HeartBeatEventHandler();



    public class MeasureLengthDevice: IEventEnabledMeasuringDevice
    {
        public event HeartBeatEventHandler HeartBeat;
        public event EventHandler NewMeasurementTaken;
        public int HeartBeatInterval { get; }

        public Units UnitsToUse { get; }
        public int[] DataCaptured { get; set; }
        public int MostRecentMeasure { get; set; }
        private DeviceController controller;
        private const DeviceType measurementType = DeviceType.LENGTH;
        public string LoggingFileName { get; set; }
        

        public void OnMeasurementTaken()
        {
            if (NewMeasurementTaken!=null)
            {

            }
        }

        public string GetLoggingFile()
        {
            return this.LoggingFileName;
        }
        public MeasureLengthDevice(Units unitsToUse) {
            this.UnitsToUse = unitsToUse;
        }
        public decimal MetricValue()
        {
            if (UnitsToUse == Units.Metric)
            {
                return MostRecentMeasure;
            }
            else
            {
                return Convert.ToDecimal(MostRecentMeasure * 25.4);
            }
        }
        public decimal ImperialValue() {
            if (UnitsToUse == Units.Imperial)
            {
                return MostRecentMeasure;
            }
            else
            {
                return Convert.ToDecimal(MostRecentMeasure * 0.03937);
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
            return DataCaptured;
        }
        public int[] GetMostRecentMeasure() {
            int[] ints  = new int[1];
            return ints;
        }
        private void GetMeasurements()
        {
            DataCaptured = new int[10];
            System.Threading.ThreadPool.QueueUserWorkItem((dummy) =>
            {
                int x = 0;
                Random timer = new Random();

                while (controller != null)
                {
                    System.Threading.Thread.Sleep(timer.Next(1000, 5000));
                    DataCaptured[x] = controller != null ?
                        controller.TakeMeasurement() : DataCaptured[x];
                    MostRecentMeasure = DataCaptured[x];
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

