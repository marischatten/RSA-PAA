using System;
using System.Numerics;

namespace ConsoleApplication5
{
    public class PrimerNumber
    {
        public Random random = new Random();

        public BigInteger PrimeGenerate(int numBits)
        {
            BigInteger e = 0;
            e = Number.GenerateRandomBigInteger(numBits);///Number.GenerateRandomBigInteger();//8 -255 bits //16-65535 //32-4294967295 //64-18446744073709551615

            if (Number.IsEven(e))
                e++;

            while (!IsPrime(e, 2048, numBits)) // executa o teste somente uma vez.
                e = BigInteger.Add(e, 2);

            return e;
        }

        //TESTE DE FERMAT
        //Recebe inteiro ímpar e o número de tentativas por segurança
        public bool IsPrime(BigInteger e, BigInteger iterations, int numBits)
        {
            BigInteger a;
            if (BigInteger.Compare(e, 1) == 0)
                return false;
            for (int i = 0; i < iterations; i++)
            {
                a = Number.GenerateRandomBigInteger(numBits) + 2;///Number.GenerateRandomBigInteger(BigInteger.Add(BigInteger.Subtract(e,2),2));// 2<a<n-2


                if (!BigInteger.ModPow(a, BigInteger.Subtract(e, 1), e).Equals(1))
                    return false;
            }
            return true;
        }
        //Encontra Primos relativos.
        public BigInteger GenerateRelativePrime(ref BigInteger _e, BigInteger phi, int numBits)
        {
            _e = PrimeGenerate(numBits);
            return Gdc(_e, phi);
        }

        //Algoritmo de Euclides.
        //Máximo Divisor Comum.
        public BigInteger Gdc(BigInteger e, BigInteger phi)
        {
            BigInteger mod;
            if (BigInteger.Compare(phi, 0) == 0)
                return e;
            BigInteger.DivRem(e, phi, out mod);
            return Gdc(phi, mod);
        }

        //Retorna o inverso modular de e.
        public BigInteger InversoModular(BigInteger a, BigInteger b)//e e phi
        {
            return EuclidianExtend(a, b, 1);
        }

        //Algoritimo de Euclides Extendido.      

        BigInteger mod(BigInteger a, BigInteger b)
        {
            BigInteger mod;
            BigInteger.DivRem(a, b, out mod);
            BigInteger r = mod;

            /* Uma correção é necessária se r e b não forem do mesmo sinal */

            /* se r for negativo e b positivo, precisa corrigir */
            if ((BigInteger.Compare(r, 0) == -1) && (BigInteger.Compare(b, 0) == 1))
                return (BigInteger.Add(b, r));

            /* Se r for positivo e b negativo, nova correção */
            if ((BigInteger.Compare(r, 0) == 1) && (BigInteger.Compare(b, 0) == -1))
                return BigInteger.Add(b, r);

            return (r);
        }

        BigInteger EuclidianExtend(BigInteger a, BigInteger b, BigInteger c)
        {
            BigInteger r;
            r = mod(b, a);
            if (r == 0)
                return (mod((c / a), (b / a))); // retorna (c/a) % (b/a)

            return (EuclidianExtend(r, a, -c) * b + c) / (mod(a, b));
        }

    }
}
