
namespace Pojazdy
{
    // Klasa abstrakcyjna Vehicle
    abstract class Vehicle
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        // Metoda abstrakcyjna
        public abstract void DisplayInfo();

        // Metoda wirtualna z domyślną implementacją
        public virtual void StartEngine()
        {
            Console.WriteLine("Silnik uruchomiony.");
        }
    }

    // Klasa Car dziedzicząca po Vehicle
    class Car : Vehicle
    {
        public int NumberOfDoors { get; set; }

        // Implementacja metody abstrakcyjnej
        public override void DisplayInfo()
        {
            Console.WriteLine($"Samochód: {Brand} {Model} ({Year}) - Liczba drzwi: {NumberOfDoors}");
        }

        // Nadpisanie metody StartEngine
        public override void StartEngine()
        {
            Console.WriteLine("Silnik samochodu uruchomiony.");
        }
    }

    // Klasa Motorcycle dziedzicząca po Vehicle
    class Motorcycle : Vehicle
    {
        public bool HasSidecar { get; set; }

        // Implementacja metody abstrakcyjnej
        public override void DisplayInfo()
        {
            Console.WriteLine($"Motocykl: {Brand} {Model} ({Year}) - Wózek boczny: {(HasSidecar ? "Tak" : "Nie")}");
        }

        // Nadpisanie metody StartEngine
        public override void StartEngine()
        {
            Console.WriteLine("Silnik motocykla uruchomiony.");
        }
    }

    class Program
    {
        static void Main()
        {
            // Lista pojazdów
            List<Vehicle> vehicles = new List<Vehicle>
            {
                new Car { Brand = "Toyota", Model = "Corolla", Year = 2020, NumberOfDoors = 4 },
                new Car { Brand = "Honda", Model = "Civic", Year = 2018, NumberOfDoors = 4 },
                new Motorcycle { Brand = "Harley-Davidson", Model = "Iron 883", Year = 2019, HasSidecar = false },
                new Motorcycle { Brand = "BMW", Model = "R 1250", Year = 2021, HasSidecar = true }
            };

            // Wywołanie
            foreach (Vehicle v in vehicles)
            {
                v.DisplayInfo();
                v.StartEngine();
                Console.WriteLine();
            }
        }
    }
}
