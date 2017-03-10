using System.Collections.Generic;
using System.Windows;

using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace SniffView
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartCapture_Click(object sender, RoutedEventArgs e)
        {
            using (PacketCommunicator communicator =
                allDevices[ListDevices.SelectedIndex].Open(
                    65536,
                    PacketDeviceOpenAttributes.Promiscuous,
                    1000))
            {
                ListPackets.Items.Clear();
                communicator.ReceiveSomePackets(out int packetnum, 20, PacketHandler);
            }
        }

        private void PacketHandler(Packet packet)
        {
            if (packet.Ethernet.EtherType == PcapDotNet.Packets.Ethernet.EthernetType.IpV4)
            {
                ListPackets.Items.Add(packet.Ethernet.IpV4.Source + ":" + packet.Ethernet.IpV4.Tcp.SourcePort +
                    " -> " + 
                    packet.Ethernet.IpV4.Destination + ":" + packet.Ethernet.IpV4.Tcp.DestinationPort);
            }
            //ListPackets.Items.Add(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length);
        }

        private void ClearCapture_Click(object sender, RoutedEventArgs e)
        {
            ListPackets.Items.Clear();
        }

        private void GetDevices_Click(object sender, RoutedEventArgs e)
        {
            ListDevices.Items.Clear();

            for (int i = 0; i < allDevices.Count; i++)
            {
                ListDevices.Items.Add(allDevices[i].Description);
            }
        }
    }
}
