// Observer
abstract class Observable
{
    private readonly List<IObserver> observers = new();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    protected void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update();
        }
    }
}

interface IObserver
{
    void Update();
}

// Model
interface IElevatorState
{
    void CallToFloor(FreightElevator elevator, int floor);
    void Load(FreightElevator elevator, double weight);
    void Unload(FreightElevator elevator, double weight);
    void RestorePower(FreightElevator elevator);
}

class FreightElevator : Observable
{
    private IElevatorState State { get; set; }
    public int CurrentFloor { get; private set; }
    public double LoadCapacity { get; private set; }
    public double CurrentLoad { get; private set; }
    private double PowerOutageProbability { get; set; }

    public FreightElevator(double loadCapacity)
    {
        LoadCapacity = loadCapacity;
        CurrentFloor = 1;
        CurrentLoad = 0;
        State = new RestState();
        PowerOutageProbability = 0.1;
    }

    public void SetElevatorState(IElevatorState newState)
    {
        State = newState;
        NotifyObservers();
    }

    public void ElevatorInfo()
    {
        Console.WriteLine();
        Console.WriteLine("--- Current Floor: " + CurrentFloor + " ---");
        Console.WriteLine("--- Current Load: " + CurrentLoad + " ---");
        Console.WriteLine("--- Current State: " + State + " ---");
        Console.WriteLine();
    }

    public void SetCurrentFloor(int floor)
    {
        CurrentFloor = floor;
        Console.WriteLine($"[Elevator] Current Floor: {floor}");
        NotifyObservers();
    }

    public void AddLoad(double weight)
    {
        CurrentLoad += weight;
        Console.WriteLine($"[Elevator] Current Load: {CurrentLoad} kg");
        NotifyObservers();
    }

    public void RemoveLoad(double weight)
    {
        CurrentLoad = Math.Max(0, CurrentLoad - weight);
        Console.WriteLine($"[Elevator] Current Load: {CurrentLoad} kg");
        NotifyObservers();
    }

    public bool CheckPowerOutage()
    {
        var random = new Random();
        return random.NextDouble() < PowerOutageProbability;
    }

    public void CallToFloor(int floor) => State.CallToFloor(this, floor);
    public void Load(double weight) => State.Load(this, weight);
    public void Unload(double weight) => State.Unload(this, weight);
    public void RestorePower() => State.RestorePower(this);
}

class RestState : IElevatorState
{
    public void CallToFloor(FreightElevator elevator, int floor)
    {
        if (elevator.CheckPowerOutage())
        {
            Console.WriteLine("[RestState] Power outage occurred!");
            elevator.SetElevatorState(new NoPowerState());
            return;
        }

        int floorDifference = Math.Abs(elevator.CurrentFloor - floor);
        int travelTime = floorDifference * 1000; // 1 second per floor

        Console.WriteLine($"[RestState] Moving to floor {floor} (Estimated time: {travelTime / 1000.0} seconds)...");
        var movingState = new MovingState();
        elevator.SetElevatorState(movingState);

        System.Threading.Thread.Sleep(travelTime);
        movingState.TransitionToRest(elevator, floor);
    }

    public void Load(FreightElevator elevator, double weight)
    {
        elevator.AddLoad(weight);
        if (elevator.CurrentLoad > elevator.LoadCapacity)
        {
            Console.WriteLine("[RestState] Elevator is overloaded!");
            elevator.SetElevatorState(new OverloadedState());
        }
    }

    public void Unload(FreightElevator elevator, double weight)
    {
        elevator.RemoveLoad(weight);
    }

    public void RestorePower(FreightElevator elevator)
    {
        Console.WriteLine("[RestState] Power is already on.");
    }
}

class MovingState : IElevatorState
{
    public void CallToFloor(FreightElevator elevator, int floor)
    {
        Console.WriteLine("[MovingState] Already moving.");
    }

    public void Load(FreightElevator elevator, double weight)
    {
        Console.WriteLine("[MovingState] Cannot load while moving.");
    }

