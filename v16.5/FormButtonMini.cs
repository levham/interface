using System.Drawing;
using System.Windows.Forms;

namespace a165
{
    public static class FormButtonMini
    {
        public static Button EkleMinimizeButonu(Form hedefForm, Control closeBtn, AppConfig veri)
        {
            // JSON'dan buton genişliği oku, yoksa 30 kullan
            int btnWidth = veri?.settings?.buttonsize ?? 30; 

            Button miniBtn = new Button
            {
                Text = "M",
                Size = new Size(btnWidth, 18),
                Font = new Font("Segoe UI", 6, FontStyle.Bold),
                Location = new Point(0, closeBtn.Bottom + 5),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(0),
                Margin = new Padding(0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            miniBtn.FlatAppearance.BorderSize = 0;

            // Minimize olayı
            miniBtn.Click += (s, e) => hedefForm.WindowState = FormWindowState.Minimized;

            // Forma ekle ve en öne al
            hedefForm.Controls.Add(miniBtn);
            hedefForm.Controls.SetChildIndex(miniBtn, 0);

            return miniBtn;
        }
    }
}
