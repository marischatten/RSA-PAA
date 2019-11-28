using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleApplication5
{
    public static class Number
    {
        //Gera número aleatório de 64 bits.
        public static BigInteger GenerateRandomBigInteger(int numBits)
        {
            //BigInteger randomA, randomB;
            Random random = new Random();
            byte[] arrayBits = new byte[(numBits / 8) / 2];  //multiplicando dois randons dobra o numero de bits.
            random.NextBytes(arrayBits);

            List<Int32> lstByte = new List<Int32>();

            BigInteger rand = new BigInteger(arrayBits);
            return BigInteger.Abs(rand);
        }

        //Verifica se o número é par.
        public static bool IsEven(BigInteger number)
        {
            BigInteger mod;
            BigInteger.DivRem(number, 2, out mod);
            if (BigInteger.Compare(mod, 0) == 0)
                return true;
            return false;
        }

        public static BigInteger GenerateRandomBigInteger(BigInteger N)
        {
            Random rand = new Random();
            BigInteger result = 0;
            do
            {
                int length = (int)Math.Ceiling(BigInteger.Log(N, 2));
                int numBytes = (int)Math.Ceiling(length / 8.0);
                byte[] data = new byte[numBytes];
                rand.NextBytes(data);
                result = new BigInteger(data);
            } while (result >= N || result <= 0);
            return result;
        }

        public static BigInteger Sqrt(BigInteger number)
        {
            return (BigInteger)Math.Exp(BigInteger.Log(number) / 2);
        }
    }
}
