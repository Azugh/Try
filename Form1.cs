using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace lab3
{
    public enum FIGURETYPE
    {
        LINE,
        ELLIPSE,
        RECT,
        ARBITRARY
    }

    public partial class Form1 : Form
    {
        public string FileName = string.Empty;

        public Color LineColor = Color.Black;
        public Color FillColor = Color.Transparent;
        public float LineThickness = 1;

        public FIGURETYPE FigureType = FIGURETYPE.LINE;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {


        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {



        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.AddRange(new object[] { "1", "2", "5", "8", "10", "12", "15" });
        }

        private Form2 GetActiveForm2()
        {

            foreach (Form form in MdiChildren)
            {
                if (form is Form2 form2 && form2.activated)
                {
                    return form2;
                }
            }

            return null;
        }

        public void checkChildren()
        {
            if (this.MdiChildren.Length == 1)
            {
                сохранитьToolStripMenuItem.Enabled = false;
                сохранитьКакToolStripMenuItem.Enabled = false;
            }
        }

        public void changeSaveButton(bool act)
        {
            сохранитьToolStripMenuItem.Enabled = act;
        }

        public void changeSaveAsButton(bool act)
        {
            сохранитьКакToolStripMenuItem.Enabled = act;
        }

        public void save()
        {
            Form2 form2 = GetActiveForm2();
            if (form2 != null)
            {
                string fileName = form2.curFilePath;
                if (fileName != "")
                {
                    form2.SaveFiguresToFile(fileName);
                    form2.hasChanges = false;
                }

            }
        }

        public void saveAs()
        {
            Form2 form2 = GetActiveForm2();
            if (form2 != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Файлы рисунков (*.fig)|*.fig";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    form2.curFilePath = fileName;
                    form2.SaveFiguresToFile(fileName);
                    form2.Text = Path.GetFileName(fileName);
                    form2.hasChanges = false;
                    сохранитьToolStripMenuItem.Enabled = true;
                }
            }
        }

        public void createNewPicture(int width, int height)
        {
            Form f = new Form2(this, width, height);
            f.MdiParent = this;
            f.Text = "Рисунок " + this.MdiChildren.Length.ToString();
            f.Size = new Size(width + 17, height + 40);
            f.Show();

            сохранитьКакToolStripMenuItem.Enabled = true;
        }


        private void новоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPicture newpicture = new NewPicture(this);
            newpicture.Show();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы рисунков (*.fig)|*.fig";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;

                Form2 form2 = new Form2(this, 0, 0);
                form2.curFilePath = fileName;
                form2.MdiParent = this;
                form2.Text = Path.GetFileName(fileName);
                form2.LoadFiguresFromFile(fileName);
                form2.Show();

                сохранитьToolStripMenuItem.Enabled = true;
                сохранитьКакToolStripMenuItem.Enabled = true;
            }
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            Form2 form2 = GetActiveForm2();
            сохранитьToolStripMenuItem.Enabled = (form2 != null);
            сохранитьКакToolStripMenuItem.Enabled = (form2 != null);
        }

        private void цветЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            colorDialog.Color = LineColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LineColor = colorDialog.Color;
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            // Установка текущего значения толщины линии, если есть
            if (LineThickness != null)
            {
                toolStripComboBox1.SelectedItem = LineThickness.ToString();
            }

            // Добавление ComboBox на форму и настройка расположения и размеров
            // Настройте свойства Location, Size, Anchor, и т.д. в соответствии с вашими требованиями
            // Например:
            // comboBox.Location = new Point(x, y);
            // comboBox.Size = new Size(width, height);
            // comboBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            // Обработка выбора толщины линии
            toolStripComboBox1.SelectedIndexChanged += (s, args) =>
            {
                // Получение выбранного значения толщины линии
                string selectedThickness = toolStripComboBox1.SelectedItem.ToString();

                LineThickness = float.Parse(selectedThickness);

                // Используйте выбранную толщину линии для нужных операций
                // Например, сохраните выбранное значение в атрибуте формы или передайте его в другой метод
                // ...
            };

        }

        private void цветЗаливкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            colorDialog.Color = FillColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                FillColor = colorDialog.Color;
            }
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FigureType = FIGURETYPE.LINE;
            линияToolStripMenuItem.Checked = true;
            прямоугольникToolStripMenuItem.Checked = false;
            эллипсToolStripMenuItem.Checked = false;
            проивзольнаяФигураToolStripMenuItem.Checked = false;
        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FigureType = FIGURETYPE.RECT;
            линияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = true;
            эллипсToolStripMenuItem.Checked = false;
            проивзольнаяФигураToolStripMenuItem.Checked = false;
        }

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FigureType = FIGURETYPE.ELLIPSE;
            линияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            эллипсToolStripMenuItem.Checked = true;
            проивзольнаяФигураToolStripMenuItem.Checked = false;
        }

        private void проивзольнаяФигураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FigureType = FIGURETYPE.ARBITRARY;
            линияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            эллипсToolStripMenuItem.Checked = false;
            проивзольнаяФигураToolStripMenuItem.Checked = true;
        }
    }
}
