using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class OffData
    {
        Point[] pointlist;
        List<Point> arrlist;
        List<Face> facelist;
        int vertices, faces, edges;
        Color c; 
        Point[,] MapArray;
        public OffData(Point[] pointlist, List<Face> facelist, int vertices, int faces, int edges)
        {
            this.pointlist = pointlist;
            this.facelist = facelist;
            this.vertices = vertices;
            this.edges = edges;
            this.faces = faces;
        }
        public OffData(List<Point> pointlist, Point[,] point2d)
        {
            this.arrlist = pointlist;
            vertices = pointlist.Count;
            this.MapArray = point2d;
            this.faces = 0;
            this.edges = 0;
        }
        public void getFaces()
        {
            int i = 0,j=0;
            facelist = new List<Face>();
            for (i = 0; i < MapArray.GetLength(0); ++i)
            {
                for (j = 0; j < MapArray.GetLength(1); ++j)
                {
                    if (MapArray[i, j] != null)
                    {
                        if (i+1<MapArray.GetLength(0)&&j+1<MapArray.GetLength(1) && MapArray[i, j + 1] != null && MapArray[i + 1, j]!= null)
                        {
                            c = getAvgColor(MapArray[i, j].colour, MapArray[i, j + 1].colour, MapArray[i + 1, j].colour);
                            facelist.Add(new Face(3, MapArray[i, j].getListInd(), MapArray[i, j + 1].getListInd(), MapArray[i + 1, j].getListInd(),c));
                        }
                        if (j+1<MapArray.GetLength(1)&& i - 1 >=0 &&MapArray[i, j + 1] != null && MapArray[i - 1, j + 1] != null)
                        {
                            facelist.Add(new Face(3, MapArray[i, j].getListInd(), MapArray[i, j + 1].getListInd(), MapArray[i - 1, j + 1].getListInd()));
                        }
                    }


                }
            }

            faces = facelist.Count;
            return;

        }

        private Color getAvgColor(Color c1, Color c2, Color c3)
        {
            Color outp = new Color();
            int r, g, b,a;
            r = c1.R + c2.R + c3.R;
            r = r / 3;
            g = c1.G + c2.G + c3.G;
            g = g / 3;
            b = c1.B + c2.B + c3.B;
            b = b / 3;
            a = c1.A + c2.A + c3.A;
            a = a / 3;
            outp = Color.FromArgb(a, r, g, b);
            return outp;
            
        }
        public void writeToFile()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"offFile.off"))
            {
                file.WriteLine("COFF");
                file.WriteLine(vertices + "\t" + faces + "\t" + edges );
                foreach(Point p in arrlist )
                {
                    file.WriteLine(p.toOff());
                }
                foreach (Face f in facelist)
                {
                    file.WriteLine(f.toOff());
                }

            }
        }


    }

}
