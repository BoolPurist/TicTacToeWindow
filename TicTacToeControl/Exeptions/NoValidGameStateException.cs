using System;

namespace TicTacToeControl
{
  public class NoValidGameStateException : Exception
  {
    public NoValidGameStateException(string message) : base(message) { }
  }
}
