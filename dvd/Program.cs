namespace dvd;

class Program
{
    static int posX = 0;
    static int posY = 0;
    static int directionX = 1;
    static int directionY = 1;
    static string dvdText = "DVD";
    static int delayMs = 50;
    static bool clearing = true;
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
    static void HandleCollision()
    {
        var textLength = dvdText.Length;
        if (posX + textLength > Console.WindowWidth)
        {
            posX = Console.WindowWidth - textLength;
            directionX = -1;
        }
        if (posX <= 0)
        {
            posX = 0;
            directionX = 1;
        }
        if (posY <= 0)
        {
            posY = 0;
            directionY = 1;
        }
        if (posY >= Console.WindowHeight - 1)
        {
            posY = Console.WindowHeight - 1;
            directionY = -1;
        }
    }
    static void Draw()
    {
        try
        {
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
