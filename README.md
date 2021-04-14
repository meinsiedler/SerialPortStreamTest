# SerialPortStreamTest

Tests behaviour for [SerialPortStream](https://github.com/jcurl/SerialPortStream).

With .NET Core 3.1 the `ReadAsync` call blocks while waiting for reading from the COM port. Even if we `WriteAsync` and `FlushAsync`, nothing gets read from the COM port and the `ReadAsync` blocks. It works for .NET 4.7.2.

The unit test project sets up the two target frameworks .NET 4.7.2 and .NET Core 3.1. All tests will be run for both frameworks.