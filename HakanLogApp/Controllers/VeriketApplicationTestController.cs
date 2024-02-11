using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HakanLogApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VeriketApplicationTestController : ControllerBase
    {
        private readonly string _logFilePath;
        public VeriketApplicationTestController()
        {
            var AppName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ApplicationName"];
            var FileName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["FileName"];
            string _logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppName);


            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }

            _logFilePath = Path.Combine(_logFolderPath, FileName);

            if (!System.IO.File.Exists(_logFilePath))
            {
                System.IO.File.Create(_logFilePath).Close();
            }

        }

        [HttpPost(Name = "LogGenerate")]
        public IActionResult GenerateLog()
        {
            try
            {
                while (true)
                {
                    string logLine = $"{DateTime.Now},{Environment.MachineName},{Environment.UserName}";

                    System.IO.File.AppendAllText(_logFilePath, logLine + Environment.NewLine);

                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet(Name = "GetLog")]
        public IActionResult GetLogFile()
        {
            var FileName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("VeriketAppSettings")["FileName"];
            try
            {
                if (!System.IO.File.Exists(_logFilePath))
                {
                    return NotFound();
                }

                byte[] fileContents = System.IO.File.ReadAllBytes(_logFilePath);

                return File(fileContents, "application/octet-stream", FileName);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
