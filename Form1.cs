using System.Text.Json;
using System.Diagnostics;
namespace a16
{
    public partial class Form1 : Form
    {
        private MenuStrip menuStrip;
        private FlowLayoutPanel panel;
        public Form1()
        {
            InitializeComponent();
            menuStrip = new MenuStrip(); // Menü nesnesini baþlat
            panel = new FlowLayoutPanel(); // Panel nesnesini baþlat

            CreateMenu();
            CreateButtonPanel();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFormSettings();
        }

        private void CreateMenu()
        {
            menuStrip = new MenuStrip
            {
                Dock = DockStyle.Top // Menü yukarý sabitlendi
            };
            this.Controls.Add(menuStrip);
        }

        private void CreateButtonPanel()
        {
            panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,  // Formun içinde tam yayýlmasýný saðla
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(0, 20, 0, 0) // Menü ile boþluk býrak
            };
            this.Controls.Add(panel);
        }


        private void LoadFormSettings()
        {
            string filePath = "data.json"; // JSON dosya yolu

            // Varsayýlan deðerler
            int defaultWidth = 500;
            int defaultHeight = 120;
            int defaultX = 0;
            int defaultY = 0;

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonData = File.ReadAllText(filePath);
                    using JsonDocument doc = JsonDocument.Parse(jsonData);
                    JsonElement root = doc.RootElement;

                    if (root.TryGetProperty("form", out JsonElement formElement))
                    {
                        int width = formElement.TryGetProperty("width", out JsonElement widthEl) ? widthEl.GetInt32() : defaultWidth;
                        int height = formElement.TryGetProperty("height", out JsonElement heightEl) ? heightEl.GetInt32() : defaultHeight;

                        int x = defaultX, y = defaultY;
                        if (formElement.TryGetProperty("location", out JsonElement locationEl) && locationEl.GetArrayLength() == 2)
                        {
                            x = locationEl[0].GetInt32();
                            y = locationEl[1].GetInt32();
                        }

                        // Form özelliklerini ayarla
                        this.Size = new System.Drawing.Size(width, height);
                        this.Location = new System.Drawing.Point(x, y);

                        // Pencere adýný ayarla
                        if (formElement.TryGetProperty("name", out JsonElement nameElement))
                        {
                            this.Text = nameElement.GetString() ?? "Form";
                        }

                    }

                    if (root.TryGetProperty("menu", out JsonElement menuElement))
                    {
                        foreach (JsonProperty section in menuElement.EnumerateObject())
                        {
                            ToolStripMenuItem menuItem = new ToolStripMenuItem(section.Name);

                            foreach (JsonElement item in section.Value.EnumerateArray())
                            {
                                string openFile = item.TryGetProperty("openfile", out JsonElement openFileElement) ? openFileElement.GetString() ?? string.Empty : string.Empty;
                                string openFolder = item.TryGetProperty("openfolder", out JsonElement openFolderElement) ? openFolderElement.GetString() ?? string.Empty : string.Empty;

                                if (item.TryGetProperty("list", out JsonElement listElement))
                                {
                                    ToolStripMenuItem subItem = new ToolStripMenuItem(listElement.GetString());
                                    subItem.Click += (sender, e) => OpenProgram(openFile, openFolder);
                                    menuItem.DropDownItems.Add(subItem);
                                }
                            }

                            menuStrip.Items.Add(menuItem);
                        }
                    }

                    if (root.TryGetProperty("buttons", out JsonElement buttonsElement))
                    {
                        foreach (JsonElement row in buttonsElement.EnumerateArray()) // Satýrlarý oku
                        {
                            FlowLayoutPanel rowPanel = new FlowLayoutPanel
                            {
                                AutoSize = true,
                                FlowDirection = FlowDirection.LeftToRight
                            };

                            foreach (JsonElement buttonItem in row.EnumerateArray()) // Butonlarý oku
                            {
                                string openFile = buttonItem.TryGetProperty("openfile", out JsonElement openFileElement) ? openFileElement.GetString() ?? string.Empty : string.Empty;
                                string openFolder = buttonItem.TryGetProperty("openfolder", out JsonElement openFolderElement) ? openFolderElement.GetString() ?? string.Empty : string.Empty;

                                if (buttonItem.TryGetProperty("text", out JsonElement textElement))
                                {
                                    Button button = new Button
                                    {
                                        Text = textElement.GetString(),
                                        AutoSize = true
                                    };

                                    button.Click += (sender, e) => OpenProgram(openFile, openFolder);
                                    rowPanel.Controls.Add(button);
                                }
                            }

                            panel.Controls.Add(rowPanel); // Satýrý ekle
                        }
                    }
                }
                catch (Exception ex)
                { MessageBox.Show("JSON verisi okunurken hata oluþtu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            }
            else
            {
                MessageBox.Show("data.json dosyasý bulunamadý!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void OpenProgram(string openfile, string openfolder)
        {
            if (!string.IsNullOrEmpty(openfile) && File.Exists(openfile))
            {
                try
                {
                    string extension = Path.GetExtension(openfile).ToLower();
                    string[] parts = openfile.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string filePath = parts[0];
                    string arguments = parts.Length > 1 ? parts[1] : "";

                    if (extension == ".exe" || extension == ".bat")
                    {
                        ProcessStartInfo psi = new ProcessStartInfo(filePath)
                        {
                            UseShellExecute = true,
                            Arguments = arguments
                        };
                        Process.Start(psi);
                    }
                    else if (extension == ".py")
                    {
                        Process.Start(new ProcessStartInfo("python", $"\"{filePath}\" {arguments}") { UseShellExecute = false });
                    }
                    else if (extension == ".js")
                    {
                        Process.Start(new ProcessStartInfo("node", $"\"{filePath}\" {arguments}") { UseShellExecute = false });
                    }
                    else if (extension == ".html")
                    {
                        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                    }
                    else
                    {
                        Process.Start(new ProcessStartInfo("explorer.exe", $"/select,\"{filePath}\"") { UseShellExecute = true });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya açýlýrken hata oluþtu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!string.IsNullOrEmpty(openfolder) && Directory.Exists(openfolder))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(openfolder) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Klasör açýlýrken hata oluþtu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Belirtilen dosya veya klasör bulunamadý!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
    }
}
