using System;

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
			Console.WriteLine("-------------------- RESULTS --------------------");
			int numberToCode = 1704;
			int C = rsaAlgorithm.EncodeNumber(numberToCode, keysConstants.e, keysConstants.n);
			if(C == -1)
            {
				return;
            }
			Console.WriteLine("C= " + C);

			int M = rsaAlgorithm.DecodeNumber(C, keysConstants.d, keysConstants.n);
			Console.WriteLine("M= " + M);

			// ------- TESTS ---------- //
			string stringToCode = "RENAISSANCE";
			string C2 = rsaAlgorithm.EncodeString(stringToCode, keysConstants.e, keysConstants.n);
			if(C2 == null)
            {
				return;
            }
            //Console.WriteLine("C2=           " + C2);
            RSAAlgorithm.WriteStringNumber("C2=           ", C2, keysConstants.n.ToString().Length);

            string M2 = rsaAlgorithm.DecodeString(C2, keysConstants.d, keysConstants.n);
			Console.WriteLine("M2= " + M2);
		}
	}
}
