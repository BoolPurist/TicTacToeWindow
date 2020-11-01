using System;
using System.Collections.Generic;

#nullable enable

namespace TicTacToeControl.Model
{
  public class TicTacToeBoxModel
  {
    
    /// <summary> 
    /// Returns the current state of the game.
    /// </summary>
    /// <value>
    /// Auto implementation for public getter only. States are who next to make their turn
    /// , draw or who won
    /// </value>
    public GameState CurrentState { get; private set; }

    /// <summary> 
    /// For finding out if game has ended, if the game has ended additional turns are ignored 
    /// until reset. A game is final if the game was won or ended in a draw.
    /// </summary>
    /// <value> 
    /// Getter only of the fact if game has ended. Returns true if game is decided
    /// or false if the game goes on (next turn will be processed)
    /// </value>
    public bool GameEnded => 
       this.CurrentState == GameState.PlayerOneWins || this.CurrentState == GameState.PlayerTwoWins;

    /// <summary> 
    /// Returns the last given field number on the most current/last turn 
    /// </summary>
    /// <value>
    /// Auto implementation for public getter only. 
    /// Returns -1 if no turn has been made yet
    /// </value>
    public int LastTakeFieldNbr { get; private set; } = COUNTER_FOR_NO_TURNS;

    /// <summary> Gives out the filed number of the turns which resulted in a victory </summary>
    /// <value> 
    /// Getter for enumerable providing the fiedNbrs. Returns nothing if state of game is not a win.
    /// The field number are not order according to history of the made turns.
    /// </value>
    public IEnumerable<int> WinSequence
    {
      get
      {
        if (this._hasEnded && this.CurrentState != GameState.Draw)
        {
          int[,] sequence = this.winSequence;

          for (int i = 0, length = sequence.GetLength(0); i < length; i++)
          {
            yield return ConvertRowColumnToFieldNbr(sequence[i, 0], sequence[i, 1]);
          }
        }
      }
    }

    // If victory is confirmed, the field numbers for the match are stored in this field.
    private readonly int[,] winSequence = new int[_MAX_WIDTH_HEIGHT, 2];

    public TicTacToeBoxModel()
    {
      this.fieldGrid = new FieldStatus[3, 3];
      this.Reset();
    }

