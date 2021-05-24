using System;
using System.Numerics;
using System.Text;

namespace RSAImplementation
{
    public class RSAAlgorithm
	{
		public struct KeysConstants
		{
			public BigInteger n, e, d;
		}
		public KeysConstants GenerateKeys()
		{
            //int p = 53;
            //int q = 61;
            // ------- TESTS ---------- //
            //			  max value = 2147483647
            int[] tmp = DrawPandQ(10, 10000);
            int p = tmp[0];
            int q = tmp[1];
            BigInteger e = 100;

            BigInteger n = BigInteger.Multiply(p,q);

			BigInteger phi = BigInteger.Multiply(p-1, q-1); //Euler function

			while (e != phi)
			{
				if (GCDEuclidean(e, phi) == 1)
					break;
				e++;
			}

			BigInteger d = ModInverse(e, phi);

			Console.WriteLine("  p = " + p);
			Console.WriteLine("  q = " + q);
			Console.WriteLine("phi = " + phi);

			return new KeysConstants
			{
				n = n,
				e = e,
				d = d
			};
		}

		private static BigInteger GCDEuclidean(BigInteger a, BigInteger b)
		{
			BigInteger temp;
			while (b != 0)
			{
				temp = b;
				b = a % b;
				a = temp;
			}

			return a;
		}

		//for two coprime numbers; based on wiki adaptation of Extended Euclidean Algorithm
		private static BigInteger ModInverse(BigInteger a, BigInteger n)
		{
			BigInteger t = 0;
			BigInteger newt = 1;
			BigInteger r = n;
			BigInteger newr = a;
			BigInteger quotient, tmp;

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

		public BigInteger EncodeNumber(BigInteger M, BigInteger e, BigInteger n)
		{
            if (M > n) // jezeli wprowadzona liczba jest wieksza niz n to znaczy blad
            {
				Console.WriteLine("Musisz wybrać większe n, żeby zakodować tą liczbę");
				return -1;
			}
			BigInteger C = BigInteger.ModPow(M, e, n);
			return C;
		}
		public BigInteger DecodeNumber(BigInteger C, BigInteger d, BigInteger n)
		{
			BigInteger M = BigInteger.ModPow(C, d, n);
			return M;
		}

		public string EncodeString(string M, BigInteger e, BigInteger n)
        {
			int blockLength = n.ToString().Length; // dlugość pojedynczego bloku
			StringBuilder sb = new StringBuilder();
			// zdekodowane znaki w kodzie ASCII
			string stringNumber = EncodeCharsToASCII(M);
			// jakbysmy chcieli zobaczyc kody ASCII znakow w slowie
			//Program.WriteStringNumber("stringNumber= ", stringNumber, 3);
			// przechodzimy co 3, bo tyle maksymalnie moze zajmowac jeden znak
			for (int i = 0; i < stringNumber.Length; i += 3)
            {
				// łączymy 3 kolejne cyfry obok siebie w jedną liczbę
				string mString = stringNumber[i].ToString() + stringNumber[i + 1] + stringNumber[i + 2];
				int m = int.Parse(mString); // wartość obecnego bloku w ASCII
				BigInteger num = BigInteger.ModPow(m, e, n); // kodujemy
				// dorzucamy zakodowany znak do wyniku doklejajac 0 z przodu jesli trzeba
				sb.Append(num.ToString().PadLeft(blockLength, '0'));
			}
			return sb.ToString();
		}
		public string DecodeString(string C, BigInteger d, BigInteger n)
		{
			int blockLength = n.ToString().Length; // dlugość pojedynczego bloku
			StringBuilder sb = new StringBuilder();
			// przechodzimy co długość pojedynczego bloku
			for (int i = 0; i < C.Length; i += blockLength)
            {
				string c = "";
				// laczymy kolejne cyfry w jedna liczbe
				for (int j = 0; j < blockLength; j++)
                {
					c += C[i + j];
				}
				// dekodujemy na ASCII
				BigInteger num = BigInteger.ModPow(BigInteger.Parse(c), d, n); 
				// dorzucamy zdekodowany znak do wyniku doklejajac 0 z przodu jesli trzeba
				sb.Append(num.ToString().PadLeft(3, '0'));
			}
			return DecodeASCIIToChars(sb.ToString());
		}

		private string EncodeCharsToASCII(string word)
        {
			StringBuilder sb = new StringBuilder();
			// przechodzimy po każdej literze ze slowa
			for(int i = 0; i < word.Length; i++)
            {
				int decodedInt = word[i]; // kod ASCII danej litery
				// dorzucamy zdekodowany kod ASCII do wyniku doklejajac 0 z przodu jesli trzeba
				sb.Append(decodedInt.ToString().PadLeft(3, '0'));
            }
			return sb.ToString();
        }
		private string DecodeASCIIToChars(string word)
        {
			StringBuilder sb = new StringBuilder();
			// przechodzimy co 3, bo tyle maksymalnie moze zajmowac jeden znak
			for (int i = 0; i < word.Length; i += 3)
            {
				// łączymy 3 kolejne cyfry obok siebie w jedną liczbę
				string codedString = word[i].ToString() + word[i + 1] + word[i + 2];
				// znak zdekodowany z kodu ASCII
				char decodedChar = (char)int.Parse(codedString);
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
                    || BigInteger.Subtract(BigInteger.Max(p, q),BigInteger.Min(p, q)) > 
					   BigInteger.Max(10, BigInteger.Divide(BigInteger.Max(p, q),10))
                    || p == q
                    || GCDEuclidean(p - 1, q - 1) > BigInteger.Divide(BigInteger.Min(p, q),2)
                    || FirstFactor(p - 1) < BigInteger.Min(BigInteger.Divide(BigInteger.Min(p, q),2), 7)
                    || FirstFactor(q - 1) < BigInteger.Min(BigInteger.Divide(BigInteger.Min(p, q), 2), 7));
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
		private static BigInteger FirstFactor(BigInteger number)
        {
			BigInteger k = 2;
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
	}
}
