using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WindowsFormsApp1;

namespace WindowsFormsApp1
{
    [Serializable]
    public abstract class Figure
    {
        protected Point previousPoint = Point.Empty;

        bool isDown = false;
        public Point point1 { get; set; }
        public Point point2 { get; set; }
        public Color LineColor { get; set; }
        public Color FillColor { get; set; }
        public float LineThickness { get; set; }
        public string FigureType { get; set; }
        public List<Point> CurvePoints { get; set; }
        public Figure(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        abstract public void MouseMove(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int h);
        abstract public void DrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH);
        abstract public void ResizeDrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH);
        abstract public void FillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH);
        abstract public void ResizeFillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH);
        abstract public void Draw(Graphics g, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH, List<Point> curvePoints);
        abstract public void DrawDash(Graphics g, Point point, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH);
        abstract public void Hide(List<Figure> figures);
        abstract public void NewDraw(Graphics g, List<Figure> figures, int hW, int hH);
    }

    [Serializable]
    class Rect : Figure
    {
        private Color black = Color.Black;
        private Color white = Color.White;
        public Rect(Point point1, Point point2) : base(point1, point2) { }

        public override void FillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
        {
            Point[] points = new Point[]
            {
                new Point(p1.X, p1.Y),
                new Point(p1.X, p2.Y),
                new Point(p2.X, p2.Y),
                new Point(p2.X, p1.Y)
            };
            Brush brush = new SolidBrush(fillColor);

            g.FillPolygon(brush, points);
        }

        public override void ResizeFillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
        {
            Point[] points = new Point[]
            {
                new Point(p1.X + hW, p1.Y + hH),
                new Point(p1.X + hW, p2.Y + hH),
                new Point(p2.X + hW, p2.Y + hH),
                new Point(p2.X + hW, p1.Y + hH)
            };
            Brush brush = new SolidBrush(fillColor);

            g.FillPolygon(brush, points);
        }

        public override void DrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
        {
            if (lineTh == 1) lineTh = 0;
            //g.DrawLine(pen, p1.X + hW, (p1.Y - lineTh / 2) + hH, p1.X + hW, (p2.Y + lineTh / 2) + hH);
            //g.DrawLine(pen, (p1.X - lineTh / 2) + hW, p1.Y + hH, (p2.X + lineTh / 2) + hW, p1.Y + hH);
            //g.DrawLine(pen, p2.X + hW, (p2.Y + lineTh / 2) + hH, p2.X + hW, (p1.Y - lineTh / 2) + hH);
            //g.DrawLine(pen, (p2.X + lineTh / 2) + hW, p2.Y + hH, (p1.X - lineTh / 2) + hW, p2.Y + hH);

            g.DrawLine(pen, p1.X, (p1.Y - lineTh / 2), p1.X, (p2.Y + lineTh / 2));
            g.DrawLine(pen, (p1.X - lineTh / 2), p1.Y, (p2.X + lineTh / 2), p1.Y);
            g.DrawLine(pen, p2.X, (p2.Y + lineTh / 2), p2.X, (p1.Y - lineTh / 2));
            g.DrawLine(pen, (p2.X + lineTh / 2), p2.Y, (p1.X - lineTh / 2), p2.Y);
        }

        public override void ResizeDrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
        {
            if (lineTh == 1) lineTh = 0;
            g.DrawLine(pen, p1.X + hW, (p1.Y - lineTh / 2) + hH, p1.X + hW, (p2.Y + lineTh / 2) + hH);
            g.DrawLine(pen, (p1.X - lineTh / 2) + hW, p1.Y + hH, (p2.X + lineTh / 2) + hW, p1.Y + hH);
            g.DrawLine(pen, p2.X + hW, (p2.Y + lineTh / 2) + hH, p2.X + hW, (p1.Y - lineTh / 2) + hH);
            g.DrawLine(pen, (p2.X + lineTh / 2) + hW, p2.Y + hH, (p1.X - lineTh / 2) + hW, p2.Y + hH);
        }

        public override void Draw(Graphics g, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH, List<Point> curvePoints)
        {
            previousPoint = Point.Empty;

            Point P1 = new Point(point1.X - hW, point1.Y - hH);
            Point P2 = new Point(point2.X - hW, point2.Y - hH);

            Figure fig = new Rect(P1, P2);
            fig.LineColor = lineColor;
            fig.LineThickness = lineThickness;
            fig.FillColor = fillColor;
            fig.FigureType = "Rect";
            fig.CurvePoints = curvePoints;

            figures.Add(fig);

            FillFigure(g, point1, point2, fillColor, lineThickness, 0, 0);
            DrawFigure(g, new Pen(lineColor, lineThickness), point1, point2, lineThickness, 0, 0);
        }

        public override void DrawDash(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
        {

            this.point2 = point2;

            float[] dashValues = { 4, 2 };
            Pen dashPen = new Pen(lineColor, 1);
            dashPen.DashPattern = dashValues;

            NewDraw(g, figures, hW, hH);

            DrawFigure(g, new Pen(white), point1, previousPoint, 0, 0, 0);


            previousPoint = point2;

            DrawFigure(g, dashPen, point1, point2, lineThickness, 0, 0);
        }

        public override void NewDraw(Graphics g, List<Figure> figures, int hW, int hH)
        {
            foreach (Figure figure in figures)
            {
                if (figure.FigureType == "Line")
                {
                    float lineTh = figure.LineThickness;

                    if (lineTh == 1) lineTh = 0;

                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                    g.DrawLine(pen, figure.point1.X + hW, figure.point1.Y + hH, figure.point2.X + hW, figure.point2.Y + hH);
                }
                else if (figure.FigureType == "Rect")
                {
                    Point[] points = new Point[]
{
                    new Point(figure.point1.X + hW, figure.point1.Y + hH),
                    new Point(figure.point1.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point1.Y + hH)
};
                    Brush brush = new SolidBrush(figure.FillColor);

                    g.FillPolygon(brush, points);

                    float lineTh = figure.LineThickness;

                    if (lineTh == 1) lineTh = 0;

                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                    g.DrawLine(pen, figure.point1.X + hW, (figure.point1.Y - lineTh / 2) + hH, figure.point1.X + hW, (figure.point2.Y + lineTh / 2) + hH);
                    g.DrawLine(pen, (figure.point1.X - lineTh / 2) + hW, figure.point1.Y + hH, (figure.point2.X + lineTh / 2) + hW, figure.point1.Y + hH);
                    g.DrawLine(pen, figure.point2.X + hW, (figure.point2.Y + lineTh / 2) + hH, figure.point2.X + hW, (figure.point1.Y - lineTh / 2) + hH);
                    g.DrawLine(pen, (figure.point2.X + lineTh / 2) + hW, figure.point2.Y + hH, (figure.point1.X - lineTh / 2) + hW, figure.point2.Y + hH);
                }
                else if (figure.FigureType == "Ellipse")
                {
                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);
                    int width = figure.point2.X - figure.point1.X;
                    int height = figure.point2.Y - figure.point1.Y;
                    g.DrawEllipse(pen, figure.point1.X + hW, figure.point1.Y + hH, width, height);
                    Brush brush = new SolidBrush(figure.FillColor);
                    g.FillEllipse(brush, figure.point1.X + hW, figure.point1.Y + hH, width, height);
                }
                else
                {
                    List<Point> newPoints = figure.CurvePoints;
                    Point[] dynamicPoints = newPoints.ToArray();
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), dynamicPoints);
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), new[] { newPoints[0], newPoints[newPoints.Count() - 1] });
                }
            }
        }

        public override void Hide(List<Figure> figures)
        {
            figures.Clear();
        }

        public override void MouseMove(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
        {
            DrawDash(g, point2, figures, lineColor, 0, fillColor, hW, hH);
        }
    }

    [Serializable]
    class Line : Figure
    {
        private Color black = Color.Black;
        private Color white = Color.White;
        public Line(Point point1, Point point2) : base(point1, point2) { }

        public override void FillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
        {

        }

        public override void ResizeFillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
        {

        }

        public override void DrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
        {
            g.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
        }

        public override void ResizeDrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
        {

        }

        public override void Draw(Graphics g, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH, List<Point> curvePoints)
        {
            previousPoint = Point.Empty;

            Point P1 = new Point(point1.X - hW, point1.Y - hH);
            Point P2 = new Point(point2.X - hW, point2.Y - hH);

            Figure fig = new Line(P1, P2);
            fig.LineColor = lineColor;
            fig.LineThickness = lineThickness;
            fig.FillColor = fillColor;
            fig.FigureType = "Line";
            fig.CurvePoints = curvePoints;

            figures.Add(fig);

            DrawFigure(g, new Pen(lineColor, lineThickness), point1, point2, lineThickness, 0, 0);
        }

        public override void DrawDash(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
        {

            this.point2 = point2;

            float[] dashValues = { 4, 2 };
            Pen dashPen = new Pen(lineColor, 1);
            dashPen.DashPattern = dashValues;

            NewDraw(g, figures, hW, hH);

            DrawFigure(g, new Pen(white, 2), point1, previousPoint, lineThickness, 0, 0);


            previousPoint = point2;

            DrawFigure(g, dashPen, point1, point2, lineThickness, 0, 0);
        }

        public override void NewDraw(Graphics g, List<Figure> figures, int hW, int hH)
        {
            foreach (Figure figure in figures)
            {
                if (figure.FigureType == "Line")
                {
                    float lineTh = figure.LineThickness;

                    if (lineTh == 1) lineTh = 0;

                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                    g.DrawLine(pen, figure.point1.X + hW, figure.point1.Y + hH, figure.point2.X + hW, figure.point2.Y + hH);
                }
                else if (figure.FigureType == "Rect")
                {
                    Point[] points = new Point[]
{
                    new Point(figure.point1.X + hW, figure.point1.Y + hH),
                    new Point(figure.point1.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point1.Y + hH)
};
                    Brush brush = new SolidBrush(figure.FillColor);

                    g.FillPolygon(brush, points);

                    float lineTh = figure.LineThickness;

                    if (lineTh == 1) lineTh = 0;

                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                    g.DrawLine(pen, figure.point1.X + hW, (figure.point1.Y - lineTh / 2) + hH, figure.point1.X + hW, (figure.point2.Y + lineTh / 2) + hH);
                    g.DrawLine(pen, (figure.point1.X - lineTh / 2) + hW, figure.point1.Y + hH, (figure.point2.X + lineTh / 2) + hW, figure.point1.Y + hH);
                    g.DrawLine(pen, figure.point2.X + hW, (figure.point2.Y + lineTh / 2) + hH, figure.point2.X + hW, (figure.point1.Y - lineTh / 2) + hH);
                    g.DrawLine(pen, (figure.point2.X + lineTh / 2) + hW, figure.point2.Y + hH, (figure.point1.X - lineTh / 2) + hW, figure.point2.Y + hH);
                }
                else if (figure.FigureType == "Ellipse")
                {
                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);
                    int width = figure.point2.X - figure.point1.X;
                    int height = figure.point2.Y - figure.point1.Y;
                    g.DrawEllipse(pen, figure.point1.X + hW, figure.point1.Y + hH, width, height);
                    Brush brush = new SolidBrush(figure.FillColor);
                    g.FillEllipse(brush, figure.point1.X + hW, figure.point1.Y + hH, width, height);
                }
                else
                {
                    List<Point> newPoints = figure.CurvePoints;
                    Point[] dynamicPoints = newPoints.ToArray();
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), dynamicPoints);
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), new[] { newPoints[0], newPoints[newPoints.Count() - 1] });
                }
            }
        }

        public override void Hide(List<Figure> figures)
        {
            figures.Clear();
        }

        public override void MouseMove(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
        {
            DrawDash(g, point2, figures, lineColor, 0, fillColor, hW, hH);
        }
    }

    [Serializable]
    class Ellipse : Figure
    {
        private Color black = Color.Black;
        private Color white = Color.White;
        public Ellipse(Point point1, Point point2) : base(point1, point2) { }

        public override void FillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
        {
            Brush brush = new SolidBrush(fillColor);
            int width = p2.X - p1.X;
            int height = p2.Y - p1.Y;
            g.FillEllipse(brush, p1.X + hW, p1.Y + hH, width, height);
        }

        public override void ResizeFillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
        {
            Point[] points = new Point[]
            {
                new Point(p1.X + hW, p1.Y + hH),
                new Point(p1.X + hW, p2.Y + hH),
                new Point(p2.X + hW, p2.Y + hH),
                new Point(p2.X + hW, p1.Y + hH)
            };
            Brush brush = new SolidBrush(fillColor);

            g.FillPolygon(brush, points);
        }

        public override void DrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
        {
            if (lineTh == 1) lineTh = 0;
            int width = p2.X - p1.X;
            int height = p2.Y - p1.Y;
            g.DrawEllipse(pen, p1.X, p1.Y, width, height);

        }

        public override void ResizeDrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
        {
            if (lineTh == 1) lineTh = 0;
            int width = p2.X - p1.X;
            int height = p2.Y - p1.Y;
            g.DrawEllipse(pen, p1.X + hW, p1.Y + hH, width, height);
        }

        public override void Draw(Graphics g, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH, List<Point> curvePoints)
        {
            previousPoint = Point.Empty;

            Point P1 = new Point(point1.X - hW, point1.Y - hH);
            Point P2 = new Point(point2.X - hW, point2.Y - hH);

            Figure fig = new Ellipse(P1, P2);
            fig.LineColor = lineColor;
            fig.LineThickness = lineThickness;
            fig.FillColor = fillColor;
            fig.FigureType = "Ellipse";
            fig.CurvePoints = curvePoints;

            figures.Add(fig);


            FillFigure(g, point1, point2, fillColor, lineThickness, 0, 0);
            DrawFigure(g, new Pen(lineColor, lineThickness), point1, point2, lineThickness, 0, 0);
        }

        public override void DrawDash(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
        {

            this.point2 = point2;

            float[] dashValues = { 4, 2 };
            Pen dashPen = new Pen(lineColor, 1);
            dashPen.DashPattern = dashValues;

            NewDraw(g, figures, hW, hH);

            DrawFigure(g, new Pen(white, 2), point1, previousPoint, 0, 0, 0);


            previousPoint = point2;

            DrawFigure(g, dashPen, point1, point2, lineThickness, 0, 0);
        }

        public override void NewDraw(Graphics g, List<Figure> figures, int hW, int hH)
        {
            foreach (Figure figure in figures)
            {
                if (figure.FigureType == "Line")
                {
                    float lineTh = figure.LineThickness;

                    if (lineTh == 1) lineTh = 0;

                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                    g.DrawLine(pen, figure.point1.X + hW, figure.point1.Y + hH, figure.point2.X + hW, figure.point2.Y + hH);
                }
                else if (figure.FigureType == "Rect")
                {
                    Point[] points = new Point[]
{
                    new Point(figure.point1.X + hW, figure.point1.Y + hH),
                    new Point(figure.point1.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point1.Y + hH)
};
                    Brush brush = new SolidBrush(figure.FillColor);

                    g.FillPolygon(brush, points);

                    float lineTh = figure.LineThickness;

                    if (lineTh == 1) lineTh = 0;

                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                    g.DrawLine(pen, figure.point1.X + hW, (figure.point1.Y - lineTh / 2) + hH, figure.point1.X + hW, (figure.point2.Y + lineTh / 2) + hH);
                    g.DrawLine(pen, (figure.point1.X - lineTh / 2) + hW, figure.point1.Y + hH, (figure.point2.X + lineTh / 2) + hW, figure.point1.Y + hH);
                    g.DrawLine(pen, figure.point2.X + hW, (figure.point2.Y + lineTh / 2) + hH, figure.point2.X + hW, (figure.point1.Y - lineTh / 2) + hH);
                    g.DrawLine(pen, (figure.point2.X + lineTh / 2) + hW, figure.point2.Y + hH, (figure.point1.X - lineTh / 2) + hW, figure.point2.Y + hH);
                }
                else if (figure.FigureType == "Ellipse")
                {
                    Pen pen = new Pen(figure.LineColor, figure.LineThickness);
                    int width = figure.point2.X - figure.point1.X;
                    int height = figure.point2.Y - figure.point1.Y;
                    g.DrawEllipse(pen, figure.point1.X + hW, figure.point1.Y + hH, width, height);
                    Brush brush = new SolidBrush(figure.FillColor);
                    g.FillEllipse(brush, figure.point1.X + hW, figure.point1.Y + hH, width, height);
                }
                else
                {
                    List<Point> newPoints = figure.CurvePoints;
                    Point[] dynamicPoints = newPoints.ToArray();
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), dynamicPoints);
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), new[] { newPoints[0], newPoints[newPoints.Count() - 1] });
                }
            }
        }

        public override void Hide(List<Figure> figures)
        {
            figures.Clear();
        }

        public override void MouseMove(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
        {
            DrawDash(g, point2, figures, lineColor, 0, fillColor, hW, hH);
        }
    }

}

