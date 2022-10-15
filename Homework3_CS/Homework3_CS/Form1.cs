using System.Globalization;
using System.Net.Sockets;
using System.Windows.Forms.VisualStyles;

namespace Homework3_CS
{
    public partial class Form1 : Form
    {
        class Packet
        {
            public int id;
            public double time;
            public string source;
            public string destination;
            public string protocol;
            public int length;
            public string info;
        }

        class Protocol
        {
            public string id;
            public int counter;
        }
        public Form1()
        {
            InitializeComponent();
        }

        int row;
        List<Packet> packets = new List<Packet>();
        List<Protocol> protocols = new List<Protocol>();
        bool check;
        string choice;
        string file;

        

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Refresh();
            packets.Clear();
            //this.button1.Enabled = false;
            choice = this.comboBox1.Text;
            if (choice == "File 1")
            {
                file = @"stream_home1.csv";
            }
            else if (choice == "File 2")
            {
                file = @"stream_home2.csv";
            }
            else if (choice == "File 3")
            {
                file = @"stream_home3.csv";
            }
            else file = @"wireshark.csv";

            using (var reader = new StreamReader(file)) 
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";

                var line = reader.ReadLine();
                var header = line.Split(',');
                foreach(var s in header)
                {
                    this.dataGridView1.Columns.Add(s.ToString(), s.ToString().Replace("\"", ""));
                }
                
                while (!reader.EndOfStream)
                {
                    
                    line = reader.ReadLine();
                    var values = line.Split(',');

                    Packet packet = new Packet();

                    Protocol protocol = new Protocol();
                    protocol.id = values[4];
                    protocol.counter = 1;

                    check = false;
                    foreach(Protocol p in protocols)
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

                    int i = 0;
                    while (i < 7){
                        values[i] = values[i].Replace("\"", "");
                        i++;
                    }
                    dataGridView1.Rows.Add(values);
                    packet.id = Convert.ToInt32(values[0].Replace("\"", ""));
                    packet.time = Convert.ToDouble(values[1].Replace("\"", ""));
                    packet.source = values[2];
                    packet.destination = values[3];
                    packet.protocol = values[4];
                    packet.length  = Convert.ToInt32(values[5].Replace("\"", ""));
                    packet.info = values[6];

                    packets.Add(packet);

                }


            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();
            this.richTextBox1.AppendText("Out of "+ packets.Count.ToString() +" packets we have:\n");
            foreach(Protocol p in protocols)
            {
                this.richTextBox1.AppendText("Packets with protocol " + p.id + ": " + p.counter.ToString()+"\n");
                
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}