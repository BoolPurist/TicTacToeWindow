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
        #region Property

        public GameScoreModel GameScoreData;

        public int DrawsCounter
        {
            get => this.drawsCounter;
            set
            {
                this.drawsCounter = value;
                this.GameScoreData.Draws = $"{this.drawsText} {this.drawsCounter}";
            }
        }
        public int PlayerOneCounter
        {
            get => this.playerOneCounter;
            set
            {
                this.playerOneCounter = value;
                this.GameScoreData.PlayerOneWins = $"{this.playerOneWinsText} {this.playerOneCounter}";
            }
        }
        public int PlayerTwoCounter
        {
            get => this.playerTwoCounter;
            set
            {
                this.playerTwoCounter = value;
                this.GameScoreData.PlayerTwoWins = $"{this.playerTwoWinsText} {this.playerTwoCounter}";
            }
        }

        #endregion

        #region Fields

        private int drawsCounter;
        private int playerOneCounter;
        private int playerTwoCounter;

        private string playerOneWinsText = "";
        private string playerTwoWinsText = "";
        private string drawsText = "";

        #endregion


        #region Constructing
        public GameScore()
        {
            InitializeComponent();
            this.GameScoreData = new GameScoreModel();
            this.InitialBinding();

            var initStats = 0;
            this.DrawsCounter = initStats;
            this.PlayerOneCounter = initStats;
            this.PlayerTwoCounter = initStats;
        }

        private void InitialBinding()
        {
            Action<Label, GameScoreModel, string> BindScoreLabelToModel = (control, dataProperty, path) 
                => 
            {                
                control.SetBinding(
                Label.ContentProperty,
                new Binding(path)
                {
                    Source = dataProperty,
                    Mode = BindingMode.OneWay,
                }
                );
            };

            var root = LogicalTreeHelper.GetChildren(this).GetEnumerator();
            root.MoveNext();
            var mainPanel = root.Current as Panel;

            foreach (UIElement child in mainPanel.Children)
            {
                if (child is Label scoreLable)
                {
                    if (scoreLable.Tag as string == nameof(this.GameScoreData.Draws))
                    {
                        this.drawsText = scoreLable.Content as string;
                        BindScoreLabelToModel(
                            scoreLable, 
                            this.GameScoreData, 
                            nameof(this.GameScoreData.Draws)
                            );
                    }
                    else if (scoreLable.Tag as string == nameof(this.GameScoreData.PlayerOneWins))
                    {
                        this.playerOneWinsText = scoreLable.Content as string;
                        BindScoreLabelToModel(
                            scoreLable, 
                            this.GameScoreData, 
                            nameof(this.GameScoreData.PlayerOneWins)
                            );
                    }
                    else if (scoreLable.Tag as string == nameof(this.GameScoreData.PlayerTwoWins))
                    {
                        this.playerTwoWinsText = scoreLable.Content as string;
                        BindScoreLabelToModel(
                            scoreLable, 
                            this.GameScoreData, 
                            nameof(this.GameScoreData.PlayerTwoWins)
                            );
                    }
                }
            }

            
        }

        #endregion
    }
}
