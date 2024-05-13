using Microsoft.AspNetCore.Mvc;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using Domain;
using AnswerCube.BL;
using AnswerCube.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnswerCube.UI.MVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RaspBerryController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<DataAnalyseController> _logger;

    // Define the UART port name (COM port on Windows, /dev/tty* on Linux)
    private const string PortName = "COM14"; // Change this to match your system
    
    // Define the baud rate
    private const int BaudRate = 9600;
    
    // Buffer to store received data
    private static StringBuilder buffer = new StringBuilder();

    public RaspBerryController(IManager manager, ILogger<DataAnalyseController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetJsonData()
    {
        try
        {
            // Create a new SerialPort object
            using (SerialPort serialPort = new SerialPort(PortName, BaudRate))
            {
                // Open the serial port
                serialPort.Open();
    
                // Read data asynchronously in chunks
                byte[] readBuffer = new byte[1024];
                int bytesRead;
    
                do
                {
                    bytesRead = await serialPort.BaseStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                    string receivedData = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
    
                    // Append the received data to the buffer
                    buffer.Append(receivedData);
    
                    // Process the buffer for complete lines
                    string[] lines = buffer.ToString().Split('\n');
    
                    // Process each complete line
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            // Process the line, for example, you can parse it as JSON
                            ProcessReceivedData(line);
                        }
                    }
    
                    // Remove processed lines from the buffer
                    int lastNewLineIndex = buffer.ToString().LastIndexOf('\n');
                    if (lastNewLineIndex >= 0)
                    {
                        buffer.Remove(0, lastNewLineIndex + 1);
                    }
                } while (bytesRead > 0);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return StatusCode(500, $"Error reading from serial port: {ex.Message}");
        }
    
        return Ok();
    }
    private void ProcessReceivedData(string line)
    {
        // Process the received line, for example, parse it as JSON
        Console.WriteLine("Received JSON data: " + line);
        // Add your JSON parsing logic here
    }
    
    public IActionResult Pico()
    {
        return View();
    }
}