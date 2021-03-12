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

            using (var serialPortStream = new SerialPortStream("COM1", 9600, 8, Parity.None, StopBits.One))
            {
                serialPortStream.Open();

                Console.WriteLine("Serial Port Opened");

                var buffer = new byte[1024];
                var readTask = Task.Run(async () => await serialPortStream.ReadAsync(buffer, 0, buffer.Length));

                Console.WriteLine("Wait for write. Enter any key.");
                Console.ReadKey();
                

                await Task.Run(async () =>
                {
                    var bytes = new byte[] { 0x01, 0x02, 0x03 };

                    Console.WriteLine("Write started");
                    await serialPortStream.WriteAsync(bytes, 0, bytes.Length);
                    await serialPortStream.FlushAsync();
                    Console.WriteLine("Write finished");
                });

                Console.WriteLine("Awaiting read...");

                await readTask;
                

                Console.WriteLine("Finished.");
            }
        }
    }
}
