using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace GraphsWithGeometrics
{
    class ExcelReader
    {
        Application xlApp = new Application();
        Worksheet ws;
        Workbook wb;
        public int[] excel_reader(int row, string path)
        {
            Application xlApp = new Application();
            wb = xlApp.Workbooks.Open(path);
            ws = wb.Worksheets["Sheet1"];
            int[] arr = new int[1000];
            for (int x = 1; x < 101; x++) arr[x-1] = Convert.ToInt16(ws.Cells[x, 1].Value);
            int z= 2;
            xlApp.Workbooks.Close();
            return arr;
        }

        public void store_data(int[] data, string path)
        {
            Application xlApp = new Application();
            wb = xlApp.Workbooks.Open(path);
            ws = wb.Worksheets["Sheet1"];
            int column = 1;
            while (ws.Cells[1, column].Value == -1) column++;
            ws.Cells[1, column].Value = -1;
            for (int row = 2; row < data.Length + 2; row++) ws.Cells[row, column].Value = Convert.ToDouble(data[row - 2]);
            wb.Save();
            xlApp.Workbooks.Close();
        }

        public int get_number_of_runs(string path)
        {
            int runs = 0;
            Application xlApp = new Application();
            wb = xlApp.Workbooks.Open(path);
            ws = wb.Worksheets["Sheet1"];
            while (ws.Cells[1, runs+1].Value == -1) runs++;
            xlApp.Workbooks.Close();
            return runs;
        }
    }
}
