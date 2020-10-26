using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    private readonly TicTacToeBoxControl ticTacToeBox;

    private readonly GameScore scoreBoard;

    private readonly Label GameResultAnnouncement;

    public MainWindow()
    {
      InitializeComponent();

      // Getting named xaml element
      this.ticTacToeBox = this.TicTacToeGird;
      this.scoreBoard = this.GameScoreBoard;
      this.GameResultAnnouncement = this.EndAnnouncement;
      this.ticTacToeBox.OnGameEnds += this.ReactOnGameEnds;
      
      
    }

    /// <summary> Empties the all fields of the tic tac toe box </summary>
    public void ResetBtn_OnClick(object sender, RoutedEventArgs e)
    {
      this.ticTacToeBox.Reset();
      this.GameResultAnnouncement.Visibility = Visibility.Hidden;
    }


    public void ReactOnGameEnds(GameState endResult)
    {
      switch (endResult)
      {
        case GameState.Draw:
          this.scoreBoard.GameScoreData.Draws++;
          this.ShowGameResult("Draw !!!");
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
    }

    public void ShowGameResult(string result)
    {
      this.GameResultAnnouncement.Content = result;
      this.GameResultAnnouncement.Visibility = Visibility.Visible;
    }
  }
}
