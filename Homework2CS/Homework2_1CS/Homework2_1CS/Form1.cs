namespace Homework2_1CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int press = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (press == 0)
            {
                press = 1;
                this.button1.Text = "Pause";
                this.timer1.Start();
            }
            else
            {
                press = 0;
                this.button1.Text = "Play";
                this.timer1.Stop();
            }
        }

        Random rand = new Random(); 
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Interval = rand.Next(1, 10) * rand.Next(100,300);        
            this.progressBar1.PerformStep();
            if (this.progressBar1.Value == 100)
            {
                this.button1.Visible = false;
            }
        }
    }
}