using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Vector3Df
    {
        static Vector3Df zaxis = new Vector3Df(0, 0, 1.0f);
        public double x, y, z;
        double length;

        public Vector3Df(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.length = x * x + y * y + z * z;
            this.length = Math.Sqrt(length);

        }

        public Vector3Df normalize()
        {
            // TODO Auto-generated method stub

            this.x = this.x / length;
            this.y = this.y / length;
            this.z = this.z / length;
            this.length = 1;
            return this;
        }

        public String toString()
        {
            String s = " x = " + x + ", y = " + y + ", z  = " + z;
            return s;
        }

        public static double dot(Vector3Df a, Vector3Df b)
        {
            return (a.x * b.x + a.y * b.y + a.z * b.z);
        }

        public static double diff(Vector3Df l, Vector3Df m)
        {
            // TODO Auto-generated method stub
            return (Math.Abs(l.x - m.x) + Math.Abs(l.y - m.y) + Math.Abs(l.z - m.z));
        }
    }
}
