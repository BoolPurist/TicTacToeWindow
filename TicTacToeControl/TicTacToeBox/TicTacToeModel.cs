using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace TicTacToeControl.TicTacToeBox
{
  public class TicTacToeModel
  {

    public TicTacToeModel()
    {
      this.currentState = GameState.TurnPlayerOne;
      this.fieldGrid = new FieldStatus[3,3];

      for (int i = 0, Columnlength = fieldGrid.GetLength(0); i < Columnlength; i++ )
      {
        for (int j = 0, RowLength = fieldGrid.GetLength(1); j < RowLength; j++)
        {
          this.fieldGrid[i, j] = FieldStatus.Empty;
        }
      }
     
        
    }

    private readonly FieldStatus[,] fieldGrid;

    private const int maximumFieldNumber = 8;

    private enum FieldStatus
    {
      Player1Occupied,
      Player2Occupied,
      Empty
    }

    private GameState currentState;

    public GameState MakeTurn(int fieldNumber)
    {
      int ColumnNumber = fieldNumber / 3;
      int RowNumber = fieldNumber % 3;
      
      if (fieldNumber < 0)
      {
        throw new ArgumentOutOfRangeException
          (
          nameof(fieldNumber), fieldNumber,
          $"Field number must not be negative!"
          );
      }
      else if (fieldNumber > maximumFieldNumber)
      {
        throw new ArgumentOutOfRangeException
          (
          nameof(fieldNumber), fieldNumber,
          $"Field number must not be greater than {maximumFieldNumber} !"
          );
      }
      else if (this.fieldGrid[ColumnNumber, RowNumber] != FieldStatus.Empty)
      {
        throw new NoEmptyPlayFieldException(
          nameof(fieldNumber),
          $"FieldNumber is not empty anymore !",
          fieldNumber
          );
      }
      else if (this.currentState == GameState.TurnPlayerOne)
      {
        this.currentState = GameState.TurnPlayerTwo;
        this.fieldGrid[ColumnNumber, RowNumber] = FieldStatus.Player1Occupied;
      }
      else
      {
        this.currentState = GameState.TurnPlayerOne;
        this.fieldGrid[ColumnNumber, RowNumber] = FieldStatus.Player2Occupied;
      }



      return this.currentState;
    }
  }

  public class NoEmptyPlayFieldException : ArgumentOutOfRangeException
  {
    public NoEmptyPlayFieldException(string paramName , string message, object? actualValue ) 
      : base(paramName, actualValue, message) { }
  }
}
