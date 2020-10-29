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
    // TODO Comment out methods and some props

    private readonly TicTacToeBoxControl ticTacToeBox;

    private readonly GameScore scoreBoard;

  
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

    /// <summary> Empties the all fields of the tic tac toe box </summary>
    public void ResetBtn_OnClick(object sender, RoutedEventArgs e)
    {
      this.Reset();
    }

    private void Reset()    
    {
      this.ticTacToeBox.Reset();
      this.AnnouncerTxt.CurrentGameState = GameState.TurnPlayerOne;
    }


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

    public void AdjustAnnouncerTxt_OnChangeTurn(GameState currentState)
     => this.AnnouncerTxt.CurrentGameState = currentState;

    //public void ShowGameResult(string result)
    //{
    //  this.AnnouncerLabel.Content = result;

    //}
  }
}
