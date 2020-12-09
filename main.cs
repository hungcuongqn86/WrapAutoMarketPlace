using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrapAutoMarketPlace
{
    public partial class main : Form
    {
        List<HardDrive> hdCollection = new List<HardDrive>();
        public main()
        {
            InitializeComponent();
        }

        public void extractAndRun(string szName)
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
                Close();
                Process x = Process.Start(exeFileName);
                x.WaitForExit();
                File.Delete(exeFileName);
            }
            catch (Exception error) { 
                MessageBox.Show(error.Message.ToString());
            }
        }
        
        public async Task activeAsync(string serial_number)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://crm-v3.efy.vn/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer N2UyNTQ2YjE1NTFj");

                    FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[] {
                            new KeyValuePair<string, string>("serial", serial_number)
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
                            this.label1.Text = model.message;
                        }
                    }
                    else
                    {
                        this.label1.Text = "Kích hoạt thất bại, xin vui lòng liên hệ với nhà cung cấp dịch vụ!";
                        MessageBox.Show("Kích hoạt thất bại, xin vui lòng liên hệ với nhà cung cấp dịch vụ!");
                    }
                    
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message.ToString());
            }
        }

        private void GetAllDiskDrives()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                HardDrive hd = new HardDrive();
                hd.Model = wmi_HD["Model"].ToString();
                hd.InterfaceType = wmi_HD["InterfaceType"].ToString();
                hd.Caption = wmi_HD["Caption"].ToString();
                hd.SerialNo = wmi_HD.GetPropertyValue("SerialNumber").ToString().Trim();
                hdCollection.Add(hd);
            }
        }

        private async void main_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Đang kiểm tra phiên bản ứng dụng ....";
            GetAllDiskDrives();
            HardDrive hd = hdCollection.First();
            await activeAsync(hd.SerialNo);
        }
    }
}
