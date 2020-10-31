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
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary> Binding source for current state of the game 
    /// and a respective text to tell the player the current state 
    /// </summary>
    /// <value> 
    /// Get/Set auto implemented. 
    /// Property CurrentGameState of this field 
    /// is the binding source for property Tag the Label "Announcer" in xaml.
    /// Property CurrentText of this field is the binding base 
    /// for the Content of the Label "Announcer" in xaml
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
      
      this.ticTacToeBox.GameEnds += this.OnGameEnds;
      this.ticTacToeBox.ChangeTurn += this.AdjustAnnouncerTxt_OnChangeTurn;

      this.Reset();
    }

    /// <summary> Issues all needed steps to reset a game </summary>
    /// <param name="sender"> Not relevant </param>
    /// <param name="e"> Not relevant </param>
    public void ResetBtn_OnClick(object sender, RoutedEventArgs e)
    {
      this.Reset();
    }

    /// <summary> 
    /// Issues reset of tic tac box control and 
    /// let the announcer label show that 1. player is to make their turn
    /// </summary>    
    private void Reset()    
    {
      this.ticTacToeBox.Reset();
      this.AnnouncerTxt.CurrentGameState = GameState.TurnPlayerOne;
    }

    /// <summary> 
    /// Event handler: Updates the winning/draw states of played parties of the games 
    /// on the game score. Shows the players text via the announcer label 
    /// the outcome of the last party
    /// </summary>
    /// <param name="endResult"> 
    /// State as an outcome of the last party. Possible outcomes are a draw or who player won
    /// </param>
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
    /// Updates the receptive text of the current state of the game party.
    /// This receptive text is shown via the announcer label to the player 
    /// </summary>
    /// <param name="currentState"> Current state of game party like who is next </param>
    public void AdjustAnnouncerTxt_OnChangeTurn(GameState currentState)
     => this.AnnouncerTxt.CurrentGameState = currentState;

    private readonly TicTacToeBoxControl ticTacToeBox;

    private readonly GameScore scoreBoard;
  }
}
