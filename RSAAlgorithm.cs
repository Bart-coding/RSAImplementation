using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSAImplementation
{
	public class RSAAlgorithm
	{
		int phi2;
		public struct KeysConstants
		{
			public int n, e, d;
		}

		private static int GCDEuclidean(int a, int b)
		{
			int temp;
			while (b != 0)
			{
				temp = b;
				b = a % b;
				a = temp;
			}

			return a;
		}

		private static int ModInverse(int a, int n) //for two coprime numbers; based on wiki adaptation of Extended Euclidean Algorithm
		{
			int t = 0;
			int newt = 1;
			int r = n;
			int newr = a;
			int quotient, tmp;

			while (newr != 0)
			{
				quotient = r / newr;

				tmp = newt;
				newt = t - quotient * newt;
				t = tmp;

				tmp = newr;
				newr = r - quotient * newr;
				r = tmp;
			}

			/*if (r>1) //a is not invertible; but a and n are coprime numbers so they should be 
				return -1;*/

			if (t < 0)
				t = t + n;

			return t;

		}

		private static List<string> GetLetterPairs(string message)
		{
			List<string> substrings = new List<string>();

			for (int i = 0; i < message.Length - 1; i = i + 2)
			{
				substrings.Add(message.Substring(i, 2));
			}
			if (message.Length % 2 != 0)
				substrings.Add(message[message.Length - 1] + " ");

			return substrings;
		}

		private static List<string> EncodeLetterPairs(List<string> substrings) //2-char strings
		{
			List<string> numbersCodes = new List<string>();

			string temp;
			int a, b;
			foreach (string s in substrings)
			{
				a = s[0] - 'A';
				temp = (a / 10).ToString() + (a % 10).ToString();

				if (s[1].Equals(' '))
					temp += 26;
				else
				{
					b = s[1] - 'A';
					temp += (b / 10).ToString() + (b % 10).ToString();
				}

				numbersCodes.Add(temp);
			}

			return numbersCodes;
		}
		private static string EncodeStringNumberFromLetters(string word)
        {
			StringBuilder sb = new StringBuilder();
			// iteracja po kazdej literze z wyrazu
			for (int i = 0; i < word.Length; i++)
            {
				char currentChar = word[i];
				int number = currentChar - 65; // kod obecnej litery
				// doklejamy zera z przodu, zeby np 8 zamienilo sie na 08
				sb.Append(number.ToString().PadLeft(2, '0'));
			}
			return sb.ToString();
        }
		private static string DecodeLettersFromStringNumber(string numbers)
        {
			StringBuilder sb = new StringBuilder();
			// bierzemy dwucyfrowe liczby
			for (int i = 0; i < numbers.Length; i += 2)
            {
				// łączymy obecny int z następnym w jedną liczbę
				int concatedNumber = int.Parse(numbers[i].ToString() + numbers[i + 1]);
				// znak tej liczby
				char decodedChar = (char)(concatedNumber + 65);
				// jesli 26('[') to znak pusty (spacja, bo nie puszcza pustego char '')
				if (decodedChar == '[')
					decodedChar = ' ';
				sb.Append(decodedChar);
			}
			return sb.ToString();
        }

		public KeysConstants GenerateKeys()
		{
            // ------- TESTS ---------- //
            //int p = 53;
            //int q = 61;
            //int e = 71;
            // ------- TESTS ---------- //
            int[] tmp = DrawPandQ(11, 100);
            int p = tmp[0];
            int q = tmp[1];
            int e = 2;

            int n = p * q;

			int phi = (p - 1) * (q - 1); //Euler function
			phi2 = phi;

            while (e != phi)
            {
                if (GCDEuclidean(e, phi) == 1)
                    break;
                e++;
            }

            int d = ModInverse(e, phi);

			Console.WriteLine("p= " + p);
			Console.WriteLine("q= " + q);
			Console.WriteLine("phi= " + phi);

			return new KeysConstants
			{
				n = n,
				e = e,
				d = d
			};
		}

		public int Encode(int M, int e, int n) // BigInteger is for great numbers from Pow
		{
			int C = (int)BigInteger.ModPow(M, e, n);
			return C;
		}

		public int Decode(int C, int d, int n) // BigInteger is for great numbers from Pow
		{
			int M = (int)BigInteger.ModPow(C, d, n);
			return M;
		}

		public string Encode2(string M, int e, int n)
		{
			StringBuilder sb = new StringBuilder();
			List<string> letterPairs = GetLetterPairs(M);
			List<string> encodedLetterPairs = EncodeLetterPairs(letterPairs);

			foreach (string s in encodedLetterPairs)
			{
				int num = (int)BigInteger.ModPow(Int32.Parse(s), e, n);
				sb.Append(num.ToString().PadLeft(4,'0') + " ");
			}

			return sb.ToString();
		}
		public string Encode(string M, int e, int n)
		{
			int nLength = n.ToString().Length;
			StringBuilder sb = new StringBuilder();
			string stringNumber = EncodeStringNumberFromLetters(M); // zdekodowany ciąg liczb
			WriteStringNumber("StringNumber= ", stringNumber, nLength);
			// dzielimy ciąg liczb na bloki długości nLength
			for (int i = 0; i < stringNumber.Length; i += nLength)
            {
				string mString = "";
				for (int j = 0; j < nLength; j++) //bierzemy ciąg długości nLength
                {
					if (i + j >= stringNumber.Length)
						break;
					mString += stringNumber[i + j];
                }
				int m = int.Parse(mString);

				if (m > n)
                {
					m = ModInverse(m,n);
                }
				int num = (int)BigInteger.ModPow(m, e, n); // kodujemy
				sb.Append(num.ToString().PadLeft(nLength, '0')); // dorzucamy do wyniku doklejajac 0 z przodu jesli trzeba
			}
			return sb.ToString();
		}
		public string Decode(string C, int d, int n)
		{
			int nLength = n.ToString().Length;
			StringBuilder sb = new StringBuilder();
			// dzielimy ciąg liczb na bloki długości nLength
			for (int i = 0; i< C.Length; i += nLength)
            {
				string c = "";
				for(int j =0; j< nLength; j++) //bierzemy ciąg długości nLength
				{
					c += C[i + j];
                }
				int num = (int)BigInteger.ModPow(int.Parse(c), d, n); // dekodujemy
				sb.Append(num.ToString().PadLeft(nLength, '0')); // dorzucamy do wyniku doklejajac 0 z przodu jesli trzeba
			}
			WriteStringNumber("StringNumber= ", sb.ToString(), nLength);	
			return DecodeLettersFromStringNumber(sb.ToString());
		}

		private static int[] DrawPandQ(int min, int max)
        {
			int p, q;
			Random random = new Random();
			do
			{
				p = random.Next(min, max);
				q = random.Next(min, max);
			}
			while (!IsPrime(p) || !IsPrime(q)
					|| Math.Max(p, q) - Math.Min(p, q) > Math.Max(10, Math.Max(p, q) * 0.2)
					|| p == q
					|| GCDEuclidean(p - 1, q - 1) > Math.Min(p, q) * 0.5
					|| FirstFactor(p - 1) < Math.Min(Math.Min(p, q) * 0.5, 7)
					|| FirstFactor(q - 1) < Math.Min(Math.Min(p, q) * 0.5, 7));
			return new int[2] { p, q };
        }

		private static bool IsPrime(int number)
        {
			int m = number / 2;
			for (int i = 2; i <= m; i++)
			{
				// jezeli jest podzielna przez ktoras z liczb oprocz siebie i 1 to nie jest pierwsza
				if (number % i == 0)
				{
					return false;
				}
			}
			return true;
		}

		private static int FirstFactor(int number)
        {
			int k = 2;
			while (number > 1)
			{
				while (number % k == 0) //dopóki liczba jest podzielna przez k
				{
					number /= k; // dzielimy liczbe przez obecny dzielnik
				}
				k++; // jezeli nie da sie podzielic to probujemy z wieksza liczba
			}
			return k - 1;
		}

		public static void WriteStringNumber(string message,string word, int nLength)
        {
			Console.Write(message);
			for (int i = 0; i < word.Length; i++)
			{
				if (i % nLength == nLength-1)
				{
					Console.Write(word[i] + " ");
				}
				else
					Console.Write(word[i]);
			}
			Console.Write("\n");
		}
	}
}
