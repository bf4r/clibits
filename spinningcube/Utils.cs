using System.Text;

public static class Utils
{
    public static string MultiplyString(string str, int times)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < times; i++)
        {
            sb.Append(str);
        }
        return sb.ToString();
    }
    public static int HSVToRGB(double hue, double saturation, double value)
    {
        double c = value * saturation;
        double x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
        double m = value - c;

        double r,
            g,
            b;
        if (hue < 60)
        {
            r = c;
            g = x;
            b = 0;
        }
        else if (hue < 120)
        {
            r = x;
            g = c;
            b = 0;
        }
        else if (hue < 180)
        {
            r = 0;
            g = c;
            b = x;
        }
        else if (hue < 240)
        {
            r = 0;
            g = x;
            b = c;
        }
        else if (hue < 300)
        {
            r = x;
            g = 0;
            b = c;
        }
        else
        {
            r = c;
            g = 0;
            b = x;
        }

        int red = (int)Math.Round((r + m) * 255);
        int green = (int)Math.Round((g + m) * 255);
        int blue = (int)Math.Round((b + m) * 255);

        return (red << 16) | (green << 8) | blue;
    }
    public static string GetColorCode(int rgb)
    {
        int red = (rgb >> 16) & 255;
        int green = (rgb >> 8) & 255;
        int blue = rgb & 255;
        return $"\x1b[38;2;{red};{green};{blue}m";
    }
}