    public void Unload(FreightElevator elevator, double weight)
    {
        Console.WriteLine("[MovingState] Cannot unload while moving.");
    }

    public void RestorePower(FreightElevator elevator)
    {
        Console.WriteLine("[MovingState] Power is already on.");
    }

    public MovingState()
    {
        Console.WriteLine("[MovingState] Elevator is moving...");
    }

    public void TransitionToRest(FreightElevator elevator, int floor)
    {
        Console.WriteLine($"[MovingState] Elevator arrived at floor {floor}.");
        elevator.SetCurrentFloor(floor);
        elevator.SetElevatorState(new RestState());
        Console.WriteLine("[MovingState] Elevator is now in RestState.");
    }
}

class OverloadedState : IElevatorState
{
    public void CallToFloor(FreightElevator elevator, int floor)
    {
        Console.WriteLine("[OverloadedState] Cannot move. Elevator is overloaded.");
    }

    public void Load(FreightElevator elevator, double weight)
    {
        Console.WriteLine("[OverloadedState] Cannot load more weight.");
    }

    public void Unload(FreightElevator elevator, double weight)
    {
        elevator.RemoveLoad(weight);
        if (elevator.CurrentLoad <= elevator.LoadCapacity)
        {
            Console.WriteLine("[OverloadedState] Load is back to normal.");
            elevator.SetElevatorState(new RestState());
        }
    }

    public void RestorePower(FreightElevator elevator)
    {
        Console.WriteLine("[OverloadedState] Power is already on.");
    }
}

class NoPowerState : IElevatorState
{
    public void CallToFloor(FreightElevator elevator, int floor)
    {
        Console.WriteLine("[NoPowerState] Cannot move. No power.");
    }

    public void Load(FreightElevator elevator, double weight)
    {
        Console.WriteLine("[NoPowerState] Cannot load without power.");
    }

    public void Unload(FreightElevator elevator, double weight)
    {
        Console.WriteLine("[NoPowerState] Cannot unload without power.");
    }

    public void RestorePower(FreightElevator elevator)
    {
        Console.WriteLine("[NoPowerState] Power restored.");
        elevator.SetElevatorState(new RestState());
    }
}

// Controller
class ElevatorController
{
    private readonly FreightElevator elevator;

    public ElevatorController(FreightElevator elevator)
    {
        this.elevator = elevator;
    }

    public void HandleInput(string command)
    {
        var inputs = command.Split(' ');
        switch (inputs[0].ToLower())
        {
            case "call":
                if (int.TryParse(inputs[1], out int floor))
                {
                    elevator.CallToFloor(floor);
                }
                else
                {
                    Console.WriteLine("Invalid floor number.");
                }
                break;

            case "load":
                if (double.TryParse(inputs[1], out double load))
                {
                    elevator.Load(load);
                }
                else
                {
                    Console.WriteLine("Invalid load value.");
                }
                break;

            case "unload":
                if (double.TryParse(inputs[1], out double unload))
                {
                    elevator.Unload(unload);
                }
                else
                {
                    Console.WriteLine("Invalid unload value.");
                }
                break;

            case "restore":
                elevator.RestorePower();
                break;

            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }
}

// View
class ElevatorView : IObserver
{
    private readonly FreightElevator model;

    public ElevatorView(FreightElevator model)
    {
        this.model = model;
        model.AddObserver(this);
    }

    public void Run(ElevatorController controller)
    {
        Console.WriteLine("Elevator system started. Commands: call [floor], load [weight], unload [weight], restore.");
        Console.WriteLine("Type 'exit' to quit.");
        while (true)
        {
            Console.Write("Enter command: ");
            var command = Console.ReadLine();
            if (command.ToLower() == "exit")
            {
                Console.WriteLine("Exiting elevator system.");
                break;
            }
            controller.HandleInput(command);
        }
    }

    public void Update()
    {
        Console.WriteLine("--- Elevator Update ---");
        model.ElevatorInfo();
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        var elevator = new FreightElevator(1000);
        var controller = new ElevatorController(elevator);
        var view = new ElevatorView(elevator);

        view.Run(controller);
    }
}
