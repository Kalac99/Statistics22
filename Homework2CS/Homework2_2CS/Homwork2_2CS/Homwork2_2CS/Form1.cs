using System.Globalization;

namespace Homwork2_2CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class Student
        {
            public string Name;
            public string Sex;
            public double Weight;
            public double Height;
            public string HairColor;
            public string EyeColor;
            public double Age;
            public double ShoeSize;
            public double NSiblings;
            public double NCars;
            public string FavoriteHobby;
            public string Smoker;
            public double NPets;
            public string Work;
            public double FNumber;

        }
        int males = 0;
        int females = 0;
        int notdefined = 0;
        int nstudents = 0;

        List<Student> students = new List<Student>();

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;

            using (var reader = new StreamReader(@"Statistics_students_dataset_22_23.csv"))
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";

                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    nstudents++;

                    Student student = new Student();

                    if (values[0].Length>1) student.Name = values[0]; else student.Name = "Null";
                    if (values[1].Length > 1) student.Sex = values[1]; else student.Sex = "Null";
                    if (values[2].Length > 1) student.Weight = Convert.ToDouble(values[2]); else student.Weight = 0;
                    if (values[3].Length > 1) student.Height = Convert.ToDouble(values[3]); else student.Height = 0;
                    if (values[4].Length > 1) student.HairColor = values[4]; else student.HairColor = "Null";
                    if (values[5].Length > 1) student.EyeColor = values[5]; else student.EyeColor = "Null";
                    if (values[6].Length > 1) student.Age = Convert.ToDouble(values[6]); else student.Age = 0;
                    if (values[7].Length > 1) student.ShoeSize = Convert.ToDouble(values[7]); else student.ShoeSize = 0;
                    if (values[8].Length > 1) student.NSiblings = Convert.ToDouble(values[8]); else student.NSiblings = 0;
                    if (values[9].Length > 1) student.NCars = Convert.ToDouble(values[9]); else student.NCars = 0;
                    if (values[10].Length > 1) student.FavoriteHobby = values[10]; else student.FavoriteHobby = "Null";
                    if (values[11].Length > 1) student.Smoker = values[11]; else student.Smoker = "Null";
                    if (values[12].Length > 1) student.NPets = Convert.ToDouble(values[12]); else student.Work = "Null";
                    if (values[13].Length > 1) student.Work = values[13]; else student.Work = "Null";
                    if (values[14].Length > 1) student.FNumber = Convert.ToDouble(values[14]); else student.FNumber = 0;

                    students.Add(student);
                }

                this.richTextBox1.AppendText("Name".PadRight(45) + "Sex".PadRight(15) + "Weight".PadRight(15) + "Height".PadRight(15) + "Hair Color".PadRight(15) + "Eye Color".PadRight(15) + "Age".PadRight(15) + "Shoe Size".PadRight(15) + "Number of siblings".PadRight(15) + "Number of cars".PadRight(15) + "Favorite hobby".PadRight(15) + "Smoker".PadRight(15) + "Number of pets".PadRight(15) + "Work".PadRight(15) + "Favorite number\n");
                foreach (Student s in students)
                {
                    this.richTextBox1.AppendText(s.Name.PadRight(45) + s.Sex.PadRight(30) + s.Weight.ToString().PadRight(15) + s.Height.ToString().PadRight(15) + s.HairColor.PadRight(15) + s.EyeColor.PadRight(15) + s.Age.ToString().PadRight(15) + s.ShoeSize.ToString().PadRight(15) + s.NSiblings.ToString().PadRight(15) + s.NCars.ToString().PadRight(15) + s.FavoriteHobby.PadRight(15) + s.Smoker.PadRight(15) + s.NPets.ToString().PadRight(15) + s.Work.PadRight(15) + s.FNumber.ToString().PadRight(15) + "\n");
                    if (s.Sex.ToLower() == "male") males++;
                    else if (s.Sex.ToLower() == "female") females++;
                    else notdefined++;
                }
            }

            this.button2.Enabled = true;


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox2.Clear();
            this.richTextBox2.AppendText("Out of "+nstudents.ToString()+" students \n\n" + " Number of male students: " + males.ToString() + "\n Number of female students: " + females.ToString() + "\n Number of typos or other:" + notdefined.ToString());
        }
    }
}