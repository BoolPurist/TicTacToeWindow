using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TicTacToeControl
{
  public class GameScoreModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public string Player1WinsBind
    {
      get => this.player1WinsBind;
      set
      {
        this.player1WinsBind = value;
        this.OnPropertyChanged(nameof(this.Player1WinsBind));
      }
    }

    public string Player2WinsBind
    {
      get => this.player2WinsBind;
      set
      {
        this.player2WinsBind = value;
        this.OnPropertyChanged(nameof(this.Player2WinsBind));
      }
    }

    public string DrawsBind
    {
      get => this.drawsBind;
      set
      {
        this.drawsBind = value;
        this.OnPropertyChanged(nameof(this.DrawsBind));
      }
    }

    public int Draws 
    {
      get => this.draws;
      set 
      {
        this.draws = value;
        this.DrawsBind = $"{this.DrawsTxt}{value}";
      }
    }
   
    public int Player1Wins
    {
      get => this.player1Wins;
      set
      {
        this.player1Wins = value;
        this.Player1WinsBind = $"{this.Player1WinsTxt}{value}";
      }
    }

    public int Player2Wins
    {
      get => this.player2Wins;
      set
      {
        this.player2Wins = value;
        this.Player2WinsBind = $"{this.DrawsTxt}{value}";
      }
    }

    public string Player1WinsTxt { get; set; }

    public string Player2WinsTxt { get; set; }

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
