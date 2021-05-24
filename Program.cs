using System;
using System.Numerics;

namespace RSAImplementation
{
    class Program
    {
		public static void Main(string[] args)
		{
			RSAAlgorithm rsaAlgorithm = new RSAAlgorithm();
			RSAAlgorithm.KeysConstants keysConstants = rsaAlgorithm.GenerateKeys();

			Console.WriteLine("  e = " + keysConstants.e);
			Console.WriteLine("  n = " + keysConstants.n);
			Console.WriteLine("  d = " + keysConstants.d);

			// ------- TESTS NUMBER ---------- //
			Console.WriteLine("----------------------------- RESULTS NUMBER -----------------------------");
			int numberToCode = 100;
			Console.WriteLine("  INPUT = " + numberToCode);
			BigInteger C = rsaAlgorithm.EncodeNumber(numberToCode, keysConstants.e, keysConstants.n);
			if(C == -1)
            {
				return;
            }
			Console.WriteLine("ENCODED = " + C);

			BigInteger M = rsaAlgorithm.DecodeNumber(C, keysConstants.d, keysConstants.n);
			Console.WriteLine("DECODED = " + M);

			// ------- TESTS STRINGS ---------- //
			Console.WriteLine("----------------------------- RESULTS STRING -----------------------------");
			string stringToCode = "RENAISSANCE";
			Console.WriteLine("  INPUT = " + stringToCode);
			string C2 = rsaAlgorithm.EncodeString(stringToCode, keysConstants.e, keysConstants.n);
            WriteStringNumber("ENCODED = ", C2, keysConstants.n.ToString().Length);

            string M2 = rsaAlgorithm.DecodeString(C2, keysConstants.d, keysConstants.n);
			Console.WriteLine("DECODED = " + M2);
		}

		public static void WriteStringNumber(string message, string word, int blockLength)
		{
			Console.Write(message);
			for (int i = 0; i < word.Length; i++)
			{
				if (i % blockLength == blockLength - 1)
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
