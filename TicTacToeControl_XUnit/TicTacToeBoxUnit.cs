using System;

using Xunit;

using TicTacToeControl;
using TicTacToeControl.TicTacToeBox;
using System.Collections.Generic;
using System.DirectoryServices;

namespace TicTacToeControl_XUnit
{

  public class TicTacToeBoxUnit
  {
    // Some test cases are described 
    // with how a tic tac toe fields looks like at the end of party
    // One example 
    // [x] [o] [|]
    // [|] [x] [|]
    // [o] [x] [|]
    // x = turn of 1. player 
    // o = turn of 2. player
    // | = empty field
    // field number can be number from 0 to 8 and is mapped to tic tac toe field like this
    // [0] [1] [2]
    // [3] [4] [5]
    // [6] [7] [8]

    [Theory]
    [MemberData(nameof(TurnsNoWinsDraws))]
    public void MakeTurn_ShouldReturnNextPlayer(int[] madeTurns, GameState[] expectedStates)
    {    
      IterCases(madeTurns, expectedStates);
    }
      
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

    [Theory]
    [MemberData(nameof(NegativeOrTooHightFieldNumberData))]
    public void MakeTurn_ShouldThrowIfFieldNumberNegativeOrTooHigh(
        int [] madeTurns
        )
    {
      Assert.Throws<ArgumentOutOfRangeException>
        (
          () => { IterCasesForExcep(madeTurns); }
        );  
    }

    public static TheoryData<int[]> NegativeOrTooHightFieldNumberData
    => new TheoryData<int[]>()
    {      
       new int[] { 2, -1 },
       new int[] { 0, 2, 12}      
    };

    // Tests if an exception is thrown if a field number is taken again and 
    // in this way it tried to occupy a non empty field.
    [Theory]
    [MemberData(nameof(NoEmptyFieldsData))]
    public void MakeTurn_ShouldThrowForNoEmptyField(int[] madeTurns)
    {
      Assert.Throws<NoEmptyPlayFieldException>(() => { IterCasesForExcep(madeTurns); });
    }

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

    // A draw always results into this sequence of returned states if 1. player starts
    private static GameState[] StatesForDraw
  => new GameState[]
  {
        GameState.TurnPlayerTwo, GameState.TurnPlayerOne, GameState.TurnPlayerTwo,
        GameState.TurnPlayerOne, GameState.TurnPlayerTwo, GameState.TurnPlayerOne,
        GameState.TurnPlayerTwo , GameState.TurnPlayerOne, GameState.Draw
  };

    // Tests if the draw as a state is returned if no players win.
    [Theory]
    [MemberData(nameof(DrawTurnsData))]
    public void MakeTurn_ShouldReturnDraw(int[] madeTurns)
    {
      IterCases(madeTurns, StatesForDraw);
    }


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

    [Theory]
    [MemberData(nameof(WinTurnData))]
    public void MakeTurn_ShouldReturnWinner(int[] madeTurns, GameState[] gameStates)
    {
      IterCases(madeTurns, gameStates);
    }

    // TODO Created more test case for a win.

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
      };

    // TODO create test cases for method Reset

    private static void IterCases(int[] input, GameState[] expectedOutput)
    {
      var ticTacToeBox = new TicTacToeModel();

      for (int i = 0, length = input.Length; i < length; i++)
      {
        Assert.Equal(ticTacToeBox.MakeTurn(input[i]), expectedOutput[i]);
      }
    }

    private static void IterCasesForExcep(int[] madeTurns)
    {
      var ticTacToeBox = new TicTacToeModel();
      foreach (int fieldNumber in madeTurns)
      {
        ticTacToeBox.MakeTurn(fieldNumber);
      }
    }
  }

  

}
