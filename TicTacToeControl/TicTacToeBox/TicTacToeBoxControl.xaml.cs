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
    /// If a win is detected the 3 symbols for the line are colored based on this property 
    /// </summary>
    /// <value> Get/set auto implementation with a initialized value </value>
    public static Brush WinnerColor { get; set; } = new SolidColorBrush(Colors.Green);

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

    public TicTacToeBoxControl()
    {
      InitializeComponent();
      this.logicalGrid = new TicTacToeModel();
      this.StateOfGame = GameState.TurnPlayerOne;
      this.playFields = new Button[9];
    }

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
#if DEBUG
          // Places the respective field number into each empty field.
          playField.Content = playField.Tag as string;
#else
          playField.Content = null;
#endif
          playField.Click -= this.PlayField_OnClick;
          playField.Click += this.PlayField_OnClick;
        }
      }

      this.logicalGrid.Reset();
      this.StateOfGame = GameState.TurnPlayerOne;

#if DEBUG
      DebugResetTurns();
#endif
    }

    // State of the current game party. 
    // (which player is next or game ended in draw or in a win of a player) 
    // If set, checks if state is final outcome of current game party (draw or win)
    // If a final state is confirmed it invokes all registers callbacks on the event GameEnds
    private GameState StateOfGame
    {
      get => this.stateOfGame;
      set
      {
        this.stateOfGame = value;

#if DEBUG
        DebugPrintCurrentTurn(this.logicalGrid.LastTakeFieldNbr, value);
#endif
        bool hasWon = value == GameState.PlayerOneWins || value == GameState.PlayerTwoWins;

        // If a player won, the symbols which lead to victory, will colored extra.
        if (hasWon)
        {
          this.MarkWinnerLine();
        }

        if ( value == GameState.Draw || hasWon )
        {

          this.Freeze();
          this.GameEnds?.Invoke(value);

#if DEBUG
          this.DebugPrintGameEnd();
#endif

        }
        else
        {
          this.ChangeTurn?.Invoke(value);
        }
      }
    }

    // Is invoked when game ends.
    // Used to remove the rest of attached click events from any play fields so new turns 
    // are not processed and prevents putting new symbol on any left empty play fields 
    // until Reset method is invoked. 
    private void Freeze()
    {
      foreach (Button playField in this.playFields)
      {
        playField.Click -= this.PlayField_OnClick;
      }
    }

    // Marks the symbols which resulted in a victory
    private void MarkWinnerLine()
    {
      foreach (int winFieldNbr in this.logicalGrid.WinSequence)
      {
        var currentPlayField = this.playFields[winFieldNbr].Content;
        if (currentPlayField is Cross cross)
        {
          cross.StrokeColor = WinnerColor;
        }
        else if (currentPlayField is Circle cirle)
        {
          cirle.StrokeColor = WinnerColor;
        }
      }
    }

    #region event handler

    // Puts symbol in the play box depending on whose turn is and 
    // removes the click event. Cross symbol 
    // represents player one Circle represents player two.
    // parameter sender as a play box 
    private void PlayField_OnClick(object sender, RoutedEventArgs e)
    {

      if (sender is Button playBox)
      {

        int selectedFieldNbr = int.Parse(playBox.Tag as string);

        playBox.Content = this.StateOfGame == GameState.TurnPlayerOne ?
          new Cross() as object : new Circle() as object;

        // No need to listen to the event anymore. 
        // Play field can be selected only once by one player.
        playBox.Click -= PlayField_OnClick;

        e.Handled = true;

        // Get the state for the next turn as a result of current turn.
        this.logicalGrid.MakeTurn(selectedFieldNbr);
        this.StateOfGame = this.logicalGrid.CurrentState;

      }
    }

    /// Loads all play fields in a array field for later manipulation, 
    /// give numbered tags and attaches click events. 
    /// In a play field a cross or circle can be inserted or removed after reset.
    /// parameter sender: play field for the tic tac toe field   
    private void PlayField_Loaded(object sender, RoutedEventArgs e)
    {
      // If for any reason the xaml is reloaded this counter will be reset to zero.
      this._playFieldInitIndex %= this.playFields.Length;

      if (sender is Button playFieldBtn)
      {
        playFieldBtn.Tag = _playFieldInitIndex.ToString();

#if DEBUG
        playFieldBtn.Content = _playFieldInitIndex;
#endif
        this.playFields[_playFieldInitIndex++] = playFieldBtn;
        playFieldBtn.Click += PlayField_OnClick;
      }      
    }

    #endregion

    // Local variable of method of PlayField_Loaded.
    private int _playFieldInitIndex = 0;


    private readonly Button[] playFields;

    private GameState stateOfGame;

    private readonly TicTacToeModel logicalGrid;

    #region debug code
    
#if DEBUG

    private readonly List<int> fieldNumbersDebug = new List<int>();
    private readonly List<GameState> playerTurnsDebug = new List<GameState>();

    private void DebugPrintCurrentTurn(int currentFieldNbr, GameState currentState)
    {
      if (currentFieldNbr != -1)
      {
        // Outputs the taken field number and the player which made the current turn
        Debug.WriteLine($"Field occupied with number: ({currentFieldNbr})");
        Debug.WriteLine($"Next turn will be: ({currentState})");
        this.fieldNumbersDebug.Add(currentFieldNbr);
        this.playerTurnsDebug.Add(currentState);
      }
    }

    private void DebugPrintGameEnd()
    {
      // Prints outs 2 sequences to the trace: Field numbers taken in order
      // Receptive order of which player made a turn

      Debug.WriteLine(
        $"Outcome is of game party ({this.fieldNumbersDebug[fieldNumbersDebug.Count - 1]})"
        );

      var currentDebugLine = new StringBuilder(64);
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
    }

    private void DebugResetTurns()
    {
      this.playerTurnsDebug.Clear();
      this.fieldNumbersDebug.Clear();
    }

    #endif

#endregion

  }

}
