using System;

interface IElevatorState
{
    void CallToFloor(FreightElevator elevator, int floor);
    void Load(FreightElevator elevator, double weight);
    void Unload(FreightElevator elevator, double weight);
    void RestorePower(FreightElevator elevator);
}

class FreightElevator
{
    public IElevatorState State { get; set; }
    public int CurrentFloor { get; private set; } = 1;
    public double LoadCapacity { get; } = 1000;
    public double CurrentLoad { get; private set; } = 0;
    public double PowerOutageProbability { get; } = 0.1;

    public FreightElevator(IElevatorState initialState)
    {
        State = initialState;
    }

    public void SetCurrentFloor(int floor)
    {
        CurrentFloor = floor;
        Console.WriteLine($"[Elevator] Current Floor: {floor}");
    }

    public void AddLoad(double weight)
    {
        CurrentLoad += weight;
        Console.WriteLine($"[Elevator] Current Load: {CurrentLoad} kg");
    }

    public void RemoveLoad(double weight)
    {
        CurrentLoad = Math.Max(0, CurrentLoad - weight);
        Console.WriteLine($"[Elevator] Current Load: {CurrentLoad} kg");
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
            elevator.State = new NoPowerState();
            return;
        }

        int floorDifference = Math.Abs(elevator.CurrentFloor - floor);
        int travelTime = floorDifference * 1000; // 1 секунда за этаж

        Console.WriteLine($"[RestState] Moving to floor {floor} (Estimated time: {travelTime / 1000.0} seconds)...");
        var movingState = new MovingState();
        elevator.State = movingState;

        System.Threading.Thread.Sleep(travelTime);
        movingState.TransitionToRest(elevator, floor);
    }

    public void Load(FreightElevator elevator, double weight)
    {
        elevator.AddLoad(weight);
        if (elevator.CurrentLoad > elevator.LoadCapacity)
        {
            Console.WriteLine("[RestState] Elevator is overloaded!");
            elevator.State = new OverloadedState();
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
        elevator.State = new RestState();
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
            elevator.State = new RestState();
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
        elevator.State = new RestState();
    }
}

class EmergencyState : IElevatorState
{
    public void CallToFloor(FreightElevator elevator, int floor)
    {
        Console.WriteLine("[EmergencyState] Cannot move. Elevator in emergency mode.");
    }

    public void Load(FreightElevator elevator, double weight)
    {
        Console.WriteLine("[EmergencyState] Cannot load in emergency mode.");
    }

    public void Unload(FreightElevator elevator, double weight)
    {
        Console.WriteLine("[EmergencyState] Cannot unload in emergency mode.");
    }

    public void RestorePower(FreightElevator elevator)
    {
        Console.WriteLine("[EmergencyState] Elevator reset to RestState.");
        elevator.State = new RestState();
    }
}

// Entry Point
class Program
{
    static void Main(string[] args)
    {
        FreightElevator elevator = new FreightElevator(new RestState());

        while (true)
        {
            Console.WriteLine("\nActions: [C]all floor, [L]oad, [U]nload, [R]estore power, [E]xit");
            char action = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            switch (action)
            {
                case 'C':
                    Console.Write("Enter floor number: ");
                    int floor = int.Parse(Console.ReadLine());
                    elevator.CallToFloor(floor);
                    break;

                case 'L':
                    Console.Write("Enter load weight: ");
                    double weightLoad = double.Parse(Console.ReadLine());
                    elevator.Load(weightLoad);
                    break;

                case 'U':
                    Console.Write("Enter unload weight: ");
                    double weightUnload = double.Parse(Console.ReadLine());
                    elevator.Unload(weightUnload);
                    break;

                case 'R':
                    elevator.RestorePower();
                    break;

                case 'E':
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid action. Try again.");
                    break;
            }
        }
    }
}
