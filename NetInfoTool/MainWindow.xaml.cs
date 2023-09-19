using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace NetworkInfoTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // 获取并显示网络连接
            this.ShowNetworkConnections();
        }

        private void ShowNetworkConnections()
        {
            // 获取本地计算机的网络接口
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                // 只显示活动的网络接口
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    this.cmbNetworkConnections.Items.Add(networkInterface.Description);
                }
            }
        }

        private void CmbNetworkConnections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // 获取选中的网络接口
            if (this.cmbNetworkConnections.SelectedItem is string selectedConnection)
            {
                // 获取选中网络接口的详细信息
                var selectedInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(n => n.Description == selectedConnection);

                if (selectedInterface != null)
                {
                    var ipProperties = selectedInterface.GetIPProperties();

                    this.txtDescription.Text = selectedInterface.Description;
                    this.txtPhysicalAddress.Text = selectedInterface.GetPhysicalAddress().ToString();
                    this.txtDhcpEnabled.Text = ipProperties.GetIPv4Properties().IsDhcpEnabled.ToString();
                    this.txtIPv4Address.Text = ipProperties.UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address.ToString();
                    this.txtIPv4SubnetMask.Text = ipProperties.UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.IPv4Mask.ToString();
                    this.txtIPv4DefaultGateway.Text = ipProperties.GatewayAddresses.FirstOrDefault()?.Address.ToString();
                    this.txtIPv4DhcpServer.Text = ipProperties.DhcpServerAddresses.FirstOrDefault()?.ToString();

                    this.lstIPv4DnsServers.Items.Clear();
                    foreach (var dns in ipProperties.DnsAddresses)
                    {
                        this.lstIPv4DnsServers.Items.Add(dns.ToString());
                    }
                }
            }
        }
    }
}
