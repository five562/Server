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
            byte[] ciphertext1 = encrypt.Encryption1(str);
            byte[] ciphertext2 = encrypt.Encryption1(ciphertext1);
            byte[] ciphertext3 = encrypt.Encryption1(ciphertext2);
            byte[] ciphertext4 = encrypt.Encryption1(ciphertext3);

            byte[] plaintext4 = encrypt.Decryption1(ciphertext4);
            byte[] plaintext3 = encrypt.Decryption1(plaintext4);
            byte[] plaintext2 = encrypt.Decryption1(plaintext3);
            byte[] plaintext1 = encrypt.Decryption1(plaintext2);
            foreach (byte i in str)
            {
                Debug.Write(i + "  ");
            }
            Debug.WriteLine("   ");
            foreach (byte i in plaintext1)
            {
                Debug.Write(Convert.ToByte(i) + "  ");
            }
        }
    }
}
