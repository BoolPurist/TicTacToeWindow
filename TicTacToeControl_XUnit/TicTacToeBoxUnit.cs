using System;
using System.Collections.Generic;
using System.DirectoryServices;

using Xunit;

using TicTacToeControl;
using TicTacToeControl.Model;
using System.Security.Cryptography.X509Certificates;

namespace TicTacToeControl_XUnit
{
  public class TicTacToeBoxUnit
  {


    #region Tests

    // Tests incomplete games (without draw or a win) in order to see 
    // if the right player is returned for the next turn.
    [Theory]
    [MemberData(nameof(TurnsNoWinsDraws))]
    public void MakeTurn_ShouldReturnNextPlayer(int[] madeTurns, GameState[] expectedStates)
    {    
      IterCases(madeTurns, expectedStates);
    }

    // Should always return the field number from the last made turn.
    [Theory]
    [MemberData(nameof(DrawTurnsData))]
    public void LastTakeFieldNbr_ShouldReturnTakenFieldNbr(int[] madeTurns)
    {
      // Should return -1 if game starts because no turn was made.
      var ticTacToeBoxModel = new TicTacToeBoxModel();
      Assert.Equal(ticTacToeBoxModel.LastTakeFieldNbr, -1);

      foreach (int fieldNbr in madeTurns)
      { 
        ticTacToeBoxModel.MakeTurn(fieldNbr);
        Assert.Equal(ticTacToeBoxModel.LastTakeFieldNbr, fieldNbr);
      }
    }

    // Testing if a negative or too high field number result 
    // in an ArgumentOutOfRangeException exception
    [Theory]
    [MemberData(nameof(NegativeOrTooHightFieldNumberData))]
    public void MakeTurn_ShouldThrowIfFieldNumberNegativeOrTooHigh(int [] madeTurns)
    {
      Assert.Throws<ArgumentOutOfRangeException>( () => { IterCasesForExcep(madeTurns); } );  
    }

    // Tests if NoEmptyPlayFieldException an  exception is thrown 
    // if a field number is taken again and in this way 
    // it tried to occupy a non empty field.
    [Theory]
    [MemberData(nameof(NoEmptyFieldsData))]
    public void MakeTurn_ShouldThrowForNoEmptyField(int[] madeTurns)
    {
      Assert.Throws<NoEmptyPlayFieldException>(() => { IterCasesForExcep(madeTurns); });
    }

    // Tests if the draw as a state is returned if no players win.
    [Theory]
    [MemberData(nameof(DrawTurnsData))]
    public void MakeTurn_ShouldReturnDraw(int[] madeTurns)
    {
      IterCases(madeTurns, StatesForDraw);
    }

    // Tests if it can detect if and which player wins on a turn.
    [Theory]
    [MemberData(nameof(WinTurnData))]
    public void MakeTurn_ShouldReturnWinner(int[] madeTurns, GameState[] gameStates)
    {
      IterCases(madeTurns, gameStates);
    }

    // Reset functionality is verified by the following case:
    // 3 cases with a receptively different outcome are invoked.
    // After each case the Reset method is invoked. It should work over all
    // this 3 cases without the need to recreate model instance.
    [Fact]
    public void Reset_ShouldReturnCorrectOutcomeFor3PartiesThroughResets()
    {
      int[] fieldNbr;
      GameState[] madeTurns;
      var logicModel = new TicTacToeBoxModel();

      // 1. case is an undone game party 

      // [|] [|] [o] 
      // [x] [o] [x]
      // [|] [x] [|] 
      fieldNbr = new int[] { 5, 4, 7, 2, 3 };
      madeTurns = new GameState[]
      {
        GameState.TurnPlayerTwo,
        GameState.TurnPlayerOne,
        GameState.TurnPlayerTwo,
        GameState.TurnPlayerOne,
        GameState.TurnPlayerTwo
      };

      IterCasesWithReset();

      // 2. case is a game ending in draw

      // [x] [o] [x] 
      // [o] [x] [x]
      // [o] [x] [o] 
      fieldNbr = new int[] { 0, 1, 4, 8, 5, 3, 2, 6, 7 };
      madeTurns = StatesForDraw;

      IterCasesWithReset();

      // 3. case is a game ending with 1. players winning

      // [o] [x] [o] 
      // [o] [x] [x]
      // [x] [x] [o] 
      
      fieldNbr = new int[] { 4, 8, 5, 3, 7, 1, 6, 0, 2 };
      madeTurns = new GameState[]
      {
        GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
        GameState.TurnPlayerOne, GameState.TurnPlayerTwo, GameState.TurnPlayerOne,
        GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.PlayerOneWins
      };

      IterCasesWithReset();

      void IterCasesWithReset()
      {
        for (int i = 0, length = fieldNbr.Length; i < length; i++)
        {
          logicModel.MakeTurn(fieldNbr[i]);
          Assert.Equal(logicModel.CurrentState, madeTurns[i]);
        }

        logicModel.Reset();
      }

    }

