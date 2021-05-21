using System;
using System.Numerics;
using System.Text;

namespace RSAImplementation
{
    public class RSAAlgorithm
	{
		public struct KeysConstants
		{
			public int n, e, d;
		}
		public KeysConstants GenerateKeys()
		{
            // ------- TESTS ---------- //
            //int p = 53;
            //int q = 61;
            //int e = 71;
            // ------- TESTS ---------- //
            int[] tmp = DrawPandQ(10, 100);
            int p = tmp[0];
            int q = tmp[1];
            int e = 2;

            int n = p * q;

			int phi = (p - 1) * (q - 1); //Euler function

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

		//for two coprime numbers; based on wiki adaptation of Extended Euclidean Algorithm
		private static int ModInverse(int a, int n)
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
			if (t < 0)
				t += n;

			return t;

		}

		public int EncodeNumber(int M, int e, int n) // BigInteger is for great numbers from Pow
		{
            if (M > n)
            {
				Console.WriteLine("Musisz wybrać większe n, żeby zakodować tą liczbę");
				return -1;
			}
			int C = (int)BigInteger.ModPow(M, e, n);
			return C;
		}
		public int DecodeNumber(int C, int d, int n) // BigInteger is for great numbers from Pow
		{
			int M = (int)BigInteger.ModPow(C, d, n);
			return M;
		}

		public string EncodeString(string M, int e, int n)
		{
			int blockLength = n.ToString().Length;
			string stringNumber = EncodeStringNumberFromLetters(M); // zdekodowany ciąg liczb
			StringBuilder sb = new StringBuilder();
			StringBuilder sb2 = new StringBuilder();
			bool needAdditionalBlock = false;
			// dzielimy ciąg liczb na bloki długości length
			for (int i = 0; i < stringNumber.Length; i += blockLength)
			{
				string mString = "";
				for (int j = 0; j < blockLength; j++) //bierzemy ciąg długości length
				{
					// jezeli wyjdziemy poza zakres tzn, trzeba dorzucic znaki puste, aby blok byl pelny
					if (i + j >= stringNumber.Length)
					{
						if (blockLength % 2 == 0) // jezeli dlugosc bloku jest parzysta
						{
							// doklejamy do konca bloku znaki puste
							while (mString.Length < blockLength)
							{
								mString += "26";
							}
						}
						else // jezeli dlugosc bloku jest nieparzysta
						{
							// jezeli obecna dlugosc bloku jest parzysta to musimy dokleić znaki puste jeszcze w kolejnym bloku
							if (mString.Length % 2 == 0)
							{
								needAdditionalBlock = true;
								mString += "2";
								while (mString.Length < blockLength)
								{
									mString += "62";
								}
							}
							else // jezeli obecna dlugosc bloku jest nieparzysta to doklejamy do konca bloku znaki puste
							{
								while (mString.Length < blockLength)
								{
									mString += "26";
								}
							}
						}
					}
					else
						mString += stringNumber[i + j];
				}
				int m = int.Parse(mString); // wartosc bloku
				sb2.Append(m.ToString().PadLeft(blockLength, '0'));
				if (m > n) // jezeli wartosc bloku wyszla wieksza niz n to oznacza błąd
				{
					sb = null;
					break;
				}
				int num = (int)BigInteger.ModPow(m, e, n); // kodujemy
				sb.Append(num.ToString().PadLeft(blockLength, '0')); // dorzucamy do wyniku doklejajac 0 z przodu jesli trzeba
			}
			if (needAdditionalBlock)
			{
				string mString = "6";
				while (mString.Length < blockLength)
				{
					mString += "26";
				}
				int m = int.Parse(mString); // wartosc bloku
				sb2.Append(m.ToString().PadLeft(blockLength, '0'));
				int num = (int)BigInteger.ModPow(m, e, n); // kodujemy
				sb.Append(num.ToString().PadLeft(blockLength, '0')); // dorzucamy do wyniku doklejajac 0 z przodu jesli trzeba
				if (m > n) // jezeli wartosc bloku wyszla wieksza niz n to oznacza błąd
				{
					sb = null;
				}
			}

            WriteStringNumber("StringNumber= ", sb2.ToString(), blockLength);

            // jezeli zwroci nam null, czyli operacja sie nie powiodla
            if (sb == null)
            {
				Console.WriteLine("Musisz wybrać większe n, żeby zakodować ten wyraz");
				return null;
			}
			return sb.ToString();
		}
		public string DecodeString(string C, int d, int n)
		{
			int blockLength = n.ToString().Length; // znajdujemy dlugość bloku
			StringBuilder sb = new StringBuilder();
			// dzielimy ciąg liczb na bloki długości blockLength
			for (int i = 0; i< C.Length; i += blockLength)
            {
				string c = "";
				for (int j = 0; j < blockLength; j++) //bierzemy ciąg długości blockLength
				{
					c += C[i + j];
                }
				int num = (int)BigInteger.ModPow(int.Parse(c), d, n); // dekodujemy
				sb.Append(num.ToString().PadLeft(blockLength, '0')); // dorzucamy do wyniku doklejajac 0 z przodu jesli trzeba
			}
			WriteStringNumber("StringNumber= ", sb.ToString(), blockLength);	
			return DecodeLettersFromStringNumber(sb.ToString());
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

		public static void WriteStringNumber(string message,string word, int blockLength)
        {
			Console.Write(message);
			for (int i = 0; i < word.Length; i++)
			{
				if (i % blockLength == blockLength-1)
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