[Serializable]
class Curve : Figure
{
    private Color black = Color.Black;
    private Color white = Color.White;

    List<Point> points = new List<Point>();
    List<Point> curvePoints = new List<Point>();

    public Curve(Point point1, Point point2) : base(point1, point2) { }

    public override void FillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
    {
    }

    public override void ResizeFillFigure(Graphics g, Point p1, Point p2, Color fillColor, float lineTh, int hW, int hH)
    {

    }

    public override void DrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
    {
        g.DrawCurve(pen, new[] { p1, p2 });
    }

    public override void ResizeDrawFigure(Graphics g, Pen pen, Point p1, Point p2, float lineTh, int hW, int hH)
    {
    }

    public override void Draw(Graphics g, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH, List<Point> curvePoints)
    {
        previousPoint = Point.Empty;

        Point P1 = new Point(point1.X - hW, point1.Y - hH);
        Point P2 = new Point(point2.X - hW, point2.Y - hH);

        Figure fig = new Curve(P1, P2);
        fig.LineColor = lineColor;
        fig.LineThickness = lineThickness;
        fig.FillColor = fillColor;
        fig.FigureType = "Curve";
        fig.CurvePoints = curvePoints;


        figures.Add(fig);
        Console.WriteLine(figures[0].CurvePoints.Count());

        g.DrawLine(new Pen(lineColor, lineThickness), points[0], points.Last());

        points.Clear();
    }

