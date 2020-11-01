using System.ComponentModel;
using TicTacToeControl;

namespace TicTacToeMainWindow
{
  /// <summary> 
  /// Takes a current state of TicTacToeBoxControl. 
  /// Holds receptive of text for a state of a tic tac toe box TicTacToeBoxControl 
  /// Some props can be binded for xaml
  /// </summary>
  public class AnnouncerTextModel : INotifyPropertyChanged
  {
    // These constant strings to change receptive texts 
    // shown the current state of game to player
    
    /// <summary> Text to show that 1. player is next to make turn </summary>
    public string player1TurnTxt = "1. Player make your turn please";
    /// <summary> Text to show that 2. player is next to make turn </summary>
    public string player2TurnTxt = "2. Player make your turn please";
    /// <summary> Text to show that a draw has occurred </summary>
    public string drawTxt = "Have a draw !";
    /// <summary> Text to show that 1. player has won </summary>
    public string player1WinTxt = "1. Player has won";
    /// <summary> Text to show that 2. player has won </summary>
    public string player2WinTxt = "2. Player has won";

    /// <summary> Used for binding by the binding engine </summary>
    public event PropertyChangedEventHandler PropertyChanged;
   
    public AnnouncerTextModel(GameState _currentGameState)
    {
      this.CurrentGameState = _currentGameState;
    }

    public AnnouncerTextModel() : this(GameState.TurnPlayerOne) { }


    private GameState currentGameState;
    /// <summary> 
    /// Holds receptive of text for a state of a tic tac toe box TicTacToeBoxControl  
    /// </summary>
    /// <value> 
    /// Can be binded , Get/Set of current state of the game. 
    /// If set it also changes CurrentText prop with a receptive text for the state 
    /// </value>
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

    private string currentText;
    /// <summary> 
    /// Holds a text to show the player the current situation of the game
    /// </summary>
    /// <value> Can be binded, Get/Set of the receptive text for state of the game </value>
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
