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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class DeviceController
    {
        public int TakeMeasurement()
        {
            return 1;
        }
    }
    public enum Units {Metric,Impretial};
    public enum DeviceType {LENGTH,MASS}

    public class MeasureLengthDevice: IMeasuringDevice
    {
        private Units unitsToUse;
        private int[] dataCaptured;
        private int mostRecentMeasure;
        private DeviceController controller;
        private DeviceType measurementType;
        public decimal MetricValue()
        {
            return 1;
        }
        public decimal ImperialValue() { 
            
            return 1; 
        }
        public void StartCollecting() {
        
        }
        public void StopCollecting() { }
        public int[] GetRawData() {
            int[] data= new int[1];
            return data;
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

    }
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

