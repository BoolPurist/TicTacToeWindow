using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToeControl.Control
{

  /// <summary>
  /// Interaction logic for UserControl1.xaml. View are 2 lines forming a cross.
  /// It provides the possibility to change the color the this cross
  /// </summary>
  public partial class Cross : UserControl
  {
    public Cross()
    {
      this.InitializeComponent();      
    }
    // Array for the references to the lines making of the cross
    private readonly Line[] lines = new Line[2];

    /// <summary> Provided to change color of the cross </summary>
    /// <value> Get/Set for lines.Stroke as Brush property </value>
    public Brush StrokeColor
    {
      get => this.lines[0].Stroke;
      set
      {
        foreach(Line line in this.lines)
        {
          line.Stroke = value;
        }
      }
    }

#nullable enable

    // Used to get the references to the 2 lines as children from the main grid.
    private void Grid_Initialized(object? sender, EventArgs e)
    {
      if (sender is Grid grid)
      {
        var index = 0;
        foreach (Line? line in grid.Children)
        {
          if (line != null)
          {
            lines[index++] = line;
          }
        }
      }

    }


  }
}