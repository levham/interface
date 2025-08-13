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
            menuStrip = new MenuStrip(); // Men� nesnesini ba�lat
            panel = new FlowLayoutPanel(); // Panel nesnesini ba�lat

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
                Dock = DockStyle.Top // Men� yukar� sabitlendi
            };
            this.Controls.Add(menuStrip);
        }

        private void CreateButtonPanel()
        {
            panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,  // Formun i�inde tam yay�lmas�n� sa�la
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(0, 20, 0, 0) // Men� ile bo�luk b�rak
            };
            this.Controls.Add(panel);
        }


        private void LoadFormSettings()
        {
            string filePath = "data.json"; // JSON dosya yolu

            // Varsay�lan de�erler
            int defaultWidth = 500;
            int defaultHeight = 120;
            int defaultX = 0;
            int defaultY = 0;

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonData = File.ReadAllText(filePath); 
                    JsonElement root;
                    using (JsonDocument doc = JsonDocument.Parse(jsonData))
                    {
                        root = doc.RootElement.Clone(); // Dispose sonras� kullan�labilir hale getiriyoruz
                    }
                    int defaultButtonWidth = 100;

                    if (root.TryGetProperty("form", out JsonElement formElement))
                    {
                        
                        if (formElement.TryGetProperty("buttonsize", out JsonElement buttonSizeElement))
                        {
                            string buttonSizeValue = buttonSizeElement.GetString() ?? "default";

                            if (buttonSizeValue.ToLower() == "auto")
                            {
                                defaultButtonWidth = -1; // Auto ayar� i�in �zel bir de�er belirleyelim
                            }
                            else if (int.TryParse(buttonSizeValue, out int parsedWidth))
                            {
                                defaultButtonWidth = parsedWidth;
                            }
                        }

                        int width = formElement.TryGetProperty("width", out JsonElement widthEl) ? widthEl.GetInt32() : defaultWidth;
                        int height = formElement.TryGetProperty("height", out JsonElement heightEl) ? heightEl.GetInt32() : defaultHeight;

                        int x = defaultX, y = defaultY;
                        if (formElement.TryGetProperty("location", out JsonElement locationEl) && locationEl.GetArrayLength() == 2)
                        {
                            x = locationEl[0].GetInt32();
                            y = locationEl[1].GetInt32();
                        }

                        // Form �zelliklerini ayarla
                        this.Size = new System.Drawing.Size(width, height);
                        this.Location = new System.Drawing.Point(x, y);

                        // Pencere ad�n� ayarla
                        if (formElement.TryGetProperty("name", out JsonElement nameElement))
                        {
                            this.Text = nameElement.GetString() ?? "Form";
                        }
                        bool alwaysOnTop = formElement.TryGetProperty("alwaysontop", out JsonElement topEl) && topEl.GetBoolean();
                        this.TopMost = alwaysOnTop;

                        if (formElement.TryGetProperty("brightness", out JsonElement brightnessEl))
                        {
                            int brightnessValue = brightnessEl.GetInt32();
                            // De�er 0-100 aras�nda varsay�l�yor; 0 => tamamen �effaf, 100 => tamamen opak
                            double opacity = Math.Clamp(brightnessValue / 100.0, 0.0, 1.0);
                            this.Opacity = opacity;
                        }

                    }

                    if (root.TryGetProperty("buttons", out JsonElement buttonsElement))
                    {
                         
                        foreach (JsonElement row in buttonsElement.EnumerateArray())
                        {
                            FlowLayoutPanel rowPanel = new FlowLayoutPanel
                            {
                                AutoSize = true,
                                FlowDirection = FlowDirection.LeftToRight
                            };

                            foreach (JsonElement buttonItem in row.EnumerateArray())
                            {
                                string openFile = buttonItem.GetPropertyOrDefault("openfile");
                                string openFolder = buttonItem.GetPropertyOrDefault("openfolder");
                                string args = buttonItem.GetPropertyOrDefault("args");
                                string openFileMin = buttonItem.GetPropertyOrDefault("openfilemin");
                                string buttonText = buttonItem.GetPropertyOrDefault("text");

                                Button button = new Button { Text = buttonText };

                                if (defaultButtonWidth == -1)
                                {
                                    button.AutoSize = true;
                                    button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                                    button.Padding = new Padding(10,0,10,0);
                                    button.Margin = new Padding(0, 0, 7, 0);  
                                    button.MinimumSize = Size.Empty;
                                }
                                else
                                {
                                    button.Width = defaultButtonWidth;
                                }

                                button.Click += (sender, e) =>
                                {
                                    LaunchProcess(openFile, openFileMin, openFolder, args);
                                };

                                rowPanel.Controls.Add(button);
                            }

                            panel.Controls.Add(rowPanel);
                        }
                    }
                    

                    if (root.TryGetProperty("menu", out JsonElement menuElement))
                    {
                        foreach (JsonProperty section in menuElement.EnumerateObject())
                        {
                            // E�er "menu" nesnesinde "control" ad�nda bir dizi varsa
                            if (section.Name == "control" && section.Value.ValueKind == JsonValueKind.Array)
                            {
                                JsonElement controlArray = section.Value;

                                // Dizi en az 3 elemanl� ve ilk eleman "alwaysontop" ise i�le
                                if (controlArray.GetArrayLength() >= 3 && controlArray[0].GetString() == "alwaysontop")
                                {
                                    // JSON'daki renkleri oku (�rnek: "#00ff00", "green" veya "gray")
                                    string greenColorValue = controlArray[1].GetString();
                                    string grayColorValue = controlArray[2].GetString();

                                    // TopMost durumuna g�re ba�lang�� rengini ayarla
                                    Color currentColor = this.TopMost
                                        ? TryParseColor(greenColorValue)
                                        : TryParseColor(grayColorValue);

                                    // K���k renkli kutucuk (ToolStripButton) olu�tur
                                    ToolStripButton topMostIndicator = new ToolStripButton
                                    {
                                        Text = "",
                                        DisplayStyle = ToolStripItemDisplayStyle.None,
                                        BackColor = currentColor,
                                        Size = new Size(15, 10),
                                        AutoSize = false,
                                        Margin = new Padding(1, 1, 1, 1)
                                    };

                                    // T�klan�nca TopMost durumunu de�i�tir ve rengi g�ncelle
                                    topMostIndicator.Click += (sender, e) =>
                                    {
                                        this.TopMost = !this.TopMost;

                                        topMostIndicator.BackColor = this.TopMost
                                            ? TryParseColor(greenColorValue)
                                            : TryParseColor(grayColorValue);
                                    };

                                    // Men�ye kutucu�u ekle (ba�l�k olmadan, sadece renkli kutu)
                                    menuStrip.Items.Add(new ToolStripSeparator());
                                    menuStrip.Items.Add(topMostIndicator);
                                }

                                // "control" zaten i�lendi, atla
                                continue;
                            }

                            // Di�er men� ba�l�klar� ("Belgelerim" gibi)
                            if (section.Value.ValueKind == JsonValueKind.Array)
                            {
                                ToolStripMenuItem menuItem = new ToolStripMenuItem(section.Name);

                                foreach (JsonElement item in section.Value.EnumerateArray())
                                {
                                    if (item.TryGetProperty("list", out JsonElement listElement))
                                    {
                                        ToolStripMenuItem subItem = new ToolStripMenuItem(listElement.GetString());

                                        subItem.Click += (sender, e) =>
                                        {
                                            string openFile = item.GetPropertyOrDefault("openfile");
                                            string openFileMin = item.GetPropertyOrDefault("openfilemin");
                                            string openFolder = item.GetPropertyOrDefault("openfolder");
                                            string args = item.GetPropertyOrDefault("args");

                                            LaunchProcess(openFile, openFileMin, openFolder, args);
                                        };

                                        menuItem.DropDownItems.Add(subItem);
                                    }
                                }

                                menuStrip.Items.Add(menuItem);
                                menuStrip.Padding = new Padding(0); // Varsay�lan� 6, azalt�nca daral�r
                            }
                        }
                    }


                }
                catch (Exception ex)
                { MessageBox.Show("JSON verisi okunurken hata olu�tu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            }
            else
            {
                MessageBox.Show("data.json dosyas� bulunamad�!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Color TryParseColor(string value)
        {
            try
            {
                // E�er hex gibi g�r�n�yorsa (�rne�in: #00ff00)
                if (value.StartsWith("#"))
                {
                    return ColorTranslator.FromHtml(value);
                }
                else
                {
                    return Color.FromName(value);
                }
            }
            catch
            {
                // Hatal�ysa varsay�lan olarak gri ver
                return Color.Gray;
            }
        }



        private void LaunchProcess(string file, string fileMin, string folder, string args)
        {
            if (!string.IsNullOrEmpty(file))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = file,
                    Arguments = args,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                });
            }
            else if (!string.IsNullOrEmpty(fileMin))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = fileMin,
                    Arguments = args,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Minimized
                });
            }
            else if (!string.IsNullOrEmpty(folder))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folder,
                    UseShellExecute = true
                });
            }
        }



        
    }
}
public static class JsonExtensions
{
    public static string GetPropertyOrDefault(this JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out JsonElement value))
        {
            try
            {
                return value.GetString() ?? string.Empty;
            }
            catch (ObjectDisposedException)
            {
                // Hata olursa, bo� de�er d�nd�r
                return string.Empty;
            }
        }

        return string.Empty;
    }
}
