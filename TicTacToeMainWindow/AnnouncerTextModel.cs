using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.CodeDom;
using TicTacToeControl;

namespace TicTacToeMainWindow
{
  public class AnnouncerTextModel : INotifyPropertyChanged
  {
    // TODO Do commenting

    public AnnouncerTextModel(GameState _currentGameState)
    {
      this.CurrentGameState = _currentGameState;
    }

    public AnnouncerTextModel() : this(GameState.TurnPlayerOne) { }

    public event PropertyChangedEventHandler PropertyChanged;

    private GameState currentGameState;
    public GameState CurrentGameState
    {
      get => this.currentGameState;
      set
      {
        this.currentGameState = value;

        switch (value)
        {
          case GameState.TurnPlayerOne:
            this.CurrentText = player1TurnTxt;            
            break;
          case GameState.TurnPlayerTwo:
            this.CurrentText = player2TurnTxt;
            break;
          case GameState.Draw:
            this.CurrentText = drawTxt;
            break;
          case GameState.PlayerOneWins:
            this.CurrentText = player1WinTxt;
            break;
          case GameState.PlayerTwoWins:
            this.CurrentText = player2WinTxt;            
            break;          
        }

        this.OnPropertyChanged(nameof(this.CurrentGameState));
      }
    }

    private const string player1TurnTxt = "1. Player make your turn please";

    private const string player2TurnTxt = "2. Player make your turn please";

    private const string drawTxt = "Have a draw !";

    private const string player1WinTxt = "1. Player has won";

    private const string player2WinTxt = "2. Player has won";

    private string currentText;

    public string CurrentText
    {
      get => this.currentText;
      set
      {
        this.currentText = value;
        this.OnPropertyChanged(nameof(this.CurrentText));
      }
    }
       
    private void OnPropertyChanged(string paramName)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(paramName));
    }
  }
}
