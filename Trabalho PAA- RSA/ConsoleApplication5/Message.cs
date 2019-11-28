using System;
using System.Collections.Generic;
using System.Text;


namespace ConsoleApplication5
{
    public class Message
    {
        private const int SIZE_MAX = 8; // Tamanho máximo dos blocos.
        public static List<byte[]> ToBreak(string message)
        {
            List<string> messageBroken = new List<string>();
            int initIndex = 0;

            //Divide quantidade de caracteres da mensagem pelo tamanho do bloco. (8)
            int mod = message.Length % SIZE_MAX;

            //Quando não existe divisão inteira por 8. 8 é o tamanho máximo do bloco
            if (mod != 0 || mod == message.Length)
            {
                messageBroken.Add(message.Substring(initIndex, mod));
                initIndex = mod;
                //Quebra mensagem em blocos de 8 caracteres.
                if(message.Length != mod)
                    Break(message, ref initIndex, ref messageBroken);
            }
            //Quando existe divisão inteira por 8.
            else
            {
                //Quebra mensagem em blocos de 8 caracteres.
                Break(message, ref initIndex, ref messageBroken);
            }
            //Transformando os caracteres de cada bloco em código ASCII. 
            return ConvertToAscii(messageBroken);
        }

        //Quebra mensagem em blocos de 8 caracteres.
        private static void Break(string message, ref int initIndex, ref List<string> messageBroken)
        {
            while (message.Length > initIndex)
            {
                messageBroken.Add(message.Substring(initIndex, 8));
                initIndex += SIZE_MAX;
            }
        }

        //Converte UTF8 em ASCII
        private static List<byte[]> ConvertToAscii(List<string> messageBroken)
        {
            List<byte[]> asciiBytes = new List<byte[]>();
            foreach (string partial in messageBroken)
                asciiBytes.Add(Encoding.ASCII.GetBytes(partial));
            return asciiBytes;
        }

        //Coverte código ASCII em UTF8.
        public static string ConvertToCaracter(string strPartialMensageAscii)
        {
            byte[] partialMensageAscii = ConvertToByteArray(strPartialMensageAscii);
            char[] messageArray = null;
            messageArray = ASCIIEncoding.ASCII.GetChars(partialMensageAscii);
            string strMessage = new string(messageArray);
            return strMessage;
        }

        //Converte a mensagem criptograda em um array de bytes.
        public static byte[] ConvertToByteArray(string partialMensageAscii)
        {
            int index = 0;
            int initIndex = 0;
            int finInit = (initIndex + 3);
            byte[] arrayAscii = new byte[8];

            try
            {
                while (partialMensageAscii.Length > initIndex)
                {
                    arrayAscii[index] = byte.Parse(partialMensageAscii.Substring(initIndex, 3));
                    initIndex += 3;
                    index++;
                }
            }
            catch
            {
                Console.WriteLine("Erro ao converter Ascii para UTF8.");
            }
            return arrayAscii;
        }
    }


}
