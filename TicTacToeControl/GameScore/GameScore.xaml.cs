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

namespace TicTacToeControl
{
  /// <summary>
  /// Interaction logic for GameScore.xaml
  /// </summary>
  public partial class GameScore : UserControl
  {


    public GameScoreModel GameScoreData;

    #region Constructing
    public GameScore()
    {
      InitializeComponent();
      
      this.GameScoreData = new GameScoreModel();
      InitialBinding();

      this.Reset();

      void InitialBinding()
      {
        var root = LogicalTreeHelper.GetChildren(this).GetEnumerator();
        root.MoveNext();
        var mainPanel = root.Current as Panel;

        foreach (UIElement child in mainPanel.Children)
        {
          if (child is Label scoreLable)
          {
            if (scoreLable.Tag as string == nameof(this.GameScoreData.DrawsBind))
            {
              this.GameScoreData.DrawsTxt = scoreLable.Content as string;
              BindScoreLabelToModel(
                scoreLable,
                this.GameScoreData,
                nameof(this.GameScoreData.DrawsBind)
                );
            }
            else if (
                scoreLable.Tag as string == nameof(this.GameScoreData.Player1WinsBind)
                )
            {
              this.GameScoreData.Player1WinsTxt = scoreLable.Content as string;
              BindScoreLabelToModel(
                scoreLable,
                this.GameScoreData,
                nameof(this.GameScoreData.Player1WinsBind)
                );
            }
            else if (
                scoreLable.Tag as string == nameof(this.GameScoreData.Player2WinsBind)
                )
            {
              this.GameScoreData.Player2WinsTxt = scoreLable.Content as string;
              BindScoreLabelToModel(
                scoreLable,
                this.GameScoreData,
                nameof(this.GameScoreData.Player2WinsBind)
                );
            }
          }
        }

        static void BindScoreLabelToModel(
          Label label,
          GameScoreModel dataProperty,
          string path
        )
        {
          label.SetBinding(
          Label.ContentProperty,
          new Binding(path)
          {
            Source = dataProperty,
            Mode = BindingMode.OneWay,
          }
          );
        }
      }
    }

    #endregion

    /// <summary> 
    /// Resets all states on the game score. 
    /// Wins of 1. player and 2. player and draws are set to zero
    /// </summary>
    public void Reset()
    {
      var initValue = 0;
      this.GameScoreData.Player1Wins = initValue;
      this.GameScoreData.Player2Wins = initValue;
      this.GameScoreData.Draws = initValue;
    }


  }
}