    // Reason suppressing xUnit1026 and IDE0060 warning here: 
    // WinTurnData already has enough field numbers leading to victory, but the second
    // array is not needed for this theory. XUnit would crash if the 2. parameter is not provided.
    // Should only return true after the last turn which caused victory.
    [Theory]
    [MemberData(nameof(WinTurnData))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
#pragma warning disable IDE0060 // Remove unused parameter
    public void GameEnded_ShouldReturnTrueIfWin(int[] madeTurns, GameState[] noRelevance)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
      var ticTacToeBoxModel = new TicTacToeBoxModel();
      int lastIndex = madeTurns.Length - 1;
      
      for (int i = 0, length = lastIndex; i < length; i++)
      {
        ticTacToeBoxModel.MakeTurn(madeTurns[i]);
        Assert.False(ticTacToeBoxModel.GameEnded);
      }

      ticTacToeBoxModel.MakeTurn(madeTurns[lastIndex]);
      Assert.True(ticTacToeBoxModel.GameEnded);
    }

    // Should return always false because a game ended in draw is still no win.
    [Theory]
    [MemberData(nameof(DrawTurnsData))]
    public void GameEnded_ShouldReturnFalseDraw(int[] madeTurns)
    {
      var ticTacToeBoxModel = new TicTacToeBoxModel();
      foreach (int fieldNumber in madeTurns)
      {
        ticTacToeBoxModel.MakeTurn(fieldNumber);
        Assert.False(ticTacToeBoxModel.GameEnded);
      }
    }

    #endregion

    #region Test cases
    // Some test cases are described in
    // how a tic tac toe fields looks like at the end of party
    // One example 
    // [x] [o] [|]
    // [|] [x] [|]
    // [o] [x] [|]
    // x = turn of 1. player 
    // o = turn of 2. player
    // | = empty field
    // field number can be from 0 to 8 and is mapped to tic tac toe field like this
    // [0] [1] [2]
    // [3] [4] [5]
    // [6] [7] [8]

    public static TheoryData<int[], GameState[]> TurnsNoWinsDraws
    => new TheoryData<int[], GameState[]>()
    {
          {
            // [|] [|] [x] 
            // [o] [x] [|]
            // [|] [|] [|] 
            new int[] {2, 4, 5 },
            new GameState[]
            {
              GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne,
              GameState.TurnPlayerTwo
            }
          },
          {
            // [x] [o] [|] 
            // [|] [|] [x]
            // [|] [|] [o] 
            new int[] { 0, 1, 5, 8 },
            new GameState[]
            {
              GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne,
              GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne
            }
          },
          {
            // [|] [|] [o] 
            // [x] [o] [x]
            // [|] [x] [|] 
            new int[] { 5, 4, 7, 2, 3 },
            new GameState[]
            {
              GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne,
              GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne,
              GameState.TurnPlayerTwo
            }
          }
    };

    public static TheoryData<int[]> NegativeOrTooHightFieldNumberData
    => new TheoryData<int[]>()
    {
           new int[] { 2, -1 },
           new int[] { 0, 2, 12}
    };

    public static TheoryData<int[]> NoEmptyFieldsData
    => new TheoryData<int[]>()
    {
          // 2. field is twice in there
          new int[] { 2, 2 },
          // 0. field is twice in there
          new int[] { 0, 2, 3, 0},
          // 4. field is twice in there
          new int[] { 2, 4, 6, 5, 4 }
    };

