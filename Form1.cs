using System;
using System.Windows.Forms;

namespace a165
{
    public partial class Form1 : Form
    {
        private ControllerMouse mouseController;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.AutoSize = false;

            var veri = FormSetting.Uygula(this); // ← tüm ayarlar burada

            // Butonları ekle
            var closeBtn = FormButtonClose.EkleKapatButonu(this, veri);
            var miniBtn = FormButtonMini.EkleMinimizeButonu(this, closeBtn, veri);
            var alwaysBtn = FormButtonAlways.EkleAlwaysOnTopButonu(this, miniBtn, veri);

            int toplamYukseklik = FormMenu.EkleSolMenu(this, closeBtn, miniBtn, veri, alwaysBtn);
            this.Height = toplamYukseklik + 30;

            mouseController = new ControllerMouse();
            mouseController.SolTiklaIleTasi(this);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
