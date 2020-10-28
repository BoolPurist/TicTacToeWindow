using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.CodeDom;

namespace TicTacToeMainWindow
{
  public class AnnouncerTextModel : INotifyPropertyChanged
  {

    public AnnouncerTextModel(string startTxt)
    {
      this.CurrentText = startTxt;
    }

    public AnnouncerTextModel() : this(player1TurnTxt) { }

    public event PropertyChangedEventHandler PropertyChanged;

    public const string player1TurnTxt = "1. Player make your turn please";

    public const string player2TurnTxt = "2. Player make your turn please";

    public const string drawTxt = "Have a draw !";

    public const string player1WinTxt = "1. Player has won";

    public const string player2WinTxt = "2. Player has won";

    private string currentText;

    public string CurrentText
    {
      get => this.currentText;
      set
      {
        this.currentText = value;
        this.OnPropertyChanged(nameof(this.CurrentText));
      }
    }

    
    
    private void OnPropertyChanged(string paramName)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(paramName));
    }
  }
}
