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

#if DEBUG
    private readonly List<int> fieldNumbersDebug = new List<int>();
    private readonly List<GameState> playerTurnsDebug = new List<GameState>();
#endif

    /// <summary> 
    /// Handler for changing the state of tic tac toe game.   
    /// </summary>
    public delegate void ChangeGameStateHandler(GameState gameResult);

    /// <summary> 
    /// Invokes if the tic toe game ends by a draw or 1. or 2. player wins    
    /// </summary>
    public event ChangeGameStateHandler GameEnds;
    
    /// <summary> 
    /// Invokes if the tic toe game continues into the next turn    
    /// </summary>
    public event ChangeGameStateHandler ChangeTurn;

    /// <summary> 
    /// State of the current game party. 
    /// (which player is next or game ended in draw or in a win of a player) 
    /// </summary>
    /// <value> 
    /// Get/Set of field stateOfGame. 
    /// If set, checks if state is final outcome of current game party (draw or win)
    /// If a final state is confirmed it invokes all registers callbacks on the event GameEnds
    /// </value>
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

#if DEBUG
          // Prints outs 2 sequences to the trace: Field numbers taken in order
          // Receptive order of which player made a turn

          Debug.WriteLine($"Outcome is of game party ({value})");
          this.playerTurnsDebug.Add(value);
          
          var currentDebugLine = new StringBuilder( 64 );
          currentDebugLine.Append("Field numbers taken: {");
          foreach (int fieldNbr in this.fieldNumbersDebug)
          {
            currentDebugLine.Append($"{fieldNbr}, ");
          }
          currentDebugLine.Remove(currentDebugLine.Length - 2, 2);
          currentDebugLine.Append(" }");
          Debug.WriteLine(currentDebugLine);
          currentDebugLine.Clear();

          currentDebugLine.Append("Made turns: {");
          foreach (GameState playerTurn in this.playerTurnsDebug)
          {
            currentDebugLine.Append($"{playerTurn}, ");
          }
          currentDebugLine.Remove(currentDebugLine.Length - 2, 2);
          currentDebugLine.Append(" }");

          Debug.WriteLine(currentDebugLine);
#endif
          this?.GameEnds.Invoke(value);
        }
      }
    }

    public TicTacToeBoxControl()
    {
      InitializeComponent();
    }

    /// <summary> 
    /// Puts symbol in the play box depending on whose turn is and 
    /// removes the click event. Cross symbol 
    /// represents player one Circle represents player two.
    /// </summary>
    /// <param name="sender"> sender as a button control as play box </param>
    /// <param name="e"> Not relevant </param>
    public void PlayField_OnClick(object sender, RoutedEventArgs e)
    {

      if (sender is Button playBox)
      {

#if DEBUG
        // Outputs the taken field number and the player which made the current turn
        Debug.WriteLine($"Field occupied with number: ({playBox.Tag})");
        Debug.WriteLine($"Turn made by ({this.stateOfGame})");
        this.fieldNumbersDebug.Add(int.Parse(playBox.Tag as string));
        this.playerTurnsDebug.Add(this.stateOfGame);
#endif

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
        playBox.Click -= PlayField_OnClick;
        e.Handled = true;

        if (++this._setPlayFiels == this.playFields.Length)
        {
          this.StateOfGame = GameState.Draw;
#if DEBUG
          this.fieldNumbersDebug.Add(int.Parse(playBox.Tag as string));
#endif
        }
        else
        {
          this.ChangeTurn?.Invoke(this.stateOfGame);
        }


      }      
    }

    // Local variables for method PlayField_OnClick
    private int _setPlayFiels = 0;

    /// <summary> 
    /// Resets the state of this control.
    /// Empties all fields in this control and attaches back all click events to every field 
    /// </summary>    
    public void Reset()
    {
      foreach (Button playField in playFields)
      {
        if (playField != null)
        {
          playField.Content = null;
          playField.Click += this.PlayField_OnClick;
        }
      }

      this._setPlayFiels = 0;
      this.StateOfGame = GameState.TurnPlayerOne;

#if DEBUG
      this.playerTurnsDebug.Clear();
      this.fieldNumbersDebug.Clear();
#endif

    }



    /// <summary> 
    /// Loads all play fields in a array field for later manipulation, 
    /// give numbered tags and attaches click events. 
    /// In a play field a cross or circle can be inserted or removed after reset.
    /// </summary>
    /// <param name="sender"> play field for the tic tac toe field </param>
    /// <param name="e"> Not used </param>
    private void PlayField_Loaded(object sender, RoutedEventArgs e)
    {
      // If for any reason the xaml is reloaded this counter will be reset to zero.
      this._playFieldInitIndex %= this.playFields.Length;

      if (sender is Button playFieldBtn)
      {
        playFieldBtn.Tag = _playFieldInitIndex.ToString();
        this.playFields[_playFieldInitIndex++] = playFieldBtn;
        playFieldBtn.Click += PlayField_OnClick;
      }      
    }

    // Local variable of method of PlayField_Loaded.
    private int _playFieldInitIndex = 0;

    private readonly Button[] playFields = new Button[9];

    private GameState stateOfGame;

  }

}
