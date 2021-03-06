﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;



namespace TicTacToeControl.Control
{
  /// <summary>
  /// Interaction logic for PlayerCircle.xaml. View is an ellipse looking like
  /// a circle. It provides the possibility to change the color the this circle
  /// </summary>
  public partial class Circle : UserControl
  {
    // Reference to the circle shape to get access to the color
    private Ellipse circleShape;

    /// <summary> Provided to change color of the circle </summary>
    /// <value> Get/Set for circleShape.Stroke as Brush property </value>
    public Brush StrokeColor
    {
      get => this.circleShape.Stroke;
      set
      {
        this.circleShape.Stroke = value;
      }
    }

    public Circle()
    {
      InitializeComponent();
    }

#nullable enable

    // Just for getting the reference of the circle shape.
    private void Circle_Initialized(object? sender, EventArgs e)
    {
      this.circleShape = sender as Ellipse;
    }

  }
}
