using System;

// Abstract Factory
abstract class CinemaFactory
{
    public abstract AudioTrack CreateAudioTrack();
    public abstract Subtitles CreateSubtitles();
}

// Concrete Factories
class EnglishCinemaFactory : CinemaFactory
{
    public override AudioTrack CreateAudioTrack()
    {
        return new EnglishAudioTrack();
    }

    public override Subtitles CreateSubtitles()
    {
        return new EnglishSubtitles();
    }
}

class RussianCinemaFactory : CinemaFactory
{
    public override AudioTrack CreateAudioTrack()
    {
        return new RussianAudioTrack();
    }

    public override Subtitles CreateSubtitles()
    {
        return new RussianSubtitles();
    }
}

// Abstract Products
abstract class AudioTrack
{
    public abstract void PlayAudio();
}

abstract class Subtitles
{
    public abstract void DisplaySubtitles();
}

// Concrete Products
class EnglishAudioTrack : AudioTrack
{
    public override void PlayAudio()
    {
        Console.WriteLine("Playing audio in English.");
    }
}

class RussianAudioTrack : AudioTrack
{
    public override void PlayAudio()
    {
        Console.WriteLine("Playing audio in Russian.");
    }
}

class EnglishSubtitles : Subtitles
{
    public override void DisplaySubtitles()
    {
        Console.WriteLine("Displaying subtitles in English.");
    }
}

class RussianSubtitles : Subtitles
{
    public override void DisplaySubtitles()
    {
        Console.WriteLine("Displaying subtitles in Russian.");
    }
}

// Cinema Rental System
class CinemaRentalSystem
{
    private AudioTrack audioTrack;
    private Subtitles subtitles;

    public void SetLanguage(CinemaFactory factory)
    {
        audioTrack = factory.CreateAudioTrack();
        subtitles = factory.CreateSubtitles();
    }

    public void PlayMovie()
    {
        audioTrack.PlayAudio();
        subtitles.DisplaySubtitles();
    }
}

// Client
class Program
{
    static void Main(string[] args)
    {
        CinemaRentalSystem rentalSystem = new CinemaRentalSystem();

        Console.WriteLine("Select language (1 - English, 2 - Russian): ");
        string choice = Console.ReadLine();

        CinemaFactory factory = choice == "1" ? new EnglishCinemaFactory() : new RussianCinemaFactory();

        rentalSystem.SetLanguage(factory);
        rentalSystem.PlayMovie();
    }
}