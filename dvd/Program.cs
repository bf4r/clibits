namespace dvd;

class Program
{
    static int posX = 0;
    static int posY = 0;
    static int directionX = 1;
    static int directionY = 1;
    static string dvdText = "DVD";
    static int delayMs = 200;
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
                Console.Clear();
                if (Console.KeyAvailable)
                {
                    var ki = Console.ReadKey(true);
                    if (ki.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        Console.CursorVisible = true;
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
