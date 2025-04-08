namespace ModularApp;

public class App
{
    private readonly IGpsService _gpsService;

    public App(IGpsService gpsService)
    {
        _gpsService = gpsService;
    }

    public void Run()
    {
        Console.WriteLine("GPS Service Instance ID:");
        Console.WriteLine($"Instance ID: {_gpsService.GetInstanceId()}");

        var location = _gpsService.GetCurrentLocation();
        Console.WriteLine($"Current Location: Latitude = {location.Latitude}, Longitude = {location.Longitude}");
        Console.WriteLine($"Instance ID: {_gpsService.GetInstanceId()}");
    }
}
