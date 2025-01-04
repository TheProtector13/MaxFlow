using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaxFlow {
    internal class Graph : IDisposable {
        private readonly List<Point> node_coordinates = new List<Point>(); //koordináták
        private readonly List<string> nodenames = new List<string>(); //elnevezés
        private readonly List<List<int>> graph_matrix = new List<List<int>>(); //matrix
        private readonly List<List<int>> local_flow_paths = new List<List<int>>(); //ideiglenes tárolás a stepbystep algoritmushoz
        private readonly List<Tree[]> local_Trees = new List<Tree[]>(); //ideiglenes tárolás a stepbystep algoritmushoz
        private readonly Canvas canvas;
        private readonly TextBlock warningbox = new TextBlock() { Background = new SolidColorBrush(Color.FromArgb(192, 255, 255, 255)), Foreground = Brushes.Red, Text = "WARNING! --- Node outside the canvas!" };
        private readonly Form1 form;
        private readonly List<Node> nodes = new List<Node>();
        private readonly List<SPLine> splines = new List<SPLine>();
        private readonly int[] capacityMinMax = new int[2] { int.MaxValue, int.MinValue };
        private string filename = string.Empty;
        private string sink, source;
        private Point mouse = new Point(0, 0);
        private Point targetOffset = new Point(0, 0); //távolság a kiválasztott node és az egér között
        private Point[] globalOffset; //ugyan az mint a targetoffset csak az osszes node mozgatasanal
        private Node target = null, sptarget = null; //target - bal click //sptarget - uj sp esetén
        private readonly List<SPLine> targets_sps = new List<SPLine>(); //mozgatás esetén node hoz csatlakozott sp-k
        private object RightTarget = null; //jobb click - bármi lehet
        private Rectangle BGRect = null;
        public static double NodeSize { get; set; } = 30;
        public static double FontSize { get; set; } = 11;
        public static double TriangleO { get; set; } = 4;
        public Point CanvasSize { get; set; } = new Point(600, 400);
        public double LastCalcTime { get; private set; } = 0;
        public bool ControlsEnabled { get; set; } = true;

        public Graph(Canvas canvas, Form1 form, Point canvassize)
        {
            this.canvas = canvas;
            this.form = form;
            CanvasSize = canvassize;
            List<int> row1 = new List<int>() { 0, 2, 0 };
            List<int> row2 = new List<int>() { 3, 0, 2 };
            List<int> row3 = new List<int>() { 0, 2, 0 };
            graph_matrix.Add(row1);
            graph_matrix.Add(row2);
            graph_matrix.Add(row3);
            nodenames.Add("A");
            nodenames.Add("B");
            nodenames.Add("C");
            ResetScale();
            node_coordinates.Add(new Point(NodeSize, NodeSize));
            node_coordinates.Add(new Point(NodeSize * 5, NodeSize));
            node_coordinates.Add(new Point(NodeSize * 3, NodeSize * 4));
            source = "A";
            sink = "C";
            Initialization();
        }

        public Graph(string file, Canvas canvas, Form1 form, Point canvassize)
        {
            filename = file;
            this.canvas = canvas;
            this.form = form;
            CanvasSize = canvassize;
            string content;
            using (FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read)) {
                using (DeflateStream decompstream = new DeflateStream(fileStream, CompressionMode.Decompress)) {
                    using (StreamReader streamReader = new StreamReader(decompstream, true)) {
                        content = streamReader.ReadToEnd();
                        //hibakezelés, olvasási hiba esetén lépjen ki es kis ablakban dobjon hibát hogy nem lehetett olvasni a fájlt ...
                        //ezt magasabb szinten kezeljük, erre ilyen alacsony szinten nem lesz megvalósítás
                    }
                }
            }
            List<string[]> neighborlist = new List<string[]>();
            foreach (string element in content.Split('\n')) {
                neighborlist.Add(element.Split(' '));
            }
            source = neighborlist[0][0];
            sink = neighborlist[0][1];
            NodeSize = int.Parse(neighborlist[1][0]);
            FontSize = int.Parse(neighborlist[1][1]);
            TriangleO = int.Parse(neighborlist[1][2]);
            neighborlist.RemoveAt(0);
            neighborlist.RemoveAt(0);
            neighborlist.RemoveAt(neighborlist.Count - 1);
            int msize = neighborlist.Count;
            foreach (string[] element in neighborlist) {
                nodenames.Add(element[0]);
                List<int> row = new List<int>();
                for (int i = 0; i < msize; i++) {
                    row.Add(0);
                }
                string[] item = element[1].Split('|');
                node_coordinates.Add(new Point(Convert.ToDouble(item[0]), Convert.ToDouble(item[1])));
                for (int i = 2; i < element.Length; i++) {
                    item = element[i].Split('-');
                    row[Convert.ToInt32(item[0])] = Convert.ToInt32(item[1]);
                }
                graph_matrix.Add(row);
            }
            Initialization();
        }

        private void Initialization()
        {
            form.sinklabel.Text = "sink: " + sink;
            form.sourcelabel.Text = "source: " + source;
            Draw();
            canvas.MouseMove += MouseMoveOnCanvas;
            canvas.MouseLeftButtonDown += MouseDownOnCanvas;
            canvas.MouseLeftButtonUp += MouseUpOnCanvas;
            canvas.MouseRightButtonDown += RightMouseDownOnCanvas;
            canvas.MouseRightButtonUp += RightMouseUpOnCanvas;
            canvas.MouseWheel += Zoom;
        }

        private void Zoom(object sender, MouseWheelEventArgs args)
        {
            if (ControlsEnabled) {
                double multiplier = args.Delta > 0 ? 1.2 : 0.8;
                for (int i = 0; i < nodes.Count; i++) {
                    node_coordinates[i] = Scale2D(mouse, nodes[i].Position, multiplier);
                }
                if (NodeSize * multiplier > 15 && NodeSize * multiplier < 300) {
                    NodeSize *= multiplier;
                }
                if (FontSize * multiplier > 5.5 && FontSize * multiplier < 110) {
                    FontSize *= multiplier;
                }
                if (TriangleO * multiplier > 2 && TriangleO * multiplier < 40) {
                    TriangleO *= multiplier;
                }
                Draw();
            }
        }

        private void MouseMoveOnCanvas(object sender, MouseEventArgs args)
        {
            if (ControlsEnabled) {
                mouse = args.GetPosition(canvas);
                if (args.LeftButton == MouseButtonState.Pressed) {
                    if (target != null) {
                        target.SetPosition(mouse.X + targetOffset.X, mouse.Y + targetOffset.Y);
                        foreach (SPLine line in targets_sps) {
                            line.Snap(graph_matrix, nodes);
                        }
                    }
                    else if (globalOffset != null) {
                        for (int i = 0; i < nodes.Count; i++) {
                            nodes[i].SetPosition(mouse.X + globalOffset[i].X, mouse.Y + globalOffset[i].Y);
                        }
                        foreach (SPLine line in splines) {
                            line.Snap(graph_matrix, nodes);
                        }
                    }
                    CheckNodesInside();
                }
            }
        }

        private void MouseDownOnCanvas(object sender, EventArgs args)
        {
            if (ControlsEnabled) {
                GCSettings.LatencyMode = GCLatencyMode.LowLatency;
                foreach (Node n in nodes) {
                    if (n.IsInside(mouse)) {
                        target = n;
                        Point targetcoords = target.Position;
                        targetOffset.X = targetcoords.X - mouse.X;
                        targetOffset.Y = targetcoords.Y - mouse.Y;
                        foreach (SPLine line in splines) {
                            if (line.FromNode == target || line.ToNode == target) {
                                targets_sps.Add(line);
                            }
                        }
                        break;
                    }
                }
                if (target == null) {
                    globalOffset = new Point[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++) {
                        Point targetcoords = nodes[i].Position;
                        globalOffset[i] = new Point(targetcoords.X - mouse.X, targetcoords.Y - mouse.Y);
                    }
                }
            }
        }

        private void MouseUpOnCanvas(object sender, EventArgs args)
        {
            if (ControlsEnabled) {
                if (target != null) {
                    int index = nodes.IndexOf(target);
                    node_coordinates[index] = target.Position;
                    target = null;
                    targets_sps.Clear();
                }
                else {
                    for (int i = 0; i < nodes.Count; i++) {
                        node_coordinates[i] = nodes[i].Position;
                    }
                    globalOffset = null;
                }
                GCSettings.LatencyMode = GCLatencyMode.Interactive;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, false, true);
            }
        }
        private void RightMouseDownOnCanvas(object sender, EventArgs args)
        {
            if (ControlsEnabled) {
                foreach (Node n in nodes) {
                    if (n.IsInside(mouse)) {
                        sptarget = n;
                        break;
                    }
                }
            }
        }

        private void RightMouseUpOnCanvas(object sender, EventArgs args)
        {
            if (sptarget != null) {
                foreach (Node n in nodes) {
                    if (n.IsInside(mouse)) {
                        if (n != sptarget) {
                            NewSPLine(sptarget, n);
                            sptarget = null;
                        }
                        break;
                    }
                }
            }
        }

        private void RightMouseUpOnCanvasBG(object sender, EventArgs args)
        {
            if (ControlsEnabled) {
                System.Drawing.Point p = form.CanvasAbsoluteLocation;
                DpiScale scale = form.DpiScale;
                form.defaContext.Show(p.X + (int)Math.Round(mouse.X * scale.DpiScaleX), p.Y + (int)Math.Round(mouse.Y * scale.DpiScaleY));
            }
        }

        private void MouseUpNode(object sender, EventArgs args)
        {
            if (ControlsEnabled) {
                Node TargetNode;
                if (sender is Ellipse ellipse) {
                    TargetNode = Search(ellipse);
                }
                else {
                    TargetNode = (Node)Search((TextBlock)sender);
                }
                if (TargetNode == sptarget) {
                    System.Drawing.Point p = form.CanvasAbsoluteLocation;
                    DpiScale scale = form.DpiScale;
                    form.nodeContext.Show(p.X + (int)Math.Round(mouse.X * scale.DpiScaleX), p.Y + (int)Math.Round(mouse.Y * scale.DpiScaleY));
                    sptarget = null;
                }
                RightTarget = TargetNode;
            }
        }

        private void MouseUpSPLine(object sender, EventArgs args)
        {
            if (ControlsEnabled) {
                SPLine TargetNode;
                if (sender is System.Windows.Shapes.Path path) {
                    TargetNode = Search(path);
                }
                else {
                    TargetNode = (SPLine)Search((TextBlock)sender);
                }
                if (TargetNode != null) {
                    System.Drawing.Point p = form.CanvasAbsoluteLocation;
                    DpiScale scale = form.DpiScale;
                    form.splineContext.Show(p.X + (int)Math.Round(mouse.X * scale.DpiScaleX), p.Y + (int)Math.Round(mouse.Y * scale.DpiScaleY));
                }
                RightTarget = TargetNode;
            }
        }

        private Node Search(Ellipse child)
        {
            foreach (Node n in nodes) {
                if (n.Ellipse == child) {
                    return n;
                }
            }
            return null;
        }

        private object Search(TextBlock child)
        {
            foreach (Node n in nodes) {
                if (n.Label == child) {
                    return n;
                }
            }
            foreach (SPLine n in splines) {
                if (n.Label == child) {
                    return n;
                }
            }
            return null;
        }

        private SPLine Search(System.Windows.Shapes.Path child)
        {
            foreach (SPLine n in splines) {
                if (n.Spline == child) {
                    return n;
                }
            }
            return null;
        }

        private SPLine Search(Node from, Node to)
        {
            foreach (SPLine n in splines) {
                if (n.FromNode == from && n.ToNode == to) {
                    return n;
                }
            }
            return null;
        }

        public void RenameNode(string _newname)
        {
            if (RightTarget != null) {
                if (RightTarget is Node node) {
                    string newname = _newname.Replace(' ', '_');
                    if (!nodenames.Contains(newname)) {
                        string prev_name = node.GetLabel();
                        node.ChangeLabel(newname);
                        nodenames[nodenames.IndexOf(prev_name)] = newname;
                    }
                    RightTarget = null;
                }
            }
        }

        public void SetSinkNode()
        {
            if (RightTarget != null) {
                if (RightTarget is Node node) {
                    sink = node.GetLabel();
                    form.sinklabel.Text = "sink: " + sink;
                    RightTarget = null;
                }
            }
        }

        public void SetSourceNode()
        {
            if (RightTarget != null) {
                if (RightTarget is Node node) {
                    source = node.GetLabel();
                    form.sourcelabel.Text = "source: " + source;
                    RightTarget = null;
                }
            }
        }

        public void RemoveNode()
        {
            if (RightTarget != null) {
                if (RightTarget is Node node) {
                    string node_name = node.GetLabel();
                    List<IDisposable> toDispose = new List<IDisposable>();
                    nodes.Remove(node);
                    SPLine[] array = splines.ToArray();
                    for (int i = array.Length - 1; i >= 0; i--) {
                        SPLine sp = array[i];
                        if (sp.FromNode == node || sp.ToNode == node) {
                            toDispose.Add(sp);
                            splines.RemoveAt(i);
                        }
                    }
                    toDispose.Add(node);
                    foreach (IDisposable disp in toDispose) {
                        disp.Dispose();
                    }
                    int index = nodenames.IndexOf(node_name);
                    nodenames.RemoveAt(index);
                    graph_matrix.RemoveAt(index);
                    foreach (List<int> row in graph_matrix) {
                        row.RemoveAt(index);
                    }
                    node_coordinates.RemoveAt(index);
                    RightTarget = null;
                }
            }
        }

        public void RemoveSPLine()
        {
            if (RightTarget != null) {
                if (RightTarget is SPLine spl) {
                    int i, j;
                    i = nodes.IndexOf(spl.FromNode);
                    j = nodes.IndexOf(spl.ToNode);
                    splines.Remove(spl);
                    spl.Dispose();
                    graph_matrix[i][j] = 0;
                    RightTarget = null;
                    foreach (SPLine element in splines) {
                        element.Snap(graph_matrix, nodes);
                    }
                }
            }
        }

        public void SetStreamSizeOfSPLine(int newsize)
        {
            if (RightTarget != null) {
                if (RightTarget is SPLine spl) {
                    int i, j;
                    i = nodes.IndexOf(spl.FromNode);
                    j = nodes.IndexOf(spl.ToNode);
                    spl.SetStreamSize(newsize);
                    graph_matrix[i][j] = newsize;
                    Draw();
                    RightTarget = null;
                }
            }
        }

        public void NewNode()
        {
            string name = "" + (nodenames.Count != 0 ? Convert.ToChar(Encoding.ASCII.GetBytes(nodenames.Last()).Last() + 1) : 'A');
            for (int i = 0; i < nodenames.Count; i++) {
                if (nodenames[i].Equals(name)) {
                    name = "" + Convert.ToChar(Encoding.ASCII.GetBytes(name).Last() + 1);
                    i = -1;
                }
            }
            Node n = new Node(canvas, name) {
                EventHandler = MouseUpNode
            };
            n.SetPosition(mouse.X, mouse.Y);
            nodes.Add(n);
            nodenames.Add(name);
            node_coordinates.Add(new Point(mouse.X, mouse.Y));
            foreach (List<int> row in graph_matrix) {
                row.Add(0);
            }
            List<int> newrow = new List<int>();
            for (int i = 0; i <= graph_matrix.Count; i++) {
                newrow.Add(0);
            }
            graph_matrix.Add(newrow);
        }

        private void NewSPLine(Node from, Node to)
        {
            if (graph_matrix[nodes.IndexOf(from)][nodes.IndexOf(to)] == 0) {
                graph_matrix[nodes.IndexOf(from)][nodes.IndexOf(to)] = 1;
                Draw();
            }
        }

        public void Save(bool saveas = false)
        {
            if (filename == string.Empty || saveas) {
                System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog() {
                    AddExtension = true,
                    CheckPathExists = true,
                    DefaultExt = "gbin",
                    DereferenceLinks = true,
                    Filter = "GBIN files (*.gbin)|*.gbin",
                    OverwritePrompt = true
                };
                if (dialog.ShowDialog(form) == System.Windows.Forms.DialogResult.OK) {
                    filename = dialog.FileName;
                }
                else {
                    return;
                }
            }
            try {
                using (FileStream fileStream = File.Open(filename, FileMode.Create, FileAccess.Write)) {
                    using (DeflateStream compstream = new DeflateStream(fileStream, CompressionLevel.Optimal)) {
                        using (StreamWriter writer = new StreamWriter(compstream, Encoding.UTF8)) {
                            writer.NewLine = "\n";
                            writer.WriteLine(source + " " + sink);
                            writer.WriteLine((int)Math.Round(NodeSize) + " " + (int)Math.Round(FontSize) + " " + (int)Math.Round(TriangleO));
                            int j = 0;
                            foreach (List<int> element in graph_matrix) {
                                writer.Write(nodenames[j] + " " + (int)node_coordinates[j].X + "|" + (int)node_coordinates[j].Y);
                                j++;
                                for (int i = 0; i < element.Count; i++) {
                                    if (element[i] != 0) {
                                        writer.Write(" " + i + "-" + element[i]);
                                    }
                                }
                                writer.Write('\n');
                                writer.Flush();
                            }
                        }
                    }
                }
            }
            catch (SystemException err) {
                MessageBox.Show("An error occurred during the save process:\n\"" + err.Message + "\"\nThe save is incomplete!", "Saving Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void Draw()
        {
            GCSettings.LatencyMode = GCLatencyMode.Interactive;
            canvas.Children.Clear();
            nodes.Clear();
            splines.Clear();
            target = null;
            targets_sps.Clear();
            form.CPath_box.ResetText();
            SetCapacityMinMax();
            //background:
            if (BGRect != null) {
                BGRect.MouseRightButtonUp -= RightMouseUpOnCanvasBG;
            }
            BGRect = new Rectangle() { Width = CanvasSize.X, Height = CanvasSize.Y, Fill = Brushes.White };
            BGRect.MouseRightButtonUp += RightMouseUpOnCanvasBG;
            canvas.Children.Add(BGRect);
            //nodes:
            for (int i = 0; i < nodenames.Count; i++) {
                Node n = new Node(canvas, nodenames[i]) {
                    EventHandler = MouseUpNode
                };
                n.SetPosition(node_coordinates[i].X, node_coordinates[i].Y);
                nodes.Add(n);
            }
            //splines:
            for (int i = 0; i < graph_matrix.Count; i++) {
                for (int j = 0; j < graph_matrix[i].Count; j++) {
                    if (graph_matrix[i][j] != 0) {
                        SPLine tmp = new SPLine(canvas, graph_matrix[i][j]) {
                            FromNode = nodes[i],
                            ToNode = nodes[j],
                            SplineThickness = GetThicknessValue(graph_matrix[i][j]),
                            EventHandler = MouseUpSPLine
                        };
                        splines.Add(tmp);
                    }
                }
            }
            foreach (SPLine element in splines) {
                element.Snap(graph_matrix, nodes);
            }
            CheckNodesInside();
        }

        public void BGRectResize()
        {
            BGRect.Width = CanvasSize.X;
            BGRect.Height = CanvasSize.Y;
            CheckNodesInside();
        }

        private void CheckNodesInside()
        {
            bool warn = false;
            foreach (Node x in nodes) {
                DpiScale scale = form.DpiScale;
                if (x.Position.X < 0 || x.Position.Y < 0 || x.Position.X + NodeSize > CanvasSize.X / scale.DpiScaleX || x.Position.Y + NodeSize > CanvasSize.Y / scale.DpiScaleY) {
                    warn = true;
                    break;
                }
            }
            if (warn) {
                if (!canvas.Children.Contains(warningbox)) {
                    canvas.Children.Add(warningbox);
                    Canvas.SetBottom(warningbox, 0);
                    Canvas.SetLeft(warningbox, 0);
                    Panel.SetZIndex(warningbox, 10);
                }
            }
            else {
                if (canvas.Children.Contains(warningbox)) {
                    canvas.Children.Remove(warningbox);
                }
            }
        }

        private static Point Scale2D(Point origo, Point target, double amount)
        {
            double nX = (target.X - origo.X) * amount + origo.X;
            double nY = (target.Y - origo.Y) * amount + origo.Y;
            return new Point(nX, nY);
        }

        private void SetCapacityMinMax()
        {
            capacityMinMax[0] = int.MaxValue;
            capacityMinMax[1] = int.MinValue;
            for (int i = 0; i < graph_matrix.Count; i++) {
                for (int j = 0; j < graph_matrix[i].Count; j++) {
                    if (graph_matrix[i][j] < capacityMinMax[0] && graph_matrix[i][j] > 0) {
                        capacityMinMax[0] = graph_matrix[i][j];
                    }
                    if (graph_matrix[i][j] > capacityMinMax[1]) {
                        capacityMinMax[1] = graph_matrix[i][j];
                    }
                }
            }
        }

        private double GetThicknessValue(int capacity)
        {
            double modifier = TriangleO / (capacityMinMax[1] - capacityMinMax[0] + 1);
            return (capacity - capacityMinMax[0]) * modifier + 2;
        }

        public static void ResetScale()
        {
            NodeSize = 30;
            FontSize = 11;
            TriangleO = 4;
        }

        private void SetStreamValues(int[,] residGraph)
        {
            Draw();
            int length = residGraph.GetLength(0);
            SPLine sP;
            for (int i = 0; i < length; i++) {
                for (int j = 0; j < length; j++) {
                    int nvalue = graph_matrix[i][j] - residGraph[i, j];
                    if (nvalue > 0) {
                        if ((sP = Search(nodes[i], nodes[j])) != null) {
                            sP.UsedStreamSize = nvalue;
                        }
                    }
                }
            }
            foreach (List<int> item in local_flow_paths) {
                int[] element = item.ToArray();
                string text = nodenames[element[0]];
                for (int i = 1; i < element.Length - 1; i++) {
                    text += " -> " + nodenames[element[i]];
                }
                text += " | amount:" + element[element.Length - 1];
                form.CPath_box.AppendText(text + "\r\n");
            }
        }

        public int SetStreamValues_StepByStep(int index)
        {
            foreach (SPLine spl in splines) {
                spl.ReDraw();
            }
            SPLine sP;
            int[] element = local_flow_paths[index].ToArray();
            string text = nodenames[element[0]];
            for (int i = 1; i < element.Length - 1; i++) {
                text += " -> " + nodenames[element[i]];
                if ((sP = Search(nodes[element[i - 1]], nodes[element[i]])) != null) {
                    sP.UsedStreamSize += element[element.Length - 1];
                    sP.Highlight();
                }
            }
            text += " | amount:" + element[element.Length - 1];
            form.CPath_box.AppendText(text + "\r\n");
            if (local_Trees.Count > 0) {
                for (int i = 0; i < nodes.Count; i++) {
                    Node node = nodes[i];
                    node.ResetColor();
                    if (local_Trees[index][i] != Tree.Free) {
                        node.SetColor(local_Trees[index][i] == Tree.T ? Brushes.DodgerBlue : Brushes.Orange);
                    }
                }
            }
            return element[element.Length - 1];
        }

        public int GetLocalFlowPathsSize()
        {
            return local_flow_paths.Count;
        }

        private bool DFS(int[,] residGraph, int[] parent, Tree[] tree, int s, int t)
        {
            BitArray visited = new BitArray(nodenames.Count, false);
            Stack<int> stack = new Stack<int>();
            stack.Push(s);
            parent[s] = -1;
            while (stack.Count > 0) {
                int u = stack.Pop();
                if (!visited.Get(u)) {
                    visited.Set(u, true);
                    for (int i = 0; i < nodenames.Count; i++) {
                        if (visited.Get(i) == false && residGraph[u, i] > 0) {
                            parent[i] = u;
                            if (i == t) {
                                return true;
                            }
                            tree[i] = Tree.S;
                            stack.Push(i);
                        }
                    }
                }
            }
            return false;
        }

        private bool BFS(int[,] residGraph, int[] parent, Tree[] tree, int s, int t)
        {
            BitArray visited = new BitArray(nodenames.Count, false);
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            visited.Set(s, true);
            parent[s] = -1;
            while (queue.Count > 0) {
                int u = queue.Dequeue();
                for (int i = 0; i < nodenames.Count; i++) {
                    if (visited.Get(i) == false && residGraph[u, i] > 0) {
                        parent[i] = u;
                        if (i == t) {
                            return true;
                        }
                        tree[i] = Tree.S;
                        queue.Enqueue(i);
                        visited.Set(i, true);
                    }
                }
            }
            return false;
        }

        private enum Tree : byte {
            Free,
            S,
            T
        }

        //Grow fázis
        private Tuple<bool, int, int> Boykov_Kolmogorov(int[,] residGraph, int[] parent, Tree[] tree, Queue<int> active, int s, int t)
        {
            parent[s] = -1;
            parent[t] = -1;
            while (active.Count > 0) {
                int p = active.Peek();
                for (int i = 0; i < nodenames.Count; i++) {
                    if (Tree_Cap(residGraph, p, i, tree[p]) > 0) {
                        if (tree[i] == Tree.Free) {
                            tree[i] = tree[p];
                            parent[i] = p;
                            active.Enqueue(i);
                        }
                        else if (tree[p] != tree[i]) {
                            return new Tuple<bool, int, int>(true, p, i);
                        }
                    }
                }
                active.Dequeue();
            }
            return new Tuple<bool, int, int>(false, -1, -1);
        }

        private int Tree_Cap(int[,] residGraph, int p, int q, Tree tp)
        {
            int capacity;
            if (tp == Tree.S) {
                capacity = residGraph[p, q];
            }
            else {
                capacity = residGraph[q, p];
            }
            return capacity;
        }

        private static Queue<T> RemoveElementsFromQueue<T>(Queue<T> source, IEnumerable<T> toRemove)
        {
            if (toRemove.Count() > 0) {
                Queue<T> output = new Queue<T>();
                T[] sourceArray = source.ToArray();
                foreach (T item in sourceArray) {
                    bool keep = true;
                    foreach (var element in toRemove) {
                        if (item.Equals(element)) {
                            keep = false;
                            break;
                        }
                    }
                    if (keep) {
                        output.Enqueue(item);
                    }
                }
                return output;
            }
            else {
                return source;
            }
        }

        public enum Method : byte {
            DFS,
            BFS,
            Boykov_Kolmogorov
        }

        public int MaxFlow(Method method = Method.DFS, bool stepbystep = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int s = nodenames.IndexOf(source);
            int t = nodenames.IndexOf(sink);
            if (s == -1 || t == -1) {
                return 0;
            }
            int maxflow = 0;
            local_flow_paths.Clear();
            local_Trees.Clear();
            Tree[] trees = new Tree[nodenames.Count];
            trees[s] = Tree.S;
            trees[t] = Tree.T;
            int[] parent = new int[nodenames.Count];
            int[,] residGraph = new int[nodenames.Count, nodenames.Count];
            for (int i = 0; i < nodenames.Count; i++) {
                for (int j = 0; j < nodenames.Count; j++) {
                    residGraph[i, j] = graph_matrix[i][j];
                }
            }
            if (method != Method.Boykov_Kolmogorov) {
                while (method == Method.BFS ? BFS(residGraph, parent, trees, s, t) : DFS(residGraph, parent, trees, s, t)) {
                    List<int> currentpath = new List<int>();
                    int pathflow = int.MaxValue;
                    for (int i = t; i != s; i = parent[i]) {
                        currentpath.Add(i);
                        int j = parent[i];
                        pathflow = Math.Min(pathflow, residGraph[j, i]);
                    }
                    currentpath.Add(s);
                    currentpath.Reverse();
                    currentpath.Add(pathflow);
                    local_Trees.Add((Tree[])trees.Clone());
                    for (int i = 0; i < trees.Length; i++) {
                        trees[i] = Tree.Free;
                    }
                    trees[s] = Tree.S;
                    trees[t] = Tree.T;
                    local_flow_paths.Add(currentpath);
                    for (int i = t; i != s; i = parent[i]) {
                        int j = parent[i];
                        residGraph[j, i] -= pathflow;
                        residGraph[i, j] += pathflow;
                    }
                    maxflow += pathflow;
                }
            }
            else {
                //Boykov_Kolmogorov
                Tuple<bool, int, int> output;
                Queue<int> active = new Queue<int>(), orphan = new Queue<int>();
                active.Enqueue(s);
                active.Enqueue(t);
                while ((output = Boykov_Kolmogorov(residGraph, parent, trees, active, s, t)).Item1) {
                    //Augmentation fázis
                    List<int> currentpath = new List<int>(), revcurrentpath = new List<int>();
                    int pathflow = Tree_Cap(residGraph, output.Item2, output.Item3, trees[output.Item2]);
                    int itS = (trees[output.Item2] == Tree.S) ? output.Item2 : output.Item3;
                    int itT = (trees[output.Item2] == Tree.S) ? output.Item3 : output.Item2;
                    // S-nek kell lennie (szabványos működés)
                    for (int i = itS; parent[i] != -1; i = parent[i]) {
                        currentpath.Add(i);
                        int j = parent[i];
                        pathflow = Math.Min(pathflow, residGraph[j, i]);
                    }
                    // T-nek kell lennie (ellentétes működés)
                    for (int i = itT; parent[i] != -1; i = parent[i]) {
                        revcurrentpath.Add(i);
                        int j = parent[i];
                        pathflow = Math.Min(pathflow, residGraph[i, j]); ;
                    }
                    currentpath.Add(s);
                    revcurrentpath.Add(t);
                    currentpath.Reverse();
                    currentpath.AddRange(revcurrentpath);
                    residGraph[itS, itT] -= pathflow;
                    residGraph[itT, itS] += pathflow;
                    // S-nek kell lennie (szabványos működés)
                    for (int i = itS; parent[i] != -1; i = parent[i]) {
                        int j = parent[i];
                        residGraph[j, i] -= pathflow;
                        residGraph[i, j] += pathflow;
                    }
                    // T-nek kell lennie (ellentétes működés)
                    for (int i = itT; parent[i] != -1; i = parent[i]) {
                        int j = parent[i];
                        residGraph[i, j] -= pathflow;
                        residGraph[j, i] += pathflow;
                    }
                    maxflow += pathflow;
                    // - élek telítettségének kezelése
                    for (int index = 0; index < currentpath.Count - 1; index++) {
                        int i = currentpath[index];
                        int j = currentpath[index + 1];
                        if (residGraph[i, j] < 1) {
                            if (trees[i] == Tree.S && trees[j] == Tree.S) {
                                parent[j] = -10;
                                orphan.Enqueue(j);
                            }
                            else if (trees[i] == Tree.T && trees[j] == Tree.T) {
                                parent[i] = -10;
                                orphan.Enqueue(i);
                            }
                        }
                    }
                    local_Trees.Add((Tree[])trees.Clone());
                    //Adoption fázis
                    List<int> toRemove = new List<int>();
                    while (orphan.Count > 0) {
                        int p = orphan.Dequeue();
                        for (int i = 0; i < nodenames.Count; i++) {
                            if (trees[i] == trees[p] && Tree_Cap(residGraph, i, p, trees[i]) > 0) {
                                int io = parent[i];
                                while (io >= 0) {
                                    io = parent[io];
                                }
                                if (io == -1) {
                                    parent[p] = i;
                                    break;
                                }
                            }
                        }
                        if (parent[p] == -10) {
                            for (int i = 0; i < nodenames.Count; i++) {
                                if (trees[i] == trees[p]) {
                                    if (Tree_Cap(residGraph, i, p, trees[i]) > 0) {
                                        active.Enqueue(i);
                                    }
                                    if (parent[i] == p) {
                                        orphan.Enqueue(i);
                                        parent[i] = -10;
                                    }
                                }
                            }
                            trees[p] = Tree.Free;
                            toRemove.Add(p);
                        }
                    }
                    active = RemoveElementsFromQueue(active, toRemove);
                    currentpath.Add(pathflow);
                    local_flow_paths.Add(currentpath);
                }
            }
            stopwatch.Stop();
            LastCalcTime = stopwatch.Elapsed.TotalSeconds;
            if (!stepbystep) {
                SetStreamValues(residGraph);
            }
            return maxflow;
        }

        public void Dispose()
        {
            ControlsEnabled = false;
            canvas.MouseMove -= MouseMoveOnCanvas;
            canvas.MouseLeftButtonDown -= MouseDownOnCanvas;
            canvas.MouseLeftButtonUp -= MouseUpOnCanvas;
            canvas.MouseRightButtonDown -= RightMouseDownOnCanvas;
            canvas.MouseRightButtonUp -= RightMouseUpOnCanvas;
            canvas.MouseWheel -= Zoom;
            BGRect.MouseRightButtonUp -= RightMouseUpOnCanvasBG;
            nodenames.Clear();
            graph_matrix.Clear();
            foreach (Node n in nodes) {
                n.Dispose();
            }
            foreach (SPLine n in splines) {
                n.Dispose();
            }
            filename = null;
            sink = null;
            source = null;
            target = null;
            sptarget = null;
            RightTarget = null;
            canvas.Children.Clear();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            GC.WaitForPendingFinalizers();
        }
    }
}
