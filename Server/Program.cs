using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Server
{
    class Program
    {
        static void Main()
        {
            /*
            Connector connector = new Connector();
            Thread connectorThread = new Thread(new ThreadStart(connector.Start));
            connectorThread.Name = "Connector";
            connectorThread.Start();
            */


            Crypto encrypt = new Crypto();


            String fileName = "test.txt";
            String filePath = @"C:\Users\liyi5\Desktop\";

            byte[] str = File.ReadAllBytes(filePath + fileName);
            Int32[] ciphertext1 = encrypt.Encryption1(str);

            byte[] byteList1 = encrypt.ConvertBigIntToByte(ciphertext1);
            Int32[] ciphertext2 = encrypt.Encryption1(byteList1);

            byte[] byteList2 = encrypt.ConvertBigIntToByte(ciphertext2);
            Int32[] ciphertext3 = encrypt.Encryption1(byteList2);

            byte[] byteList3 = encrypt.ConvertBigIntToByte(ciphertext3);
            Int32[] ciphertext4 = encrypt.Encryption1(byteList3);

            Int32[] plaintext4 = encrypt.Decryption1(ciphertext4);
            byte[] byteList33 = encrypt.ConvertSmallIntToByte(plaintext4);
            Int32[] list33 = encrypt.ConvertByteToBigInt(byteList33);

            Int32[] plaintext3 = encrypt.Decryption1(ciphertext3);
            byte[] byteList22 = encrypt.ConvertSmallIntToByte(plaintext3);
            Int32[] list22 = encrypt.ConvertByteToBigInt(byteList22);

            Int32[] plaintext2 = encrypt.Decryption1(list22);
            byte[] byteList11 = encrypt.ConvertSmallIntToByte(plaintext2);
            Int32[] list11 = encrypt.ConvertByteToBigInt(byteList11);

            Int32[] plaintext1 = encrypt.Decryption1(list11);
            foreach (byte i in str)
            {
                Debug.Write(i + "  ");
            }
            Debug.WriteLine("   ");
            foreach (int i in plaintext1)
            {
                Debug.Write(Convert.ToByte(i) + "  ");
            }
        }
    }
}
