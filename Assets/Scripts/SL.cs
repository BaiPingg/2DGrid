using System.Collections.Generic;

public interface IService
{
    void Init();
    void Shutdown();
}

public class SL
{
    private static Dictionary<string, IService> _services = new();

    public static void Register<T>(IService service) where T : IService
    {
        if (!_services.ContainsKey(typeof(T).Name))
        {
            _services[typeof(T).Name] = service;
            service.Init();
        }
    }

    public static T Get<T>() where T : class, IService
    {
        if (_services.ContainsKey(typeof(T).Name))
        {
            return _services[typeof(T).Name] as T;
        }

        return null;
    }

    public static void Shutdown()
    {
        foreach (var service in _services)
        {
            service.Value.Shutdown();
        }
    }
}