using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Microsoft.Office.Interop.Excel;
using System.IO.Ports;

namespace GraphsWithGeometrics
{
    public partial class Form1 : Form
    {
        Serial_interface serial_Interface = new Serial_interface();
        ExcelReader excelReader = new ExcelReader();
        string excel_path = Properties.Settings.Default["Excel_path"].ToString();
        int i = 0;
        int graph_windows = 0;
        int xCord = 0, yCord = 0;
        SerialPort serialPort = new SerialPort();
        GraphBuilder graphBuilder = new GraphBuilder();
        int number_of_runs, _panel_height = 0, _panel_width = 0;
        

        public Form1()
        {
            InitializeComponent();
            path_textbox.Text = excel_path;
            number_of_runs = excelReader.get_number_of_runs(excel_path);
            this.MinimumSize = new System.Drawing.Size(800, 300);
        }

        private int[] SampleData(int Range_min, int Range_max)
        {
            Random rnd = new Random();
            int[] y = new int[200];
            y[0] = rnd.Next(100, 120);
            for (int z = 1; z < 200; z++) y[z] = rnd.Next(y[z - 1] + Range_min, y[z - 1] + Range_max);
            return y;
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            foreach (var item in ports)
            {
                comboBox1.Items.Add(item);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() != "")
            {
                Serial_button.Enabled = true;
            }
            else
            {
                Serial_button.Enabled = false;
            }
        }

        private void Serial_button_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {               
                serial_Interface.open_serial_port(comboBox1.SelectedItem.ToString(), 9600);
                Serial_button.Text = "Close";
            }
            else
            {
               
                Serial_button.Text = "open";
            }
        }

