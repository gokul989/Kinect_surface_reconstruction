using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace WindowsFormsApplication1
{
    class Face
    {
        int count, f1, f2, f3;
        Color col;

        public Face(int c, int f1, int f2, int f3)
        {
            count = c;
            this.f1 = f1;
            this.f2 = f2;
            this.f3 = f3;
        }

        public Face(int c, int f1, int f2, int f3,Color col) : this(c,f1,f2,f3)
        {
            this.col = col;
        }
        public String toOff()
        {
            String s;
            s = count + "\t" + f1 + "\t" + f2 + "\t" + f3 + "\t" + Convert.ToString((((float)col.R) / (float)255)) + "\t" + Convert.ToString((((float)col.G) / (float)255)) + "\t" + Convert.ToString((((float)col.B) / (float)255)) + "\t" + Convert.ToString((((float)col.A) / (float)255));
            return s;
        }
    }
}
