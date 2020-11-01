using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TicTacToeControl.Model;

namespace TicTacToeControl.Control
{
  /// <summary>
  /// Interaction logic for GameScore.xaml
  /// </summary>
  public partial class GameScore : UserControl
  {
    public GameScoreModel GameScoreData;

    public GameScore()
    {
      InitializeComponent();
      
      this.GameScoreData = new GameScoreModel();            
    }

    /// <summary> 
    /// Resets all states on the game score. 
    /// Wins of 1. player and 2. player and draws are set to zero
    /// </summary>
    public void Reset()
    {
      this.GameScoreData.Player1Wins = INTIT_STAT_VALUE;
      this.GameScoreData.Player2Wins = INTIT_STAT_VALUE;
      this.GameScoreData.Draws = INTIT_STAT_VALUE;
    }

    /// Binds the Label to the game score model so later another control/window can update the 
    /// state counter on the game score view
    /// parameter sender: Label from the game score view </param>
    private void StateLabel_OnLoaded(object sender, RoutedEventArgs e)
    {
      if (sender is Label statsLabel)
      {        
        string currentTag = statsLabel.Tag as string;

        DependencyProperty dependencyProperty = Label.ContentProperty;
        GameScoreModel source = this.GameScoreData;        

        if (currentTag == "DrawsBind")
        {
          source.DrawsTxt = statsLabel.Content as string;
          source.Draws = INTIT_STAT_VALUE;
          bindIt(nameof(source.DrawsBind));
        }
        else if (currentTag == "Player1WinsBind")
        {
          this.GameScoreData.Player1WinsTxt = statsLabel.Content as string;
          this.GameScoreData.Player1Wins = INTIT_STAT_VALUE;
          bindIt(nameof(this.GameScoreData.Player1WinsBind));
        }
        else if (currentTag == "Player2WinsBind")
        {
          this.GameScoreData.Player2WinsTxt = statsLabel.Content as string;
          this.GameScoreData.Player2Wins = INTIT_STAT_VALUE;
          bindIt(nameof(this.GameScoreData.Player2WinsBind));
        }

        void bindIt(string path)
        => statsLabel.SetBinding(dependencyProperty, new Binding(path) { Source = source });

      }

    }

    private const int INTIT_STAT_VALUE = 0;

  }
}
