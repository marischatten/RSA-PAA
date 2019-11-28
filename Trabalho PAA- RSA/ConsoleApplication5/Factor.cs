using System;
using System.Numerics;

namespace ConsoleApplication5
{
    public class Factor
    {
        public BigInteger p { get; set; }
        public BigInteger q { get; set; }

        public BigInteger PollardRho(BigInteger number)
        {
            PrimerNumber primer = new PrimerNumber();
            BigInteger i = 1;
            BigInteger k = 2;
            Random random = new Random();

            BigInteger.Subtract(number, 1);


            BigInteger x = Number.GenerateRandomBigInteger(BigInteger.Add(BigInteger.Subtract(number, 2), 2)); //2<x<n-1     //fmodl ????
            BigInteger y = number;
            BigInteger factor, mod;
            do
            {
                i++;
                BigInteger.DivRem(BigInteger.Add(BigInteger.Multiply(x, x), 1), number, out mod);   //fmodl ????
                factor = primer.Gdc(y - mod, number);
                if (factor != 1 && factor != number)
                    Console.WriteLine("");
                if (i == k)
                {
                    y = mod;
                    k = 2 * k;
                }
            } while (factor == 1);
            return factor;
        }
        /*
        static BigInteger[] pollardRho(BigInteger n)
        {
            Random r = new Random();
            // Checa casos triviais
            if (n.equals(BigInteger.ONE)) return new BigInteger[] { n };  // É 1
            if (n.testBit(0) == false) return new BigInteger[] { BigInteger.valueOf(2), n.shiftRight(1) }; // Divisível por 2
                                                                                                           // Gera número aleatorio inicial para x
            BigInteger x = new BigInteger(n.bitLength(), r);
            BigInteger y = x;
            // Gera uma constante aleatória
            BigInteger c = new BigInteger(n.bitLength(), r);
            // Considerando f(x) = x*x + c
            BigInteger d = BigInteger.ONE;
            do
            {
                // Calcula x' = f(x)
                x = x.multiply(x).add(c).mod(n);
                // y' = f(f(x))
                y = y.multiply(y).add(c).mod(n);
                y = y.multiply(y).add(c).mod(n);

                d = x.subtract(y).abs().gcd(n);
                if (d.equals(n)) return pollardRho(n); // Números aleatorios não funcionam, tenta reiniciar
            } while (d.equals(BigInteger.ONE)); // Continua enquanto não encontrou um divisor
                                                // 
            return new BigInteger[] { d, n.divide(d) };
        }
        */

        public BigInteger[] BruteForce(BigInteger number)
        {
            BigInteger p = 3;
            BigInteger q = number;
            BigInteger mod = 0;

            do
            {
                while (BigInteger.Multiply(p, q) < number)
                    p += 2;
                while (BigInteger.Multiply(p, q) > number)
                    q -= 2;
                BigInteger.DivRem(number, p, out mod);
            } while (mod != 0 && p < q);

            if (BigInteger.Multiply(p, q) == number)
                return new BigInteger[] { p, q };
            return new BigInteger[] { 0 };
        }

        public static BigInteger[] pollardRho(BigInteger n)
        {
            PrimerNumber primer = new PrimerNumber();
            Random random = new Random();
            if (n == 1)
                return new BigInteger[] { n };  // É 1
            if (n % 2 == 0)
                return new BigInteger[] { 2, n / 2 }; // Divisível por 2
            // Gera número aleatorio inicial para x
            BigInteger x = random.Next();
            BigInteger y = x;
            // Gera uma constante aleatória
            BigInteger c = random.Next();
            // Considerando f(x) = x*x + c
            BigInteger d = 1;
            do
            {
                x = (x * x + c) % n; // x' = f(x)
                y = (y * y + c) % n; // y' = f(f(x))
                y = (y * y + c) % n;
                d = primer.Gdc(BigInteger.Abs(x - y), n);
                if (d == n)
                    return pollardRho(n); // Testa novos números
            } while (d == 1); // Continua enquanto não encontrou um divisor

            return new BigInteger[] { d, n / d };
        }

        /*public static long Gdc(long e, long phi)
        {
            if (phi == 0)
                return e;
            return Gdc(phi, e%phi);
        }*/


    }
}
/*
 //Implementar Pollard Rho, pg 709 Cormen
long double PollardRho(long double num){ //recebe o randomico gerado
int i = 1, k = 2;
long double x = (fmodl(rand(),(num-2)))+2;
long double y = num; //armazena o valor original na primeira iteracao, nas proximas vai armazenar o calculado
long double fator, res;

printf("Pollard Rho");

do  {
printf("Iteracao %d\n", i);
i++;
res = fmodl((x * x + 1),num);
fator = mdc((y - res), num); //calcula o fator maximo de divisao entre os numeros
if (fator != 1 && fator != num)
    printf("Fator %Lf\n", fator);
if (i == k ){
    y = res;
    k = 2 * k;
}
} while (fator == 1);

printf("Fatoracao com Pollard Rho Completa\n");
return fator;
}
 */







