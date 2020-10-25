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
    public enum GameState
    {
        TurnPlayerOne,
        TurnPlayerTwo,
        PlayerOneWins,
        PlayerTwoWins,
        Draw
    }

    

    /// <summary>
    /// Interaction logic for TicTacToeBoxControl.xaml. Represents a Tic Tac Toe grid
    /// which can be used by 2 players. It also manages the state of the game, whose turn is
    /// when and who wins or in which case a draw occurs.
    /// </summary>
    public partial class TicTacToeBoxControl : UserControl
    {
        private readonly Button[] playFields = new Button[9];

        private GameState stateOfGame = GameState.TurnPlayerOne;
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
                if (possibleWrapper is WrapPanel wrap )
                {
                    foreach (UIElement controlElement in wrap.Children)
                    {
                        if (controlElement is Button playFieldBtn)
                        {
                            Button playField = playFields[count++];
                            playField = playFieldBtn;
                            playField.Tag = count;
                            playField.Click += PlayField_Click;
                        }
                    }
                }
            }           
        }

        /// <summary> 
        /// Puts symbol in the play box depending on whose turn is and removes the click event.
        /// Cross symbol represents player one Circle represents player two.
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

                } else if (this.stateOfGame == GameState.TurnPlayerTwo)
                {
                    playBox.Content = new Circle();
                    this.stateOfGame = GameState.TurnPlayerOne;
                }

                // No need to listen to the event anymore. Play field can be selected only once by one player.
                playBox.Click -= PlayField_Click;
                e.Handled = true;
            }

            
        }


    }


}
