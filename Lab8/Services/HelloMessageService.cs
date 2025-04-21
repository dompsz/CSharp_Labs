using DependencyInjectionMVC_Auth_Fixed.Interfaces;

namespace DependencyInjectionMVC_Auth_Fixed.Services;

public class HelloMessageService : IMessageService
{
    public string GetMessage() => "Witaj użytkowniku!";
}
