using DependencyInjectionMVC_Auth_Fixed.Interfaces;

namespace DependencyInjectionMVC_Auth_Fixed.Services;

public class AdminMessageService : IMessageService
{
    public string GetMessage() => "Witaj administratorze!";
}
