using System;
using System.Collections.Generic;

// Enum for Meal Plans
public enum MealPlan
{
    TwoMeals,
    ThreeMeals,
    AllInclusive
}

// Enum for Tour Package Types
public enum TourPackageType
{
    Beach,
    Excursion,
    Skiing
}

// Base class for Tour Packages
public abstract class TourPackage
{
    public int Duration { get; set; } // Duration in days
    public string Country { get; set; }
    public int HotelStars { get; set; } // Hotel star rating
    public MealPlan MealPlan { get; set; } // Meal plan

    public abstract decimal CalculateCost(); // Abstract method for cost calculation
}

// Beach Vacation Class
public class BeachVacation : TourPackage
{
    public override decimal CalculateCost()
    {
        decimal baseCost = 100m * Duration;
        baseCost += HotelStars * 50m;
        baseCost += MealPlan switch
        {
            MealPlan.TwoMeals => 20m * Duration,
            MealPlan.ThreeMeals => 30m * Duration,
            MealPlan.AllInclusive => 50m * Duration,
            _ => 0m
        };
        return baseCost;
    }
}

// Excursion Class
public class Excursion : TourPackage
{
    public override decimal CalculateCost()
    {
        decimal baseCost = 80m * Duration;
        baseCost += HotelStars * 40m;
        baseCost += MealPlan switch
        {
            MealPlan.TwoMeals => 15m * Duration,
            MealPlan.ThreeMeals => 25m * Duration,
            MealPlan.AllInclusive => 40m * Duration,
            _ => 0m
        };
        return baseCost;
    }
}

// Skiing Class
public class Skiing : TourPackage
{
    public override decimal CalculateCost()
    {
        decimal baseCost = 150m * Duration;
        baseCost += HotelStars * 70m;
        baseCost += MealPlan switch
        {
            MealPlan.TwoMeals => 30m * Duration,
            MealPlan.ThreeMeals => 45m * Duration,
            MealPlan.AllInclusive => 60m * Duration,
            _ => 0m
        };
        return baseCost;
    }
}

// Facade Class
public class TourPackageFacade
{
    private readonly Dictionary<TourPackageType, Func<TourPackage>> _packageFactories;

    public TourPackageFacade()
    {
        _packageFactories = new Dictionary<TourPackageType, Func<TourPackage>>
        {
            {TourPackageType.Beach, () => new BeachVacation()},
            {TourPackageType.Excursion, () => new Excursion()},
            {TourPackageType.Skiing, () => new Skiing()}
        };
    }

    public TourPackage CreatePackage(TourPackageType type, int duration, string country, int hotelStars, MealPlan mealPlan)
    {
        if (!_packageFactories.ContainsKey(type))
            throw new ArgumentException("Invalid package type");

        var package = _packageFactories[type].Invoke();
        package.Duration = duration;
        package.Country = country;
        package.HotelStars = hotelStars;
        package.MealPlan = mealPlan;
        return package;
    }

    public decimal GetPackageCost(TourPackage package)
    {
        return package.CalculateCost();
    }
}

// Console Application
class Program
{
    static void Main()
    {
        var facade = new TourPackageFacade();

        Console.WriteLine("Choose package type (0: Beach, 1: Excursion, 2: Skiing):");
        TourPackageType type = (TourPackageType)int.Parse(Console.ReadLine());

        Console.WriteLine("Enter duration (days):");
        int duration = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter country:");
        string country = Console.ReadLine();

        Console.WriteLine("Enter hotel stars (1-5):");
        int hotelStars = int.Parse(Console.ReadLine());

        Console.WriteLine("Choose meal plan (0: TwoMeals, 1: ThreeMeals, 2: AllInclusive):");
        MealPlan mealPlan = (MealPlan)int.Parse(Console.ReadLine());

        try
        {
            var package = facade.CreatePackage(type, duration, country, hotelStars, mealPlan);
            decimal cost = facade.GetPackageCost(package);

            Console.WriteLine($"The cost of your {type} package is: {cost:C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
