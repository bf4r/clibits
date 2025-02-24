namespace spinningcube
{
    public class Program
    {
        static float[,] cubeVertices = {
            {-1, -1, -1},
            {-1, -1,  1},
            {-1,  1, -1},
            {-1,  1,  1},
            { 1, -1, -1},
            { 1, -1,  1},
            { 1,  1, -1},
            { 1,  1,  1}
        };

        static int[,] cubeEdges = {
            {0, 1}, {1, 3}, {3, 2}, {2, 0},
            {4, 5}, {5, 7}, {7, 6}, {6, 4},
            {0, 4}, {1, 5}, {2, 6}, {3, 7}
        };

        static float angleX, angleY, angleZ;
        static int MsDelay = 15;
        static int CyclesPerTick = 1;
        static int Thickness = 2;
        static string Symbol = "█";
        static int XChangeMult = 5;
        static int YChangeMult = 3;
        static int ZChangeMult = 4;
        static bool ShowInfo = false;
        static bool Clearing = true;
        static bool FreezeFrame = false;

        private static float frozenAngleX, frozenAngleY, frozenAngleZ;
        public static void Reset()
        {
            angleX = 0;
            angleY = 0;
            angleZ = 0;
            MsDelay = 15;
            CyclesPerTick = 1;
            Thickness = 2;
            Symbol = "█";
            XChangeMult = 5;
            YChangeMult = 3;
            ZChangeMult = 4;
            ShowInfo = false;
            Clearing = true;
            FreezeFrame = false;
        }
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            long it = 0;

            while (true)
            {
                it++;
                try
                {
                    if (Console.WindowWidth != width || Console.WindowHeight != height)
                    {
                        width = Console.WindowWidth;
                        height = Console.WindowHeight;
                        Console.Clear();
                    }

                    if (Clearing)
                    {
                        Console.Clear();
                    }

                    if (ShowInfo)
                    {
                        var info = "[*,/] Thickness: " + Thickness + ", [+/-] Delay: " + MsDelay + ", [M] Switch color mode, [J/L] Cycles per tick: " + CyclesPerTick + ", [C] Symbol: " + Symbol + $", [A,D,S,W,Q,E] Change angle rates ({XChangeMult}, {YChangeMult}, {ZChangeMult}), [O] Clearing: {Clearing} [F] Freeze frame {FreezeFrame} [F1] Hide menu, [Esc] Exit";
                        Console.SetCursorPosition(0, 0);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(info);
                    }

                    float currentAngleX = FreezeFrame ? frozenAngleX : angleX;
                    float currentAngleY = FreezeFrame ? frozenAngleY : angleY;
                    float currentAngleZ = FreezeFrame ? frozenAngleZ : angleZ;

                    float cosX = (float)Math.Cos(currentAngleX), sinX = (float)Math.Sin(currentAngleX);
                    float cosY = (float)Math.Cos(currentAngleY), sinY = (float)Math.Sin(currentAngleY);
                    float cosZ = (float)Math.Cos(currentAngleZ), sinZ = (float)Math.Sin(currentAngleZ);

                    StringsToPrint.Clear();

                    for (int i = 0; i < cubeEdges.GetLength(0); i++)
                    {
                        int startVertIndex = cubeEdges[i, 0];
                        int endVertIndex = cubeEdges[i, 1];
                        float[] rotatedStartVertex = RotateVertex(cubeVertices[startVertIndex, 0], cubeVertices[startVertIndex, 1], cubeVertices[startVertIndex, 2], cosX, sinX, cosY, sinY, cosZ, sinZ);
                        float[] rotatedEndVertex = RotateVertex(cubeVertices[endVertIndex, 0], cubeVertices[endVertIndex, 1], cubeVertices[endVertIndex, 2], cosX, sinX, cosY, sinY, cosZ, sinZ);

                        int startX = (int)((rotatedStartVertex[0] + 2) * width / 4);
                        int startY = (int)((rotatedStartVertex[1] + 2) * height / 4);
                        int endX = (int)((rotatedEndVertex[0] + 2) * width / 4);
                        int endY = (int)((rotatedEndVertex[1] + 2) * height / 4);

                        if (it % CyclesPerTick == 0)
                        {
                            DrawLineToBuffer(startX, startY, endX, endY, width, it);
                        }
                    }

                    foreach (var kvp in StringsToPrint)
                    {
                        Console.SetCursorPosition(kvp.Key.Item1, kvp.Key.Item2);
                        Console.Write(kvp.Value);
                    }

                    if (!FreezeFrame)
                    {
                        angleX += 0.01f * XChangeMult;
                        angleY += 0.01f * YChangeMult;
                        angleZ += 0.01f * ZChangeMult;
                    }

                    if (it % CyclesPerTick == 0)
                    {
                        Thread.Sleep(MsDelay);
                    }

                    if (Console.KeyAvailable)
                    {
                        var ki = Console.ReadKey(true);
                        var key = ki.Key;

                        switch (key)
                        {
                            case ConsoleKey.D:
                                if (FreezeFrame) frozenAngleX += 0.01f * XChangeMult; else XChangeMult++;
                                break;
                            case ConsoleKey.A:
                                if (FreezeFrame) frozenAngleX -= 0.01f * XChangeMult; else XChangeMult--;
                                break;
                            case ConsoleKey.W:
                                if (FreezeFrame) frozenAngleY += 0.01f * YChangeMult; else YChangeMult++;
                                break;
                            case ConsoleKey.S:
                                if (FreezeFrame) frozenAngleY -= 0.01f * YChangeMult; else YChangeMult--;
                                break;
                            case ConsoleKey.E:
                                if (FreezeFrame) frozenAngleZ += 0.01f * ZChangeMult; else ZChangeMult++;
                                break;
                            case ConsoleKey.Q:
                                if (FreezeFrame) frozenAngleZ -= 0.01f * ZChangeMult; else ZChangeMult--;
                                break;
                            case ConsoleKey.L:
                                CyclesPerTick++;
                                break;
                            case ConsoleKey.J:
                                if (CyclesPerTick > 1)
                                {
                                    CyclesPerTick--;
                                }
                                break;
                            case ConsoleKey.Escape:
                                Console.Clear();
                                Console.CursorVisible = true;
                                return;
                            case ConsoleKey.Multiply:
                                Thickness++;
                                break;
                            case ConsoleKey.Divide:
                                if (Thickness > 1)
                                {
                                    Thickness--;
                                }
                                break;
                            case ConsoleKey.Add:
                                MsDelay++;
                                break;
                            case ConsoleKey.Subtract:
                                if (MsDelay > 0)
                                {
                                    MsDelay--;
                                }
                                break;
                            case ConsoleKey.O:
                                Clearing = !Clearing;
                                break;
                            case ConsoleKey.C:
                                Console.Clear();
                                Console.Write("Enter the symbol: ");
                                Symbol = Console.ReadLine() ?? "";
                                Console.Clear();
                                break;
                            case ConsoleKey.R:
                                Reset();
                                break;
                            case ConsoleKey.F1:
                                ShowInfo = !ShowInfo;
                                break;
                            case ConsoleKey.M:
                                SwitchColorMode();
                                break;
                            case ConsoleKey.F:
                                FreezeFrame = !FreezeFrame;
                                if (FreezeFrame)
                                {
                                    frozenAngleX = angleX;
                                    frozenAngleY = angleY;
                                    frozenAngleZ = angleZ;
                                }
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    //just catch all
                }
            }
        }

        static float[] RotateVertex(float x, float y, float z, float cosX, float sinX, float cosY, float sinY, float cosZ, float sinZ)
        {
            float newY = y * cosX - z * sinX;
            float newZ = y * sinX + z * cosX;
            float newX = x * cosY + newZ * sinY;
            newZ = -x * sinY + newZ * cosY;
            x = newX * cosZ - newY * sinZ;
            y = newX * sinZ + newY * cosZ;

            return [x, y, newZ];
        }

        public enum ColorType
        {
            RainbowEdges = 0,
            Solid,
            RainbowCycle,
        }

        static ColorType colors = ColorType.RainbowEdges;

        static void SwitchColorMode()
        {
            colors++;
            var lastValue = Enum.GetValues(typeof(ColorType)).Cast<int>().Max();
            if ((int)colors > lastValue)
            {
                colors = 0;
            }
        }

        public static Dictionary<(int, int), string> StringsToPrint = new Dictionary<(int, int), string>();

        static void DrawLineToBuffer(int x1, int y1, int x2, int y2, int width, long outerIt)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            float incX = dx / (float)steps;
            float incY = dy / (float)steps;

            float x = x1, y = y1;
            for (int i = 0; i <= steps; i++)
            {
                var rgb = 0xFFFFFF;
                switch (colors)
                {
                    case ColorType.RainbowEdges:
                        int hue = (int)((i / (float)steps) * 360) % 360;
                        rgb = Utils.HSVToRGB(hue, 1, 1);
                        break;
                    case ColorType.Solid:
                        rgb = 0xFFFFFF;
                        break;
                    case ColorType.RainbowCycle:
                        rgb = Utils.HSVToRGB(outerIt % 360, 1, 1);
                        break;
                }
                var color = Utils.GetColorCode(rgb);
                var xpos = (int)(x * 0.5) + (width / 4) - (Thickness / 2) + 1;
                var ypos = (int)y;
                StringsToPrint[(xpos, ypos)] = Utils.MultiplyString(color + Symbol, Thickness);
                x += incX;
                y += incY;
            }
        }
    }
}