        private void change_button_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Properties.Settings.Default["Excel_path"] = openFileDialog1.FileName;
            Properties.Settings.Default.Save();
            excel_path = openFileDialog1.FileName;
            path_textbox.Text = excel_path;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //graph_windows++;
            draw_graphs();
        }

        private Panel make_panel(int width, int height, Point location)
        {
            Panel panel = new Panel { Width = width, Height = height, Location = location, Tag = "Remove", Name = "PAINT" };
            CheckedListBox checkedListBox = make_checklistbox();
            panel.Controls.Add(checkedListBox);
            return panel;
        }

        private CheckedListBox make_checklistbox()
        {
            CheckedListBox checkedListBox = new CheckedListBox { Width = 100, Height = this.Height-150, Location = new Point(this.Width-150, 10), Tag = "Remove" };
            checkedListBox.ItemCheck += new ItemCheckEventHandler(Item_selected);
            for(int x = 0; x < number_of_runs; x++)
            {                
                checkedListBox.Items.Insert(x, $"Run {x+1}");
            }
            return checkedListBox;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            draw_graphs();
        }

        private void draw_graphs()
        {
            if (graph_windows == 0)
            {
                foreach (Control control in Controls) if (control.Tag == "Remove") Controls.Remove(control);

                Panel panel = make_panel(this.Width - 50, this.Height - 120, new Point(30, 100));
                _panel_height = panel.Height;
                _panel_width = panel.Width;
                Controls.Add(panel);
                Graphics graphics = panel.CreateGraphics();
                int[] Y = SampleData(-5, 5);
                System.Threading.Thread.Sleep(100);
                int[] Y2 = SampleData(-10, 10);
                PointF[] pointF = graphBuilder.Create_polygon(Y, panel.Height -50, 20, Width-200, panel.Height -70);
                PointF[] pointF2 = graphBuilder.Create_polygon(Y2, panel.Height - 50, 20, Width - 200, panel.Height - 70);
                graphics.Clear(panel.BackColor);
                graphBuilder.DrawScaleX(Width - 200, 20, panel.Height - 50, 10, panel);
                graphBuilder.DrawScaleY(panel.Height - 70, 20, panel.Height - 50, 10, panel);
                Brush brush = Brushes.ForestGreen;
                SolidBrush opaqueBrush = new SolidBrush(Color.FromArgb(255, 0, 255,0));
                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(70, 0, 255, 0));
                graphics.FillPolygon(semiTransBrush, pointF);
                SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(70,255, 0, 0));
                graphics.FillPolygon(semiTransBrush2, pointF2);
                

            }

          /*  else if (graph_windows == 1)
            {
                graph_windows++;
                Panel panel = make_panel(700, 300, new Point(30, 400));
                CheckedListBox checkedListBox = make_checklistbox();
                panel.Controls.Add(checkedListBox);
                this.Controls.Add(panel);
                Graphics graphics = panel.CreateGraphics();

                int[] Y = SampleData(-5, 5);
                PointF[] pointF = graphBuilder.Create_polygon(Y, 280, 20, 580);
                graphics.Clear(panel.BackColor);
                graphBuilder.DrawScaleX(600, 10, 290, 10, panel);
                graphBuilder.DrawScaleY(280, 10, 290, 5, panel);
                graphics.FillPolygon(Brushes.ForestGreen, pointF);
            }

            else if (graph_windows == 2)
            {
                graph_windows++;
                Panel panel = make_panel(700, 300, new Point(780, 100));
                CheckedListBox checkedListBox = make_checklistbox();
                panel.Controls.Add(checkedListBox);
                this.Controls.Add(panel);
                Graphics graphics = panel.CreateGraphics();

                int[] Y = SampleData(-5, 5);
                PointF[] pointF = graphBuilder.Create_polygon(Y, 280, 20, 600);

                graphics.Clear(panel.BackColor);
                graphBuilder.DrawScaleX(600, 10, 290, 10, panel);
                graphBuilder.DrawScaleY(280, 10, 290, 5, panel);
                graphics.FillPolygon(Brushes.ForestGreen, pointF);

            }

            else if (graph_windows == 3)
            {
                graph_windows++;
                Panel panel = make_panel(700, 300, new Point(780, 400));
                CheckedListBox checkedListBox = make_checklistbox();
                panel.Controls.Add(checkedListBox);
                this.Controls.Add(panel);
                Graphics graphics = panel.CreateGraphics();
                int[] Y = SampleData(-5, 5);
                PointF[] pointF = graphBuilder.Create_polygon(Y, 280, 20, 600);
                graphics.Clear(panel.BackColor);
                graphBuilder.DrawScaleX(600, 10, 290, 10, panel);
                graphBuilder.DrawScaleY(280, 10, 290, 5, panel);
                graphics.FillPolygon(Brushes.ForestGreen, pointF);

            }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serial_Interface.transmit(1);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int[] test = serial_Interface.get_data();
            timer1.Stop();

            try
            {
                int x = 0;
                for (; test[x] != '*'; x++) ;
                int[] values = new int[x - 3];
                
                for (int z = 1; z + 3 < x; z++)
                {
                    values[z-1] = (test[z * 2] << 8) + test[z * 2 + 1];
                }
                excelReader.store_data(values, excel_path);
            }
            catch
            {

            }
        }

        private void Delete_button_pressed(object sender, EventArgs e)
        {
            Button current_button = sender as Button;
            foreach(Control panel in this.Controls)
            {
                if(panel.Contains(current_button))
                {
                    current_button.Click -= new EventHandler(Delete_button_pressed);
                    this.Controls.Remove(panel);
                }
            }
        }

        private void Item_selected(object sender,ItemCheckEventArgs e)
        {
            CheckedListBox checkedListBox = sender as CheckedListBox;
            for(int x = 0; x < checkedListBox.Items.Count; x++)
            {
                if (checkedListBox.GetItemCheckState(x) == CheckState.Checked)
                {
                    int[] Y = excelReader.excel_reader(x, excel_path);
                    PointF[] points = graphBuilder.Create_polygon(Y, _panel_height - 50, 20, Width - 200, _panel_height - 70);
                  //  int index = this.Controls.IndexOf();
                  //  Graphics graphics = this.Controls.IndexOf        .CreateGraphics();
                }
            }
        }
    }
}
