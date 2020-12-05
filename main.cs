using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrapAutoMarketPlace
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            extractAndRun("AutoMarketPlace.exe");
        }

        public static void extractAndRun(string szName)
        {
            try
            {
                string exeFileName = Path.Combine(Directory.GetCurrentDirectory(), szName);
                File.Delete(exeFileName);

                using (FileStream fsDst = new FileStream(exeFileName,
                                           FileMode.CreateNew, FileAccess.Write))
                {
                    byte[] bytes = WrapAutoMarketPlace.Properties.Resources.GetAutoMarketPlace_2_0_0_9();
                    fsDst.Write(bytes, 0, bytes.Length);
                }

                Process x = Process.Start(exeFileName);
                x.WaitForExit();
                File.Delete(exeFileName);
            }
            catch (Exception error) { 
                MessageBox.Show(error.Message.ToString());
            }
        }
    }
}
