using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Homework7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        Random rand = new Random();
        public Bitmap b;
        public Graphics g;
        public Bitmap b2;
        public Graphics g2;
        double successprob = 0.5;
        int simulations = 10;
        int executed = 0;
        int trials = 100;
        int lambda = 50;
        int distance = 0;
        int num_intervals=0;
        public Rectangle r;
        int maxkey=0;
        int maxvalue;

        Dictionary<int, int> intervals = new Dictionary<int, int>(); 

        private void button1_Click(object sender, EventArgs e)
        {

            if (this.button1.Text == "Start")
            {
                this.button1.Text = "Stop";
                //g2.Clear(Color.Black);
                timer1.Start();
                this.trackBar1.Enabled = false;
                this.trackBar2.Enabled = false;
            }
            else
            {
                this.button1.Text = "Start";
                timer1.Stop();
                this.trackBar1.Enabled = true;
                this.trackBar2.Enabled = true;
            } 
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int current = (int)trackBar1.Value;
            trials = (int)trackBar1.Value;
            this.label1.Text = "Number of tosses: " + current.ToString();
            
            if(this.trackBar2.Value >= this.trackBar1.Value)
            {
                this.trackBar2.Value=this.trackBar1.Value-1;
                this.label2.Text = "Lambda: " + (this.trackBar2.Value).ToString();
            }
            this.trackBar2.Maximum = current - 1;
            lambda = (int)trackBar2.Value;
            successprob = Math.Round((double)lambda / (double)trials,5);
            this.label3.Text = "Success probability will be: "+(Math.Round(successprob*100,2)).ToString()+"%";

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.label2.Text = "Lambda: "+(this.trackBar2.Value).ToString();
            lambda = (int)trackBar2.Value;
            successprob = Math.Round((double)lambda / (double)trials, 5);
            this.label3.Text = "Success probability will be: " +(Math.Round(successprob*100,2)).ToString()+"%";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int toss = 0;
            executed++;
            

            Point[] points1 = new Point[trials];

            for (int i = 0; i < trials; i++)
            {
                double uniform = rand.NextDouble();
                if (uniform < successprob)
                {
                    toss++;
                    if (distance > maxkey) maxkey = distance;
                    if (intervals.ContainsKey(distance))
                    {
                        intervals[distance]++;
                        
                    }
                    else intervals.Add(distance, 1);
                    distance = 0;
                }
                else distance++;
                Point p1 = new Point(); //absolute frequency
                p1.Y = resizeY(0, trials, pictureBox1.Height, toss);
                p1.X = resizeX(0, trials, pictureBox1.Width, i);
                points1[i] = p1;;

            }

            this.g.DrawLines(Pens.Lime, points1);
            this.pictureBox1.Image = b;

            //here i have to do the histogram...
            
            
            if (executed == simulations)
            {
                this.button1.Text = "Start";
                timer1.Stop();
                this.trackBar1.Enabled = true;
                this.trackBar2.Enabled = true;
                executed = 0;
                this.g2.FillRectangle(Brushes.White, r);
                this.g2.DrawRectangle(Pens.White, r);

                num_intervals = maxkey+1;
                int space_height = r.Bottom - r.Top - 20;
                int space_width = r.Right - r.Left - 20;
                int start = r.X;
                int hist_width = space_width / num_intervals;
                maxvalue = intervals.Values.Max();

                for (int i = 0; i < maxkey + 1; i++)
                {
                    int size = 0;
                    if (intervals.ContainsKey(i))
                    {
                        size = intervals[i];

                    }
                    int rect_height = (int)(((double)size / (double)maxvalue) * ((double)space_height));
                    //if (size == 0) rect_height = 1;

                    Rectangle hist_rect = new Rectangle(start, r.Bottom - rect_height, hist_width, rect_height);

                    this.g2.FillRectangle(Brushes.Blue, hist_rect);
                    this.g2.DrawRectangle(Pens.White, hist_rect);

                    Rectangle stringPos = new Rectangle(start, r.Bottom + 5, hist_width, 10);
                    g2.FillRectangle(Brushes.White, stringPos);
                    g2.DrawRectangle(Pens.Black, stringPos);
                    string text = i.ToString();
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    g2.DrawString(text, Font, Brushes.Black, stringPos, stringFormat);
                    this.pictureBox2.Image = b2;
                    if (i != maxkey) start += hist_width;
                }
                this.pictureBox2.Image = b2;
            }

        }

        public int resizeX(double minX, double maxX, int W, double x)
        {
            return Convert.ToInt32(((x - minX) * W) / (maxX - minX));
        }
        public int resizeY(double minY, double maxY, int H, double y)
        {
            return Convert.ToInt32(H - (y - minY) * H / (maxY - minY));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.g = Graphics.FromImage(b);
            this.b2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            this.g2 = Graphics.FromImage(b2);
            r = new Rectangle(pictureBox2.Location.X-20, 20, b2.Width - 40, b2.Height - 40);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }
}