using System;
using System.Collections.Generic;

namespace BrainfuckCompilier
{
	public class Compiler
	{
		private List<Byte> registers;
		private UInt16 CurrentRegisterIndex;
		private static List<Char> Instructions = new List<char> {
			'>',
			'<',
			'+',
			'-',
			',',
			'.',
			'[',
			']'
		};
		private const int REGISTERS_SIZE = 30000;
		private List<int> ErrorPos;
		private List<String> ErrorText;
		private Dictionary<int, int> OpenToCloseIndex, CloseToOpenIndex;
		
		public string SourceCode;
		
		public Compiler()
		{
			registers = new List<byte>();
			for (int i = 0; i < REGISTERS_SIZE; i++)
				registers.Add(0);
			CurrentRegisterIndex = 0;
			ErrorPos = new List<int>();
			ErrorText = new List<string>();
			OpenToCloseIndex = new Dictionary<int, int>();
			CloseToOpenIndex = new Dictionary<int, int>();
		}
		
		private void SyntaxCheck()
		{		//return index where syntax error found
			int OpenSquare = 0;
			int CloseSquare = 0;
			
			this.SourceCode.Replace(' ', '\0');	//remove white spaces
			
			//check for invalid instructions
			for (int i = 0; i < SourceCode.Length; i++) {
				if (!Instructions.Contains(SourceCode[i])) {
					ErrorPos.Add(i);
					ErrorText.Add("Invalid instruction: " + SourceCode[i]);
				} else {
					if (SourceCode[i] == '[')
						OpenSquare++;
					else if (SourceCode[i] == ']')
						CloseSquare++;
				}
			}

			//check for missing open square brackets
			if (OpenSquare < CloseSquare) {
				ErrorPos.Add(0);
				ErrorText.Add("Missing open square bracket at 0");
			} else if (OpenSquare > CloseSquare) {
				ErrorPos.Add(SourceCode.Length);
				ErrorText.Add("Missing open square bracket at " + SourceCode.Length);
			}
		}
		
		private void PrepareBracketsPairs()
		{
			Stack<int> OpenIndex = new Stack<int>();
			
			for (int i = 0; i < this.SourceCode.Length; i++) {
				if (SourceCode[i] == '[')
					OpenIndex.Push(i);
				else if (SourceCode[i] == ']') {
					OpenToCloseIndex.Add(OpenIndex.Peek(), i);
					CloseToOpenIndex.Add(i, OpenIndex.Peek());
					OpenIndex.Pop();
				}
			}
		}
		
		public bool Compile()
		{
			this.SyntaxCheck();
			this.PrepareBracketsPairs();
			if (ErrorPos.Count == 0) {//if no errors found
				int CurrentSourceCodeIndex = 0;
				while (CurrentSourceCodeIndex < this.SourceCode.Length) {
					switch (SourceCode[CurrentSourceCodeIndex]) {
						case '+':
							this.registers[this.CurrentRegisterIndex]++;
							break;
						case '-':
							this.registers[this.CurrentRegisterIndex]--;
							break;
						case '>':
							this.CurrentRegisterIndex++;
							break;
						case '<':
							this.CurrentRegisterIndex--;
							break;
						case '.':
							Console.Write(Convert.ToChar(this.registers[this.CurrentRegisterIndex]));
							break;
						case ',':
							Console.Write("Input: ");
							this.registers[this.CurrentRegisterIndex] = Convert.ToByte(Console.ReadKey().KeyChar);
							Console.WriteLine();
							break;
						case '[':
							if (this.registers[this.CurrentRegisterIndex] == 0) {
								CurrentSourceCodeIndex = this.OpenToCloseIndex[CurrentSourceCodeIndex];
							}
							break;
						default:
							if (this.registers[this.CurrentRegisterIndex] != 0)
								CurrentSourceCodeIndex = this.CloseToOpenIndex[CurrentSourceCodeIndex];
							break;
					}
					CurrentSourceCodeIndex++;
				}
				return true;
			} else
				return false;
		}
		
		public void GetError(ref List<int> ErrorPos, ref List<String> ErrorText)
		{
			ErrorPos = this.ErrorPos;
			ErrorText = this.ErrorText;
		}
	}
}
