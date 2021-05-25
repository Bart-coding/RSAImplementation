using System;

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

			// ------- TESTS ---------- //
			Console.WriteLine("----------------------------- RESULTS -----------------------------");
			string stringToCode = "012345678901234567890";
			Console.WriteLine("  INPUT = " + stringToCode);
			string C2 = rsaAlgorithm.Encode(stringToCode, keysConstants.e, keysConstants.n);
            WriteStringNumber("ENCODED = ", C2, keysConstants.n.ToString().Length);

            string M2 = rsaAlgorithm.Decode(C2, keysConstants.d, keysConstants.n);
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
