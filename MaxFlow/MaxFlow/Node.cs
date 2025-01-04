using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaxFlow {
    internal class Node : IDisposable {
        private readonly double size;
        private readonly double fontsize;
        private readonly Canvas canvas;
        private readonly Brush defColor = Brushes.Red;
        public Ellipse Ellipse { get; }
        public TextBlock Label { get; } = new TextBlock() { FontWeight = FontWeights.Bold };
        public Point Position { get; private set; } = new Point();
        private MouseButtonEventHandler eventHandler;
        public MouseButtonEventHandler EventHandler
        {
            set {
                if (value != null) {
                    if (eventHandler != null) {
                        Ellipse.MouseRightButtonUp -= eventHandler;
                        Label.MouseRightButtonUp -= eventHandler;
                    }
                    eventHandler = value;
                    Ellipse.MouseRightButtonUp += eventHandler;
                    Label.MouseRightButtonUp += eventHandler;
                }
            }
        }

        public Node(Canvas canvas, string label, Brush defColor = null)
        {
            size = Graph.NodeSize;
            fontsize = Graph.FontSize;
            Label.Text = label;
            this.canvas = canvas;
            if (defColor != null) {
                this.defColor = defColor;
            }
            Ellipse = new Ellipse {
                Width = size,
                Height = size,
                Fill = this.defColor
            };
            Label.FontSize = fontsize;
            canvas.Children.Add(Ellipse);
            canvas.Children.Add(this.Label);
        }

        public void ChangeLabel(string newlabel)
        {
            Label.Text = newlabel;
            Canvas.SetLeft(Label, Position.X + size / 2 - (fontsize / 2.75) * Label.Text.Length);
        }

        public string GetLabel()
        {
            return Label.Text;
        }

        public void SetColor(Brush newcolor)
        {
            Ellipse.Fill = newcolor;
        }

        public void ResetColor()
        {
            Ellipse.Fill = this.defColor;
        }

        public void SetPosition(double x, double y)
        {
            Position = new Point(x, y);
            Canvas.SetLeft(Ellipse, x);
            Canvas.SetTop(Ellipse, y);
            Canvas.SetLeft(Label, x + size / 2 - (fontsize / 2.75) * Label.Text.Length);
            Canvas.SetTop(Label, y + size / 2 - fontsize / 2);
            Panel.SetZIndex(Label, 3);
            Panel.SetZIndex(Ellipse, 2);
        }

        public bool IsInside(Point p)
        {
            double distance = Math.Sqrt(Math.Pow((Position.X + size / 2 - p.X), 2) + Math.Pow((Position.Y + size / 2 - p.Y), 2));
            if (distance < size / 2) {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            canvas.Children.Remove(Label);
            canvas.Children.Remove(Ellipse);
            if (eventHandler != null) {
                Ellipse.MouseRightButtonUp -= eventHandler;
                Label.MouseRightButtonUp -= eventHandler;
                eventHandler = null;
            }
        }
    }
}
