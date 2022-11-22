namespace Homeowrk8

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            r1 = new Rectangle(570, pictureBox1.Bottom / 2 - 160, b.Height / 2+30, b.Height / 2+30);
            PenTrajectory = new Pen(Color.OrangeRed, 1);

        }

        Bitmap b;
        Graphics g;

        Rectangle r1;
        Pen PenTrajectory;

        int pointnumber = 1000;

        List<RealPoint> points;

        double minX;
        double maxX;
        double minY;
        double maxY;

        Dictionary<Interval, int> distribution_x;
        Dictionary<Interval, int> distribution_y;

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            minX = -100d;
            maxX = 100d;
            minY = -100d;
            maxY = 100d;

            CoordinatesGenerator cg = new CoordinatesGenerator(100);
            points = new List<RealPoint>();

            for (int i = 0; i < pointnumber; i++)
            {
                (double X, double Y) = cg.getNewPair();
                RealPoint punto = new RealPoint(X, Y);
                points.Add(punto);
            }

            compute_distribution_x();
            compute_distribution_y();

            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            drawPoints();
            drawHistogram_x();
            drawHistogram_y();
            pictureBox1.Image = b;
        }

        private void drawPoints()
        {
            
            g.FillRectangle(Brushes.Black, r1);

            List<Point> punti = new List<Point>();

            foreach (RealPoint p in points)
            {
                int xCord = linearTransformX(p.X, minX, maxX, r1.Left, r1.Width);
                int yCord = linearTransformY(p.Y, minY, maxY, r1.Top, r1.Height);

                Point draw_p = new Point(xCord, yCord);
                punti.Add(draw_p);
            }

            foreach (Point dp in punti)
            {
                Rectangle rect = new Rectangle(dp.X - 1, dp.Y - 1, 2, 2);
                g.FillEllipse(Brushes.Lime, rect);
            }
        }

        private void compute_distribution_x()
        {

            distribution_x = new Dictionary<Interval, int>();

            int current = (int)Math.Floor(minY);
            int max = (int)Math.Ceiling(maxY);

            int size = 10;

            while (current < max)
            {
                Interval i = new Interval(current, current + size);
                current = current + size;

                distribution_x[i] = 0;
            }

            foreach (RealPoint p in points)
            {
                List<Interval> keys = distribution_x.Keys.ToList();
                bool p_inserted = false;
                foreach (Interval v in keys)
                {
                    if (p.X >= v.down && p.X <= v.up && !p_inserted)
                    {
                        p_inserted = true;
                        distribution_x[v]++;
                    }
                }
            }
        }

        private void compute_distribution_y()
        {

            distribution_y = new Dictionary<Interval, int>();

            int current = (int)Math.Floor(minY);
            int max = (int)Math.Ceiling(maxY);

            int size = 10;

            while (current < max)
            {
                Interval i = new Interval(current, current + size);
                current = current + size;

                distribution_y[i] = 0;
            }

            foreach (RealPoint p in points)
            {
                List<Interval> keys = distribution_y.Keys.ToList();
                bool p_inserted = false;
                foreach (Interval v in keys)
                {
                    if (p.Y >= v.down && p.Y <= v.up && !p_inserted)
                    {
                        p_inserted = true;
                        distribution_y[v]++;
                    }
                }
            }
        }

        private void drawHistogram_y()
        {
            int maxvalue = distribution_y.Values.Max();
            int space_height = (r1.Right - r1.Left) / 2;

            foreach (KeyValuePair<Interval, int> kv in distribution_y)
            {

                Interval i = kv.Key;

                int rect_height = (int)(((double)kv.Value / (double)maxvalue) * ((double)space_height));

                int down_mod = linearTransformY(i.down, minY, maxY, r1.Top, r1.Height);
                int up_mod = linearTransformY(i.up, minY, maxY, r1.Top, r1.Height);

                int size = Math.Abs(down_mod - up_mod);

                Rectangle histrect = new Rectangle(r1.Right, up_mod, rect_height, size);
                g.FillRectangle(Brushes.Green, histrect);
                g.DrawRectangle(Pens.Red, histrect);
            }
        }

        private void drawHistogram_x()
        {
            int maxvalue = distribution_x.Values.Max();
            int space_height = (r1.Bottom - r1.Top) / 2;

            foreach (KeyValuePair<Interval, int> kv in distribution_x)
            {

                Interval i = kv.Key;

                int rect_height = (int)(((double)kv.Value / (double)maxvalue) * ((double)space_height));

                int down_mod = linearTransformX(i.down, minY, maxY, r1.Left, r1.Width);
                int up_mod = linearTransformX(i.up, minY, maxY, r1.Left, r1.Width);

                int size = Math.Abs(down_mod - up_mod);

                Rectangle histrect = new Rectangle(down_mod, r1.Top - rect_height, size, rect_height);
                g.FillRectangle(Brushes.Green, histrect);
                g.DrawRectangle(Pens.Red, histrect);
            }
        }

        public int linearTransformX(double X, double minX, double maxX, int Left, int W)
        {
            return Left + (int)(W * ((X - minX) / (maxX - minX)));
        }

        public int linearTransformY(double Y, double minY, double maxY, int Top, int H)
        {
            return Top + (int)(H - H * ((Y - minY) / (maxY - minY)));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pointnumber = trackBar1.Value;
            label1.Text = "Generated points: " + pointnumber.ToString();
        }
    }


    class RealPoint
    {
        public double X;
        public double Y;

        public RealPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return X.ToString() + " - " + Y.ToString();
        }
    }

    internal class CoordinatesGenerator
    {

        Random r_module;
        Random r_angle;

        int radius;

        public CoordinatesGenerator(int r)
        {
            r_module = new Random();
            r_angle = new Random();

            radius = r;
        }

        public (double, double) getNewPair()
        {
            double r = r_module.NextDouble() * radius;
            double angle = r_angle.NextDouble() * 2 * Math.PI;

            double x = r * Math.Cos(angle);
            double y = r * Math.Sin(angle);

            return (x, y);
        }
    }

    internal class Interval : IComparable<Interval>
    {
        public int up;
        public int down;

        public Interval(int down, int up)
        {
            this.up = up;
            this.down = down;
        }

        public override string ToString()
        {
            return down.ToString() + " - " + up.ToString();
        }

        public int CompareTo(Interval other)
        {
            if (other == null)
                throw new Exception("Error: Null Interval");
            if (this.down == other.down && this.up == other.up)
                return 0;
            else if (this.up <= other.down)
                return -1;
            else if (this.down >= other.up)
                return 1;
            else
                throw new Exception("Error: Intervals intersect");

        }
    }



    
}