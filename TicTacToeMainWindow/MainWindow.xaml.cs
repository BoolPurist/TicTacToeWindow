using System;
using System.Windows;
using TicTacToeControl;
using TicTacToeControl.Control;


namespace TicTacToeMainWindow
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

#nullable enable

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
      
      this.AnnouncerTxt.CurrentGameState = GameState.TurnPlayerOne;
      
      this.ticTacToeBox.GameEnds += this.OnGameEnds;
      this.ticTacToeBox.ChangeTurn += this.AdjustAnnouncerTxt_OnChangeTurn;
      this.ContentRendered += SetMinSize;


      void SetMinSize(object? sender, EventArgs e)
      {
        this.MinWidth = this.ActualWidth;
        this.MinHeight = this.ActualHeight;
      }
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

    #region event handlers 

    // Issues all needed steps to reset a game 
    private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
    {
      this.Reset();
    }

    // It is added to tic tac toe box control which invokes the event handler if a game ends
    // on the game score. Invocation increments draw counter or win counter of the receptive 
    // player on the game score control. It also shows the outcome of the game by changing 
    // the text of the label "Announcer" in xaml.
    private void OnGameEnds(GameState endResult)
    {
      this.AnnouncerTxt.CurrentGameState = endResult;

      switch (endResult)
      {
        case GameState.Draw:
          this.scoreBoard.GameScoreData.Draws++;          
          break;
        case GameState.PlayerOneWins:
          this.scoreBoard.GameScoreData.Player1Wins++;
          break;
        case GameState.PlayerTwoWins:
          this.scoreBoard.GameScoreData.Player2Wins++;
          break;
        default:
          throw new NoValidGameStateException(
            $"{endResult} should not appear as end result of a game"
            );          
      }

      this.AnnouncerTxt.CurrentGameState = endResult;
                  
    }

    #endregion

    // Updates the receptive text of the current state of the game party.
    // This receptive text is shown via the announcer label to the player 
    // parameter currentStat: Current state of game for example who is next to make a turn
    private void AdjustAnnouncerTxt_OnChangeTurn(GameState currentState)
     => this.AnnouncerTxt.CurrentGameState = currentState;

    private readonly TicTacToeBox ticTacToeBox;

    private readonly GameScore scoreBoard;
  }
}
