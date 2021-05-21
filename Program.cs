using System;
using System.Collections.Generic;

namespace RSAImplementation
{
    class Program
    {
		public static void Main(string[] args)
		{
			RSAAlgorithm rsaAlgorithm = new RSAAlgorithm();
			RSAAlgorithm.KeysConstants keysConstants = rsaAlgorithm.GenerateKeys();

			Console.WriteLine("e= " + keysConstants.e);
			Console.WriteLine("n= " + keysConstants.n);
			Console.WriteLine("d= " + keysConstants.d);

			// ------- TESTS ---------- //
			Console.WriteLine("---------- RESULTS ----------");
			int intToCode = 5555;
			int C = rsaAlgorithm.Encode(intToCode, keysConstants.e, keysConstants.n);
			Console.WriteLine("C= " + C);

			int M = rsaAlgorithm.Decode(C, keysConstants.d, keysConstants.n);
			Console.WriteLine("M= " + M);

			// ------- TESTS ---------- //
			string stringToCode = "ABCDEFGHIJKLMNOPQRSTWXYZ";
			string C2 = rsaAlgorithm.Encode(stringToCode, keysConstants.e, keysConstants.n); //to get results like on lecture, p should be 53, q 61 and e 791
			//Console.WriteLine("C2= " + C2);
			RSAAlgorithm.WriteStringNumber("C2=           ", C2, keysConstants.n.ToString().Length);

			string M2 = rsaAlgorithm.Decode(C2.Replace(" ",""), keysConstants.d, keysConstants.n);
			Console.WriteLine("M2= " + M2);
		}
	}
}
