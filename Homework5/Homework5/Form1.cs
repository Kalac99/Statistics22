using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Homework5
{
    public partial class Form1 : Form
    {

        Bitmap b;
        Graphics g;

        int max;
        int intervals;

        EditableRectangle r1;

        int row;
        List<Packet> packets = new List<Packet>();
        List<Protocol> protocols = new List<Protocol>();
        bool check;
        string file;

        public Form1()
        {
            InitializeComponent();

            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            r1 = new EditableRectangle(300, 100, pictureBox1.Width / 2, pictureBox1.Height / 2, pictureBox1, this);          

            this.g.DrawRectangle(Pens.Black, r1.r);
            this.pictureBox1.Image = b;  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            file = @"wireshark.csv";

            using (var reader = new StreamReader(file))
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";

                var line = reader.ReadLine();
                var header = line.Split(',');

                while (!reader.EndOfStream)
                {

                    line = reader.ReadLine();
                    var values = line.Split(',');

                    Packet packet = new Packet();

                    Protocol protocol = new Protocol();
                    protocol.id = values[4];
                    protocol.counter = 1;

                    check = false;
                    foreach (Protocol p in protocols)
                    {
                        if (p.id == protocol.id)
                        {
                            p.counter++;
                            check = true;
                        }
                    }
                    if (check == false)
                    {
                        protocols.Add(protocol);
                    }
                }
            }

            max = 0;
            intervals = 0;
            foreach (Protocol p in protocols)
            {
                if (p.counter > max) max = p.counter;
                intervals++;
            }

            this.timer1.Start();
            this.button1.Enabled = false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.g.Clear(pictureBox1.BackColor);
            if (this.checkBox1.Checked) drawVertical();
            else drawHorizontal(); 
        }
        private void drawHorizontal()
        {
            int space_height = r1.r.Bottom - r1.r.Top - 20;
            int space_width = r1.r.Right - r1.r.Left - 20;

            g.FillRectangle(Brushes.Black, r1.r);
            g.DrawRectangle(Pens.Black, r1.r);


            int histrect_width = space_width / intervals;
            int start = r1.r.X;

            foreach(Protocol p in protocols)
            {
                int rect_height = (int)(((double)p.counter / (double)max) * ((double)space_height));
                Rectangle hist_rect = new Rectangle(start, r1.r.Bottom - rect_height, histrect_width, rect_height);

                g.FillRectangle(Brushes.Lime, hist_rect);
                g.DrawRectangle(Pens.Black, hist_rect);

                string text = p.id;
                Rectangle stringPos = new Rectangle(start, r1.r.Bottom + 20, histrect_width, histrect_width + 20);
                Font font1 = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                Font goodFont = findFont(g, text, stringPos.Size, font1);

                g.DrawString(text, goodFont, Brushes.Black, stringPos, stringFormat);

                start += histrect_width;
            }

            pictureBox1.Image = b;
        }
        private void drawVertical()
        {
            int space_height = r1.r.Bottom - r1.r.Top - 20;
            int space_width = r1.r.Right - r1.r.Left - 20;

            g.FillRectangle(Brushes.Black, r1.r);
            g.DrawRectangle(Pens.Black, r1.r);


            int histrect_width = space_height / intervals;
            int start = r1.r.Y;

            foreach (Protocol p in protocols)
            {
                int rect_height = (int)(((double)p.counter / (double)max) * ((double)space_width));
                Rectangle hist_rect = new Rectangle(r1.r.Left, start, rect_height, histrect_width);

                g.FillRectangle(Brushes.Lime, hist_rect);
                g.DrawRectangle(Pens.Black, hist_rect);


                string text = p.id;
                Rectangle stringPos = new Rectangle(r1.r.Left - (histrect_width * 7 + 10), start, histrect_width * 7, histrect_width);
                Font font1 = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                Font goodFont = findFont(g, text, stringPos.Size, font1);

                g.DrawString(text, goodFont, Brushes.Black, stringPos, stringFormat);


                start += histrect_width;
            }

            pictureBox1.Image = b;
        }

        private Font findFont(Graphics g, string myString, Size Room, Font PreferedFont)
        {
            SizeF RealSize = g.MeasureString(myString, PreferedFont);
            float HeightScaleRatio = Room.Height / RealSize.Height;
            float WidthScaleRatio = Room.Width / RealSize.Width;

            float ScaleRatio = (HeightScaleRatio < WidthScaleRatio) ? ScaleRatio = HeightScaleRatio : ScaleRatio = WidthScaleRatio;

            float ScaleFontSize = PreferedFont.Size * ScaleRatio/(2);

            return new Font(PreferedFont.FontFamily, ScaleFontSize);
        }  
    }
}