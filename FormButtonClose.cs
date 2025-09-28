using System.Drawing;
using System.Windows.Forms;

namespace a165
{
    public static class FormButtonClose
    {// FormButtonClose.cs
        public static Button EkleKapatButonu(Form hedefForm, AppConfig veri)
        {
            // JSON'dan buton genişliği oku, yoksa 30 kullan
            int btnWidth = veri?.settings?.buttonsize ?? 30;

            Button closeBtn = new Button
            {
                Text = "X",
                Size = new Size(btnWidth, 18),
                Font = new Font("Segoe UI", 6, FontStyle.Bold),
                Location = new Point(0, 0),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(0),
                Margin = new Padding(0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Kenarlığı kaldır
            closeBtn.FlatAppearance.BorderSize = 0;

            // Kapatma olayı
            closeBtn.Click += (s, e) => hedefForm.Close();

            // Forma ekle ve en öne al
            hedefForm.Controls.Add(closeBtn);
            hedefForm.Controls.SetChildIndex(closeBtn, 0);

            return closeBtn;
        }
    }
}