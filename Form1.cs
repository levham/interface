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
            var formData = FormController.LoadFormSettings();

            if (formData != null && formData.form != null)
            {
                this.Size = new Size(formData.form.width, formData.form.height);
                this.Location = new Point(formData.form.location[0], formData.form.location[1]);
            }
            else
            {
                MessageBox.Show("Form ayarlar� y�klenemedi! JSON i�eri�ini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadButtons();
        }



        private void LoadButtons()
        {
            var buttonsData = controller.GetButtons();
            var eventMappings = controller.GetEventMappings();

            int yPos = 20;
            using (Graphics g = CreateGraphics())
            {
                foreach (var row in buttonsData)
                {
                    int xPos = 20; // Butonlar�n yatay ba�lang�� noktas�

                    foreach (var btnData in row)
                    {
                        int buttonWidth = (int)g.MeasureString(btnData.text, this.Font).Width + 20;

                        Button button = new Button
                        {
                            Text = btnData.text,
                            Size = new Size(buttonWidth, 30),
                            Location = new Point(xPos, yPos),
                            Tag = btnData.id
                        };
                        button.Click += Button_Click;
                        this.Controls.Add(button);

                        xPos += buttonWidth + 10; // Yatay olarak ilerle
                    }

                    yPos += 40; // Yeni sat�r i�in a�a�� kayd�r
                }
            }

            this.eventMappings = eventMappings;
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is int buttonId)
            {
                if (eventMappings.TryGetValue(buttonId, out string? command) && !string.IsNullOrWhiteSpace(command))
                {
                    if (File.Exists(command)) // Dosya ger�ekten varsa �al��t�r
                    {
                        try
                        {
                            Process.Start(command);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hata: {ex.Message}\nGe�erli bir dosya veya komut girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Dosya bulunamad�: {command}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Dosya ad� sa�lanmad�! JSON i�inde ge�erli bir dosya veya komut olup olmad���n� kontrol edin.");
                }
            }
        }



    }
}
