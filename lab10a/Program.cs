using System;
using System.Management;

class RemoteRpcExample
{
    static void Main()
    {
        string remoteHost = "192.168.226.20"; // Użyj IP lub nazwy komputera w sieci
        string username = "DESKTOP-UH6262H\\Krzysztof"; // Nazwa konta domenowego lub lokalnego
        string password = "abc123"; // Hasło

        ConnectionOptions options = new ConnectionOptions
        {
            Username = username,
            Password = password,
            Impersonation = ImpersonationLevel.Impersonate,
            Authentication = AuthenticationLevel.PacketPrivacy,
            EnablePrivileges = true
        };

        ManagementScope scope = new ManagementScope($"\\\\{remoteHost}\\root\\cimv2", options);

        try
        {
            scope.Connect();
            Console.WriteLine("Połączono zdalnie z: " + remoteHost);

            ManagementClass processClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), null);
            object[] methodArgs = { "notepad.exe", null, null, 0 };

            var result = processClass.InvokeMethod("Create", methodArgs);

            Console.WriteLine((uint)result == 0 ? "Proces uruchomiony." : $"Błąd: kod {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd RPC/WMI: " + ex.Message);
        }
    }
}