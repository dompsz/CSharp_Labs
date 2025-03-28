abstract class Vehicle
{
    public string Brand { get; }
    public string Model { get; }
    public int Year { get; }
    
    public Vehicle(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
    }
    
    public abstract void Move();
}

interface IFuelVehicle
{
    double FuelLevel { get; }
    void Refuel(double amount);
}

interface IElectricVehicle
{
    double BatteryLevel { get; }
    void Charge(double amount);
}

abstract class FuelVehicle : Vehicle, IFuelVehicle
{
    public double FuelLevel { get; private set; }
    
    public FuelVehicle(string brand, string model, int year, double initialFuel)
        : base(brand, model, year)
    {
        FuelLevel = initialFuel;
    }
    
    public void Refuel(double amount)
    {
        FuelLevel += amount;
        Console.WriteLine($"{Brand} {Model} zatankowano o {amount} litrów. Aktualny poziom paliwa: {FuelLevel}.");
    }
}

abstract class ElectricVehicle : Vehicle, IElectricVehicle
{
    public double BatteryLevel { get; private set; }
    
    public ElectricVehicle(string brand, string model, int year, double initialCharge)
        : base(brand, model, year)
    {
        BatteryLevel = initialCharge;
    }
    
    public void Charge(double amount)
    {
        BatteryLevel += amount;
        Console.WriteLine($"{Brand} {Model} naładowano o {amount}%. Aktualny poziom baterii: {BatteryLevel}%.");
    }
}

class Car : FuelVehicle
{
    public Car(string brand, string model, int year, double initialFuel)
        : base(brand, model, year, initialFuel) { }
    
    public override void Move()
    {
        Console.WriteLine($"Samochód {Brand} {Model} z {Year} jedzie.");
    }
}

class ElectricCar : ElectricVehicle
{
    public ElectricCar(string brand, string model, int year, double initialCharge)
        : base(brand, model, year, initialCharge) { }
    
    public override void Move()
    {
        Console.WriteLine($"Elektryczny samochód {Brand} {Model} z {Year} jedzie.");
    }
}

class Motorcycle : FuelVehicle
{
    public Motorcycle(string brand, string model, int year, double initialFuel)
        : base(brand, model, year, initialFuel) { }
    
    public override void Move()
    {
        Console.WriteLine($"Motocykl {Brand} {Model} z {Year} jedzie.");
    }
}

class Program
{
    static void Main()
    {
        List<Vehicle> vehicles = new List<Vehicle>
        {
            new Car("Toyota", "Corolla", 2020, 50),
            new ElectricCar("Tesla", "Model S", 2022, 80),
            new Motorcycle("Yamaha", "R1", 2019, 20)
        };
        
        foreach (var vehicle in vehicles)
        {
            vehicle.Move();
        }
        
        ((IFuelVehicle)vehicles[0]).Refuel(10);
        ((IElectricVehicle)vehicles[1]).Charge(15);
    }
}

/*
Move() - zaimplementowałem jako metode abstrakcyjną ponieważ używa jej każdy pojazd i każdy inaczej
Pojazdy spalinowe i elektryczne to osobne interfejsy, które definiują dodatkowe metody
Vehicle jest klasą abstrakcyjną ponieważ pojazdy mają wspólne cechy
pojazdy spalinowe i elektryczne są różnymi interfejsami bo mają inne metody,
    których nie ma potrzeby implementować w innych klasach.
FuelVehicle i ElectricVehicle dziedziczą po Vehicle i iplementują interfejsy, mając na myśli rozszerzalność.
*/