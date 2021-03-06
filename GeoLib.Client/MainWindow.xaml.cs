using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using GeoLib.Proxies;
//using GeoLib.Contracts;
using GeoLib.Client.Contracts;
using System.Threading;
using System.Diagnostics;
using GeoLib.Contracts;

namespace GeoLib.Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "UI Running on Thread " + Thread.CurrentThread.ManagedThreadId +
                " | Process " + Process.GetCurrentProcess().Id.ToString();
        }

        private void btnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            if (txtZipCode.Text != "")
            {
                //GeoClient proxy = new GeoClient("tcpEP");
                GeoClient proxy = new GeoClient("webEP");
                
                ZipCodeData data = proxy.GetZipInfo(txtZipCode.Text);
                if (data != null)
                {
                    lblCity.Content = data.City;
                    lblState.Content = data.State;
                }

                proxy.Close();
            }
        }

        private void btnGetZipCodes_Click(object sender, RoutedEventArgs e)
        {
            if (txtState.Text != null)
            {
                // 1) Use config settings in C#:
                ////EndpointAddress address = new EndpointAddress("net.tcp://localhost:8009/GeoService");
                ////Binding binding = new NetTcpBinding();
                EndpointAddress address = new EndpointAddress("http://localhost:57394/GeoService.svc");
                Binding binding = new WSHttpBinding();

                GeoClient proxy = new GeoClient(binding, address);
                IEnumerable<ZipCodeData> data = proxy.GetZips();//proxy.GetZips(txtState.Text);
                if (data != null)
                    lstZips.ItemsSource = data;

                proxy.Close();

                // OR

                // 2) Use config settings in app.config:
                //GeoClient proxy = new GeoClient("tcpEP");
                //GeoClient proxy = new GeoClient("webEP");

                //IEnumerable<ZipCodeData> data = proxy.GetZips(txtState.Text);
                //if (data != null)
                //{
                //    lstZips.ItemsSource = data;
                //}

                //proxy.Close();
            }
        }

        private void btnMakeCall_Click(object sender, RoutedEventArgs e)
        {
            EndpointAddress address = new EndpointAddress("http://localhost:8010/MessageService");
            Binding binding = new NetTcpBinding();

            ChannelFactory<IMessageService> factory = new ChannelFactory<IMessageService>(binding, address);

            //ChannelFactory<IMessageService> factory = new ChannelFactory<IMessageService>("");
            IMessageService proxy = factory.CreateChannel();

            proxy.ShowMessage(txtMessage.Text);

            factory.Close();
        }
    }
}
