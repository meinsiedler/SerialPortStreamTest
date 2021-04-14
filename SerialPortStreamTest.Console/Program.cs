using System;
using System.Threading.Tasks;
using RJCP.IO.Ports;

namespace SerialPortStreamTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Start");

            using (var serialPortStreamWrite = new SerialPortStream("COM1", 9600, 8, Parity.None, StopBits.One))
            using (var serialPortStreamRead = new SerialPortStream("COM2", 9600, 8, Parity.None, StopBits.One))
            {
                serialPortStreamWrite.Open();
                serialPortStreamRead.Open();

                Console.WriteLine("Serial Port Opened");

                var buffer = new byte[1024];
                var readTask = Task.Run(async () => await serialPortStreamRead.ReadAsync(buffer, 0, buffer.Length));

                Console.WriteLine("Wait for write. Enter any key.");
                Console.ReadKey();
                

                var bytes = new byte[] { 0x01, 0x02, 0x03 };

                Console.WriteLine("Write started");
                await serialPortStreamWrite.WriteAsync(bytes, 0, bytes.Length);
                await serialPortStreamWrite.FlushAsync();
                Console.WriteLine("Write finished");


                Console.WriteLine("Awaiting read...");

                await readTask;

                Console.WriteLine($"Buffer: {buffer[0]}, {buffer[1]}, {buffer[2]}");

                Console.WriteLine("Finished.");
            }
        }
    }
}
