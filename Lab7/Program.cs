using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularApp;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var gpsInjection = Environment.GetEnvironmentVariable("GPS_INJECTION") ?? "Singleton";

        // Dynamiczna rejestracja IGpsService w zale¿noœci od GPS_INJECTION
        switch (gpsInjection)
        {
            case "Singleton":
                services.AddSingleton<IGpsService, GpsService>();
                break;
            case "Scoped":
                services.AddScoped<IGpsService, GpsService>();
                break;
            case "Transient":
                services.AddTransient<IGpsService, GpsService>();
                break;
            default:
                throw new InvalidOperationException($"Nieznany typ wstrzykiwania: {gpsInjection}");
        }

        services.AddTransient<App>();
    })
    .Build();

var app = host.Services.GetRequiredService<App>();
app.Run();

public interface IGpsService
{
    (double Latitude, double Longitude) GetCurrentLocation();
    Guid GetInstanceId();
}

public class GpsService : IGpsService
{
    private readonly Guid _instanceId;

    public GpsService()
    {
        _instanceId = Guid.NewGuid();
    }

    public (double Latitude, double Longitude) GetCurrentLocation()
    {
        return (Latitude: 52.2297, Longitude: 21.0122); // Warszawa
    }

    public Guid GetInstanceId() => _instanceId;
}
