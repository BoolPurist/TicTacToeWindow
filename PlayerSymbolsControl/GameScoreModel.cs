using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TicTacToeControl
{
    public class GameScoreModel : INotifyPropertyChanged
    {
        private string playerOneWins;

        private string playerTwoWins;

        private string draws;

        public event PropertyChangedEventHandler PropertyChanged;

        public string PlayerOneWins
        {
            get => this.playerOneWins;
            set
            {
                playerOneWins = value;
                this.OnPropertyChanged(nameof(this.PlayerOneWins));
            }
        }

        public string PlayerTwoWins
        {
            get => this.playerTwoWins;
            set
            {
                playerTwoWins = value;
                this.OnPropertyChanged(nameof(this.PlayerTwoWins));
            }
        }

        public string Draws
        {
            get => this.draws;
            set
            {
                this.draws = value;
                this.OnPropertyChanged(nameof(this.Draws));
            }
        }

        protected void OnPropertyChanged( string paramName )
        {
            this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs(paramName) );
        }
    }
}
