using System.Globalization;
using System.Windows.Media;

namespace WpfColorView
{
    public class ColorInfo
    {
        private readonly Color Color;

        public ColorInfo(string colorName, Color color)
        {
            ColorName = colorName;
            Color = color;
        }

        public float Brigthness => System.Drawing.Color.FromName(ColorName).GetBrightness();
        public string ColorName { get; set; }
        public string HexString => Color.ToString(CultureInfo.CurrentCulture);
        public long HexValue => long.Parse(HexString.Replace("#", ""), NumberStyles.HexNumber);
        public float HSL => System.Drawing.Color.FromName(ColorName).GetHue();
        public Brush SampleBrush => new SolidColorBrush(Color);
        public float Saturation => System.Drawing.Color.FromName(ColorName).GetSaturation();
    }
}