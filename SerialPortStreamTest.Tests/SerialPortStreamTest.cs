using System.Threading.Tasks;
using NUnit.Framework;
using RJCP.IO.Ports;

namespace SerialPortStreamTest.Tests
{
    public class SerialPortStreamTest
    {
        [Test]
        [Ignore("When we don't ignore this test (which runs first), the second test works even with .NET Core 3.1 too.")]
        [Order(1)]
        public async Task WriteAsync()
        {
            using (var serialPortStream = new SerialPortStream("COM1", 9600, 8, Parity.None, StopBits.One))
            {
                serialPortStream.Open();

                var bytes = new byte[] { 0x01, 0x02, 0x03 };

                await serialPortStream.WriteAsync(bytes, 0, bytes.Length);
                await serialPortStream.FlushAsync();
            }
        }

        [Test]
        [Order(2)]
        [Timeout(2000)] // We abort the test after timeout so that we can "test" the waiting time in WriteAsync.
        public async Task ReadAndWriteInDifferntThreadsAtSameTime()
        {
            using (var serialPortStream = new SerialPortStream("COM1", 9600, 8, Parity.None, StopBits.One))
            {
                serialPortStream.Open();

                var buffer = new byte[1024];
                var readTask = Task.Run(async () => await serialPortStream.ReadAsync(buffer, 0, buffer.Length));

                await Task.Run(async () =>
                {
                    var bytes = new byte[] { 0x01, 0x02, 0x03 };

                    await serialPortStream.WriteAsync(bytes, 0, bytes.Length); // Call blocks here with .NET Core 3.1.
                    await serialPortStream.FlushAsync();
                });
            }
        }
    }
}