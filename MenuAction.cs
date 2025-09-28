using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace a165
{
    public static class MenuAction
    {
        public static void Calistir(MenuItem item)
        {
            try
            {
                if (!string.IsNullOrEmpty(item.openfolder))
                {
                    Process.Start("explorer.exe", item.openfolder);
                }
                else if (!string.IsNullOrEmpty(item.openfile))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = item.openfile,
                        Arguments = item.args ?? "",
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else if (!string.IsNullOrEmpty(item.openfilemin))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = item.openfilemin,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show("Geçerli bir işlem tanımı yok.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem hatası: " + ex.Message);
            }
        }
    }
}