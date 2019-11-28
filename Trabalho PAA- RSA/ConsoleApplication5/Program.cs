using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace ConsoleApplication5
{
    class Program
    {
        static string path = string.Empty;
        static List<byte[]> PartialMessage = new List<byte[]>();
        static List<List<string>> cifra = new List<List<string>>();
        static readonly Cryptography crypt = new Cryptography();
        static string strcifra = string.Empty;
        static List<List<BigInteger>> teste = new List<List<BigInteger>>();
        static string pathLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempoProcessamento.txt");
        static string messageFile = string.Empty;
        static string text = string.Empty;
        static void Main(string[] args)
        {
            //Experiment();
            //Console.WriteLine("Experimento Encerrado");
            Menu();
        }

        private static void Menu()
        {
            Stopwatch stopWatch = new Stopwatch();
            Console.Clear();
            int opcao;
            strcifra = string.Empty;
            Console.WriteLine("CRIPTOGRAFIA RSA");
            if (teste.Count != 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (crypt.PublicKey.Count != 0)
                    Console.WriteLine("Chave Publica: (" + crypt.PublicKey[0] + "," + crypt.PublicKey[1] + ")");
                Console.ResetColor();
                Console.WriteLine("Mensagem criptografada:");
                Console.ForegroundColor = ConsoleColor.Red;
                int i = 0;
                foreach (var partial in teste)
                {
                    foreach (var caracter in partial)
                    {
                        strcifra += "[ " + messageFile[i] + " : " + caracter.ToString() + " ]";
                        i++;
                    }
                }
                Console.WriteLine(strcifra);
                Console.ResetColor();
            }
            Console.WriteLine("[1] - CRIPTOGRAFAR");
            if (teste.Count != 0)
            {
                Console.WriteLine("[2] - DESCRIPTOGRAFAR");
                Console.WriteLine("[3] - FORCA BRUTA");
                Console.WriteLine("[4] - HEURISTICA");
            }
            Console.WriteLine("[0] - SAIR");

            opcao = Convert.ToInt32(Console.ReadLine());
            switch (opcao)
            {
                case 1:
                    Console.WriteLine("Digite a mensagem para ser criptografada:");
                    text = Console.ReadLine();
                    Console.WriteLine("Tamanho da chave gerada em bits:");
                    int numBits = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Criptografando...");
                    stopWatch.Start();
                    Encryption(numBits);
                    Console.ReadLine();
                    stopWatch.Stop();
                    break;
                case 2:
                    Console.WriteLine("Descriptografando...");
                    stopWatch.Start();
                    Decryption();
                    Console.ReadLine();
                    stopWatch.Stop();
                    break;
                case 3:
                    BruteForce(crypt.PublicKey[1]);
                    Console.ReadLine();
                    break;
                case 4:
                    Heuristic(crypt.PublicKey[1]);
                    Console.ReadLine();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opcao inválida");
                    Menu();
                    break;
            }
            Menu();
        }

        public static void Encryption(int numBits)
        {
            path = "mensagem.txt";
            string FilePath = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(FilePath, path);
            try
            {
                if (!string.IsNullOrEmpty(text))
                    File.WriteAllText(path, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escrever no arquivo.\n" + ex.Message);
            }

            try
            {
                messageFile = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao abrir o arquivo.\n" + ex.Message);
            }

            PartialMessage = Message.ToBreak(messageFile);

            foreach (byte[] partial in PartialMessage)
                teste.Add(crypt.Encryption(partial, numBits));

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Chave Publica (" + crypt.e + "," + crypt.n + ")");
            Console.ResetColor();
            //Console.ReadKey();
        }
        public static void Decryption()
        {
            string codeAscii = string.Empty;
            string messageAscii = string.Empty;
            string message = string.Empty;
            foreach (var bloco in teste)
                codeAscii += crypt.Decryption(bloco);

            message = Message.ConvertToCaracter(codeAscii);
            Console.WriteLine(codeAscii);
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine("MENSAGEM:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("************************************************************************************************");
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
                Console.WriteLine("************************************************************************************************");
                Console.ResetColor();
            }
        }
        public static void BruteForce(BigInteger n)
        {
            string FilePath = AppDomain.CurrentDomain.BaseDirectory;
            //string FileNameLog3 = "logFatoracaoForcaBruta.csv";
            string log = string.Empty;
            Stopwatch time = new Stopwatch();

            Factor bruteForce = new Factor();
            Console.WriteLine("Fatorando N");
            time.Start();
            BigInteger[] factor = bruteForce.BruteForce(n);
            time.Stop();

            log = (time.Elapsed.Milliseconds).ToString();
            try
            {
                using (StreamWriter write = new StreamWriter(path, true))
                    write.WriteLine(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("N:" + n);
            Console.WriteLine("P:" + factor[0]);
            Console.WriteLine("Q:" + factor[1]);
        }

        public static void Heuristic(BigInteger n)
        {
            string FilePath = AppDomain.CurrentDomain.BaseDirectory;
            string FileNameLog2 = "logFatoracaoPollardRho.csv";
            string path;
            string log = string.Empty;
            Stopwatch time = new Stopwatch();

            path = Path.Combine(FilePath, FileNameLog2);

            Factor bruteForce = new Factor();
            Console.WriteLine("Fatorando N:" + n);
            BigInteger[] factor;
            time.Start();
            factor = Factor.pollardRho(n);
            time.Stop();
            log = time.Elapsed.Milliseconds.ToString();
            try
            {
                using (StreamWriter write = new StreamWriter(path, true))
                    write.WriteLine(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escrever o log.\n" + ex.Message);
            }
            Console.WriteLine("N:" + n);
            Console.WriteLine("P:" + factor[0]);
            Console.WriteLine("Q:" + factor[1]);
        }

        public static void Experiment()
        {
            string FilePath = AppDomain.CurrentDomain.BaseDirectory;
            //string FileNameLog1 = "logGeracaoChaves.csv";
            //string FileNameLog3 = "logFatoracaoForcaBruta.csv";
            //string FileNameLog2 = "logFatoracaoPollardRho.csv";
            // string path;
            string log = string.Empty;
            Stopwatch time = new Stopwatch();

            /*
            //GERAR CHAVES
            path = Path.Combine(FilePath, FileNameLog1);
            try
            {
                log = "INICIO EXPERIMENTO!!!!!!!";
                using (StreamWriter write = new StreamWriter(path, true))
                    write.WriteLine(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escrever o log.\n" + ex.Message);
            }
            for (int i = 8; i <= 1024; i += 8)
            {
                Console.WriteLine("Criptografando chave de "+i+" bits.");
                log = string.Empty;
                log += i + ";";
                time.Start();
                Encryption(i);
                time.Stop();
                /*log += time.Elapsed.Milliseconds;

                try
                {
                    using (StreamWriter write = new StreamWriter(path, true))
                        write.WriteLine(log);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao escrever o log.\n" + ex.Message);
                }
            }
            Console.WriteLine("FINALIZADO.");
            
            //POLLARD RHO
            path = Path.Combine(FilePath, FileNameLog2);
            try
            {
                log = "HEURISTICA!!!!!!!";
                using (StreamWriter write = new StreamWriter(path, true))
                    write.WriteLine(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escrever o log.\n" + ex.Message);
            }
            for (int i = 16; i <= 256; i += 8)
            {
            	PrimerNumber primer = new PrimerNumber();
            	BigInteger p = primer.PrimeGenerate(i);
            	BigInteger q = primer.PrimeGenerate(i); 
            	BigInteger n = p*q;
                Console.WriteLine("Chave de:" + i + "bits");
                Console.WriteLine("P:"+p);
                Console.WriteLine("Q:" + q);
                Console.WriteLine("N:" + n);
                log = string.Empty;
                log += i + ";";

                Heuristic(n);            
            }
            Console.WriteLine("FINALIZADO.");
            */
            //FORÇA BRUTA
            //GERAR CHAVES
            /*
            path = Path.Combine(FilePath, FileNameLog3);
            try
            {
                log = "FORCA BRUTA!!!!!!!";
                using (StreamWriter write = new StreamWriter(path, true))
                    write.WriteLine(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escrever o log.\n" + ex.Message);
            }

            for (int i = 16; i <= 128; i += 8)
            {
                PrimerNumber primer = new PrimerNumber();
                BigInteger p = primer.PrimeGenerate(i);
                BigInteger q = primer.PrimeGenerate(i);
                BigInteger n = p * q;

                Console.WriteLine("Chave de:" + i + "bits");
                Console.WriteLine("P:" + p);
                Console.WriteLine("Q:" + q);
                Console.WriteLine("N:" + n);

                BruteForce(n);
            }
            Console.WriteLine("FINALIZADO.");
            */
        }
    }
}
