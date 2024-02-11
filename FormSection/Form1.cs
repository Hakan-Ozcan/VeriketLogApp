using System.Net.Http;

namespace FormSection
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async Task button1_Click(object sender, EventArgs e)
        {
            try
            {

                // Web servisinden logları al
                var response = await _httpClient.GetAsync("\"http://localhost:5011/Log\"");

                // İstek başarılı mı kontrol et
                if (response.IsSuccessStatusCode)
                {
                    // Logları oku
                    var logs = await response.Content.ReadAsStringAsync();

                    // DataGridView'e logları ekle
                    dataGridView1.Rows.Clear(); // Önceki verileri temizle
                    var logLines = logs.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var logLine in logLines)
                    {
                        var logParts = logLine.Split(',');
                        dataGridView1.Rows.Add(logParts);
                    }
                }
                else
                {
                    MessageBox.Show("Logları alırken bir hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }

        }
    }
}
