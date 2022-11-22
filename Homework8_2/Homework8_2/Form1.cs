namespace Homework8_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            r1 = new Rectangle(20, pictureBox1.Bottom / 2 + 40, b.Width / 4 - 100, b.Height / 2 - 100);
            r2 = new Rectangle(b.Width / 4 +100, pictureBox1.Bottom / 2 + 40, b.Width / 4 + 100, b.Height / 2 - 100);
            r3 = new Rectangle(250, 70, b.Width / 4 - 100, b.Height / 2 - 100);
            r4 = new Rectangle(b.Width / 4 + 700, pictureBox1.Bottom / 2 + 40, b.Width / 4 - 100, b.Height / 2 - 100);
            r5 = new Rectangle(b.Width / 2 + 150, 70, b.Width / 4 - 100, b.Height / 2 - 100);
        }

        Bitmap b;
        Graphics g;
        Rectangle r1;
        Rectangle r2;
        Rectangle r3;
        Rectangle r4;
        Rectangle r5;

        List<double> normal;
        Dictionary<Interval, int> normal_distribution;

        List<double> chisquared;
        Dictionary<Interval, int> chisquared_distribution;

        List<double> cauchy;
        Dictionary<Interval, int> cauchy_distribution;

        List<double> Ffisher;
        Dictionary<Interval, int> Ffisher_distribution;

        List<double> Tstudent;
        Dictionary<Interval, int> Tstudent_distribution;

        int numbersamples = 100000;

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            NormalRVGenerator RVgen = new NormalRVGenerator();

            normal = new List<double>();
            chisquared = new List<double>();
            cauchy = new List<double>();
            Ffisher = new List<double>();
            Tstudent = new List<double>();

            for (int i = 0; i < numbersamples; i++)
            {
                (double X, double Y) = RVgen.getNewPair();

                normal.Add(X);
                chisquared.Add(X * X);
                cauchy.Add(X / Y);
                Ffisher.Add((X * X) / (Y * Y));
                Tstudent.Add(X / Math.Sqrt((Y * Y)));
            }

            double cauchy_average = cauchy.Average();
            cauchy = cauchy.Where(x => (x >= cauchy_average - 50 && x <= cauchy_average + 50)).ToList();

            Ffisher = Ffisher.Where(x => (x <= 50)).ToList();
            Tstudent = Tstudent.Where(x => (x >= -50 && x <= 50)).ToList();

            normal_distribution = compute_distribution(normal, 0.2d);
            chisquared_distribution = compute_distribution(chisquared, 0.6d);
            cauchy_distribution = compute_distribution(cauchy, 5);
            Ffisher_distribution = compute_distribution(Ffisher, 1);
            Tstudent_distribution = compute_distribution(Tstudent, 1);

            timer1.Start();
        }
        private Dictionary<Interval, int> compute_distribution(List<double> lista, double interval_size)
        {
            Dictionary<Interval, int> distribution = new Dictionary<Interval, int>();
            double next = Math.Floor(lista.Min());

            foreach (double inter in lista)
            {
                bool inserted = false;

                List<Interval> keys = distribution.Keys.ToList();
                foreach (Interval v in keys)
                {
                    if (inter >= v.down && inter < v.up)
                    {
                        inserted = true;
                        distribution[v]++;
                    }
                }
                while (!inserted)
                {
                    Interval newint = new Interval(next, next + interval_size);
                    next = next + interval_size;
                    distribution[newint] = 0;

                    if (inter >= newint.down && inter < newint.up)
                    {
                        distribution[newint]++;
                        inserted = true;
                    }
                }
            }

            return distribution;
        }

        private void drawHistogram(Dictionary<Interval, int> distribution, Rectangle r, Graphics g, string text)
        {
            g.DrawRectangle(Pens.Black, r);
            g.FillRectangle(Brushes.Black, r);

            int maxvalue = distribution.Values.Max();
            int space_height = (r.Bottom - r.Top) - 20;

            int size = r.Width / distribution.Count;
            int X = r.Left;

            foreach (KeyValuePair<Interval, int> kv in distribution)
            {

                Interval i = kv.Key;

                int rect_height = (int)(((double)kv.Value / (double)maxvalue) * ((double)space_height));

                Rectangle histrect = new Rectangle(X, r.Bottom - rect_height, size, rect_height);
                g.FillRectangle(Brushes.Lime, histrect);
                g.DrawRectangle(Pens.Black, histrect);

                X = X + size;
            }

            Rectangle stringPos = new Rectangle(r.Left, r.Top - 2 * (r.Height / 5), r.Width, r.Height / 5);
            Font font1 = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Font goodFont = findFont(g, text, stringPos.Size, font1);

            g.DrawString(text, goodFont, Brushes.Black, stringPos, stringFormat);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            drawHistogram(normal_distribution, r1, g, "Normal");
            drawHistogram(chisquared_distribution, r2, g, "Chi-squared");
            drawHistogram(cauchy_distribution, r3, g, "Cauchy (0,1)");
            drawHistogram(Ffisher_distribution, r4, g, "F (1,1)");
            drawHistogram(Tstudent_distribution, r5, g, "T (1)");
            pictureBox1.Image = b;
        }

        private Font findFont(Graphics g, string myString, Size Room, Font PreferedFont)
        {
            SizeF RealSize = g.MeasureString(myString, PreferedFont);
            float HeightScaleRatio = Room.Height / RealSize.Height;
            float WidthScaleRatio = Room.Width / RealSize.Width;

            float ScaleRatio = (HeightScaleRatio < WidthScaleRatio) ? ScaleRatio = HeightScaleRatio : ScaleRatio = WidthScaleRatio;

            float ScaleFontSize = PreferedFont.Size * ScaleRatio;

            if (ScaleFontSize <= 0)
            {
                ScaleFontSize = 0.0000001f;
            }

            return new Font(PreferedFont.FontFamily, ScaleFontSize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numbersamples = trackBar1.Value;
            label1.Text = "Number of samples: " + numbersamples.ToString();
        }
    }

    internal class NormalRVGenerator
    {
        Random r_module;
        Random r_angle;

        double mean;
        double variance;

        double mse;

        public NormalRVGenerator(double m, double v)
        {
            r_module = new Random();
            r_angle = new Random();

            if (m >= 0) mean = m;
            else mean = 0;

            if (v > 0) variance = v;
            else variance = 1;

            mse = Math.Sqrt(variance);
        }

        public NormalRVGenerator()
        {
            r_module = new Random();
            r_angle = new Random();

            mean = 0;
            variance = 1;

            mse = Math.Sqrt(variance);
        }

        public (double, double) getNewPair()
        {
            double r = Math.Sqrt(-2 * Math.Log(r_module.NextDouble()));
            double angle = r_angle.NextDouble() * 2 * Math.PI;

            double x = r * Math.Cos(angle) * mse + mean;
            double y = r * Math.Sin(angle) * mse + mean;

            return (x, y);
        }
    }

    internal class Interval : IComparable<Interval>
    {
        public double up;
        public double down;

        public Interval(double down, double up)
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