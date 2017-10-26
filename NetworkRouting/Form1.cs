using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetworkRouting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void clearAll()
        {
            startNodeIndex = -1;
            stopNodeIndex = -1;
            sourceNodeBox.Clear();
            sourceNodeBox.Refresh();
            targetNodeBox.Clear();
            targetNodeBox.Refresh();
            arrayTimeBox.Clear();
            arrayTimeBox.Refresh();
            heapTimeBox.Clear();
            heapTimeBox.Refresh();
            differenceBox.Clear();
            differenceBox.Refresh();
            pathCostBox.Clear();
            pathCostBox.Refresh();
            arrayCheckBox.Checked = false;
            arrayCheckBox.Refresh();
            return;
        }

        private void clearSome()
        {
            arrayTimeBox.Clear();
            arrayTimeBox.Refresh();
            heapTimeBox.Clear();
            heapTimeBox.Refresh();
            differenceBox.Clear();
            differenceBox.Refresh();
            pathCostBox.Clear();
            pathCostBox.Refresh();
            return;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            int randomSeed = int.Parse(randomSeedBox.Text);
            int size = int.Parse(sizeBox.Text);

            Random rand = new Random(randomSeed);
            seedUsedLabel.Text = "Random Seed Used: " + randomSeed.ToString();

            clearAll();
            this.adjacencyList = generateAdjacencyList(size, rand);
            List<PointF> points = generatePoints(size, rand);
            resetImageToPoints(points);
            this.points = points;
        }

        // Generates the distance matrix.  Values of -1 indicate a missing edge.  Loopbacks are at a cost of 0.
        private const int MIN_WEIGHT = 1;
        private const int MAX_WEIGHT = 100;
        private const double PROBABILITY_OF_DELETION = 0.35;

        private const int NUMBER_OF_ADJACENT_POINTS = 3;

        private List<HashSet<int>> generateAdjacencyList(int size, Random rand)
        {
            List<HashSet<int>> adjacencyList = new List<HashSet<int>>();

            for (int i = 0; i < size; i++)
            {
                HashSet<int> adjacentPoints = new HashSet<int>();
                while (adjacentPoints.Count < 3)
                {
                    int point = rand.Next(size);
                    if (point != i) adjacentPoints.Add(point);
                }
                adjacencyList.Add(adjacentPoints);
            }

            return adjacencyList;
        }

        private List<PointF> generatePoints(int size, Random rand)
        {
            List<PointF> points = new List<PointF>();
            for (int i = 0; i < size; i++)
            {
                points.Add(new PointF((float) (rand.NextDouble() * pictureBox.Width), (float) (rand.NextDouble() * pictureBox.Height)));
            }
            return points;
        }

        private void resetImageToPoints(List<PointF> points)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics graphics = Graphics.FromImage(pictureBox.Image);
            Pen pen;

            if (points.Count < 100)
                pen = new Pen(Color.Blue);
            else
                pen = new Pen(Color.LightBlue);
            foreach (PointF point in points)
            {
                graphics.DrawEllipse(pen, point.X, point.Y, 2, 2);
            }

            this.graphics = graphics;
            pictureBox.Invalidate();
        }

        // These variables are instantiated after the "Generate" button is clicked
        private List<PointF> points = new List<PointF>();
        private Graphics graphics;
        private List<HashSet<int>> adjacencyList;

        // Use this to generate paths (from start) to every node; then, just return the path of interest from start node to end node
        private void solveButton_Click(object sender, EventArgs e)
        {
            // This was the old entry point, but now it is just some form interface handling
            bool ready = true;

            if(startNodeIndex == -1)
            {
                sourceNodeBox.Focus();
                sourceNodeBox.BackColor = Color.Red;
                ready = false;
            }
            if(stopNodeIndex == -1)
            {
                if(!sourceNodeBox.Focused)
                    targetNodeBox.Focus();
                targetNodeBox.BackColor = Color.Red;
                ready = false;
            }
            if (points.Count > 0)
            {
                resetImageToPoints(points);
                paintStartStopPoints();
            }
            else
            {
                ready = false;
            }
            if(ready)
            {
                clearSome();
                solveButton_Clicked();  // Here is the new entry point
            }
        }

        private void solveButton_Clicked()
        {
            // *** Implement this method, use the variables "startNodeIndex" and "stopNodeIndex" as the indices for your start and stop points, respectively ***

            PriorityQueue priorityHeap = new HeapPriorityQueue();
            List<int> heapList = new List<int>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            heapList = dijkstrasAlgorithm(priorityHeap, false);
            stopwatch.Stop();

            double heapTime = ((double)stopwatch.ElapsedMilliseconds / 1000);
            heapTimeBox.Text = heapTime.ToString();

            List<int> arrayList = new List<int>();

            // Runs the array priority queue if it is selected
            if(arrayCheckBox.Checked)
            {
                PriorityQueue priorityArray = new ArrayPriorityQueue();
                stopwatch.Restart();
                arrayList = dijkstrasAlgorithm(priorityArray, true);
                stopwatch.Stop();

                double arrayTime = ((double)stopwatch.ElapsedMilliseconds / 1000);
                arrayTimeBox.Text = arrayTime.ToString();
                double difference = (arrayTime - heapTime) / heapTime;
                differenceBox.Text = difference.ToString();

                //drawPath(ref arrayList);
            }

            // Prints out heap implementation because the heap is used by default
            drawPath(ref heapList);
        }

        //-----------------------------------------------------------------------------//
        //-------------------------- Dijkstra's Algorithm -----------------------------//
        //-----------------------------------------------------------------------------//


        /**
         * This Dijkstra's is implemented based off of the pseudocode found in the text.
         * Time complexity for the Binary Heap Priority queue is O(|V| * |logV|)
         * because the implementation requires all the nodes to be visited but the 
         * update methods require |log V| time.  The array priority queue is O(|V|^2) 
         * because it has to visit all the nodes on implementation and has to itterate 
         * through the nodes in order to delete and update the queue.  The space complexity
         * for both is O(|V|) because each queue stores all of the nodes generated
         **/
        private List<int> dijkstrasAlgorithm(PriorityQueue queue, bool isArray)
        {
            queue.makeQueue(points.Count);

            List<int> previous = new List<int>();
            List<double> distances = new List<double>();

            // Sets all distance values to "infinity"
            // sets previous as 'undefined'
            for(int i = 0; i < points.Count; i++)
            {
                previous.Add(-1);
                distances.Add(double.MaxValue);
            }

            // distance to start node
            distances[startNodeIndex] = 0;

            // checks which priority queue will be used
            if(isArray)
            {
                queue.insert(startNodeIndex, 0);
            }
            else
            {
                queue.insert(ref distances, startNodeIndex);
            }

            // main loop
            // loops until there arent any nodes with a permanent distance to the end node
            while(!queue.isEmpty())
            {
                int minIndex;

                if(isArray)
                {
                    minIndex = queue.deleteMin();
                }
                else
                {
                    minIndex = queue.deleteMin(ref distances);
                }

                PointF u = points[minIndex];

                // finds the best path amongst its neighbors, if it exists
                foreach(int index in adjacencyList[minIndex])
                {
                    PointF alt = points[index];
                    double altDistance = distances[minIndex] + distanceBetween(u, alt);

                    if(altDistance < distances[index])
                    {
                        previous[index] = minIndex;
                        distances[index] = altDistance;

                        if(isArray)
                        {
                            queue.decreaseKey(index, altDistance);
                        }
                        else
                        {
                            queue.decreaseKey(ref distances, index);
                        }
                    }
                }
            }

            //queue.printQueue();

            // gives the path of the nodes
            return previous;
        }

        //--------------------------- Helper Method -----------------------------------//

        private double distanceBetween(PointF u, PointF v)
        {
            double deltaXSqr = Math.Pow(v.X - u.X, 2);
            double deltaYSqr = Math.Pow(v.Y - u.Y, 2);
            double distance = Math.Sqrt(deltaXSqr + deltaYSqr);
            return distance;
        }

        //-----------------------------------------------------------------------------//
        //---------------------------- Draw Methods -----------------------------------//
        //-----------------------------------------------------------------------------//

        private PointF findMidPointOfPoints(int firstIndex, int secondIndex)
        {
            PointF midpoint = new PointF();

            midpoint.X = (points[firstIndex].X + points[secondIndex].X) / 2;
            midpoint.Y = (points[firstIndex].Y + points[secondIndex].Y) / 2;

            return midpoint;
        }

        // draws the shortest path
        private void drawPath(ref List<int> path)
        {
            int cur = stopNodeIndex;
            int previousIndex = cur;
            double totalPathCost = 0;

            Console.Write("path nodes are: ");
            foreach(int num in path)
            {
                Console.Write(num + ", ");
            }
            Console.WriteLine();

            while(true)
            {
                cur = path[cur];
                if (cur == -1) break;

                Pen pen = new Pen(Color.Black, 1);
                graphics.DrawLine(pen, points[cur], points[previousIndex]);

                double distance = distanceBetween(points[cur], points[previousIndex]);
                totalPathCost += distance;

                PointF midPoint = findMidPointOfPoints(previousIndex, cur);

                int distanceTruncated = (int)distance;
                
                graphics.DrawString(distanceTruncated.ToString(), SystemFonts.DefaultFont, Brushes.Black, midPoint);
                previousIndex = cur;
            }

            pathCostBox.Text = totalPathCost.ToString();
        }

        private Boolean startStopToggle = true;
        private int startNodeIndex = -1;
        private int stopNodeIndex = -1;
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (points.Count > 0)
            {
                Point mouseDownLocation = new Point(e.X, e.Y);
                int index = ClosestPoint(points, mouseDownLocation);
                if (startStopToggle)
                {
                    startNodeIndex = index;
                    sourceNodeBox.ResetBackColor();
                    sourceNodeBox.Text = "" + index;
                }
                else
                {
                    stopNodeIndex = index;
                    targetNodeBox.ResetBackColor();
                    targetNodeBox.Text = "" + index;
                }
                resetImageToPoints(points);
                paintStartStopPoints();
            }
        }

        private void sourceNodeBox_Changed(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                try{ startNodeIndex = int.Parse(sourceNodeBox.Text); }
                catch { startNodeIndex = -1; }
                if (startNodeIndex < 0 | startNodeIndex > points.Count-1)
                    startNodeIndex = -1;
                if(startNodeIndex != -1)
                {
                    sourceNodeBox.ResetBackColor();
                    resetImageToPoints(points);
                    paintStartStopPoints();
                    startStopToggle = !startStopToggle;
                }
            }
        }

        private void targetNodeBox_Changed(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                try { stopNodeIndex = int.Parse(targetNodeBox.Text); }
                catch { stopNodeIndex = -1; }
                if (stopNodeIndex < 0 | stopNodeIndex > points.Count-1)
                    stopNodeIndex = -1;
                if(stopNodeIndex != -1)
                {
                    targetNodeBox.ResetBackColor();
                    resetImageToPoints(points);
                    paintStartStopPoints();
                    startStopToggle = !startStopToggle;
                }
            }
        }
        
        private void paintStartStopPoints()
        {
            if (startNodeIndex > -1)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.DrawEllipse(new Pen(Color.Green, 6), points[startNodeIndex].X, points[startNodeIndex].Y, 1, 1);
                this.graphics = graphics;
                pictureBox.Invalidate();
            }

            if (stopNodeIndex > -1)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.DrawEllipse(new Pen(Color.Red, 2), points[stopNodeIndex].X - 3, points[stopNodeIndex].Y - 3, 8, 8);
                this.graphics = graphics;
                pictureBox.Invalidate();
            }
        }

        private int ClosestPoint(List<PointF> points, Point mouseDownLocation)
        {
            double minDist = double.MaxValue;
            int minIndex = 0;

            for (int i = 0; i < points.Count; i++)
            {
                double dist = Math.Sqrt(Math.Pow(points[i].X-mouseDownLocation.X,2) + Math.Pow(points[i].Y - mouseDownLocation.Y,2));
                if (dist < minDist)
                {
                    minIndex = i;
                    minDist = dist;
                }
            }

            return minIndex;
        }
    }
}
