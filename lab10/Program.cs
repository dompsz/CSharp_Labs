using System;
using System.Management;
using System.Net.Security;
using System.Security.Principal;

class RpcWmiExample
{
    static void Main()
    {
        string fullUser = WindowsIdentity.GetCurrent().Name;
        Console.WriteLine("Zalogowany użytkownik: " + fullUser);

        string userName = Environment.UserName;
        string domainName = Environment.UserDomainName;

        Console.WriteLine("Użytkownik: " + userName);
        Console.WriteLine("Domena / komputer: " + domainName);

        string remoteHost = "192.168.226.20"; // Nazwa lub IP zdalnego komputera
        string user = "DESKTOP-UH6262H\\Krzysztof"; // Nazwa użytkownika zdalnego systemu
        string password = "abc123"; // Hasło użytkownika abc123

        var options = new ConnectionOptions
        {
            Username = user,
            Password = password,
            EnablePrivileges = true,
            Impersonation = ImpersonationLevel.Impersonate,
            Authentication = System.Management.AuthenticationLevel.PacketPrivacy
        };

         var scope = new ManagementScope($"\\\\{remoteHost}\\root\\cimv2", options);
        //var scope = new ManagementScope(@"\\.\root\cimv2"); // kropka = localhost

        try
        {
            scope.Connect();
            Console.WriteLine("Połączenie nawiązane zdalnie przez RPC.");

            var processClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), null);

            string commandLine = "notepad.exe";
            object[] methodArgs = { commandLine, null, null, 0 };

            var result = processClass.InvokeMethod("Create", methodArgs);

            if ((uint)result == 0)
                Console.WriteLine("Zdalnie uruchomiono: " + commandLine);
            else
                Console.WriteLine("Błąd przy uruchamianiu procesu. Kod: " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd RPC/WMI: " + ex.Message);
        }
    }
}
