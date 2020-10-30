using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

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

        int columnNumber = fieldNumber % _maxWidthHeight;
        int rowNumber = fieldNumber / _maxWidthHeight;

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
        else if (this.fieldGrid[rowNumber, columnNumber] != FieldStatus.Empty)
        {
          throw new NoEmptyPlayFieldException(
            nameof(fieldNumber),
            $"FieldNumber is not empty anymore !",
            fieldNumber
            );
        }
        else
        {
          this.fieldGrid[rowNumber, columnNumber] = 
            this.currentState == GameState.TurnPlayerOne
            ? FieldStatus.Player1Occupied : FieldStatus.Player2Occupied;
          
          this.currentState = this.ValidateTurn(rowNumber, columnNumber);
        }
        
        // Checks if game is already decided by a win or draw
        if (
          this.currentState == GameState.PlayerOneWins ||
          this.currentState == GameState.PlayerTwoWins
          )
        {
          this.hasEnded = true;
        }
        else if (++this.turnedCounter == maximumFieldNumber)
        {
          this.currentState = GameState.Draw;
          this.hasEnded = true;
        }
        else
        {
          this.currentState = this.currentState == GameState.TurnPlayerOne 
            ? GameState.TurnPlayerTwo : GameState.TurnPlayerOne;
        }        
      }

      return this.currentState;
    }

    private int turnedCounter = -1;
    private bool hasEnded = false;

    // This routine assumes that the this.currentState is only GameState.TurnPlayerOne or
    // GameState.TurnPlayerTwo
    private GameState ValidateTurn(int rowNumber, int columnNumber)
    {
      FieldStatus toBeOccupiedField = this.fieldGrid[rowNumber, columnNumber];

      const int startCountAdjField = 1;
      int adjacentFields = startCountAdjField;
      int currentRowNbr = columnNumber;
      int currentColumnNbr = rowNumber;

      // From the toBeOccupiedField 4 axis must be traversed to
      // determine if a player has won on a turn.

      // Checking from toBeOccupiedField to top left in the grid.
      if (checkForWin(() => --currentColumnNbr > -1 && --currentRowNbr > -1))
      {
        // A win was found already no need to proceed with the next direction
        return this.currentState;
      }
      // Checking from toBeOccupiedField to bottom right in the grid.      
      if (checkForWin(() => ++currentColumnNbr < _maxWidthHeight && ++currentRowNbr > _maxWidthHeight))
      {
        return this.currentState;
      }
            
      // 1. Axis done !
      // Reset counter for next axis
      adjacentFields = startCountAdjField;

      // Checking from toBeOccupiedField to top right in the grid.
      if (checkForWin(() => ++currentColumnNbr < _maxWidthHeight && --currentRowNbr > -1))
      {
        return this.currentState;
      }
      
      // Checking from toBeOccupiedField to bottom left in the grid.
      if (checkForWin(() => --currentColumnNbr > -1 && ++currentRowNbr < _maxWidthHeight))
      {
        return this.currentState;
      }

      // 2. Axis done !
      adjacentFields = startCountAdjField;

      // Checking from toBeOccupiedField to top in the grid.
      if (checkForWin(() => --currentRowNbr > -1))
      {
        return this.currentState;
      }

      // Checking from toBeOccupiedField to bottom in the grid.
      if (checkForWin(() => ++currentRowNbr < _maxWidthHeight))
      {
        return this.currentState;
      }

      // 3. Axis done !

      adjacentFields = startCountAdjField;

      // Checking from toBeOccupiedField to left in the grid.      
      if (checkForWin(() => --currentColumnNbr > -1))
      {
        return this.currentState;
      }

      // Checking from toBeOccupiedField to right in the grid.
      if (checkForWin(() => ++currentColumnNbr < _maxWidthHeight))
      {
        return this.currentState;
      }

      // 4. and with it all axis done !

      // Traverses by a given direction from the last occupied field along a half of axis 
      // It returns true if a win is detected or false otherwise.
      bool checkForWin(Func<bool> directionToCheck)
      {
        currentRowNbr = rowNumber;
        currentColumnNbr = columnNumber;

        while (directionToCheck())
        {
          if (this.fieldGrid[currentRowNbr, currentColumnNbr] == toBeOccupiedField)
          {
            if (++adjacentFields == _maxWidthHeight)
            {
              this.currentState = this.currentState == GameState.TurnPlayerOne ?
              GameState.PlayerOneWins : GameState.PlayerTwoWins;
              return true;
            }
          }
          else
          {
            break;
          }
        }

        return false;
      }

      // No win yet so the current state is returned back unchanged.
      return this.currentState;

    }

    #region Private sector
    private const int _maxWidthHeight = 3;

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
