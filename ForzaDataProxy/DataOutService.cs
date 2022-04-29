using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ForzaDataProxy
{
    public class DataOutService : BackgroundService
    {
        private UdpClient? DataOutClient;
        private readonly IConfiguration Config;
        private readonly ILogger<DataOutService> Logger;

        private FileStream? CaptureFile;

        public DataOutService(IConfiguration config, ILogger<DataOutService> logger)
        {
            Config = config;
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var capture = Config.GetValue<bool>("ForzaDataProxy:Capture:Enabled");
            var captureDrivingOnly = Config.GetValue<bool>("ForzaDataProxy:Capture:DrivingOnly");
            var savePath = Config.GetValue<string>("ForzaDataProxy:Capture:SavePath");

            var listenBindAddress = Config.GetValue<string>("ForzaDataProxy:BindAddress");
            var listenPort = Config.GetValue<int>("ForzaDataProxy:ListenPort");
            ListenServer listenServer = new ListenServer(listenBindAddress, listenPort, Logger);
            listenServer.Start(stoppingToken);

            var dataOutPort = Config.GetValue<int>("ForzaDataProxy:DataOutPort");
            Logger.LogInformation($"ForzaDataProxy listening on port {dataOutPort}");
            DataOutClient = new UdpClient(dataOutPort);

            if (capture)
            {
                await CreateCaptureFile(savePath);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var receiveResult = await DataOutClient.ReceiveAsync(stoppingToken);
                var telemetryData = new TelemetryPacketDecoder(receiveResult.Buffer);
                var json = telemetryData.toJson();

                // Might want to dump this into a queue instead, but will wait to check performance.
                if (capture && (!captureDrivingOnly || (captureDrivingOnly && telemetryData.IsDriving)))
                {
                    await WriteToCaptureFile(json);
                    Logger.LogDebug($"[{telemetryData.Timestamp}] {receiveResult.Buffer.Length}\n {json}");
                }
                await listenServer.Broadcast(Encoding.UTF8.GetBytes(json + "\n"));
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await CloseCaptureFile();
            try
            {
                DataOutClient?.Close();
            }
            catch {}
        }

        private async Task CreateCaptureFile(string savePath)
        {
            var fileName = DateTime.Now.ToString("yyyyMMdd-hhmm") + ".json";
            try
            {
                CaptureFile = File.Open(Path.Combine(savePath, fileName), FileMode.Create);
                await WriteToCaptureFile("[");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error creating capture file: {fileName}");
            }
        }

        private async Task WriteToCaptureFile(string text)
        {
            if (CaptureFile != null)
            {
                try
                {
                    await CaptureFile.WriteAsync(Encoding.UTF8.GetBytes(text));
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Writing to capture file");
                }
            }
        }

        private async Task CloseCaptureFile()
        {
            if (CaptureFile != null)
            {
                await WriteToCaptureFile("]");
                await CaptureFile.FlushAsync();
                CaptureFile.Close();
                CaptureFile = null;
            }
        }
    }
}