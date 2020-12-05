using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Phải nhập tên đăng nhập!");
                return;
            }
            if (this.textBox2.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Phải nhập mật khẩu!");
                return;
            }

            await activeAsync(this.textBox1.Text, this.textBox2.Text);
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
        
        public static async Task activeAsync(string email, string pass)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    User user = new User() { email = email, password = pass };
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/login", user);

                    if (response.IsSuccessStatusCode)
                    {
                        // Active
                        // var resultArray = await response.Content.ReadAsStringAsync();
                        extractAndRun("AutoMarketPlace.exe");
                    }
                    else
                    {
                        MessageBox.Show("Kích hoạt thất bại, xin vui lòng kiểm tra lại thông tin tài khoản và thực hiện lại!");
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message.ToString());
            }
        }
    }
}
