using System.Diagnostics;
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
        public string ProcessName { get; set; }
    }

    public class NetworkScanner
    {
        public void LogUnsignedProcess(ConnectionInfo conn, string signatureStatus)
        {
            const string source = "NetworkMonitorApp";
            const string logName = "Application";

            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }

                string message = $"Wykryto niepodpisany proces:\n" +
                                 $"PID: {conn.ProcessId}\n" +
                                 $"Nazwa: {conn.ProcessName}\n" +
                                 $"Adres lokalny: {conn.LocalAddress}\n" +
                                 $"Adres zdalny: {conn.RemoteAddress}\n" +
                                 $"Status podpisu: {signatureStatus}";

                EventLog.WriteEntry(source, message, EventLogEntryType.Warning);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[EventLog] Nie udało się zapisać logu: {ex.Message}");
            }
        }

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
                        if (tokens.Length >= 5 && int.TryParse(tokens[4], out var pid))
                        {
                            string processName = "[unknown]";
                            try
                            {
                                processName = Process.GetProcessById(pid).ProcessName;
                            }
                            catch { }

                            connections.Add(new ConnectionInfo
                            {
                                Protocol = tokens[0],
                                LocalAddress = tokens[1],
                                RemoteAddress = tokens[2],
                                State = tokens[3],
                                ProcessId = pid,
                                ProcessName = processName
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

                    var cert = new X509Certificate2(X509Certificate.CreateFromSignedFile(filePath));
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

                    return chain.Build(cert) ? "[SIGNED & VALID]" : "[UNSIGNED or INVALID]";
                }
            }
            catch
            {
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

            Console.WriteLine("Prot\tLocal Address\t\tRemote Address\t\tPID\tProcess Name\t\tState\t\tSignature");
            foreach (var conn in connections)
            {
                var signatureStatus = scanner.VerifyFileSignature(conn.ProcessId);

                if (signatureStatus != "[ACCESS DENIED]")
                {
                    Console.WriteLine($"{conn.Protocol}\t{conn.LocalAddress,-22}\t{conn.RemoteAddress,-22}\t{conn.ProcessId}\t{conn.ProcessName,-16}\t{conn.State,-12}\t{signatureStatus}");

                    if (signatureStatus == "[UNSIGNED or INVALID]")
                    {
                        scanner.LogUnsignedProcess(conn, signatureStatus);
                    }
                }
            }

        }
    }
}
