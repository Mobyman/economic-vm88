using System;
using System.Windows.Forms;

namespace EconomicGrid
{
    public partial class ResultEdit : Form
    {
        public ResultEdit()
        {
            InitializeComponent();
        }


        public void questionLabelChange(string question)
        {
            questionlabel.Text = question;
        }


        private void questionFormLoad(object sender, EventArgs e)
        {
            var adm = (Admin) Owner;
            AcceptButton = OK;
            team1box.Text = adm.teamsscore[0].ToString();
            team2box.Text = adm.teamsscore[1].ToString();
            team3box.Text = adm.teamsscore[2].ToString();
            team4box.Text = adm.teamsscore[3].ToString();
            team5box.Text = adm.teamsscore[4].ToString();
            team6box.Text = adm.teamsscore[5].ToString();
        }

        private void button1Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okClick(object sender, EventArgs e)
        {
            var adm = (Admin) Owner;
            try
            {
                adm.teamsscore[0] = Convert.ToInt32(team1box.Text);
                adm.teamsscore[1] = Convert.ToInt32(team2box.Text);
                adm.teamsscore[2] = Convert.ToInt32(team3box.Text);
                adm.teamsscore[3] = Convert.ToInt32(team4box.Text);
                adm.teamsscore[4] = Convert.ToInt32(team5box.Text);
                adm.teamsscore[5] = Convert.ToInt32(team6box.Text);
            }

            catch (FormatException)
            {
                MessageBox.Show(@"Неверный ввод!");
            }
            DialogResult = DialogResult.OK;
        }
    }
}