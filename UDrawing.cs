using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modelowanie
{
    class UDrawing
    {
        #region Fields

        public static Pen pen
        {
            get
            {
                return new Pen(Brushes.SteelBlue, (int)Math.Log(fontsize, 3));
            }
        }

        private static Brush br = Brushes.White;

        public static FontFamily fontFamily = new FontFamily("Arial");

        public static double fontsize = 12;

        public static string sA, sB, sOp;

        public static string eA, eB, eC;

        public static char oper = ' ';

        public DrawingContext dc;

        string textpion = "OPERACJA SEKWENCJONOWANIA PIONOWEGO:";
        #endregion

        #region Initalizers

        public UDrawing(DrawingContext drawingContext)
        {
            dc = drawingContext;
        }

        #endregion

        #region Public Methods
        public void Redraw()
        {
            if (oper != ' ')
            {
                DrawSwitched(new Point(20, fontsize + 30));
            }
            else
            {
                if (sA != "")
                {
                    DrawSekPion(new Point(30, fontsize + 30));
                }
                if (eA != "")
                {
                    DrawSekPoziom(new Point(30, fontsize * 3 + 30));
                }
            }
        }

        public static void ClearAll()
        {
            sA = sB = sOp = "";
            eA = eB = eC = "";
            oper = ' ';
        }

        public void DrawSekPion(Point pt)
        {
            if (sA == "" || sOp == "") return;
            int len = GetTextHeight(sA) * 3 + 10;


           // DrawText(pt, textpion);
            DrawText(pt, "\n\n" + sA);
            DrawText(pt, "\n\n\n" + sOp);
            DrawText(pt, "\n\n\n\n" + sB);
            DrawBezierPion(new Point(pt.X, pt.Y + (GetTextHeight(textpion) * 2) - 1), len);
            
        }

        public void DrawSekPoziom(Point pt)
        {

            if (eA == "" || eB == "" || eC == "") return;

            Point p2 = new Point(pt.X + 2, pt.Y);
            string text = Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + eA + eC + eB;
            string text1 = Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + Environment.NewLine.ToString() + eA + eC + eB;
          string textpoz = "OPERACJA SEKWENCJONOWANIA POZIOMEGO:";
           DrawText(new Point(pt.X, pt.Y + (GetTextHeight(textpion) * 2) + (GetTextHeight(eA)) * 4), textpoz);
            int l = GetTextLength(text) + 7;
            int l1 = GetTextHeight(text1);
            DrawText(p2, text);
            DrawBezierPoziom(new Point(pt.X, pt.Y + l1), l);
        }

        public void DrawSwitched(Point pt)
        {
            if (sA == "" || sOp == "" || eC == "" || eA == "" || eB == "" || eC == "") return;


            string text = sOp + Environment.NewLine.ToString() + sB;
            string text1 = eA + eC + eB;
            string text2 = sA + Environment.NewLine.ToString() + sOp;

            int length = GetTextHeight(text);
            int length1 = GetTextLength(eA + eB + eC) + 7;
            int length2 = GetTextHeight(text2);
            int length3 = GetTextHeight(sB);

            if (oper == 'A')
            {

                DrawBezierPion(new Point(pt.X, pt.Y + 5), length * 2 + 5);
                DrawText(new Point(pt.X + 5, pt.Y + length3 + 20), text);
                DrawBezierPoziom(new Point(pt.X + 5, pt.Y + 17), length1 + 5);
                DrawText(new Point(pt.X + 7, pt.Y + 19), text1);
            }
            if (oper == 'B')
            {
                DrawText(new Point(pt.X + 5, pt.Y + 5), text2);
                DrawBezierPion(new Point(pt.X, pt.Y + 5), length + length2);
                DrawBezierPoziom(new Point(pt.X + 5, pt.Y + length2 + 15), length1 + 5);
                DrawText(new Point(pt.X + 7, pt.Y + length2 + 17), text1);

            }


        }
        #endregion

        #region Private Methods


        private void DrawBezierPion(Point p0, int length)
        {
            Point start = p0;
            Point p1 = new Point(), p2 = new Point(), p3 = new Point();

            p3.X = p0.X;
            p3.Y = p0.Y + length;

            int b = (int)Math.Sqrt(length) + 2;

            p1.Y = p0.Y + (int)(length * 0.25);
            p1.X = p0.X - b;

            p2.Y = p0.Y + (int)(length * 0.75);
            p2.X = p0.X - b;

            foreach (Point pt in GetBezierPoints(p0, p1, p2, p3))
            {
                dc.DrawLine(pen, start, pt);
                start = pt;
            }
        }

        private void DrawBezierPoziom(Point poz0, int length1)
        {
            Point start1 = poz0;
            Point poz1 = new Point(), poz2 = new Point(), poz3 = new Point();

            poz3.Y = poz0.Y;
            poz3.X = poz0.X + length1;

            int b = (int)Math.Sqrt(length1) + 2;

            poz1.X = poz0.X + (int)(length1 * 0.25);
            poz1.Y = poz0.Y - b;

            poz2.X = poz0.X + (int)(length1 * 0.75);
            poz2.Y = poz0.Y - b;

            foreach (Point pt1 in GetBezierPoints(poz0, poz1, poz2, poz3))
            {
                dc.DrawLine(pen, start1, pt1);
                start1 = pt1;
            }
        }

        private void DrawText(Point point, string text)
        {
            dc.DrawText(GetFormattedText(text), point);
        }

        private int GetTextHeight(string text)
        {
            return (int)GetFormattedText(text).Height;
        }

        private int GetTextLength(string text)
        {
            return (int)GetFormattedText(text).Width;
        }

        private IEnumerable<Point> GetBezierPoints(Point A, Point B, Point C, Point D)
        {
            List<Point> points = new List<Point>();

            for (double t = 0.0d; t <= 1.0; t += 1.0 / 500)
            {
                double tbs = Math.Pow(t, 2);
                double tbc = Math.Pow(t, 3);
                double tas = Math.Pow((1 - t), 2);
                double tac = Math.Pow((1 - t), 3);

                points.Add(new Point
                {
                    X = +tac * A.X
                        + 3 * t * tas * B.X
                        + 3 * tbs * (1 - t) * C.X
                        + tbc * D.X,
                    Y = +tac * A.Y
                        + 3 * t * tas * B.Y
                        + 3 * tbs * (1 - t) * C.Y
                        + tbc * D.Y
                });
            }

            return points;
        }

        private FormattedText GetFormattedText(string text)
        {
            FontStyle style = FontStyles.Normal;

            style = FontStyles.Normal;
            Typeface typeface = new Typeface(fontFamily, style, FontWeights.Light, FontStretches.Medium);

            FormattedText formattedText = new FormattedText(text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface, fontsize, Brushes.Black);

            formattedText.TextAlignment = TextAlignment.Left;

            return formattedText;
        }

        #endregion
    }
}

