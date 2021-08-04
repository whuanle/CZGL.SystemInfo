using CZGL.ProcessMetrics.Prometheus;
using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics.MetricsSources
{
    public class NetworkMetrics : IMerticsSource
    {

        private readonly NetworkInfo[] networkInfos;

        public NetworkMetrics()
        {
            networkInfos = NetworkInfo.GetRealNetworkInfos();
        }

        public async Task InvokeAsync(ProcessMetricsCore metricsCore)
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge networkinfosGaugeSend = metricsCore.CreateGauge("dotnet_network_info_send", "network");
                Gauge networkinfosGaugeReceived = metricsCore.CreateGauge("dotnet_network_info_received", "network");

                Gauge networkinfosGaugeSend_ = metricsCore.CreateGauge("dotnet_network_send_speed", "network speed");
                Gauge networkinfosGaugeReceived_ = metricsCore.CreateGauge("dotnet_network_received_speed", "network speed");

                foreach (var item in networkInfos)
                {
                    var networksSendLabel = networkinfosGaugeSend.Create();
                    var networksreceivedLabel = networkinfosGaugeReceived.Create();

                    var networksSendLabel_ = networkinfosGaugeSend_.Create();
                    var networksreceivedLabel_ = networkinfosGaugeReceived_.Create();

                    var speed = item.GetInternetSpeed();
                    networksSendLabel.AddLabel("name", item.Name)
                        .AddLabel("ip_address", item.AddressIpv4?.ToString())
                        .AddLabel("mac", item.Mac)
                        .AddLabel("status", item.Status.ToString())
                        .AddLabel("network_interface_type", item.NetworkType.ToString())
                        .AddLabel("send_size_bytes", item.SendLengthIpv4.ToString())
                        .AddLabel("speed_send", (speed.Sent.OriginSize >> 10).ToString())
                        .SetValue((speed.Sent.OriginSize >> 10));

                    networksreceivedLabel.AddLabel("name", item.Name)
                        .AddLabel("ip_address", item.AddressIpv4?.ToString())
                        .AddLabel("mac", item.Mac)
                        .AddLabel("status", item.Status.ToString())
                        .AddLabel("network_interface_type", item.NetworkType.ToString())
                        .AddLabel("received_size_bytes", item.ReceivedLengthIpv4.ToString())
                        .AddLabel("speed_received", (speed.Received.OriginSize >> 10).ToString())
                        .SetValue((speed.Received.OriginSize >> 10));

                    networksSendLabel_.AddLabel("name", item.Name)
                        .AddLabel("ip_address", item.AddressIpv4?.ToString())
                        .AddLabel("mac", item.Mac)
                        .AddLabel("network_interface_type", item.NetworkType.ToString())
                        .SetValue((speed.Sent.OriginSize >> 10));

                    networksreceivedLabel_.AddLabel("name", item.Name)
                        .AddLabel("ip_address", item.AddressIpv4?.ToString())
                        .AddLabel("mac", item.Mac)
                        .AddLabel("network_interface_type", item.NetworkType.ToString())
                        .SetValue((speed.Received.OriginSize >> 10));

                }
            });
        }
    }
}
