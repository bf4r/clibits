﻿namespace dvd;

class Program
{
    static int posX = 0;
    static int posY = 0;
    static int directionX = 1;
    static int directionY = 1;
    static string dvdText = "DVD";
    static int delayMs = 50;
    static bool clearing = true;
    static int hue = 0;
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        var dvdTextLength = dvdText.Length;
        Draw();
        while (true)
        {
            try
            {
                Move();
                HandleCollision();
                Draw();
                Thread.Sleep(delayMs);
                if (clearing) Console.Clear();
                if (Console.KeyAvailable)
                {
                    var ki = Console.ReadKey(true);
                    switch (ki.Key)
                    {
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q:
                            Console.CursorVisible = true;
                            return;
                        case ConsoleKey.Add:
                            delayMs++;
                            break;
                        case ConsoleKey.C:
                            clearing = !clearing;
                            break;
                        case ConsoleKey.Subtract:
                            if (delayMs > 0) delayMs--;
                            break;
                        case ConsoleKey.T:
                            Console.CursorVisible = true;
                            Console.Clear();
                            var input = Console.ReadLine();
                            if (!string.IsNullOrEmpty(input) && input.Length < Console.WindowWidth)
                            {
                                dvdText = input;
                                posX = 0;
                                posY = 0;
                            }
                            Console.CursorVisible = false;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
    static void Move()
    {
        posX += directionX;
        posY += directionY;
    }
    static void ChangeColor()
    {
        hue = Random.Shared.Next(0, 360);
    }
    static void HandleCollision()
    {
        var textLength = dvdText.Length;
        if (posX + textLength > Console.WindowWidth)
        {
            posX = Console.WindowWidth - textLength;
            directionX = -1;
            ChangeColor();
        }
        if (posX <= 0)
        {
            posX = 0;
            directionX = 1;
            ChangeColor();
        }
        if (posY <= 0)
        {
            posY = 0;
            directionY = 1;
            ChangeColor();
        }
        if (posY >= Console.WindowHeight - 1)
        {
            posY = Console.WindowHeight - 1;
            directionY = -1;
            ChangeColor();
        }
    }
    static void Draw()
    {
        try
        {
            Utils.SetColor(Utils.HSVToRGB((int)((hue / 360.0) * 360) % 360, 1, 1));
            Console.SetCursorPosition(posX, posY);
            string newDvdText = dvdText;
            if (posX + dvdText.Length > Console.WindowWidth)
            {
                newDvdText = dvdText.Substring(Console.WindowWidth - posX);
            }
            Console.Write(newDvdText);
        }
        catch (Exception)
        {
        }
    }
}
