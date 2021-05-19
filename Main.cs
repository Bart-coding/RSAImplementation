using System;
using System.Numerics;
using System.Collections.Generic;
using System.Text;

namespace RSAProject
{

					
public class RSAProgram
{
        public static void Main(string[] args)
         {
                RSAAlgorithm rsaAlgorithm = new RSAAlgorithm();
                RSAAlgorithm.KeysConstants keysConstants = rsaAlgorithm.GenerateKeys();


                Console.WriteLine("e: "+keysConstants.e+" n: "+keysConstants.n);
                Console.WriteLine("d: "+keysConstants.d); //tmp

                int inputNum = 5;
                int C = rsaAlgorithm.Encode(inputNum, keysConstants.d, keysConstants.n);
                Console.WriteLine("C: "+C);

                int M = rsaAlgorithm.Decode(C, keysConstants.e, keysConstants.n);
                Console.WriteLine("M: "+M);

                //--------TESTS--------
		string message = "ameryka";
		message = message.ToUpper();
		
		List<string> substrings = RSAAlgorithm.GetLetterPairs(message);
		
		foreach (string s in substrings)
		{
			Console.WriteLine(s);
		}
        
        	List<string> codedNumbers = RSAAlgorithm.EncodeLetterPairs(substrings);
        	foreach (string s in codedNumbers)
		{
			Console.WriteLine(s);
		}
        
        	string C2 = rsaAlgorithm.Encode("RENAISSANCE", keysConstants.d, keysConstants.n); //to get results like on lecture, p should be 53, q 61 and e 791
        	Console.WriteLine("C2: "+C2); 

         }
}

public class RSAAlgorithm
{
	public struct KeysConstants 
	{
		public int n, e, d;
	}
	
	public static int GCDEuclidean (int a, int b) 
	{
		int temp;
		while (b!=0)
		{
			temp = b;
			b = a%b;
			a = temp;
		}
		
		return a;
	}
	
	public static int ModInverse (int a, int n) //for two coprime numbers; based on wiki adaptation of Extended Euclidean Algorithm
	{
		int t = 0;
		int newt = 1;
		int r = n;
		int newr = a;
		int quotient, tmp;
		
		while (newr!=0)
		{
			quotient = r/newr;
			
			tmp = newt;
			newt = t - quotient*newt;
			t = tmp;
			
			tmp = newr;
			newr = r - quotient*newr;
			r = tmp;
		}
		
		/*if (r>1) //a is not invertible; but a and n are coprime numbers so they should be 
			return -1;*/
		
		if (t<0)
			t = t+n;
		
		return t;
		
	}
	
	public static List<string> GetLetterPairs (string message)
	{
		List<string> substrings = new List<string>();
		
		for (int i = 0; i<message.Length-1; i=i+2 )
		{
		  substrings.Add(message.Substring(i,2));
		}
		if (message.Length%2!=0)
	          substrings.Add(message[message.Length-1]+" ");
		
		return substrings;
	}
    
	  public static List<string> EncodeLetterPairs (List<string> substrings) //2-char strings
	  {
	      List<string> numbersCodes = new List<string>();

	      string temp;
	      int a, b;
	      foreach (string s in substrings)
	      {
		  a = s[0]-'A';
		  temp = (a/10).ToString() + (a%10).ToString();

		  if (s[1].Equals(' '))
		       temp+=26;
		  else
		  {
		      b = s[1]-'A';
		      temp += (b/10).ToString() + (b%10).ToString();
		  }

		  numbersCodes.Add(temp);
	      }

	      return numbersCodes;
	  }
	
	public KeysConstants GenerateKeys()
	{
		int p = 61; //choosing p and q should be extended -> ToDo
		int q = 533;
		
		int n = p*q;
		
		int phi = (p-1)*(q-1); //Euler function
		
		int e = 2;
		
		while(e!=phi)
		{
			if (GCDEuclidean(e,phi)==1)
				break;
			e++;
		}
		
		int d = ModInverse(e,phi);
		
		return new KeysConstants 
		{
			n = n,
			e = e,
			d = d
		};
	}
	
	public int Encode (int M, int d, int n) //M and C will be strings; BigInteger is for great numbers from Pow
	{
		int C;
		
		C = (int) BigInteger.ModPow(M, d, n);
		return C;
	}
	
	public int Decode (int C, int e, int n) //M and C will be strings; BigInteger is for great numbers from Pow
	{
		int M;
		
		M = (int) BigInteger.ModPow(C, e, n);
		return M;
	}
	
	public string Encode (string M, int d, int n)
	{
		//List<string> Csubstrings = new List<string>();
		StringBuilder sb = new StringBuilder();
		string C;
		List<string> letterPairs = GetLetterPairs(M);
		List<string> encodedLetterPairs = EncodeLetterPairs(letterPairs);
		
		foreach (string s in encodedLetterPairs)
		{
			int num = (int) BigInteger.ModPow(Int32.Parse(s), d, n);
		    	sb.Append(num);
		}
		
		C = sb.ToString();
		return C;
	}
}


}
