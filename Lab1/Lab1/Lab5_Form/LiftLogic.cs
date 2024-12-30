using System;

abstract class LiftState
{
    public abstract void CallToFloor(Lift lift, int floor);
    public abstract void LoadCargo(Lift lift, double weight);
    public abstract void UnloadCargo(Lift lift, double weight);
    public abstract void RestorePower(Lift lift);
}

class IdleState : LiftState
{
    public override void CallToFloor(Lift lift, int floor)
    {
        Console.WriteLine($"Lift is moving from floor {lift.CurrentFloor} to floor {floor}.");
        lift.SetState(new MovingState(floor));
    }

    public override void LoadCargo(Lift lift, double weight)
    {
        lift.CurrentLoad += weight;
        Console.WriteLine($"Cargo loaded: {weight} kg. Current load: {lift.CurrentLoad} kg.");
        if (lift.CurrentLoad > lift.Capacity)
        {
            Console.WriteLine("Lift is overloaded!");
            lift.SetState(new OverloadedState());
        }
    }

    public override void UnloadCargo(Lift lift, double weight)
    {
        lift.CurrentLoad -= weight;
        Console.WriteLine($"Cargo unloaded: {weight} kg. Current load: {lift.CurrentLoad} kg.");
    }

    public override void RestorePower(Lift lift)
    {
        Console.WriteLine("Power is already available.");
    }
}

class MovingState : LiftState
{
    private readonly int _targetFloor;

    public MovingState(int targetFloor)
    {
        _targetFloor = targetFloor;
    }

    public override void CallToFloor(Lift lift, int floor)
    {
        Console.WriteLine("Lift is already moving.");
    }

    public override void LoadCargo(Lift lift, double weight)
    {
        Console.WriteLine("Cannot load cargo while moving.");
    }

    public override void UnloadCargo(Lift lift, double weight)
    {
        Console.WriteLine("Cannot unload cargo while moving.");
    }

    public override void RestorePower(Lift lift)
    {
        Console.WriteLine("Power is already available.");
    }

    public async Task MoveLift(Lift lift, Action updateUI)
    {
        int direction = lift.CurrentFloor < _targetFloor ? 1 : -1;

        while (lift.CurrentFloor != _targetFloor)
        {
            await Task.Delay(500); // Пауза для имитации движения
            lift.CurrentFloor += direction;

            // Вызов обновления UI в безопасном контексте
            updateUI?.Invoke();
        }

        Console.WriteLine($"Lift has arrived at floor {_targetFloor}.");
        lift.SetState(new IdleState());

        // Финальное обновление UI после завершения движения
        updateUI?.Invoke();
    }
}

class OverloadedState : LiftState
{
    public override void CallToFloor(Lift lift, int floor)
    {
        Console.WriteLine("Cannot move. The lift is overloaded.");
    }

    public override void LoadCargo(Lift lift, double weight)
    {
        Console.WriteLine("Cannot load more cargo. The lift is overloaded.");
    }

    public override void UnloadCargo(Lift lift, double weight)
    {
        lift.CurrentLoad -= weight;
        Console.WriteLine($"Cargo unloaded: {weight} kg. Current load: {lift.CurrentLoad} kg.");
        if (lift.CurrentLoad <= lift.Capacity)
        {
            Console.WriteLine("Lift is no longer overloaded.");
            lift.SetState(new IdleState());
        }
    }

    public override void RestorePower(Lift lift)
    {
        Console.WriteLine("Power is already available.");
    }
}

class NoPowerState : LiftState
{
    public override void CallToFloor(Lift lift, int floor)
    {
        Console.WriteLine("Cannot move. No power.");
    }

    public override void LoadCargo(Lift lift, double weight)
    {
        Console.WriteLine("Cannot load cargo. No power.");
    }

    public override void UnloadCargo(Lift lift, double weight)
    {
        Console.WriteLine("Cannot unload cargo. No power.");
    }

    public override void RestorePower(Lift lift)
    {
        Console.WriteLine("Power restored. Returning to idle state.");
        lift.SetState(new IdleState());
    }
}

class EmergencyState : LiftState
{
    public override void CallToFloor(Lift lift, int floor)
    {
        Console.WriteLine("Cannot move. Lift is in emergency state.");
    }

    public override void LoadCargo(Lift lift, double weight)
    {
        Console.WriteLine("Cannot load cargo. Lift is in emergency state.");
    }

    public override void UnloadCargo(Lift lift, double weight)
    {
        Console.WriteLine("Cannot unload cargo. Lift is in emergency state.");
    }

    public override void RestorePower(Lift lift)
    {
        Console.WriteLine("Power restored, but lift needs maintenance.");
    }
}

class Lift
{
    public int CurrentFloor { get; set; } = 0;
    public double Capacity { get; }
    public double CurrentLoad { get; set; } = 0;
    public bool PowerAvailable { get; private set; } = true;
    private LiftState _currentState;

    public Lift(double capacity)
    {
        Capacity = capacity;
        _currentState = new IdleState();
    }

    public string CurrentStateName => _currentState.GetType().Name;

    public void SetState(LiftState state)
    {
        _currentState = state;
    }

    public async void CallToFloor(int floor, Action updateUI)
    {
        if (_currentState is MovingState movingState)
        {
            Console.WriteLine("Lift is already moving.");
        }
        else
        {
            _currentState.CallToFloor(this, floor);

            if (_currentState is MovingState newState)
            {
                await newState.MoveLift(this, updateUI);
            }
        }
    }

    public void LoadCargo(double weight)
    {
        _currentState.LoadCargo(this, weight);
    }

    public void UnloadCargo(double weight)
    {
        _currentState.UnloadCargo(this, weight);
    }

    public void RestorePower()
    {
        _currentState.RestorePower(this);
    }
}
