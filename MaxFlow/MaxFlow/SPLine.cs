using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaxFlow {
    internal class SPLine : IDisposable {
        private readonly Canvas canvas;
        private PointCollection trianglepoints;
        private readonly Polygon triangle = new Polygon() {
            Stroke = Brushes.Gray,
            StrokeThickness = 1,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeDashCap = PenLineCap.Round,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeLineJoin = PenLineJoin.Round,
            Fill = Brushes.Black
        };
        private readonly double triangleO;
        private int stream_size, used_stream_size = 0;
        private double splineWidth = 3;
        private Point labelCoordinates = new Point();
        private Point[] _points = new Point[2] { new Point(0, 0), new Point(0, 0) };
        private MouseButtonEventHandler eventHandler;
        public MouseButtonEventHandler EventHandler
        {
            set {
                if (value != null) {
                    if (eventHandler != null) {
                        Label.MouseRightButtonUp -= eventHandler;
                    }
                    eventHandler = value;
                    Label.MouseRightButtonUp += eventHandler;
                    ReDraw();
                }
            }
        }
        public Path Spline { get; private set; }
        public TextBlock Label { get; } = new TextBlock() { Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255)) };
        public Node FromNode { get; set; }
        public Node ToNode { get; set; }
        public Point[] Points
        {
            get { return _points; }
            set {
                _points = value;
                ReDraw();
            }
        }
        public int UsedStreamSize
        {
            get { return used_stream_size; }
            set {
                used_stream_size = value;
                Label.Text = used_stream_size.ToString() + " / " + stream_size.ToString();
            }
        }
        public double SplineThickness
        {
            get {
                return splineWidth;
            }
            set {
                splineWidth = value;
                Spline.StrokeThickness = value;
            }
        }

        public SPLine(Canvas canvas, int stream_size)
        {
            this.canvas = canvas;
            this.stream_size = stream_size;
            Label.FontSize = Graph.FontSize;
            triangleO = Graph.TriangleO;
            Draw();
        }

        private Point[] GenControlPoints(Point[] points)
        {
            double distance = Math.Sqrt(Math.Pow(points[0].X - points.Last().X, 2) + Math.Pow(points[0].Y - points.Last().Y, 2));
            double tension = 0.1 + 0.9 * (1 / (1 + Math.Pow(Math.E, 0.02 * (distance - 60))));
            double c_scale = tension / 0.5 * 0.175;
            List<Point> pts = new List<Point> {
                points[0]
            };
            for (int i = 0; i < points.Length - 1; i++) {
                Point pt = points[i];
                Point pt_before = points[Math.Max(i - 1, 0)];
                Point pt_after = points[i + 1];
                Point pt_after2 = points[Math.Min(i + 2, points.Length - 1)];
                pts.Add(new Point(pt.X + c_scale * (pt_after.X - pt_before.X), pt.Y + c_scale * (pt_after.Y - pt_before.Y)));
                pts.Add(new Point(pt_after.X - c_scale * (pt_after2.X - pt.X), pt_after.Y - c_scale * (pt_after2.Y - pt.Y)));
                pts.Add(pt_after);
            }
            return pts.ToArray();
        }

        private void Draw()
        {
            Point[] control_points = GenControlPoints(_points);
            Path path = new Path {
                Stroke = Brushes.Black,
                StrokeEndLineCap = PenLineCap.Triangle,
                StrokeDashCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeThickness = splineWidth
            };
            triangle.Fill = Brushes.Black;
            PathGeometry pathGeometry = new PathGeometry();
            path.Data = pathGeometry;
            PathFigure pathFigure = new PathFigure();
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.StartPoint = control_points[0];
            PathSegmentCollection segments = new PathSegmentCollection();
            pathFigure.Segments = segments;
            PointCollection pointCollection = new PointCollection(control_points.Length - 1);
            for (int i = 1; i < control_points.Length; i++) {
                pointCollection.Add(control_points[i]);
            }
            PolyBezierSegment bezierSegment = new PolyBezierSegment {
                Points = pointCollection
            };
            segments.Add(bezierSegment);
            Spline = path;
            triangle.Points = trianglepoints;
            canvas.Children.Add(Spline);
            canvas.Children.Add(Label);
            canvas.Children.Add(triangle);
            Canvas.SetLeft(Label, labelCoordinates.X);
            Canvas.SetTop(Label, labelCoordinates.Y);
            Panel.SetZIndex(Label, 4);
            Panel.SetZIndex(Spline, 1);
            Panel.SetZIndex(triangle, 2);
            if (eventHandler != null) {
                Spline.MouseRightButtonUp += eventHandler;
            }
        }

        public void ReDraw()
        {
            canvas.Children.Remove(Label);
            canvas.Children.Remove(Spline);
            canvas.Children.Remove(triangle);
            Draw();
        }

        public void Highlight()
        {
            Spline.Stroke = Brushes.Blue;
            triangle.Fill = Brushes.Blue;
        }

        private PointCollection Arrow(Point center, double degree)
        {
            return new PointCollection(new Point[] {
                Rotate2D(center, new Point(center.X - triangleO, center.Y - triangleO), degree),
                Rotate2D(center, new Point(center.X - triangleO, center.Y + triangleO), degree),
                Rotate2D(center, new Point(center.X + triangleO, center.Y), degree)
            });
        }

        public void Snap(List<List<int>> graphm, List<Node> nodes)
        {
            Label.Text = used_stream_size.ToString() + " / " + stream_size.ToString();
            double nodesizehalf = Graph.NodeSize / 2.0;
            List<Point> newarray = new List<Point>();
            Point begining = new Point(FromNode.Position.X + nodesizehalf, FromNode.Position.Y + nodesizehalf);
            Point ending = new Point(ToNode.Position.X + nodesizehalf, ToNode.Position.Y + nodesizehalf);

            double alpha;
            int i = nodes.IndexOf(FromNode), j = nodes.IndexOf(ToNode);
            if (graphm[i][j] != 0 && graphm[j][i] != 0) {
                Point aux_point = Rotate2D(begining, ending, 45);
                double a = aux_point.Y - begining.Y;
                double b = aux_point.X - begining.X;
                double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                double ratio = nodesizehalf / c;
                newarray.Add(new Point(begining.X + b * ratio, begining.Y + a * ratio));
                newarray.Add(Rotate2D(ending, new Point(ending.X - b * ratio - b * (triangleO / c), ending.Y - a * ratio - a * (triangleO / c)), -90));
                labelCoordinates.X = newarray[0].X + (newarray[1].X - newarray[0].X) / 2 - (Label.Text.Length / 3) * Graph.FontSize;
                labelCoordinates.Y = newarray[0].Y + (newarray[1].Y - newarray[0].Y) / 2;
                alpha = Math.Asin((ending.Y - begining.Y) / c) / (Math.PI / 180) * (begining.X > ending.X ? -1 : 1) + (begining.X > ending.X ? 180 : 0);
            }
            else {
                double a = ending.Y - begining.Y;
                double b = ending.X - begining.X;
                double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                double ratio = nodesizehalf / c;
                newarray.Add(new Point(begining.X + b * ratio, begining.Y + a * ratio));
                newarray.Add(new Point(ending.X - b * ratio - b * (triangleO / c), ending.Y - a * ratio - a * (triangleO / c)));
                labelCoordinates.X = begining.X + (ending.X - begining.X) / 2 - (Label.Text.Length / 3) * Graph.FontSize;
                labelCoordinates.Y = begining.Y + (ending.Y - begining.Y) / 2 + triangleO;
                alpha = Math.Asin(a / c) / (Math.PI / 180) * (begining.X > ending.X ? -1 : 1) + (begining.X > ending.X ? 180 : 0);
            }
            trianglepoints = Arrow(newarray.Last(), alpha);

            Points = newarray.ToArray();
        }

        private static Point Rotate2D(Point origo, Point target, double degree)
        {
            double rad = degree * (Math.PI / 180);
            double nX = (target.X - origo.X) * Math.Cos(rad) - (target.Y - origo.Y) * Math.Sin(rad) + origo.X;
            double nY = (target.X - origo.X) * Math.Sin(rad) + (target.Y - origo.Y) * Math.Cos(rad) + origo.Y;
            return new Point(nX, nY);
        }

        public void SetStreamSize(int streamsize)
        {
            stream_size = streamsize;
            Label.Text = used_stream_size.ToString() + " / " + stream_size.ToString();
        }

        public void Dispose()
        {
            canvas.Children.Remove(Label);
            canvas.Children.Remove(Spline);
            canvas.Children.Remove(triangle);
            if (eventHandler != null) {
                Spline.MouseRightButtonUp -= eventHandler;
                Label.MouseRightButtonUp -= eventHandler;
                eventHandler = null;
            }
            Spline = null;
            FromNode = null;
            ToNode = null;
        }
    }
}
