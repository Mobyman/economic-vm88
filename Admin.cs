using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections;

namespace EconomicGrid
{
    public partial class Admin : Form
    {
        public string question, answer;
        public int[] costs = new[] { 100, 200, 300, 400, 500 };
        public int[] teamsscore = new[] { 0, 0, 0, 0, 0, 0 };
        public int currCost, row = -1, column = -1, team = -1, timeriteration, remainquestion = 30;
        public User u = new User();
        public string[,] questionMass = new string[6, 5];
        public string[,] answerMass = new string[6, 5];
        public string[] subjectsmass = new string[6];
        public int[,] answered = new int[6, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
        public string[,] getQuestionMass(){
            return questionMass;
        }

        private void readXmlDocument()
        {
            string filepath = Application.StartupPath + "\\db.xml";

            var xd = new XmlDocument();
            var fs = new FileStream(filepath, FileMode.Open);
            xd.Load(fs);

            for (int i = 0; i < 6; i++){
                subjectsmass[i] = xd.SelectSingleNode("descendant::category[@catid=\"" + i + "\"]/@name").InnerText;

                for (int j = 0; j < 5; j++){
                    
                    questionMass[i, j]  = xd.SelectSingleNode("descendant::category[@catid=\"" + i + "\"]/questions[@id=\"" + j + "\"]/@question").InnerText;
                    answerMass[i,j]     = xd.SelectSingleNode("descendant::category[@catid=\"" + i + "\"]/questions[@id=\"" + j + "\"]/@answer").InnerText;
                }
            }

            fs.Close();
        }  

        public string[] getSubjects() { 
            return subjectsmass;
        }
        public Admin()
        {
            InitializeComponent();
        }

        private string getQuestion(int questionSubject, int questionCost)
        {
            return questionMass[questionSubject, questionCost];
        }

        private string getAnswer(int answerSubject, int answerCost)
        {
            return answerMass[answerSubject, answerCost];
        }

        private bool isAnswered(int r, int clmn) {
            if (r != -1 && clmn != -1)
                if (answered[r, clmn] == 1)
                    return true;
                else
                    return false;
            return false;
        }

        private void Answered(int row, int column)
        {
            answered[row, column] = 1;
        }

        private void answerMenuAction(bool act) {

            applyanswermenu.Enabled = act;

            com1true.Enabled = act;
            com2true.Enabled = act;
            com3true.Enabled = act;
            com4true.Enabled = act;
            com5true.Enabled = act;
            com6true.Enabled = act;
            nottrue.Enabled = act;
            closequestion.Enabled = act;

            team1button.Enabled = act;
            team2button.Enabled = act;
            team3button.Enabled = act;
            team4button.Enabled = act;
            team5button.Enabled = act;
            team6button.Enabled = act;
            noteambutton.Enabled = act;
        }

        private void clickCell(object sender, DataGridViewCellEventArgs e)
        {
            int clmn = e.ColumnIndex;
            int r = e.RowIndex;
            if (clmn != 0 && !isAnswered(r, clmn - 1))
            {
                questiontable.CellClick -= clickCell;
                timermenuitem.Enabled = true;
                for (int i = 0; i < 6; i++) {
                    for (int j = 0; j < 5; j++) {
                        if ((i != r) || (j != clmn-1))
                        {
                            if (isAnswered(i, j))
                                questiontable.Rows[i].Cells[j+1].Tag = questiontable.Rows[i].Cells[j+1].Value;
                            if (questiontable.Rows[i].Cells[j+1].Tag == null)
                                questiontable.Rows[i].Cells[j+1].Value = "";
                        }
                    }
                }

                answerMenuAction(true);
                column = Convert.ToInt32(e.ColumnIndex.ToString()) - 1;
                row = Convert.ToInt32(e.RowIndex.ToString());

                question = getQuestion(row, column);
                answer = getAnswer(row, column);
                if (!isAnswered(row, column))
                    if (Convert.ToInt32(e.ColumnIndex.ToString()) > 0)
                    {
                        questionlabel.Text = question;
                        answerlabel.Text = answer;
                        u.getQuestion(row, column);
                        timerstrip.Text = @"60";
                        timerstrip.ForeColor = Color.Black;
                        timer1.Start();
                        timelabel.Visible = true;
                        timerstrip.Visible = true;
                        u.timerEnabled(true, timerstrip.Text);
                        currCost = costs[column];
                    }
                    else
                        currCost = 0;
            }
            else {
                answerMenuAction(false);
            }
        }

        private void refreshPoints() {
            
            team1.Text = teamsscore[0].ToString();
            team2.Text = teamsscore[1].ToString();
            team3.Text = teamsscore[2].ToString();
            team4.Text = teamsscore[3].ToString();
            team5.Text = teamsscore[4].ToString();
            team6.Text = teamsscore[5].ToString();

            u.refreshpoints(teamsscore);
        }


        private void assignPoints(int teamNumber)
        {
            if (teamNumber != -1)
            {
                teamsscore[teamNumber] += currCost;
            }
            currCost = 0;
            Answered(row, column);
            refreshPoints();
            u.closeQuestion();
            fillTable();
            remainquestion--;
            questiontable.CellClick += clickCell;
        }

        private void acceptAnswer(object sender, EventArgs e)
        {
            timermenuitem.Enabled = false;
            team = -1;
            string type = sender.GetType().ToString();
            if (type == "System.Windows.Forms.ToolStripMenuItem")
            {
                if (sender == com1true)
                    team = 0;
                if (sender == com2true)
                    team = 1;
                if (sender == com3true)
                    team = 2;
                if (sender == com4true)
                    team = 3;
                if (sender == com5true)
                    team = 4;
                if (sender == com6true)
                    team = 5;
                if (sender == nottrue)
                    team = -1;
            }
            else
            { 
              if (sender == team1button)
                team = 0;
              if (sender == team2button)
                team = 1;
              if (sender == team3button)
                team = 2;
              if (sender == team4button)
                team = 3;
              if (sender == team5button)
                team = 4;
              if (sender == team6button)
                team = 5;
              if (sender == noteambutton)
                team = -1;

            }
            if (!isAnswered(row, column))
                if (team != -1)
                {
                    questiontable.Rows[row].Cells[column + 1].Value = "№" + (team + 1) + ", " + timerstrip.Text + "c.";
                    questiontable.Rows[row].Cells[column + 1].Tag = "№" + (team + 1) + ", " + timerstrip.Text + "c.";
                }
                else
                {
                    questiontable.Rows[row].Cells[column + 1].Value = "[Никто]";
                    questiontable.Rows[row].Cells[column + 1].Tag = "[Никто]";
                }
            assignPoints(team);
            questiontable.Rows[row].Cells[column + 1].Style.SelectionBackColor = Color.Green;
            questiontable.Rows[row].Cells[column + 1].Style.BackColor = Color.Green;
            u.disableQuestion(row, column);
            answerMenuAction(false);
            timer1.Stop();
            timelabel.Visible = false;
            timerstrip.Visible = false;
        }

        private void fillTable() {
            // Заполняем таблицу "ценами" ответов…
            int i = 0, j = 0;
            for (; j < 6; j++)
            {
                questiontable.Rows[j].Cells[0].Style.SelectionBackColor = Color.Navy;
                questiontable.Rows[j].Cells[0].Style.SelectionForeColor = Color.White;
                for (; i < 5; i++)
                    if (questiontable.Rows[j].Cells[i + 1].Tag == null)
                        questiontable.Rows[j].Cells[i + 1].Value = costs[i];
                    else
                        questiontable.Rows[j].Cells[i + 1].Value = questiontable.Rows[j].Cells[i + 1].Tag;
                i = 0;
            }
        }

        private void adminLoad(object sender, EventArgs e)
        {
            readXmlDocument();

            questiontable.Rows.Add(subjectsmass[0]);
            questiontable.Rows.Add(subjectsmass[1]);
            questiontable.Rows.Add(subjectsmass[2]);
            questiontable.Rows.Add(subjectsmass[3]);
            questiontable.Rows.Add(subjectsmass[4]);
            questiontable.Rows.Add(subjectsmass[5]);
            fillTable();
            u.Owner = this;
            u.Show();
        }

        private void closeQuestionHandler(object sender, EventArgs e)
        {
            fillTable();
            questiontable.CellClick += clickCell;
            u.closeQuestion();
        }


        private void timerPause(object sender, EventArgs e)
        {
            if (!timermenuitem.Checked)
            {
                timer1.Enabled = false;
                timer1.Enabled = false;
                timermenuitem.Checked = true;
            }
            else
            {
                timer1.Enabled = true;
                timer1.Enabled = true;
                timermenuitem.Checked = false;
            }
        }

        private void fullScreen(object sender, EventArgs e)
        {
            u.fullscreen();
        }


        private void resultEditButton(object sender, EventArgs e)
        {
            var re = new ResultEdit();
            re.Owner = this;
            re.ShowDialog();
            if (re.DialogResult == DialogResult.OK)
            {
                refreshPoints();
            }
        }

        private void exitMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timerTick(object sender, EventArgs e)
        {
            int secs = Convert.ToInt32(timerstrip.Text);
            timerstrip.Text = (secs - 1).ToString();
            u.timerEnabled(true, timerstrip.Text);
            if (secs < 11)
                timerstrip.ForeColor = Color.Red;
            if (secs == 1)
            {
                timer1.Stop();
                timelabel.Visible = false;
                timerstrip.Visible = false;
            }
        }

        private void finish_Click(object sender, EventArgs e)
        {
            u.StartCapRound();
            questionlabel.Visible = false;
            answerlabel.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            questiontable.Visible = false;
            var re = new ResultEdit();
            re.Owner = this;
            re.ShowDialog();
            if (re.DialogResult == DialogResult.OK)
            {
                refreshPoints();
            }
        }

        private void FinishGame(object sender, EventArgs e)
        {
            /*Выделить массив на 4 элемента с номерами и + еще запоминать самое большое значение + 
             * 
             * текущая позиция в этом массиве. Потом идти по исходному массиву и, как только находишь 
             * число больше последнего, пишешь его по позиции и смещаешь позицию в массиве результатов 
             * дальше, как позиция больше 4 â€“ снова на начало. В итоге у тебя останутся 4 самых больших
             * числа. Потом ты сравниваешь заполненность конечного массива (чтобы если у команды 1 больше всех очков оно не глючило),
             * если полный, то норм, если не полный, опять повторяешь цикл
             */




            int[] mass = new int[4];
            int max = 0, position = 0;
            for (int i = 0; i < 6; i++)
            {
                if (teamsscore[i] > max)
                {
                    mass[3] = mass[2];
                    mass[2] = mass[1];
                    mass[1] = mass[0];
                    mass[position] = teamsscore[i];
                }
            }


            //u.showCongratulation(teams, places);
        }

    }
}
