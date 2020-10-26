using System;
using System.Collections.Generic;
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
    private readonly TicTacToeBoxControl playField;

    private readonly GameScore scoreBoard;

    private readonly Label GameResultAnnouncement;

    public MainWindow()
    {
      InitializeComponent();

      // Getting named xaml element
      this.playField = this.TicTacToeGird;
      this.scoreBoard = this.GameScoreBoard;
      this.GameResultAnnouncement = this.EndAnnouncement;
    }

    /// <summary> Empties the all fields of the tic tac toe box </summary>
    public void ResetBtn_OnClick(object sender, RoutedEventArgs e) 
      => this.playField.Reset();

  }
}
