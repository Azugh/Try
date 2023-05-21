using System.Collections.Generic;
using System;
using WindowsFormsApp1;

[Serializable]
public class DrawingData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public List<Figure> Figures { get; set; }

    public DrawingData()
    {
        Figures = new List<Figure>();
    }
}