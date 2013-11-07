using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Form load event-------------------");

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Console.WriteLine("Form load resize-------------------");

        }
        private void Form1_Move(object sender, EventArgs e)
        {
            Console.WriteLine("Form moved-------------------");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.tiltUp();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.tiltDown();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.takeSnapshot = true;
        }

    }
}
