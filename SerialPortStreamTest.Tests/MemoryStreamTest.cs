using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SerialPortStreamTest.Tests
{
    /// <summary>
    /// Just a baseline test to make sure writing and waiting for read works with a memory stream.
    /// </summary>
    public class MemoryStreamTest
    {
        [Test]
        [Timeout(2000)] // We abort the test after timeout so that we can "test" the waiting time in WriteAsync.
        public async Task ReadAndWriteInDifferntThreadsAtSameTime()
        {
            using (var memoryStream = new MemoryStream())
            {
                var buffer = new byte[1024];
                var readTask = Task.Run(async () => await memoryStream.ReadAsync(buffer, 0, buffer.Length));

                await Task.Run(async () =>
                {
                    var bytes = new byte[] { 0x01, 0x02, 0x03 };

                    await memoryStream.WriteAsync(bytes, 0, bytes.Length);
                    await memoryStream.FlushAsync();
                });
            }
        }
    }
}
