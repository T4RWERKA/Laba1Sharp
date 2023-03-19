using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Laba1Sharp
{
    public partial class Form1 : Form
    {
        public List<string> code = new List<string>();
        public Form1()
        {
            InitializeComponent();
            InitializeTable(tblOperands);
            InitializeTable(tblOperators);
        }

        void InitializeTable(DataGridView table)
        {
            var numColumn = new DataGridViewTextBoxColumn();
            var opColumn = new DataGridViewTextBoxColumn();
            var countColumn = new DataGridViewTextBoxColumn();

            numColumn.Name = "numColumn";
            numColumn.HeaderText = "№";

            if (table.Name == "tblOperators")
                opColumn.HeaderText = "Оператор";
            else
                opColumn.HeaderText = "Операнд";

            opColumn.Name = "opColumnColumn";


            countColumn.Name = "countColumn";
            countColumn.HeaderText = "Количество";

            table.Columns.AddRange(numColumn, opColumn, countColumn);
        }

        public static void FillTable(DataGridView table, Dictionary<string, int> dictionary)
        {
            table.Rows.Clear();
                     

            int i = 1, sum = 0;
            DataGridViewRow row;
            DataGridViewTextBoxCell num, op, count;
            foreach (var c in dictionary)
            {
                row = new DataGridViewRow();

                num = new DataGridViewTextBoxCell();
                op = new DataGridViewTextBoxCell();
                count = new DataGridViewTextBoxCell();

                num.Value = i++;
                op.Value = c.Key;
                count.Value = c.Value;
                sum += c.Value;

                row.Cells.AddRange(num, op, count);
                table.Rows.Add(row);
            }

            row = new DataGridViewRow();
            num = new DataGridViewTextBoxCell(); num.Value = "Sum";
            op = new DataGridViewTextBoxCell(); op.Value = "";
            count = new DataGridViewTextBoxCell(); count.Value = sum;
            row.Cells.AddRange(num, op, count);
            table.Rows.Add(row);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var reader = new StreamReader(openFileDialog1.FileName);
                code = new List<string>();
                try
                {
                    while (!reader.EndOfStream)
                        code.Add(reader.ReadLine());
                }
                finally
                {
                    reader.Close();
                }
                boxCode.Text = "";
                foreach (var str in code)
                    boxCode.Text += str + "\n";
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            Halsted.FillDictionaries(code);
            FillTable(tblOperands, Halsted.OperandDictionary);
            FillTable(tblOperators, Halsted.OperatorDictionary);

            int dict, length, volume;
            Halsted.GetMetrics(out dict, out length, out volume);

            lblDict.Text = dict.ToString();
            lblLength.Text = length.ToString();
            lblVolume.Text = volume.ToString();
        }

        private void boxCode_TextChanged(object sender, EventArgs e)
        {
            code = boxCode.Lines.ToList();
        }
    }
}
