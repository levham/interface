using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace a165
{
    public static class FormSetting
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static AppConfig Uygula(Form hedefForm)
        {
            string jsonPath = "data.json";
            if (!File.Exists(jsonPath))
                return null;

            string json = File.ReadAllText(jsonPath);
            var veri = JsonSerializer.Deserialize<AppConfig>(json, new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            });

            var ayar = veri?.settings;
            if (ayar == null)
                return veri;

            // Boyut
            if (ayar.width.HasValue && ayar.height.HasValue)
            {
                var boyut = new Size(ayar.width.Value, ayar.height.Value);
                hedefForm.Size = boyut;
                hedefForm.MinimumSize = boyut;
            }

            // Konum
            hedefForm.StartPosition = FormStartPosition.Manual;
            if (ayar.location?.Length == 2)
                hedefForm.Location = new Point(ayar.location[0], ayar.location[1]);

            // Parlaklık
            if (ayar.brightness.HasValue)
            {
                int b = Clamp(ayar.brightness.Value, 0, 100);
                hedefForm.Opacity = b / 100.0;
            }

            // Sağdan kırpma
            if (ayar.windowclipright.HasValue)
            {
                int clip = ayar.windowclipright.Value;
                hedefForm.Bounds = new Rectangle(hedefForm.Left, hedefForm.Top, hedefForm.Width - clip, hedefForm.Height);
            }

            // Always on top
            if (ayar.alwaysontop.HasValue)
                hedefForm.TopMost = ayar.alwaysontop.Value;

            return veri;
        }
    }

}
