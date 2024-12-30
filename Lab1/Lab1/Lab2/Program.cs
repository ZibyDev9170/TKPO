// Интерфейс управления двигателем
public interface IEngineControl
{
    void Start();
    void Stop();
}

// Интерфейс управления топливом
public interface IFuelManagement
{
    void Refuel(double liters);
}

public abstract class Engine : IEngineControl, IFuelManagement
{
    public string Type { get; protected set; }
    public bool IsRunning { get; private set; }
    public double FuelLevel { get; private set; }

    public Engine(string type)
    {
        Type = type;
    }

    public virtual void Start()
    {
        if (FuelLevel > 0)
        {
            IsRunning = true;
            Console.WriteLine($"{Type} engine started.");
        }
        else
        {
            Console.WriteLine($"{Type} engine cannot start. No fuel.");
        }
    }

    public virtual void Stop()
    {
        IsRunning = false;
        Console.WriteLine($"{Type} engine stopped.");
    }

    public virtual void Refuel(double liters)
    {
        FuelLevel += liters;
        Console.WriteLine($"{liters} liters added to the {Type} engine. Current fuel: {FuelLevel} liters.");
    }
}

public class GasolineEngine : Engine
{
    public GasolineEngine() : base("Gasoline") { }

    public override void Start()
    {
        base.Start();
        Console.WriteLine("Gasoline engine is running smoothly.");
    }
}

public class DieselEngine : Engine
{
    public DieselEngine() : base("Diesel") { }

    public override void Start()
    {
        base.Start();
        Console.WriteLine("Diesel engine is rumbling strongly.");
    }
}

public class JetEngine : Engine
{
    public JetEngine() : base("Jet") { }

    public override void Start()
    {
        base.Start();
        Console.WriteLine("Jet engine is roaring powerfully.");
    }
}

public class EngineManager
{
    private readonly IEngineControl _engineControl;
    private readonly IFuelManagement _fuelManagement;

    public EngineManager(IEngineControl engineControl, IFuelManagement fuelManagement)
    {
        _engineControl = engineControl;
        _fuelManagement = fuelManagement;
    }

    public void OperateEngine()
    {
        _fuelManagement.Refuel(50);
        _engineControl.Start();
        _engineControl.Stop();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Работа с бензиновым двигателем
        GasolineEngine gasolineEngine = new GasolineEngine();
        EngineManager manager1 = new EngineManager(gasolineEngine, gasolineEngine);
        Console.WriteLine("\nOperating Gasoline Engine:");
        manager1.OperateEngine();

        // Работа с дизельным двигателем
        DieselEngine dieselEngine = new DieselEngine();
        EngineManager manager2 = new EngineManager(dieselEngine, dieselEngine);
        Console.WriteLine("\nOperating Diesel Engine:");
        manager2.OperateEngine();

        // Работа с реактивным двигателем
        JetEngine jetEngine = new JetEngine();
        EngineManager manager3 = new EngineManager(jetEngine, jetEngine);
        Console.WriteLine("\nOperating Jet Engine:");
        manager3.OperateEngine();
    }
}
