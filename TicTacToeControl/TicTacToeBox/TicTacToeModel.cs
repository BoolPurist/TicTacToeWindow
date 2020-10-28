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

    
    // TODO Implement returning GameState.PlayerOneWins and GameState.PlayerTwoWins
    private int turnedCounter = -1;
    private bool hasEnded = false;

    /// <summary> 
    /// Processes a made turned and returns the state of the tic tac toe play box
    /// after the made turn.
    /// </summary>
    /// <param name="fieldNumber"> 
    /// Number of the tic tac toe field from 0 to 8. Counting starts from the top left 
    /// corner and ends at the bottom right corner of the box
    /// </param>
    /// <returns> 
    /// Returns the state after a turn. The state tells 
    /// which player is next, has already won or if a draw occurred
    /// </returns>
    /// <exception cref="NoEmptyPlayFieldException"> 
    /// If a field number is given twice for party (Would occupy a non-empty field)
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"> 
    /// If field number is above 8 or negative
    /// </exception>
    public GameState MakeTurn(int fieldNumber)
    {

      if (!this.hasEnded)
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

        this.turnedCounter++;

        if (turnedCounter == maximumFieldNumber)
        {
          this.currentState = GameState.Draw;
          this.hasEnded = true;
        }

      }

      return this.currentState;
    }

    #region Private sector

    private enum FieldStatus
    {
      Player1Occupied,
      Player2Occupied,
      Empty
    }

    private const int maximumFieldNumber = 8;

    private readonly FieldStatus[,] fieldGrid;

    private GameState currentState;

    #endregion

  }

  public class NoEmptyPlayFieldException : ArgumentOutOfRangeException
  {
    public NoEmptyPlayFieldException(string paramName , string message, object? actualValue ) 
      : base(paramName, actualValue, message) { }
  }


}
