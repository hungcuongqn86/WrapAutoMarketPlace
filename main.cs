using Newtonsoft.Json;
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
                    client.BaseAddress = new Uri("https://crm-v3.efy.vn/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("email", email),
                            new KeyValuePair<string, string>("password", pass)
                        });
                    HttpResponseMessage response = client.PostAsync("api/check-active", formContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Active
                        string resultArray = await response.Content.ReadAsStringAsync();
                        ResBody model = JsonConvert.DeserializeObject<ResBody>(resultArray);
                        if (model.status == 200)
                        {
                            extractAndRun("xxxyyyz.exe");
                        }
                        else
                        {
                            MessageBox.Show(model.message);
                        }
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
