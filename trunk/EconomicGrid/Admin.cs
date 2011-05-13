using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EconomicGrid
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();

            this.dataGridView1.Rows.Add("Тема #0 ");
            this.dataGridView1.Rows.Add("Тема #1");
            this.dataGridView1.Rows.Add("Тема #2");
            this.dataGridView1.Rows.Add("Тема #3");
            this.dataGridView1.Rows.Add("Тема #4");
            this.dataGridView1.Rows.Add("Тема #5");

            int[] readonly costs = new int[5] {100, 250, 500, 750, 1000};

            for (int i = 0; i < 5; i++){
                this.dataGridView1.Rows[1].Cells[i].Value = "";

        }

        private void ClickCell(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(e.RowIndex.ToString() + ":"  +e.ColumnIndex.ToString());
        }




    }
}
