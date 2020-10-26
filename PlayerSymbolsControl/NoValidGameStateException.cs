using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeControl
{
  public class NoValidGameStateException : Exception
  {
    public NoValidGameStateException(string message) : base(message) { }
  }
}
