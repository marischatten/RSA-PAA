using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace ConsoleApplication5
{
    public class Cryptography
    {
        #region Propiedades
        private const int SIZE_MAX = 8;
        private BigInteger p = 0;
        private BigInteger q = 0;
        public BigInteger n = 0;
        public BigInteger e = 0;
        private BigInteger d = 0;
        public string M { get; set; }
        public BigInteger C { get; set; }
        private BigInteger phi { get; set; }
        public List<BigInteger> PublicKey = new List<BigInteger>();
        private List<BigInteger> PrivateKey = new List<BigInteger>();
        #endregion

        private void GenerateP(int numBits)
        {
            Console.WriteLine("Gerando P.");
            PrimerNumber primer = new PrimerNumber();
            Stopwatch time = new Stopwatch();
            p = primer.PrimeGenerate(numBits);
            Console.WriteLine("Gerado P.");
        }

        private void GenerateQ(int numBits)
        {
            Console.WriteLine("Gerando Q.");
            PrimerNumber primer = new PrimerNumber();
            q = primer.PrimeGenerate(numBits);
            Console.WriteLine("Gerado Q.");
        }

        private void GenerateN()
        {
            Console.WriteLine("Gerando N.");
            this.n = (BigInteger.Multiply(this.p, this.q));
            Console.WriteLine("Gerado N.");
        }

        private void PrivateKeyGenerate()
        {
            this.GenerateD();
            //this.PrivateKey.Add(this.d);
            //this.PrivateKey.Add(this.n);
            Console.WriteLine("Chave privada gerada.");
        }

        private void PublicKeyGenerate(int numBits)
        {
            this.GenerateE(numBits);
            // this.PublicKey.Add(this.e);
            //this.PublicKey.Add(this.n);
            Console.WriteLine("Chave publica gerada.");
        }

        public List<BigInteger> Encryption(byte[] message, int numBits)
        {

            string FilePath = AppDomain.CurrentDomain.BaseDirectory;
            string FileNameLog1 = "logGeracaoChaves.csv";
            string path = Path.Combine(FilePath, FileNameLog1);
            string log = string.Empty;

            PublicKey.Clear();
            List<BigInteger> cifra = new List<BigInteger>();
            List<string> crypto = new List<string>();

            if (p == 0 && q == 0 && n == 0)
            {
                this.GenerateP(numBits);
                this.GenerateQ(numBits);
                this.GenerateN();
                this.PublicKeyGenerate(numBits);
                this.PrivateKeyGenerate();
            }
            try
            {
                using (StreamWriter write = new StreamWriter(path, true))
                    write.WriteLine(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escrever o log.\n" + ex.Message);
            }
            Console.WriteLine("Gerando cifra.");
            foreach (var caracter in message)
                cifra.Add(BigInteger.ModPow(caracter, e, n));

            Console.WriteLine("Criptografia finalizada.");
            return cifra;
        }

        public string Decryption(List<BigInteger> partialMessageCrypto)
        {
            //PublicKey.Clear();
            BigInteger c;
            string messageAscii = "";

            //descriptografando por partes
            foreach (var caracterCrypto in partialMessageCrypto)
            {
                c = caracterCrypto;
                messageAscii += (BigInteger.ModPow(c,d, n).ToString("000"));
            }
            Console.WriteLine("Mensagem descriptografada.");
            return messageAscii;
        }

        private void GenerateE(int numBits)
        {
            Console.WriteLine("Gerando E.");
            PrimerNumber primer = new PrimerNumber();
            BigInteger _e = 0;
            if ((BigInteger.Compare(primer.GenerateRelativePrime(ref _e, this.PhiN(this.p, this.q), numBits), 1) == 0) && (BigInteger.Compare(_e, 1) == 1))
            {
                e = (_e);
                Console.WriteLine("Gerado E.");
            }else
                Console.WriteLine("Não gerado E.");
        }

        private void GenerateD()
        {
            Console.WriteLine("Gerando D.");
            PrimerNumber primer = new PrimerNumber();
            d = primer.InversoModular(e, this.PhiN(this.p, this.q));
            Console.WriteLine("Gerado D.");
        }

        //Φ(n) Quociente de Euler.
        private BigInteger PhiN(BigInteger p, BigInteger q)
        {
            phi = BigInteger.Multiply(BigInteger.Subtract(p, 1), BigInteger.Subtract(q, 1));
            return phi;
        }
    }
}