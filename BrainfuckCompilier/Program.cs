using System;
using System.Collections.Generic;

namespace BrainfuckCompilier
{
	class BrainfuckCompilier
	{
		private static BrainfuckCompilier BFCompiler;
		private String SourceCode;
		private Compiler Compiler;
		private List<int> ErrorPos;
		private List<String> ErrorText;

		
		private BrainfuckCompilier()
		{
			SourceCode = "";
			Compiler = new Compiler();
			ErrorPos = new List<int>();
			ErrorText = new List<string>();
		}
		
		private void InputSourceCode()
		{
			Console.WriteLine("Source code: ");
			String temp = Console.ReadLine();
			while (temp.Length > 0) {
				SourceCode += temp;
				temp = Console.ReadLine();
			}
		}
		
		private void Compile()
		{
			Compiler.SourceCode = this.SourceCode;
			if (!Compiler.Compile()) {
				this.Compiler.GetError(ref this.ErrorPos, ref this.ErrorText);
				Console.WriteLine("Compilation failed" + "\n" + this.ErrorPos.Count + " error(s)");
				for (int i = 0; i < this.ErrorPos.Count; i++) {
					Console.Write(i + "(" + this.ErrorPos[i] + "): " + this.ErrorText[i] + "\n");
				}
			}
		}
		
		public static void Main(string[] args)
		{
			BFCompiler = new BrainfuckCompilier();
			BFCompiler.InputSourceCode();
			BFCompiler.Compile();
			Console.ReadKey(true);
		}
	}
}