using System;
using System.Diagnostics;
using System.Collections.Generic;
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
using System.Xml;

namespace TicTacToeControl
{
  /// <summary> All possible stages of the tic tac toe game and outcomes</summary>
  public enum GameState
  {
    /// <summary> it is the turn of 1. player now </summary>
    TurnPlayerOne,
    /// <summary> it is the turn of 2. player now </summary>
    TurnPlayerTwo,
    /// <summary> 1. player has won </summary>
    PlayerOneWins,
    /// <summary> 2. player has won </summary>
    PlayerTwoWins,
    /// <summary> No one has won. A draw occurred </summary>
    Draw
  }

  /// <summary>
  /// Interaction logic for TicTacToeBoxControl.xaml. Represents a Tic Tac Toe grid
  /// which can be used by 2 players. It also manages the state of the game, 
  /// whose turn is when and who wins or in which case a draw occurs.
  /// </summary>
  public partial class TicTacToeBoxControl : UserControl
  {
    /// <summary> 
    /// Handler for changing the state of tic tac toe game.   
    /// </summary>
    public delegate void EndGameHandler(GameState gameResult);

    /// <summary> 
    /// Invokes if the tic toe game is decided by a draw or 1. or 2. player wins    
    /// </summary>
    public event EndGameHandler OnGameEnds;

    private readonly Button[] playFields = new Button[9];

    private GameState stateOfGame;

    private GameState StateOfGame
    {
      get => this.stateOfGame;
      set
      {
        this.stateOfGame = value;

        if (
          value == GameState.Draw || 
          value == GameState.PlayerOneWins || 
          value == GameState.PlayerTwoWins
          )
        {
          this?.OnGameEnds.Invoke(value);
        }
      }
    }

    public TicTacToeBoxControl()
    {
      InitializeComponent();

      // Get References of all playFields to add click events to them.
      var rootElement = LogicalTreeHelper.GetChildren(this).GetEnumerator();
      rootElement.MoveNext();
      var mainStack = rootElement.Current as StackPanel;

      int count = 0;

      foreach (UIElement possibleWrapper in mainStack.Children)
      {
        if (possibleWrapper is WrapPanel wrap)
        {
          foreach (UIElement controlElement in wrap.Children)
          {
            if (controlElement is Button playFieldBtn)
            {                            
              playFieldBtn.Tag = count;
              playFieldBtn.Click += PlayField_Click;
              playFields[count++] = playFieldBtn;
            }
          }
        }
      }
    }

    private static int setPlayFiels = 0;

    /// <summary> 
    /// Puts symbol in the play box depending on whose turn is and 
    /// removes the click event. Cross symbol 
    /// represents player one Circle represents player two.
    /// </summary>
    /// <param name="sender"> sender as a button control as play box </param>
    public void PlayField_Click(object sender, RoutedEventArgs e)
    {
      if (sender is Button playBox)
      {
        if (this.stateOfGame == GameState.TurnPlayerOne)
        {
          playBox.Content = new Cross();
          this.stateOfGame = GameState.TurnPlayerTwo;

        }
        else if (this.stateOfGame == GameState.TurnPlayerTwo)
        {
          playBox.Content = new Circle();
          this.stateOfGame = GameState.TurnPlayerOne;
        }

        // No need to listen to the event anymore. 
        // Play field can be selected only once by one player.
        playBox.Click -= PlayField_Click;
        e.Handled = true;

        if (setPlayFiels++ == this.playFields.Length)
        {
          this.StateOfGame = GameState.Draw;
        }
      }
    }

    /// <summary> 
    /// Resets the state of this control.
    /// Empties all fields in this control and attaches all click events back 
    /// </summary>    
    public void Reset()
    {

      foreach (Button playField in playFields)
      {
        if (playField != null)
        {
          playField.Content = null;
          playField.Click += this.PlayField_Click;
        }
      }

      setPlayFiels = 0;
      this.StateOfGame = GameState.TurnPlayerOne;
    }


  }


}
