namespace lab4
{
    // Interface
    public interface IPaymentMethod
    {
        bool ProcessPayment(decimal amount);
        
        string GetMethodName();
    }
    
    // credit card implementation
    public class CreditCardPayment : IPaymentMethod
    {
        private const decimal Limit = 5000;

        public bool ProcessPayment(decimal amount)
        {
            if (amount > Limit)
            {
                Console.WriteLine("Płatność kartą kredytową nie powiodła się (max 5000 PLN).");
                return false;
            }
            Console.WriteLine($"Płatność kartą kredytową przetworzona: kwota {amount} PLN.");
            return true;
            
        }

        public string GetMethodName() => "Płatność kartą kredytową";
    }

    // Paypal implementation
    public class PayPalPayment : IPaymentMethod
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Płatność PayPal przetworzona: kwota {amount} PLN pobrana z konta PayPal.");
            return true;
        }

        public string GetMethodName() => "PayPal";
    }

    // Crypto implementation
    public class CryptoPayment : IPaymentMethod
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Płatność kryptowalutą przetworzona: kwota {amount} PLN.");
            return true;
        }

        public string GetMethodName() => "Crypto Payment";
    }
    
    // Blik implementation
    public class BlikPayment : IPaymentMethod
    {
        private const decimal Limit = 500;

        public bool ProcessPayment(decimal amount)
        {
            if (amount > Limit)
            {
                Console.WriteLine("Płatność Blik nie powiodła się (max 500 PLN).");
                return false;
            }
            Console.WriteLine($"Płatność Blik przetworzona: kwota {amount} PLN.");
            return true;
            
        }

        public string GetMethodName() => "Płatność Blik";
    }

    class Program
    {
        static void Main(string[] args)
        {
            // payment methods list
            List<IPaymentMethod> paymentMethods = new List<IPaymentMethod>
            {
                new CreditCardPayment(),
                new PayPalPayment(),
                new CryptoPayment(),
                new BlikPayment()
            };

            Console.WriteLine("Wybierz metodę płatności:");
            for (int i = 0; i < paymentMethods.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {paymentMethods[i].GetMethodName()}");
            }

            Console.Write("Twój wybór: ");
            // 'int.TryParse(Console.ReadLine(), out int x' tries to convert user input from console to int
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= paymentMethods.Count)
            {
                Console.Write("Podaj kwotę transakcji (PLN): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    bool result = paymentMethods[choice - 1].ProcessPayment(amount);
                    if (result)
                    {
                        Console.WriteLine("Transakcja zakończona powodzeniem.");
                        string filePath = "payments.txt";
                        LogPayments(filePath, paymentMethods[choice - 1].GetMethodName(), amount.ToString());
                        Console.WriteLine("Payment logged successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Transakcja nie powiodła się.");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór metody płatności.");
            }
        }
        
        public static void LogPayments(string filePath, string payment_method, string amount)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{payment_method}: {amount}");
            }
        }
    }
}
