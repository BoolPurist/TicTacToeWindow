using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TicTacToeControl;


namespace TicTacToeMainWindow
{
  /// <summary>
  /// Code-behind of MainWindow.xaml.  
  /// </summary>
  public partial class MainWindow : Window
  {

    /// <summary> 
    /// Model for holding all label texts for showing the current situation. 
    /// </summary>
    /// <value> 
    /// Get/Set of the object which binds a receptive text as the current state 
    /// to a label in xaml
    /// </value>
    public AnnouncerTextModel AnnouncerTxt { get; set;}

    public MainWindow()
    {
      InitializeComponent();

      this.DataContext = this;
      this.AnnouncerTxt = new AnnouncerTextModel();

      // Getting named xaml element
      this.ticTacToeBox = this.TicTacToeGird;
      this.scoreBoard = this.GameScoreBoard;

      // Events for reacting if the state of game changes, 
      // (see GameState in name space TicTacToeControl)
      this.ticTacToeBox.GameEnds += this.OnGameEnds;
      this.ticTacToeBox.ChangeTurn += this.AdjustAnnouncerTxt_OnChangeTurn;

      this.Reset();
    }

    /// <summary> 
    /// Issues reset of  the controls of tic tac toe box and announcer text. 
    /// </summary>
    /// <param name="sender"> Not relevant </param>
    /// <param name="e"> Not relevant </param>
    public void ResetBtn_OnClick(object sender, RoutedEventArgs e)
    {
      this.Reset();
    }

    // Resets the control tic tac toe box and announcer text.
    private void Reset()    
    {
      this.ticTacToeBox.Reset();
      this.AnnouncerTxt.CurrentGameState = GameState.TurnPlayerOne;
    }

    /// <summary> 
    /// Event handler to announce a draw or a the winner. Increases the respective state
    /// counters of the displayed game score
    /// </summary>
    /// <param name="endResult"> State as a draw or who player won </param>
    public void OnGameEnds(GameState endResult)
    {
      switch (endResult)
      {
        case GameState.Draw:
          this.scoreBoard.GameScoreData.Draws++;
          break;
        case GameState.PlayerOneWins:
          throw new NotImplementedException(
            $"No case for {endResult} is implemented yet"
            );
        case GameState.PlayerTwoWins:
          throw new NotImplementedException(
            $"No case for {endResult} is implemented yet"
            );
        default:
          throw new NoValidGameStateException(
            $"{endResult} should not appear as end result of a game"
            );          
      }

      this.AnnouncerTxt.CurrentGameState = endResult;
    }

    /// <summary> 
    /// Binds receptive text to displays current course of the game to the players 
    /// </summary>
    /// <param name="currentState"> State of the current course of the game </param>
    public void AdjustAnnouncerTxt_OnChangeTurn(GameState currentState)
     => this.AnnouncerTxt.CurrentGameState = currentState;

    // Control in which the a player clicks to make their turn.
    private readonly TicTacToeBoxControl ticTacToeBox;
    // Control which shows the counts of draws and wins of a player
    private readonly GameScore scoreBoard;

  }
}
