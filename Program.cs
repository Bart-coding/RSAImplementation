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
            RSAAlgorithm.WriteStringNumber("ENCODED = ", C2, keysConstants.n.ToString().Length);

            string M2 = rsaAlgorithm.DecodeString(C2, keysConstants.d, keysConstants.n);
			Console.WriteLine("DECODED = " + M2);
		}
	}
}
