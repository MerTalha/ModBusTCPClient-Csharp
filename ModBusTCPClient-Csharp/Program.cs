using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModBusTCPClient_Csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Byte[] data ={
                0x00,0x01,   // Operation Identifier
                0x00,0x00,   // Protocol Identifier (Modbus)
                0x00,0x06,   // PDU Length
                0x11,         // Address (Decimal 17)
                0x03,         // Read Register Command
                0x00,0x64,    // Start Reading from Register 0
                0x00,0x01     // Read Only 1 Register (2 bytes)

            };
            Byte[] rData = new byte[11];
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("192.168.1.5", 502);
                if (tcpClient != null)
                {
                    if (tcpClient.Connected)
                    {
                        NetworkStream networkStream = tcpClient.GetStream();
                        networkStream.Write(data, 0, data.Length);
                        while (true)
                        {
                            int bytes = networkStream.Read(rData, 0, rData.Length);
                            if (bytes > 0)
                            {
                                var sb = new StringBuilder(rData.Length * 2);
                                foreach (var b in rData)
                                {
                                    sb.AppendFormat("0x{0:x2}-", b);
                                }
                                Console.Write($"Gelen Data :{sb}");
                                break;
                            }
                        }
                        byte[] gelenDeger = new byte[2] { rData[10], rData[9] };
                        ushort deger = BitConverter.ToUInt16(gelenDeger, 0);
                        Console.WriteLine($"Gelen Değer : {deger}");
                        Console.Read();
                        networkStream.Close();
                        tcpClient.Close();
                    }
                }
            }
            catch (System.Exception e)
            {

                Console.WriteLine("Hata:{0}", e.Message);
            }
        }
    }
}
