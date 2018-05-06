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

        public byte[] Encryption1(byte[] plaintext)
        {
            Int32[] plaintexInt = ConvertByteToInt32(plaintext);
            plaintext = null;
            Int32[] plaintextIntAddSalt = AddSalt(plaintexInt);
            plaintexInt = null;
            Int32[] ciphertextInt = Encrypt1(plaintextIntAddSalt);
            plaintextIntAddSalt = null;
            byte[] ciphertextByte = ConvertBigIntToByte(ciphertextInt);
            ciphertextInt = null;
            return ciphertextByte;
        }

        public byte[] Decryption1(byte[] ciphertextByte)
        {
            Int32[] ciphertextInt = ConvertByteToBigInt(ciphertextByte);
            ciphertextByte = null;
            Int32[] plaintextIntWithSalt = Decrypt1(ciphertextInt);
            ciphertextInt = null;
            Int32[] plaintextIntWithoutSalt = RemoveSalt(plaintextIntWithSalt);
            plaintextIntWithSalt = null;
            byte[] plaintextByteWithoutSalt = ConvertSmallIntToByte(plaintextIntWithoutSalt);
            plaintextIntWithoutSalt = null;
            return plaintextByteWithoutSalt;
        }

        public Int32[] ConvertByteToInt32(byte[] dataByteArray)
        {
            int length = dataByteArray.Length;
            Int32[] intArray = new Int32[length];
            for (int i = 0; i< length; i++)
            {
                intArray[i] = (Int32)(dataByteArray[i]);
            }
            return intArray;
        }

        //Each element of the inserted intList is not larger than 256.
        public byte[] ConvertSmallIntToByte(Int32[] intArray)
        {
            int length = intArray.Length;
            byte[] byteArray = new byte[length];
            for (int i = 0; i < length; i++)
            {
                byteArray[i] = Convert.ToByte(intArray[i]);
            }
            return byteArray;
        }

        public byte[] ConvertBigIntToByte(Int32[] intArray)
        {
            byte[] byteArray = new byte[intArray.Length * 2];
            for (int i = 0; i < intArray.Length; i++)
            {
                byteArray[i * 2] = Convert.ToByte((intArray[i] >> 8) & 0xFF);
                byteArray[i * 2 + 1] = Convert.ToByte(intArray[i] & 0xFF);
            }
            return byteArray;
        }

        public Int32[] ConvertByteToBigInt(byte[] byteArray)
        {
            int length = byteArray.Length / 2;
            Int32[] intArray = new Int32[length];
            for (int i = 0; i < length; i++)
            {
                Int32 x = (Int32)byteArray[i * 2];
                Int32 y = (Int32)byteArray[i * 2 + 1];
                intArray[i] = x * 256 + y;
            }
            return intArray;
        }


        private Int32[] AddSalt(Int32[] numArray)
        {
            Random ran = new Random();
            int length = numArray.Length;
            Int32[] saltedArray = new Int32[length * 4];
            for(int i = 0; i < length; i++)
            {
                saltedArray[i * 4] = ran.Next(64, 127) * 2 + 1;
                saltedArray[i * 4 + 1] = (numArray[i] == 0) ? (ran.Next(64, 127) * 2 + 1) : ((numArray[i] % 2 == 1) ? (ran.Next(64, 127) * 2 + 1) : (ran.Next(65, 127) * 2));
                saltedArray[i * 4 + 2] = (numArray[i] == 0) ? (ran.Next(65, 127) * 2) : ((numArray[i] % 2 == 1) ? (ran.Next(64, 127) * 2 + 1) : (ran.Next(64, 127) * 2 + 1));
                saltedArray[i * 4 + 3] = (numArray[i] == 0) ? (ran.Next(64, 127) * 2 + 1) : ((numArray[i] % 2 == 1) ? (((Int32)saltedArray[i*4+2] + numArray[i]) / 2) : (numArray[i] + 1));
            }
            return saltedArray;
        }


        private Int32[] RemoveSalt(Int32[] saltedArray)
        {
            int length = saltedArray.Length / 4;
            Int32[] numArray = new Int32[length];
            for(int i = 0; i < length; i++)
            {
                Int32 num = (saltedArray[i * 4 + 1] % 2 == 0) ? (saltedArray[i * 4 + 3] - 1) : ((saltedArray[i * 4 + 2] % 2 == 0) ? 0 : (saltedArray[i * 4 + 3] * 2 - saltedArray[i * 4 + 2]));
                numArray[i] = num;
            }
            return numArray;
        }


        private Int32[] Encrypt1(Int32[] intArray)
        {
            int length = intArray.Length;
            Int32[] encryptedArray = new Int32[length];
            for (int i = 0; i < length - 1; i++)
            {
                encryptedArray[i] = intArray[i] * intArray[i + 1];
            }
            //Embedded key element, this element will be useful for decryption later
            encryptedArray[length - 1] = intArray[0] * 255;
            return encryptedArray;
        }


        private Int32[] Decrypt1(Int32[] intArray)
        {
            int length = intArray.Length;
            Int32[] decryptedArray = new Int32[length];
            decryptedArray[0] = intArray[length - 1] / 255;
            for (int i = 0; i < length - 1; i++)
            {
                decryptedArray[i + 1] = intArray[i] / decryptedArray[i];
            }
            return decryptedArray;

        }
    }
}
