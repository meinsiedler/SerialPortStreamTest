using System.Threading.Tasks;
using NUnit.Framework;
using RJCP.IO.Ports;

namespace SerialPortStreamTest.Tests
{
    public class SerialPortStreamTest
    {
        [Test]
        [Order(1)]
        [Timeout(2000)] // We abort the test after timeout. This tests the blocking behavior in ReadAsync and the test will fail if ReadAsync blocks.
        public async Task WriteAsync()
        {
            using (var serialPortStreamWrite = new SerialPortStream("COM1", 9600, 8, Parity.None, StopBits.One))
            using (var serialPortStreamRead = new SerialPortStream("COM2", 9600, 8, Parity.None, StopBits.One))
            {
                serialPortStreamWrite.Open();
                serialPortStreamRead.Open();

                var buffer = new byte[1024];
                var readTask = Task.Run(async () => await serialPortStreamRead.ReadAsync(buffer, 0, buffer.Length));

                var bytes = new byte[] { 0x01, 0x02, 0x03 };
                await serialPortStreamWrite.WriteAsync(bytes, 0, bytes.Length);
                await serialPortStreamWrite.FlushAsync();

                // ReadAsync blocks here even if something gets written and flushed to the underlying COM device.
                // Fails for netcoreapp3.1, works with net472
                await readTask;

                Assert.That(buffer[0], Is.EqualTo(0x01));
                Assert.That(buffer[1], Is.EqualTo(0x02));
                Assert.That(buffer[2], Is.EqualTo(0x03));
            }
        }
    }
}