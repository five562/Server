using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Server
{
    class Crypto
    {
        /*
         * Steps:
         * 1. Convert plaintext from byte[] to Int32<>
         * 2. Add salt
         * 3. Convert plaintext with salt from Int32<> to byte[]
         * 4. Encrypt data
         * 5. Decrypt data
         * 6. Convert decrypted plaintext with salt from Int32<> to byte[]
         * 7. Remove salt to obtian the original plaintext
         */

        static Random ran = new Random();

        public List<Int32> ConvertByteToInt32(byte[] dataByteArray)
        {
            List<Int32> intList = new List<int>();
            foreach (byte dataByte in dataByteArray)
            {
                intList.Add(Convert.ToInt32(dataByte));
            }
            return intList;
        }

        //Each element of the inserted intList is not larger than 256.
        public byte[] ConvertSmallIntToByte(List<Int32> intList)
        {
            int length = intList.Count;
            byte[] byteArray = new byte[length];
            for (int i = 0; i < length; i++)
            {
                byteArray[i] = Convert.ToByte(intList[i] & 0xFF);
            }
            return byteArray;
        }

        public byte[] ConvertBigIntToByte(List<Int32> intList)
        {
            byte[] byteArray = new byte[intList.Count * 2];
            for (int i = 0; i < intList.Count; i++)
            {
                byteArray[i * 2] = Convert.ToByte((intList[i] >> 8) & 0xFF);
                byteArray[i * 2 + 1] = Convert.ToByte(intList[i] & 0xFF);
            }
            return byteArray;
        }

        public List<Int32> ConvertByteToBigInt(byte[] byteArray)
        {
            List<Int32> intList = new List<Int32>();
            int length = byteArray.Length / 2;
            for (int i = 0; i < length; i++)
            {
                Int32 x = Convert.ToInt32(byteArray[i * 2]);
                Int32 y = Convert.ToInt32(byteArray[i * 2 + 1]);
                intList.Add(x * 256 + y);
            }
            return intList;
        }

        public byte[] ConvertInt32ToByteWithSalt(List<Int32> numList)
        {
            int listLength = numList.Count;
            byte[] byteArray = new byte[listLength * 4];
            for (int i = 0; i < listLength; i++)
            {
                Int32 num = numList[i];
                //Salt, ensure no element will be odd numbers between 128 and 256
                byteArray[i * 4] = Convert.ToByte((ran.Next(64, 128) * 2 + 1) & 0xFF);
                byteArray[i * 4 + 1] = Convert.ToByte((ran.Next(64, 128) * 2 + 1) & 0xFF);
                byteArray[i * 4 + 2] = Convert.ToByte((ran.Next(64, 128) * 2 + 1) & 0xFF);
                //Useful data
                byteArray[i * 4 + 3] = Convert.ToByte(num & 0xFF);
            }
            return byteArray;
        }

        public List<Int32> ConvertByteToInt32RemoveSalt(byte[] byteArray)
        {
            int length = byteArray.Length;
            List<Int32> intArray = new List<int>();
            for (int i = 0; i < length / 4; i++)
            {
                intArray.Add(Convert.ToInt32(byteArray[4 * i + 3]));
            }
            return intArray;
        }

        public List<Int32> Encrypt1(List<Int32> intList)
        {
            List<Int32> encryptedList = new List<Int32>();
            int listLength = intList.Count;
            for (int i = 0; i < listLength - 1; i++)
            {
                encryptedList.Add(intList[i] * intList[i + 1]);
            }
            //Embedded key element, this element will be useful for decryption later
            encryptedList.Add(intList[0] * 255);
            return encryptedList;
        }

        public List<Int32> Decrypt1(List<Int32> intList)
        {
            List<Int32> decryptedList = new List<Int32>();
            int listLength = intList.Count;
            decryptedList.Add(intList[listLength - 1] / 255);
            for (int i = 0; i < listLength - 1; i++)
            {
                decryptedList.Add(intList[i] / decryptedList[i]);
            }
            return decryptedList;
        }
    }
}
