using System.Windows.Forms;

namespace Homework4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random r = new Random();
        public Bitmap b;
        public Graphics g;
        double successprob = 0.5;
        int simulations = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.g = Graphics.FromImage(b);
        }



        public int resizeX(double minX,double maxX,int W, double x)
        {
            
            return Convert.ToInt32(((x - minX)*W) / (maxX - minX));

        }
        public int resizeY(double minY, double maxY,int H,double y)
        {
            return Convert.ToInt32(H - ((y - minY)*H) / (maxY - minY)) ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.button1.Text == "Start")
            {
                this.numericUpDown1.Enabled = false;
                this.button1.Text = "Stop";
                successprob = ((Convert.ToDouble(numericUpDown1.Value)) / 100);
                timer1.Start();
            }
            else
            {
                this.button1.Text = "Start";
                timer1.Stop();
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            int trials = 1000;
            int toss = 0;
            simulations++;
            
            this.richTextBox1.Text = "Number of simulations: " + simulations.ToString();

            Point[] points1 = new Point[trials];
            Point[] points2 = new Point[trials];
            Point[] points3 = new Point[trials];

            for (int i = 0; i < trials; i++)
            {
                double uniform = r.NextDouble();
                if (uniform < successprob) toss++;
                Point p1 = new Point(); //absolute frequency
                Point p2 = new Point(); //relative frequency
                Point p3 = new Point(); //normalized frequency
                p1.Y = resizeY(0, trials, pictureBox1.Height, toss);
                p1.X = resizeX(0, trials, pictureBox1.Width, i);
                p2.Y = resizeY(0, trials, pictureBox1.Height, toss*trials/(i+1));
                p2.X = resizeX(0, trials, pictureBox1.Width, i);
                p3.Y = resizeY(0, trials*successprob, pictureBox1.Height, toss * (Math.Sqrt(trials)) / (Math.Sqrt(i + 1)));
                p3.X = resizeX(0, trials, pictureBox1.Width, i); 
                points1[i] = p1;
                points2[i] = p2;
                points3[i] = p3;

            }

            this.g.DrawLines(Pens.Lime, points1);
            this.g.DrawLines(Pens.Purple, points2);
            this.g.DrawLines(Pens.Yellow, points3);
            this.pictureBox1.Image = b;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}