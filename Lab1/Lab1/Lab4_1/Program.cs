using System;

// Адаптируемый класс
class GasCylinder
{
    public double Volume { get; set; } // Объём баллона, м3
    public double Mass { get; set; } // Масса газа, кг
    public double Molar { get; set; } // Молярная масса газа, кг/моль

    public double GetPressure(int T)
    {
        const double R = 8.314; // Газовая постоянная, Дж/(моль*К)
        return (Mass / Molar) * R * T / Volume;
    }

    public double AmountOfMatter()
    {
        return Mass / Molar;
    }

    public override string ToString()
    {
        return $"Volume: {Volume} m3, Mass: {Mass} kg, Molar: {Molar} kg/mol";
    }
}

// Целевой интерфейс
interface ICylinder
{
    void ModifVolume(double dV);
    double GetDp(int T0, int T1);
    string Passport();
}

// Адаптер
class GasCylinderAdapter : ICylinder
{
    private readonly GasCylinder _cylinder;

    public GasCylinderAdapter(GasCylinder cylinder)
    {
        _cylinder = cylinder;
    }

    public void ModifVolume(double dV)
    {
        _cylinder.Volume += dV;
    }

    public double GetDp(int T0, int T1)
    {
        return _cylinder.GetPressure(T1) - _cylinder.GetPressure(T0);
    }

    public string Passport()
    {
        return _cylinder.ToString();
    }
}

// Клиент
class Program
{
    static void Main(string[] args)
    {
        GasCylinder cylinder = new GasCylinder
        {
            Volume = 1.0,
            Mass = 2.0,
            Molar = 0.04
        };

        ICylinder adapter = new GasCylinderAdapter(cylinder);

        Console.WriteLine("Initial Passport:");
        Console.WriteLine(adapter.Passport());

        adapter.ModifVolume(0.5);
        Console.WriteLine("After Modifying Volume:");
        Console.WriteLine(adapter.Passport());

        Console.WriteLine($"Pressure Change (T0=300K, T1=350K): {adapter.GetDp(300, 350):F2} Pa");
    }
}