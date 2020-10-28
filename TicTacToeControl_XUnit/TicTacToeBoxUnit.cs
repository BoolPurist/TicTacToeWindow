using System;

using Xunit;

using TicTacToeControl;
using TicTacToeControl.TicTacToeBox;
using System.Collections.Generic;

namespace TicTacToeControl_XUnit
{

  public class TicTacToeBoxUnit
  {


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
            new int[] {2, 4, 5 },
            new GameState[]
            {
              GameState.TurnPlayerTwo,
              GameState.TurnPlayerOne,
              GameState.TurnPlayerTwo
            }
          },
          {
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
        new int[] { 2, 2 },
        new int[] { 0, 2, 3, 0},
        new int[] { 2, 4, 6, 5, 4 }
      };


    // TODO Implement test for returning draw
    // TODO Implement test for returning GameState.PlayerOneWins

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
