using System.Net.Http;
using System.Windows.Forms;

namespace FormSec
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;

        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();

        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
              
                //_httpClient.BaseAddress = new Uri("http://localhost:5000/");
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5000/VeriketApplicationTest");
                var logs = await response.Content.ReadAsStringAsync();
             
                dataGridView1.Rows.Clear(); // Önceki verileri temizle

                var logLines = logs.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

              
                foreach (var logLine in logLines)
                {
                    var logParts = logLine.Split(',');
                    dataGridView1.Rows.Add(logParts);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("Column1", "Tarih");
            dataGridView1.Columns.Add("Column2", "Bilgisayar Adı");
            dataGridView1.Columns.Add("Column3", "Windowsta Oturum Açan Kullanıcı Adı");
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 200;
            await _httpClient.PostAsync("http://localhost:5000/VeriketApplicationTest", null);
          
        }
    }
}
