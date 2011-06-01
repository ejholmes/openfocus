using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ASCOM.OpenFocus
{
    public class FocusMax
    {
        public struct Point
        {
            public double temperature;
            public UInt16 position;
        }

        public static Point[] ParseFile(string filename)
        {
            List<Point> points = new List<Point>();

            StreamReader fp = new StreamReader(filename);

            string line = String.Empty;

            while ((line = fp.ReadLine()) != null)
            {
                string[] tokens = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    Point p;
                    p.temperature = double.Parse(tokens[1]);
                    p.position = UInt16.Parse(tokens[3]);
                    points.Add(p);
                }
                catch { }
            }

            fp.Close();

            return points.ToArray();
        }

        public static double Slope(Point[] points)
        {
            double xAvg = 0, yAvg = 0;

            for (int i = 0; i < points.Length; i++)
            {
                xAvg += points[i].temperature;
                yAvg += points[i].position;
            }

            xAvg = xAvg / points.Length;
            yAvg = yAvg / points.Length;

            double v1 = 0;
            double v2 = 0;

            for (int i = 0; i < points.Length; i++)
            {
                v1 += (points[i].temperature - xAvg) * (points[i].position - yAvg);
                v2 += Math.Pow(points[i].temperature - xAvg, 2);
            }

            double a = v1 / v2;

            return a;
        }
    }
}
