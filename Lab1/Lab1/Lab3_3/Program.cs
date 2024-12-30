using System;
using System.Collections.Generic;

// Abstract Product
abstract class TetrisFigure
{
    public abstract void Display();
}

// Concrete Products
class StandardFigure : TetrisFigure
{
    private string shape;

    public StandardFigure(string shape)
    {
        this.shape = shape;
    }

    public override void Display()
    {
        Console.WriteLine($"Standard Figure: {shape}");
    }
}

class SuperFigure : TetrisFigure
{
    private string shape;

    public SuperFigure(string shape)
    {
        this.shape = shape + " with extra blocks";
    }

    public override void Display()
    {
        Console.WriteLine($"Super Figure: {shape}");
    }
}

// Abstract Creator
abstract class FigureCreator
{
    public abstract TetrisFigure CreateFigure();
}

// Concrete Creators
class StandardFigureCreator : FigureCreator
{
    private static readonly string[] Shapes = { "L", "T", "Z", "I", "O" };

    public override TetrisFigure CreateFigure()
    {
        Random random = new Random();
        string shape = Shapes[random.Next(Shapes.Length)];
        return new StandardFigure(shape);
    }
}

class SuperFigureCreator : FigureCreator
{
    private static readonly string[] Shapes = { "Super-L", "Super-T", "Super-Z", "Super-I", "Super-O" };

    public override TetrisFigure CreateFigure()
    {
        Random random = new Random();
        string shape = Shapes[random.Next(Shapes.Length)];
        return new SuperFigure(shape);
    }
}

// Game Class
class Game
{
    private FigureCreator creator;

    public Game(FigureCreator creator)
    {
        this.creator = creator;
    }

    public void GenerateFigure()
    {
        TetrisFigure figure = creator.CreateFigure();
        figure.Display();
    }
}

// Client
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Select figure type (1 - Standard, 2 - Super): ");
        string choice = Console.ReadLine();

        FigureCreator creator = choice == "1"
            ? new StandardFigureCreator()
            : new SuperFigureCreator();

        Game game = new Game(creator);
        game.GenerateFigure();
    }
}