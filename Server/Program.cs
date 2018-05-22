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
            
            Connector connector = new Connector();
            Thread connectorThread = new Thread(new ThreadStart(connector.Start));
            connectorThread.Name = "Connector";
            connectorThread.Start();

            XmlParser p = new XmlParser();
            p.CreateFolderTreeXml();
            

            /*
            Crypto encrypt = new Crypto();
            String fileName = "test.txt";
            String filePath = @"C:\Users\liyi5\Desktop\";


            byte[] str = File.ReadAllBytes(filePath + fileName);
            foreach (byte x in str)
            {
                byte[] b = new byte[] { x };
                byte[] ciphertext1 = encrypt.Encryption1(b);
                byte[] ciphertext2 = encrypt.Encryption1(ciphertext1);
                ciphertext1 = null;
                byte[] ciphertext3 = encrypt.Encryption1(ciphertext2);
                ciphertext2 = null;
                byte[] ciphertext4 = encrypt.Encryption1(ciphertext3);
                //byte[] ciphertext5 = encrypt.Encryption1(ciphertext4);
                ciphertext3 = null;

                //byte[] plaintext5 = encrypt.Decryption1(ciphertext5);
                //ciphertext4 = null;
                
                byte[] plaintext4 = encrypt.Decryption1(ciphertext4);
                ciphertext4 = null;
                byte[] plaintext3 = encrypt.Decryption1(plaintext4);
                plaintext4 = null;
                byte[] plaintext2 = encrypt.Decryption1(plaintext3);
                plaintext3 = null;
                byte[] plaintext1 = encrypt.Decryption1(plaintext2);
                plaintext2 = null;
            }
            */


        }
    }
}
