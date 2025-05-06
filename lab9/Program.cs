using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;


namespace NetworkMonitorApp
{
    public class ConnectionInfo
    {
        public string Protocol { get; set; }
        public string LocalAddress { get; set; }
        public string RemoteAddress { get; set; }
        public string State { get; set; }
        public int ProcessId { get; set; }
    }
    public class NetworkScanner
    {
        public List<ConnectionInfo> GetActiveConnections()
        {
            var connections = new List<ConnectionInfo>();

            var startInfo = new ProcessStartInfo
            {
                FileName = "netstat",
                Arguments = "-ano",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.StartsWith("  TCP") || line.StartsWith("TCP"))
                    {
                        var tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens.Length >= 5)
                        {
                            connections.Add(new ConnectionInfo
                            {
                                Protocol = tokens[0],
                                LocalAddress = tokens[1],
                                RemoteAddress = tokens[2],
                                State = tokens[3],
                                ProcessId = int.TryParse(tokens[4], out var pid) ? pid : 0
                            });
                        }
                    }
                }
            }
            return connections;
        }
        public string VerifyFileSignature(int processId)
        {
            try
            {
                using (var process = Process.GetProcessById(processId))
                {
                    string filePath;

                    try
                    {
                        filePath = process.MainModule?.FileName;
                    }
                    catch (System.ComponentModel.Win32Exception)
                    {
                        return "[ACCESS DENIED]";
                    }

                    if (string.IsNullOrEmpty(filePath))
                        return "[UNSIGNED or INVALID]";

                    X509Certificate certBase = X509Certificate.CreateFromSignedFile(filePath);
                    if (certBase == null)
                        return "[UNSIGNED or INVALID]";

                    var cert = new X509Certificate2(certBase);

                    var chain = new X509Chain
                    {
                        ChainPolicy = new X509ChainPolicy
                        {
                            RevocationMode = X509RevocationMode.Online,
                            RevocationFlag = X509RevocationFlag.EntireChain,
                            VerificationFlags = X509VerificationFlags.NoFlag,
                            VerificationTime = DateTime.Now
                        }
                    };

                    bool isValid = chain.Build(cert);

                    return isValid ? "[SIGNED & VALID]" : "[UNSIGNED or INVALID]";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Signature check failed for PID {processId}: {ex.Message}");
                return "[UNSIGNED or INVALID]";
            }
        }

    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var scanner = new NetworkScanner();
            var connections = scanner.GetActiveConnections();
            Console.WriteLine("Aktywne połączenia:");
            foreach (var conn in connections)
            {
                var signatureStatus = scanner.VerifyFileSignature(conn.ProcessId);
                if (!(signatureStatus == "[ACCESS DENIED]"))
                    Console.WriteLine($"{conn.Protocol} | {conn.LocalAddress} -> {conn.RemoteAddress} | PID: {conn.ProcessId} | {conn.State} | Signature: {signatureStatus}");
            }
        }
    }
}
