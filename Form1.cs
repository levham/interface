using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using a16.Controllers;
using a16.Models;

namespace a16
{
    public partial class Form1 : Form
    {
        private Dictionary<int, string> eventMappings = new Dictionary<int, string>();
        private FormController controller = new FormController();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var formData = controller.LoadFormSettings();
            if (formData != null)
            {
                this.Size = new Size(formData.form.width, formData.form.height);
                this.Location = new Point(formData.form.location[0], formData.form.location[1]);
            }

            LoadButtons();
        }

        private void LoadButtons()
        {
            var buttons = controller.GetButtons();
            var eventMappings = controller.GetEventMappings();

            int yPos = 20;
            using (Graphics g = CreateGraphics())
            {
                foreach (var btnData in buttons)
                {
                    int buttonWidth = (int)g.MeasureString(btnData.text, this.Font).Width + 20;

                    Button button = new Button
                    {
                        Text = btnData.text,
                        Size = new Size(buttonWidth, 30),
                        Location = new Point(20, yPos),
                        Tag = btnData.id
                    };
                    button.Click += Button_Click;
                    this.Controls.Add(button);
                    yPos += 40;
                }
            }

            this.eventMappings = eventMappings; // Olay eþleþmelerini güncelle
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is int buttonId)
            {
                if (eventMappings.TryGetValue(buttonId, out string? command) && !string.IsNullOrWhiteSpace(command))
                {
                    if (File.Exists(command)) // Dosya gerçekten varsa çalýþtýr
                    {
                        try
                        {
                            Process.Start(command);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hata: {ex.Message}\nGeçerli bir dosya veya komut girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Dosya bulunamadý: {command}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Dosya adý saðlanmadý! JSON içinde geçerli bir dosya veya komut olup olmadýðýný kontrol edin.");
                }
            }
        }



    }
}
