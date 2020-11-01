using System.ComponentModel;

namespace TicTacToeControl.Model
{
  public class GameScoreModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary> Binding: Represents how often the 1. player won </summary>
    /// <value> get/set of this.player1WinsBind </value>
    public string Player1WinsBind
    {
      get => this.player1WinsBind;
      set
      {
        this.player1WinsBind = value;
        this.OnPropertyChanged(nameof(this.Player1WinsBind));
      }
    }

    /// <summary> Binding: Represents how often the 2. player won </summary>
    /// <value> get/set of this.player2WinsBind </value>
    public string Player2WinsBind
    {
      get => this.player2WinsBind;
      set
      {
        this.player2WinsBind = value;
        this.OnPropertyChanged(nameof(this.Player2WinsBind));
      }
    }


    /// <summary> Binding: Represents how often a draw has occurred </summary>
    /// <value> get/set of this.drawsBind </value>
    public string DrawsBind
    {
      get => this.drawsBind;
      set
      {
        this.drawsBind = value;
        this.OnPropertyChanged(nameof(this.DrawsBind));
      }
    }

    /// <summary> Count of how often a draw has occurred. </summary>
    /// <value> 
    /// Get/set of this.draws. If set, it sets the property DrawsBind too
    /// DrawsBind = DrawsTxt + Draws as string concatenation.
    /// </value>
    public int Draws 
    {
      get => this.draws;
      set 
      {
        this.draws = value;
        this.DrawsBind = $"{this.DrawsTxt} {value}";
      }
    }

    /// <summary> Count of how often 1. player has won. </summary>
    /// <value> 
    /// Get/set of this.draws. If set, it sets the property Player1WinsBind too
    /// Player1WinsBind = Player1WinsTxt + Player1Wins as string concatenation.
    /// </value>
    public int Player1Wins
    {
      get => this.player1Wins;
      set
      {
        this.player1Wins = value;
        this.Player1WinsBind = $"{this.Player1WinsTxt} {value}";
      }
    }

    /// <summary> Count of how often 2. player has won </summary>
    /// <value> 
    /// Get/set of this.draws. If set, it sets the property Player2WinsBind too
    /// Player2WinsBind = Player2WinsTxt + Player2Wins as string concatenation.
    /// </value>
    public int Player2Wins
    {
      get => this.player2Wins;
      set
      {
        this.player2Wins = value;
        this.Player2WinsBind = $"{this.Player2WinsTxt} {value}";
      }
    }


    /// <summary> Text which serves as template for binding property Player1WinsBind </summary>
    /// <value> Get/Setter of auto implementation </value>
    public string Player1WinsTxt { get; set; }

    /// <summary> Text which serves as template for binding property Player2WinsBind </summary>
    /// <value> Get/Setter of auto implementation </value>
    public string Player2WinsTxt { get; set; }

    /// <summary> Text which serves as template for binding property DrawsBind </summary>
    /// <value> Get/Setter of auto implementation </value>
    public string DrawsTxt { get; set; }


    private string player1WinsBind;

    private string player2WinsBind;

    private string drawsBind;

    private int player1Wins;

    private int player2Wins;

    private int draws;


    protected void OnPropertyChanged(string paramName)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(paramName));
    }
  }
}
