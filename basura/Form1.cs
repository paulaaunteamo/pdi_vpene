using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace basura
{
    public partial class Form1 : Form
    {
        Thread HiloDescargaHilo; // hue
        String Hilo;

        void CambiarTexto()
        {
            label2.Text = Hilo;
        }

        void RehabilitarBoton()
        {
            button2.Enabled = true;
        }

        void Descarga()
        {
	    // Aquí es donde mino mis bitcoins.
            Hilo = "Hilo iniciado...";
            label2.Invoke(new MethodInvoker(CambiarTexto));
            try
            {
                Regex rx = new Regex("(http://www\\.choroychan\\.org/.+?/src/(\\d+\\.(jpg|png|gif)))");
                Regex r2 = new Regex("\\d+\\.(jpg|png|gif)");
                System.Net.WebClient Dl = new System.Net.WebClient();
                int Cuenta = 1;

                Hilo = "Descargando hilo.";
                label2.Invoke(new MethodInvoker(CambiarTexto));
                String HTML = Dl.DownloadString(textBox1.Text);

                MatchCollection Matches = rx.Matches(HTML);

                foreach (Match m in Matches)
                {
                    String URL = m.Groups[0].Value;
                    String Filename = r2.Match(URL).Groups[0].Value;

                    Hilo = "Descargando" + String.Format(" {0} de {1}: ", Cuenta, Matches.Count) + Filename;
                    label2.Invoke(new MethodInvoker(CambiarTexto));
                    if (!System.IO.File.Exists(Filename)) // don't overwrite mang
                        Dl.DownloadFile(URL, Filename);
                    Cuenta++;
                }
            }
            catch (System.Exception E)
            {
                MessageBox.Show(E.Message);
            }

            Hilo = "Finalizado.";
            label2.Invoke(new MethodInvoker(CambiarTexto));

            // terminamos, woot.
            button2.Invoke(new MethodInvoker(RehabilitarBoton));
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            HiloDescargaHilo = new Thread(new ThreadStart(Descarga));
            HiloDescargaHilo.Start();
        }
    }
}