    public static TheoryData<int[]> DrawTurnsData
    => new TheoryData<int[]>()
    {  
            // [o] [x] [x] 
            // [x] [o] [o]
            // [x] [o] [x]         
            new int[] { 3, 4, 1, 5, 2, 0, 8, 7, 6 },
            // [o] [x] [o] 
            // [x] [x] [o]
            // [x] [o] [x] 
            new int[] { 3, 7, 6, 0, 4, 5, 8, 2, 1 },
            // [x] [o] [x] 
            // [o] [x] [x]
            // [o] [x] [o] 
            new int[] { 0, 1, 4, 8, 5, 3, 2, 6, 7 }
    };


    public static TheoryData<int[], GameState[]> WinTurnData
    => new TheoryData<int[], GameState[]>()
    {
          {
            // [|] [x] [o] 
            // [|] [x] [|]
            // [|] [x] [o] 
            new int[] { 7, 8, 4, 2, 1 },
            new GameState[]
            {
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.PlayerOneWins
            }
          },
          {
            // [x] [x] [o] 
            // [x] [o] [|]
            // [o] [|] [x] 
            new int[] { 3, 4, 0, 6, 8, 2 },
            new GameState[]
            {
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.TurnPlayerTwo, GameState.PlayerTwoWins
            }
          },
          { 
            // [o] [o] [x] 
            // [o] [x] [x]
            // [o] [x] [|] 
            new int[] { 7, 6, 4, 1, 5, 3, 2, 0 },
            new GameState[]
            {
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.TurnPlayerTwo, GameState.TurnPlayerOne,
              GameState.TurnPlayerTwo, GameState.PlayerTwoWins
            }
          },
          {
            // [o] [x] [o] 
            // [o] [x] [x]
            // [x] [x] [o] 
            new int[] { 4, 8, 5, 3, 7, 1, 6, 0, 2 },
            new GameState[]
            {
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.TurnPlayerTwo, GameState.TurnPlayerOne,
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.PlayerOneWins
            }
          },
          {
            // [o] [|] [|] 
            // [x] [x] [x]
            // [|] [|] [o] 
            new int[] { 4, 8, 5, 0, 3 },
            new GameState[]
            {
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.PlayerOneWins,
            }
          },
          {
            // [x] [|] [o] 
            // [x] [o] [|]
            // [o] [|] [x] 
            new int[] { 3, 4, 0, 6, 8, 2 },
            new GameState[]
            {
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.TurnPlayerTwo, GameState.PlayerTwoWins
            }
          },
          {
            // [|] [|] [|] 
            // [o] [o] [|]
            // [x] [x] [x] 
            new int[] { 7, 4, 8, 3, 6 },
            new GameState[]
            {
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.PlayerOneWins
            }
          },
          {
            // [x] [|] [|] 
            // [|] [x] [|]
            // [o] [o] [x] 
            new int[] { 8, 7, 4, 6, 0 },
            new GameState[]
            { 
              GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne, GameState.PlayerOneWins 
            }
          },

    };


    // A draw always results into this sequence of returned states if 1. player starts    
    private static GameState[] StatesForDraw
    => new GameState[]
    {
          GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
          GameState.TurnPlayerOne, GameState.TurnPlayerTwo, GameState.TurnPlayerOne,
          GameState.TurnPlayerTwo , GameState.TurnPlayerOne, GameState.Draw
    };

    #endregion

    #region routines
    // Used for most theories. 
    // Parameter input sequence of field numbers for the turns
    // Parameter expectedOutput sequence which contains the expected state 
    // for the next turn for the receptive field number provide by the parameter input
    private static void IterCases(int[] input, GameState[] expectedOutput)
    {
      var ticTacToeBox = new TicTacToeBoxModel();

      for (int i = 0, length = input.Length; i < length; i++)
      {
        ticTacToeBox.MakeTurn(input[i]);
        Assert.Equal(ticTacToeBox.CurrentState, expectedOutput[i]);
      }
    }

    // Used for theories to check if exceptions are thrown for turn sequences which
    // contain invalid turns
    // parameter madeTurns are the sequence which contains at least one invalid turn
    private static void IterCasesForExcep(int[] madeTurns)
    {
      var ticTacToeBox = new TicTacToeBoxModel();
      foreach (int fieldNumber in madeTurns)
      {
        ticTacToeBox.MakeTurn(fieldNumber);
      }
    }

    #endregion
  
  }

}