    /// <summary> 
    /// Processes a made turned and changes the current state of the game. If current
    /// game state is final, the invocation of this number has no effect.
    /// </summary>
    /// <param name="fieldNumber"> 
    /// Number of the tic tac toe field from 0 to 8. Counting starts from the top left 
    /// corner and ends at the bottom right corner of the box
    /// </param>
    /// <exception cref="NoEmptyPlayFieldException"> 
    /// If a field number is given twice for party (Would occupy a non-empty field)
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"> 
    /// If field number is above 8 or negative
    /// </exception>
    public void MakeTurn(int fieldNumber)
    {      
      // If a final outcome is encounter, this state is returned back until reset.
      if (!this._hasEnded)
      {
        this.LastTakeFieldNbr = fieldNumber;

        // Mapping field number to field coordinates for a 2d array.
        int columnNumber = fieldNumber % _MAX_WIDTH_HEIGHT;
        int rowNumber = fieldNumber / _MAX_WIDTH_HEIGHT;

        // Checking for exception cases
        if (fieldNumber < 0)
        {
          throw new ArgumentOutOfRangeException
            (
            nameof(fieldNumber), fieldNumber,
            $"Field number must not be negative!"
            );
        }
        else if (fieldNumber > MAXIMUM_FIELD_NBR)
        {
          throw new ArgumentOutOfRangeException
            (
            nameof(fieldNumber), fieldNumber,
            $"Field number must not be greater than {MAXIMUM_FIELD_NBR} !"
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
          // Process a valid turn

          // Mapping current state into a field status to put into an empty field which
          // is turned into an occupied one by the player made their turn.
          this.fieldGrid[rowNumber, columnNumber] = 
            this.CurrentState == GameState.TurnPlayerOne
            ? FieldStatus.Player1Occupied : FieldStatus.Player2Occupied;
          
          // Checks if a player has won on the current turn
          this.ValidateTurn(rowNumber, columnNumber);
        }
        
        // Checks if game is already decided by a win or draw
        if (
          this.CurrentState == GameState.PlayerOneWins ||
          this.CurrentState == GameState.PlayerTwoWins
          )
        {
          this._hasEnded = true;
        }
        else if (++this._turnedCounter == MAXIMUM_FIELD_NBR)
        {
          this.CurrentState = GameState.Draw;
          this._hasEnded = true;
        }
        else
        {
          // No Draw or win yet.
          this.CurrentState = this.CurrentState == GameState.TurnPlayerOne 
            ? GameState.TurnPlayerTwo : GameState.TurnPlayerOne;
        }        
      }      
    }
    
    // Local variables for method above
    private int _turnedCounter = -1;    
    private bool _hasEnded = false;

    /// <summary> Resets the model as if it was just created </summary>
    public void Reset()
    {
      
      
      this._hasEnded = false;
      this._turnedCounter = COUNTER_FOR_NO_TURNS;
      this.LastTakeFieldNbr = COUNTER_FOR_NO_TURNS;
      this.MakeFieldsEmpty();      
      this.CurrentState = GameState.TurnPlayerOne;
    }

    // This routine assumes that the this.CurrentState is only GameState.TurnPlayerOne or
    // GameState.TurnPlayerTwo
    private void ValidateTurn(int rowNumber, int columnNumber)
    {
      FieldStatus toBeOccupiedField = this.fieldGrid[rowNumber, columnNumber];

      const int START_COUNT_ADJFIELD = 1;      
      int adjacentFields = START_COUNT_ADJFIELD;
      int currentRowNbr = columnNumber;
      int currentColumnNbr = rowNumber;

      //  4 axis with the toBeOccupiedField as starting point must be traversed to
      // determine if a player has won on a turn.

      // Checking from toBeOccupiedField to top left in the grid.
      if (checkForWin(() => --currentColumnNbr > -1 && --currentRowNbr > -1))
      {
        // A win was found already no need to proceed with the next direction
        return;
      }
      // Checking from toBeOccupiedField to bottom right in the grid.      
      if (checkForWin(() => ++currentColumnNbr < _MAX_WIDTH_HEIGHT && ++currentRowNbr < _MAX_WIDTH_HEIGHT))
      {
        return;
      }
            
      // 1. Axis done !
      // Reset counter for next axis
      adjacentFields = START_COUNT_ADJFIELD;

      // Checking from toBeOccupiedField to top right in the grid.
      if (checkForWin(() => ++currentColumnNbr < _MAX_WIDTH_HEIGHT && --currentRowNbr > -1))
      {
        return;
      }
      
      // Checking from toBeOccupiedField to bottom left in the grid.
      if (checkForWin(() => --currentColumnNbr > -1 && ++currentRowNbr < _MAX_WIDTH_HEIGHT))
      {
        return;
      }

      // 2. Axis done !
      adjacentFields = START_COUNT_ADJFIELD;

      // Checking from toBeOccupiedField to top in the grid.
      if (checkForWin(() => --currentRowNbr > -1))
      {
        return;
      }

      // Checking from toBeOccupiedField to bottom in the grid.
      if (checkForWin(() => ++currentRowNbr < _MAX_WIDTH_HEIGHT))
      {
        return;
      }

      // 3. Axis done !

      adjacentFields = START_COUNT_ADJFIELD;

      // Checking from toBeOccupiedField to left in the grid.      
      if (checkForWin(() => --currentColumnNbr > -1))
      {
        return;
      }

      // Checking from toBeOccupiedField to right in the grid.
      if (checkForWin(() => ++currentColumnNbr < _MAX_WIDTH_HEIGHT))
      {
        return;
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
            
            this.winSequence[adjacentFields, 0] = currentRowNbr;
            this.winSequence[adjacentFields, 1] = currentColumnNbr;

            if (++adjacentFields == _MAX_WIDTH_HEIGHT)
            {
              this.CurrentState = this.CurrentState == GameState.TurnPlayerOne ?
              GameState.PlayerOneWins : GameState.PlayerTwoWins;
              this.winSequence[0, 0] = rowNumber;
              this.winSequence[0, 1] = columnNumber;
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
     
    }
   
    private void MakeFieldsEmpty()
    {
      for (int i = 0, Columnlength = fieldGrid.GetLength(0); i < Columnlength; i++)
      {
        for (int j = 0, RowLength = fieldGrid.GetLength(1); j < RowLength; j++)
        {
          this.fieldGrid[i, j] = FieldStatus.Empty;
        }
      }
    }

    private static int ConvertRowColumnToFieldNbr(int rowNbr, int columnNbr) 
    => (rowNbr * _MAX_WIDTH_HEIGHT) + columnNbr;

    // A field can be occupied or empty
    private enum FieldStatus
    {
      Player1Occupied,
      Player2Occupied,
      Empty
    }

    // A tic tac toe box has a width of 3
    // _MAX_WIDTH_HEIGHT
    private const int _MAX_WIDTH_HEIGHT = 3;
    // MAXIMUM_FIELD_NBR
    // A tic tac toe has 9 fields. This model counts fields from 0 to 8.
    private const int MAXIMUM_FIELD_NBR = 8;

    private const int COUNTER_FOR_NO_TURNS = -1;

    // Array gird with each cell which stores information if a field is still empty 
    // or is occupied by player already. The model decides on this base if a turn leads to 
    // a victory of a player
    private readonly FieldStatus[,] fieldGrid;

  }

  public class NoEmptyPlayFieldException : ArgumentOutOfRangeException
  {
    public NoEmptyPlayFieldException(string paramName , string message, object? actualValue ) 
      : base(paramName, actualValue, message) { }
  }

}
