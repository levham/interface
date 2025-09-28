using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace a165
{
    public class SettingsRoot
    {
        public FormSettings settings { get; set; }
    }

    public class FormSettings
    {
        public int? width { get; set; }
        public int? height { get; set; }
        public int[] location { get; set; }
        public int? brightness { get; set; }
        public int? windowclipright { get; set; }
        public int? buttonsize { get; set; }
        public bool? alwaysontop { get; set; }
    }

    public class MenuItem
    {
        public string list { get; set; }
        public string openfolder { get; set; }
        public string openfile { get; set; }
        public string openfilemin { get; set; }
        public string args { get; set; }
    }

    public class MenuSection
    {
        public string title { get; set; }
        public List<MenuItem> items { get; set; }
    }

    public class AppConfig
    {
        public FormSettings settings { get; set; }
        public List<MenuSection> menu { get; set; }
    }

    public class MenuRoot
    {
        public List<MenuSection> menu { get; set; }
    }


    public static class FormMenu
    {
        // FormMenu.cs
        public static int EkleSolMenu(

            Form hedefForm,
            Control closeButton,
            Control miniButton,
            AppConfig veri,
            Control alwaysBtn = null)
        {
            /*
            if (veri?.settings?.width != null)
            {
                hedefForm.Width = veri.settings.width.Value;
            }
            */

            if (veri?.menu == null)
            {
                MessageBox.Show("Menü verisi geçersiz.");
                return 0;
            }

            int btnWidth = veri?.settings?.buttonsize ?? 10;
            Control sonButon = alwaysBtn ?? miniButton;

            Panel menuPanel = new Panel
            {
                Width = btnWidth + 1,
                Left = 0,
                Top = sonButon.Bottom + 5,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            hedefForm.Controls.Add(menuPanel);


            int yOffset = 10;
            int toplamYukseklik = 0;

            foreach (var section in veri.menu)
            {
                string menuBaslik = section.title;
                var itemList = section.items ?? new List<MenuItem>();

                Button btn = new Button
                {
                    Width = btnWidth,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Text = ""
                };
                btn.FlatAppearance.BorderSize = 0;

                using (Graphics g = hedefForm.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(menuBaslik, btn.Font);
                    btn.Height = (int)textSize.Width + 5;
                }

                btn.Location = new Point(0, yOffset);
                yOffset += btn.Height + 5;
                toplamYukseklik += btn.Height + 5;

                btn.Paint += (s, eArgs) =>
                {
                    SizeF textSize = eArgs.Graphics.MeasureString(menuBaslik, btn.Font);
                    float x = (btn.Height - textSize.Width) / 2;
                    float y = (btn.Width - textSize.Height) / 2;

                    eArgs.Graphics.TranslateTransform(0, btn.Height);
                    eArgs.Graphics.RotateTransform(-90);
                    eArgs.Graphics.DrawString(menuBaslik, btn.Font, Brushes.Black, x, y);
                };

                btn.MouseEnter += (s, e) => btn.BackColor = Color.LightBlue;
                btn.MouseLeave += (s, e) => btn.BackColor = Color.White;

                ContextMenuStrip altMenu = new ContextMenuStrip();
                foreach (var alt in itemList)
                {
                    var item = new ToolStripMenuItem(alt.list);
                    item.Click += (s, e) => MenuAction.Calistir(alt);
                    altMenu.Items.Add(item);
                }

                btn.Click += (s, e) =>
                {
                    altMenu.Show(btn, new Point(btn.Width, 0));
                };

                menuPanel.Controls.Add(btn);
            }

            menuPanel.Height = toplamYukseklik + 10;
            return closeButton.Height + 5 + miniButton.Height + 5 + (alwaysBtn != null ? alwaysBtn.Height + 5 : 0) + menuPanel.Height;
        }

    }

}