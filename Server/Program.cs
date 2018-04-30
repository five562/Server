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





            String fileName = "Mercury Track.txt";                                  //"Mercury Track.txt";    "New Text Document.txt";
            String filePath = @"C:\Users\lwu\Desktop\";
            byte[] str = File.ReadAllBytes(filePath + fileName);
            //string str = "sjudyfaolkl$%^*surhfow ef";

            Crypto encrypt = new Crypto();
            //byte[] b = Encoding.ASCII.GetBytes(str);
            List<Int32> list = encrypt.ConvertByteToInt32(str);
            byte[] list1 = encrypt.ConvertInt32ToByteWithSalt(list);
            List<Int32> list2 = encrypt.ConvertByteToInt32(list1);
            List<Int32> list3 = encrypt.Encrypt1(list2);

            //Encrypt again
            byte[] list4 = encrypt.ConvertBigIntToByte(list3);
            List<Int32> list5 = encrypt.ConvertByteToInt32(list4);
            byte[] list6 = encrypt.ConvertInt32ToByteWithSalt(list5);
            List<Int32> list7 = encrypt.ConvertByteToInt32(list6);
            List<Int32> list8 = encrypt.Encrypt1(list7);

            //Decrypt the second encryption, it will return the decrypted data after the first encryption
            List<Int32> list9 = encrypt.Decrypt1(list8);
            //It will return list6
            byte[] list10 = encrypt.ConvertSmallIntToByte(list9);
            List<Int32> list11 = encrypt.ConvertByteToInt32RemoveSalt(list10);
            byte[] list111 = encrypt.ConvertSmallIntToByte(list11);
            List<Int32> list12 = encrypt.ConvertByteToBigInt(list111);

            //Decrypt the first encryption
            List<Int32> list13 = encrypt.Decrypt1(list12);
            byte[] list14 = encrypt.ConvertSmallIntToByte(list13);
            List<Int32> list15 = encrypt.ConvertByteToInt32RemoveSalt(list14);



            //byte[] byteList = encrypt.ConvertInt32ToByteWithSalt(list1);
            //string str1 = System.Text.Encoding.Default.GetString(byteList);
            //Debug.WriteLine("change line");
            //Debug.Write(BitConverter.ToString(byteList));



        }
    }
}