    public override void DrawDash(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
    {

    }

    public override void NewDraw(Graphics g, List<Figure> figures, int hW, int hH)
    {
        foreach (Figure figure in figures)
        {
            if (figure.FigureType == "Line")
            {
                float lineTh = figure.LineThickness;

                if (lineTh == 1) lineTh = 0;

                Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                g.DrawLine(pen, figure.point1.X + hW, figure.point1.Y + hH, figure.point2.X + hW, figure.point2.Y + hH);

           
            }
            else if (figure.FigureType == "Rect")
            {
                Point[] points = new Point[]
{
                    new Point(figure.point1.X + hW, figure.point1.Y + hH),
                    new Point(figure.point1.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point2.Y + hH),
                    new Point(figure.point2.X + hW, figure.point1.Y + hH)
};
                Brush brush = new SolidBrush(figure.FillColor);

                g.FillPolygon(brush, points);

                float lineTh = figure.LineThickness;

                if (lineTh == 1) lineTh = 0;

                Pen pen = new Pen(figure.LineColor, figure.LineThickness);

                g.DrawLine(pen, figure.point1.X + hW, (figure.point1.Y - lineTh / 2) + hH, figure.point1.X + hW, (figure.point2.Y + lineTh / 2) + hH);
                g.DrawLine(pen, (figure.point1.X - lineTh / 2) + hW, figure.point1.Y + hH, (figure.point2.X + lineTh / 2) + hW, figure.point1.Y + hH);
                g.DrawLine(pen, figure.point2.X + hW, (figure.point2.Y + lineTh / 2) + hH, figure.point2.X + hW, (figure.point1.Y - lineTh / 2) + hH);
                g.DrawLine(pen, (figure.point2.X + lineTh / 2) + hW, figure.point2.Y + hH, (figure.point1.X - lineTh / 2) + hW, figure.point2.Y + hH);
            }
            else if (figure.FigureType == "Ellipse")
            {
                Pen pen = new Pen(figure.LineColor, figure.LineThickness);
                int width = figure.point2.X - figure.point1.X;
                int height = figure.point2.Y - figure.point1.Y;
                g.DrawEllipse(pen, figure.point1.X + hW, figure.point1.Y + hH, width, height);
                Brush brush = new SolidBrush(figure.FillColor);
                g.FillEllipse(brush, figure.point1.X + hW, figure.point1.Y + hH, width, height);
            }

            {
                List<Point> newPoints = curvePoints;
                if (newPoints.Count < 2) return;
                g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), newPoints.ToArray());
                /*g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), dynamicPoints);
                g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), new[] { newPoints[0], newPoints[newPoints.Count() - 1] });*/
            }
        }
    }

    public override void Hide(List<Figure> figures)
    {
        figures.Clear();
    }

    public override void MouseMove(Graphics g, Point point2, List<Figure> figures, Color lineColor, float lineThickness, Color fillColor, int hW, int hH)
    {
        curvePoints.Add(point2);
        points.Add(point2);

        NewDraw(g, figures, hW, hH);

       
            if (points.Count < 2) return;
            g.DrawCurve(new Pen(lineColor, lineThickness), points.ToArray());
      
    }
}
