using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace WindowsFormsApplication1
{
    static class Program
    {
        static Form1 form;
        static KinectSensor sensor;
        static volatile int nearX, nearY;
        static bool flag = false, depthFlag = false;
        static byte[] imagePixelData;
        static Bitmap bmap;
        static short[] depthPixelData;
        static int[,] depth;
        static ColorImagePoint cp;
        static DepthImagePoint dp;
        public static bool takeSnapshot = false;
        static int[,] snapshotDepthData;
        static double HORIZONTAL_TAN = Math.Tan((57D * Math.PI) / 360D);
        static double VERTICAL_TAN = Math.Tan((43D * Math.PI) / 360D);
        static double KINECT_TO_KINECT = 2.6D;
        static double HEAD_SIZE = 1.2D;
        static double HEAD_SIZE_PIXELS = 100;
        public static bool block =false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            nearX = 320;
            nearY = 300;
           
            form = new Form1();
            form.FormClosing += CloseForm1;
            sensor = KinectSensor.KinectSensors[0];

            ///sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
           // sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.SkeletonStream.Enable();

            sensor.AllFramesReady += AllFramesReady;
            block = false;
            sensor.Start();
            //sensor.ElevationAngle -= 10;
          //  Console.WriteLine("\nfoobar" + sensor.IsRunning);
            Application.Run(form);
           // Console.WriteLine("\n*************"+form);
           
        }

        static void AllFramesReady(Object sender, AllFramesReadyEventArgs e)
        {
            
          //  Console.WriteLine("AllFramesReady() called");
            if (e != null)
            {
                SkeletonFrame sFrame = e.OpenSkeletonFrame();


                CoordinateMapper cm = new CoordinateMapper(sensor);
                if (sFrame != null)
                {
                  //  Console.WriteLine("Got Skeleton");
                    Skeleton[] skeletons = new Skeleton[sFrame.SkeletonArrayLength];
                    sFrame.CopySkeletonDataTo(skeletons);
                    SkeletonPoint sLoc = new SkeletonPoint();
                    foreach (Skeleton s in skeletons)
                    {
                        if (s.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            sLoc = s.Joints[JointType.Head].Position;
                            DepthFrameReady(sender, e.OpenDepthImageFrame(),e.OpenColorImageFrame(), sLoc);
                          //  Console.WriteLine("Head coordinates: " + sLoc.X + "," + sLoc.Y + "," + sLoc.Z);
                            ColorImagePoint cLoc = cm.MapSkeletonPointToColorPoint(sLoc, ColorImageFormat.RgbResolution640x480Fps30);
                            DepthImagePoint dLoc = cm.MapSkeletonPointToDepthPoint(sLoc, DepthImageFormat.Resolution640x480Fps30);
                            Console.WriteLine("Head coordinates: " + dLoc.X + "," + dLoc.Y);
                          
                        }
                    }
                    sFrame.Dispose();
                }

                if (block == false)
                {
                    dp = new DepthImagePoint();
                    cp = new ColorImagePoint();
                    cp.X = 320;
                    cp.Y = 240;
                    dp.X = 320;
                    dp.Y = 240;
                    ////cp = cLoc;
                    //cp = cm.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, dp, ColorImageFormat.RgbResolution640x480Fps30);
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + cp.X + "    " + cp.Y);
                    //dp = dLoc;
                }
                // ColorImagePoint cLoc = sensor.MapSkeletonPointToColor(sLoc, e.OpenColorImageFrame().Format);
                // nearX = dLoc.X;
                // nearY = dLoc.Y;
                nearX = dp.X;
                nearY = dp.Y;
                 ImageFrameReady(sender, e.OpenColorImageFrame());
            }
        }

        static void ImageFrameReady(Object sender, ColorImageFrame imageFrame)
        {
            //Console.WriteLine("Foobar");
            if (imageFrame != null)
            {
                Bitmap bmap = ImageToBitmap(imageFrame,true);
                form.GetPictureBox1().Image = bmap;
                imageFrame.Dispose();
            }
        }

        static void DepthFrameReady(Object sender, DepthImageFrame imageFrame,ColorImageFrame cimgframe, SkeletonPoint sLoc)
        {
            //Console.WriteLine("Depth");
            if (imageFrame != null && cimgframe != null)
            {
                //form.GetPictureBox1().Image = DepthToBitmap(imageFrame);

                if (takeSnapshot)
                {
                    Point[,] pointarr = new Point[(int)HEAD_SIZE_PIXELS * 2+ 1, (int)HEAD_SIZE_PIXELS * 2 + 1] ;
                    block = true;
                    List<Point> plist = new List<Point>();
                    Bitmap bmap = ImageToBitmap(cimgframe,false);
                    DateTime d = DateTime.Now;
                    Color col;
                    List<String> slist = new List<String>();
                    String dtm = d.ToString();
                    String dtmr = "data" + dtm + "Reverse.txt";
                    dtm = "data" + dtm + ".txt";
                    Console.WriteLine(dtm);
                    CoordinateMapper cm = new CoordinateMapper(sensor);
                    snapshotDepthData = GetDepthArray(imageFrame);
                    double faceZ = (double)snapshotDepthData[dp.Y, dp.X] / 1000D;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"data.txt"))
                    using (System.IO.StreamWriter colorfile = new System.IO.StreamWriter(@"colordata.txt"))
                    using (System.IO.StreamWriter fileReverse = new System.IO.StreamWriter(@"dataReverse.txt"))
                    {
                        for (int x = 0; x < snapshotDepthData.GetLength(1); x++)
                        {
                            for (int y = 0; y < snapshotDepthData.GetLength(0); y++)
                            {
                                
                                if (Math.Abs(x - dp.X) <= HEAD_SIZE_PIXELS && Math.Abs(y - dp.Y) <= HEAD_SIZE_PIXELS)
                                {
                                    /*dp.X = x;
                                    dp.Y = y;
                                    ColorImagePoint c = cm.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, dp, ColorImageFormat.RgbResolution640x480Fps30);
                                    c = imageFrame.MapToColorImagePoint(x, y, ColorImageFormat.RgbResolution640x480Fps30);
                                    Console.WriteLine("dp.X: " + dp.X +"dp.Y: " + dp.Y +"c.X: " + c.X +"c.Y: " + c.Y);
                                    if(c.X < 640 && c.X >= 0 && c.Y < 480 && c.Y>=0)
                                    {
                                        col = bmap.GetPixel(c.X, c.Y);
                                    }
                                    else
                                    {
                                        col = bmap.GetPixel(nearX, nearY);
                                    }*/
                                 
                                   /* if (Math.Abs(x - dp.X) <= 10 && Math.Abs(y - dp.Y) <= 10)
                                    {
                                        col = Color.LightYellow;
                                    }*/
                                    
                                    col = bmap.GetPixel(x+(cp.X-dp.X), y+(cp.Y-dp.Y));
                                    float r, g, b;
                                    r = (float)col.R;
                                    r = r / 255;
                                    g = (float)col.G;
                                    g = g / 255;
                                    b = (float)col.B;
                                    b = b / 255;
                                    double newX = -((double)((x - imageFrame.Width / 2) * HORIZONTAL_TAN * snapshotDepthData[y, x])) / (1000 * (double)(imageFrame.Width / 2));
                                    double newY = ((double)((y - imageFrame.Height / 2) * VERTICAL_TAN * snapshotDepthData[y, x]) / (1000 * (double)(imageFrame.Height / 2)));
                                    double newZ = (double)snapshotDepthData[y, x] / 1000D;
                                    if (Math.Abs(newZ - faceZ) <= HEAD_SIZE / 2)
                                    {
                                        r = 0;
                                        g = 0;
                                        b = 0;
                                        file.WriteLine(newX + " " + newY + " " + newZ);
                                        colorfile.WriteLine(newX + " " + newY + " " + newZ + " " + r.ToString() + " " + g.ToString() + " " + b.ToString() + " " + "1.0");
                                        String s = new String(new char[] { });
                                        s = newX + " " + newY + " " + newZ + " " + r.ToString() + " " + g.ToString() + " " + b.ToString() + " " + "1.0";
                                        slist.Add(s);
                                        pointarr[x + (int)HEAD_SIZE_PIXELS - dp.X, y + (int)HEAD_SIZE_PIXELS - dp.Y] = new Point(newX, newY, newZ, col);
                                        pointarr[x + (int)HEAD_SIZE_PIXELS - dp.X, y + (int)HEAD_SIZE_PIXELS - dp.Y].setArrX(x + (int)HEAD_SIZE_PIXELS - dp.X);
                                        pointarr[x + (int)HEAD_SIZE_PIXELS - dp.X, y + (int)HEAD_SIZE_PIXELS - dp.Y].setArrY(y + (int)HEAD_SIZE_PIXELS - dp.Y);
                                        pointarr[x + (int)HEAD_SIZE_PIXELS - dp.X, y + (int)HEAD_SIZE_PIXELS - dp.Y].setListInd(slist.Count-1);
                                        plist.Add( pointarr[x + (int)HEAD_SIZE_PIXELS - dp.X, y + (int)HEAD_SIZE_PIXELS - dp.Y]);

                                    }
                                    else {
                                        pointarr[x + (int)HEAD_SIZE_PIXELS - dp.X, y + (int)HEAD_SIZE_PIXELS - dp.Y] = null;
                                    }
                                }
                              
                            }
                        }
                    }
                    int vert = slist.Count;
                    OffData dat = new OffData(plist, pointarr);
                    dat.getFaces();
                    dat.writeToFile();

                     using (System.IO.StreamWriter fileoff = new System.IO.StreamWriter(@"fulldatacolor.off"))
                     {
                          fileoff.WriteLine("COFF");
                          fileoff.WriteLine(vert.ToString() + "\t0\t0");
                          int i = 0;
                         for(i=0;i<vert;++i)
                         {
                             fileoff.WriteLine(slist.ElementAt(i));
                         }
                     }
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = @"rcocone-win.exe";
                    startInfo.Arguments = @"data.txt output";
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    using (Process proc = Process.Start(startInfo))
                    {
                        proc.WaitForExit();

                        // Retrieve the app's exit code
                        //exitCode = proc.ExitCode;
                    }
                    //Process.Start(startInfo);
                    takeSnapshot = false;
                    block = false;
                }

                imageFrame.Dispose();
            }
        }

        static int getDepthValue(DepthImageFrame imageFrame, int x, int y)
        {
            short[] pixelData = new short[imageFrame.PixelDataLength];
        
            return ((ushort)imagePixelData[x + y * imageFrame.Width]) >> 3;
        }

        static int[,] GetDepthArray(DepthImageFrame imageFrame)
        {
            int rows =imageFrame.Height;
            int columns = imageFrame.Width;
           
            if (!depthFlag)
            {
                depthPixelData = new short[imageFrame.PixelDataLength];
                depth = new int[imageFrame.Height, imageFrame.Width];
                depthFlag = true;
            }
          
            imageFrame.CopyPixelDataTo(depthPixelData);
            int minI = 0, minJ = 0;
            int min = ((ushort)depthPixelData[0]) >> 3;
            for (int i = 0; i < imageFrame.Height; i++)
            {
                for (int j = 0; j < imageFrame.Width; j++)
                {

                  depth[i, j] = ((ushort)depthPixelData[j + i * imageFrame.Width]) >> 3;
                  if (depth[i, j] < min)
                  {
                      min = depth[i, j];
                      minI = i;
                      minJ = j;
                  }
                }
            }
            //nearX = minJ;
            //nearY = minI;

            return depth;
        }

        static Bitmap DepthToBitmap(DepthImageFrame imageFrame)
        {
            short[] pixelData = new short[imageFrame.PixelDataLength];
            imageFrame.CopyPixelDataTo(pixelData);

            Bitmap bmap = new Bitmap(
            imageFrame.Width,
            imageFrame.Height,
            PixelFormat.Format16bppRgb555);

            BitmapData bmapdata = bmap.LockBits(
             new Rectangle(0, 0, imageFrame.Width,
                                    imageFrame.Height),
             ImageLockMode.WriteOnly,
             bmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(pixelData,
             0,
             ptr,
             imageFrame.Width *
               imageFrame.Height);
            bmap.UnlockBits(bmapdata);
            return bmap;
        }

        static Bitmap ImageToBitmap(ColorImageFrame Image,bool makebox)
        {
            if (!flag)
            {
                imagePixelData = new byte[Image.PixelDataLength];
                bmap = new Bitmap(Image.Width, Image.Height, PixelFormat.Format32bppRgb);
                
                flag = true;
            }
            Image.CopyPixelDataTo(imagePixelData);
            BitmapData bmapdata = bmap.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.WriteOnly, bmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(imagePixelData, 0, ptr, Image.PixelDataLength);
            bmap.UnlockBits(bmapdata);
           if (makebox)
            
            {
               // makeBox(bmap, cp.X, cp.Y, 20);
                makeBox(bmap, cp.X, cp.Y, 40);
            }
            return bmap;
        }

        static void makeBox(Bitmap bmap, int x, int y, int width)
        {
            for (int i = x - width / 2; i < x + width / 2; i++)
            {
                for (int j = y - width / 2; j < y + width / 2; j++)
                {
                    if (inRange(i, j, bmap))
                    {
                        bmap.SetPixel(i, j, Color.BlanchedAlmond);
                    }
                }
            }
        }

        private static bool inRange(int i, int j, Bitmap bmap)
        {
            return (i >= 0 && j >= 0 && i < bmap.Width && j < bmap.Height);
        }

        static void CloseForm1(Object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("closing------------------------------");
            sensor.Stop();
            Application.Exit();
        }

        public static void tiltDown()
        {
            sensor.ElevationAngle -= 5;
        }

        public static void tiltUp()
        {
            sensor.ElevationAngle += 5;
        }

    }
}
