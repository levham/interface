using System.Drawing;
using System.Windows.Forms;

namespace a165
{
    public class Settings
    {
        public bool? alwaysontop { get; set; }
        public int? buttonsize { get; set; }
        public int? width { get; set; }
    }

    public static class FormButtonAlways
    {
        public static Button EkleAlwaysOnTopButonu(Form hedefForm, Control miniBtn, AppConfig veri)
        {
            bool? durum = veri?.settings?.alwaysontop;

            if (durum == null)
                return null;

            int btnWidth = veri?.settings?.buttonsize ?? 30;

            Button alwaysBtn = new Button
            {
                Text = "++",
                Size = new Size(btnWidth, 18),
                Font = new Font("Segoe UI", 6, FontStyle.Bold),
                Location = new Point(0, miniBtn.Bottom + 5),
                BackColor = durum.Value ? Color.Green : Color.Yellow,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(0),
                Margin = new Padding(0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            alwaysBtn.FlatAppearance.BorderSize = 0;

            hedefForm.TopMost = durum.Value;

            alwaysBtn.Click += (s, e) =>
            {
                hedefForm.TopMost = !hedefForm.TopMost;
                alwaysBtn.BackColor = hedefForm.TopMost ? Color.Green : Color.Yellow;
            };

            hedefForm.Controls.Add(alwaysBtn);
            hedefForm.Controls.SetChildIndex(alwaysBtn, 0);

            return alwaysBtn;
        }

    }
}