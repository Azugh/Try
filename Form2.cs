using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.Collections.Generic;
using System.Linq;

namespace lab3
{
    [Serializable]
    public partial class Form2 : Form
    {
        private Form1 parentForm1;

        bool isVerticalScrollVisible = false;
        bool isHorizontalScrollVisible = false;

        int hiddenWidth = 0;
        int hiddenHeight = 0;

        bool isDown = false;
        bool isMouseMove = false;
        bool whiteRectPaint = false;
        bool scroll = false;
        public bool hasChanges = false;
        public bool activated = false;
        public Point point1;
        public Point point2;
        public string curFilePath = "";

        List<Figure> figures = new List<Figure>();
        List<Point> curvePoints = new List<Point>();

        public int formWidth;

        public int formHeight;

        Figure figure = null;

        Rect rect = new Rect(Point.Empty, Point.Empty);
        Line line = new Line(Point.Empty, Point.Empty);
        Ellipse ellipse = new Ellipse(Point.Empty, Point.Empty);
        Curve curve = new Curve(Point.Empty, Point.Empty);

        public bool IsActive
        {
            get { return ActiveForm == this; }
        }

        public Form2(Form1 parentForm, int width, int height)
        {
            parentForm1 = parentForm;
            formWidth = width;
            formHeight = height;
            this.AutoScrollMinSize = new Size(formWidth, formHeight);
            InitializeComponent();
        }

        public void CreateNewPictureBox()
        {
            Graphics g = CreateGraphics();
            Brush brush = new SolidBrush(Color.White);
            Brush brush1 = new SolidBrush(Color.Gray);
            g.FillRectangle(brush1, 0, 0, 10000, 10000);
            g.FillRectangle(brush, 0, 0, formWidth, formHeight);
        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button != MouseButtons.Left || (e.X > formWidth) || (e.Y > formHeight))
            {
                return;
            }

            isDown = true;
            isMouseMove = false;
            hasChanges = true;

            figure = getFigure(parentForm1.FigureType);

            figure.point1 = new Point(e.X, e.Y);    

           /* if (parentForm1.FigureType == FIGURETYPE.LINE)
            {
                line.point1 = new Point(e.X, e.Y);
            }
            else if (parentForm1.FigureType == "Rect")
            {
                rect.point1 = new Point(e.X, e.Y);
            }
            else if (parentForm1.FigureType == "Ellipse")
            {
                ellipse.point1 = new Point(e.X, e.Y);
            }
            else
            {
                curvePoints.Add(new Point(e.X, e.Y));
                curve.point1 = new Point(e.X, e.Y);
            }*/

            return;

        }

        private Figure getFigure(FIGURETYPE curFig)
        {

            switch(curFig)
            {
                case FIGURETYPE.LINE:
                    return line;
                case FIGURETYPE.ELLIPSE:
                    return ellipse;
                case FIGURETYPE.RECT:
                    return (rect);
                case FIGURETYPE.ARBITRARY:
                    return (curve);
                default: return (line);
            }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {

            if (!isDown) return;

            Graphics g = CreateGraphics();

            int XPos = e.X;
            int YPos = e.Y;

            if (XPos > formWidth + hiddenWidth)
            {
                XPos = formWidth + hiddenWidth;
            }

            if (YPos > formHeight + hiddenHeight)
            {
                YPos = formHeight + hiddenHeight;
            }

            figure.MouseMove(g, new Point(XPos, YPos), figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight);

            /*if (parentForm1.FigureType == "Line")
            {
                line.MouseMove(g, new Point(XPos, YPos), figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
            }
            else if (parentForm1.FigureType == "Rect")
            {
                rect.MouseMove(g, new Point(XPos, YPos), figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
            }
            else if (parentForm1.FigureType == "Ellipse")
            {
                ellipse.MouseMove(g, new Point(XPos, YPos), figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
            }
            else
            {
                curve.MouseMove(g, new Point(XPos, YPos), figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
            }*/



            isMouseMove = true;

            return;
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {

            if (!isMouseMove)
            {
                isDown = false;

            }

            if (isDown)
            {
                isDown = false;

                Graphics g = CreateGraphics();

                figure.Draw(g, figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
                /*if (parentForm1.FigureType == "Line")
                {
                    line.Draw(g, figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
                }
                else if (parentForm1.FigureType == "Rect")
                {
                    rect.Draw(g, figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
                }
                else if (parentForm1.FigureType == "Ellipse")
                {
                    ellipse.Draw(g, figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
                }
                else
                {
                    curve.Draw(g, figures, parentForm1.LineColor, parentForm1.LineThickness, parentForm1.FillColor, hiddenWidth, hiddenHeight, curvePoints);
                }*/


            }
        }
        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();
            CreateNewPictureBox();
            if (isHorizontalScrollVisible)
            {
                hiddenWidth = this.AutoScrollPosition.X;
            }
            else
            {
                hiddenWidth = 0;
            }

            if (isVerticalScrollVisible)
            {
                hiddenHeight = this.AutoScrollPosition.Y;
            }
            else
            {
                hiddenHeight = 0;
            }

            int hW = hiddenWidth;
            int hH = hiddenHeight;


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
                else if (figure.FigureType == "Curve")
                {
                    List<Point> newPoints = figure.CurvePoints;
                    Point[] dynamicPoints = newPoints.ToArray();
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), dynamicPoints);
                    g.DrawCurve(new Pen(figure.LineColor, figure.LineThickness), new[] { newPoints[0], newPoints[newPoints.Count() - 1] });
                }
            }

        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            activated = true;

            if (curFilePath != "")
            {
                parentForm1.changeSaveButton(true);
                parentForm1.changeSaveButton(true);
            }
            else
            {
                parentForm1.changeSaveButton(false);
            }
        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {
            activated = false;
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasChanges)
            {
                DialogResult result = MessageBox.Show("У вас есть несохраненные элементы. Хотите сохранить файл?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (curFilePath != "")
                    {
                        parentForm1.save();
                    }
                    else
                    {
                        parentForm1.saveAs();
                    }
                    parentForm1.checkChildren();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    parentForm1.checkChildren();
                }
            }
            else
            {
                parentForm1.checkChildren();
            }
        }

        public void SaveFiguresToFile(string fileName)
        {
            DrawingData data = new DrawingData();
            data.Width = formWidth;
            data.Height = formHeight;
            data.Figures = figures;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
        }

        public void LoadFiguresFromFile(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                DrawingData data = (DrawingData)formatter.Deserialize(stream);
                formWidth = data.Width;
                formHeight = data.Height;
                figures = data.Figures;
                this.Width = formWidth + 17;
                this.Height = formHeight + 40;
                this.AutoScrollMinSize = new Size(formWidth, formHeight);
            }
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            isVerticalScrollVisible = this.VerticalScroll.Visible;
            isHorizontalScrollVisible = this.HorizontalScroll.Visible;
        }
    }
}
