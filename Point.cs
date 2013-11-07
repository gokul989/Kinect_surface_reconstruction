using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class Point 
    {
        	Vector3Df location;
	        Vector3Df normal;
	        public Color colour;
            private int arrx, arry, lind;
           


	public Point(double x,double y, double z, double xn, double yn, double zn, float r, float g, float b , float a){
		this.location = new Vector3Df(x, y, z);
		this.normal = new Vector3Df(xn, yn, zn).normalize();
		this.colour = new Color();
        this.colour = Color.FromArgb((int)a*255,(int)r*255,(int)g*255,(int)b*255);
	}

    public Point(double x, double y, double z, double xn, double yn, double zn, int r, int g, int b, int a)
    {
        this.location = new Vector3Df(x, y, z);
        this.normal = new Vector3Df(xn, yn, zn).normalize();
        this.colour = new Color();
        this.colour = Color.FromArgb(a,r,g,b);
    }

	public Point(double x, double y, double z) : this(x,y,z,0,0,0,0,0,0,0){
	
		
	}
	
	public Point(double x,double y, double z, double xn, double yn, double zn) {
	
		this.location = new Vector3Df(x, y, z);
		this.normal = new Vector3Df(xn, yn, zn).normalize();
	}
	public Point(double x,double y, double z, float r, float g, float b)  {
	
		this.location = new Vector3Df(x, y, z);
        this.colour = new Color();
        this.colour = Color.FromArgb((int)r*255,(int)g*255,(int)b*255);
		
	}

    public Point(double x, double y, double z, Color col)
    {
        // TODO: Complete member initialization
        this.location = new Vector3Df(x, y, z);
        this.colour = col;
    }
	
	public String ToString()
	{
		String s = Convert.ToString(location.x) +"," +Convert.ToString(location.y) +"," +Convert.ToString(location.z) ;
		return s;
	}
	public void print() {
		Console.WriteLine("location is " + location.toString());
		//System.out.println("location is " + location.toString());
	}
	public String toOff() {
		String s;
        s = Convert.ToString(location.x) + "\t" + Convert.ToString(location.y) + "\t" + Convert.ToString(location.z) + "\t" + Convert.ToString((((float)colour.R) / (float)255)) + "\t" + Convert.ToString((((float)colour.G) / (float)255)) + "\t" + Convert.ToString((((float)colour.B) / (float)255)) + "\t" + Convert.ToString((((float)colour.A) / (float)255));
		return s;
	}
		
	public static double diff(Point o,Point a) {
		return Vector3Df.diff(o.location,a.location);
		
	}
    public void setArrX(int x)
    {
        this.arrx = x;
    }
    public void setArrY(int y)
    {
        this.arry = y;
    }
    public int getArrX()
    {
        return arrx;
    }
    public int getArrY()
    {
        return arry;
    }
    public int getListInd()
    {
        return lind;
    }
    public void setListInd(int l)
    {
       this.lind = l;
    }
    }
    
}
