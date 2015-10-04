using System;
using System.Drawing;
using System.Windows.Forms;

namespace EconomicGrid
{
    public partial class User : Form
    {
        private readonly MouseEventHandler _moveDelegate;
        private int _oldX;
        private int _oldY;

        public string answer;
        public int cost;
        public int[] costs = new int[5] {100, 200, 300, 400, 500};
        public int currCost;
        public string question;
        public int subject;
        public int[] teamsscore = new int[6] {0, 0, 0, 0, 0, 0};
        public int timeriteration;
        public string quote;
        public int quoteiteration = 0;

        public User()
        {
            InitializeComponent();
            _moveDelegate += userMouseMove;
            MouseDown += User_MouseDown;
            MouseUp += userMouseUp;
        }

        private void User_MouseDown(object sender, MouseEventArgs e)
        {
            MouseMove += _moveDelegate;
            _oldX = Cursor.Position.X;
            _oldY = Cursor.Position.Y;
        }

        private void userMouseMove(object sender, MouseEventArgs e)
        {
            MouseMove -= _moveDelegate;
            Location = new Point(Location.X + Cursor.Position.X - _oldX, Location.Y + Cursor.Position.Y - _oldY);
            _oldX = Cursor.Position.X;
            _oldY = Cursor.Position.Y;
            MouseMove += _moveDelegate;
        }

        private void userMouseUp(object sender, MouseEventArgs e)
        {
            MouseMove -= _moveDelegate;
        }

        public void timerEnabled(bool status, string time)
        {
            this.time.Text = time;
            if (time == "0")
                closeQuestion();
        }

        public void showQuestion(string question)
        {
            panel1.Visible = false;
            questiontable.Visible = false;
            questionlabel.Visible = true;
            questionlabel.Text = question;
            questionname.Visible = true;
            time.Visible = true;
            questionname.Text = "Вопрос номиналом " + costs[cost] + " на тему «" +
                                questiontable.Rows[subject].Cells[0].Value + "»";
        }

        public void getQuestion(int questionSubject, int questionCost)
        {
            subject = questionSubject;
            cost = questionCost;
            time.Text = "60";
            var a = (Admin) Owner;
            string[,] questionMass = a.getQuestionMass();
            showQuestion(questionMass[questionSubject, questionCost]);
        }

        public void disableQuestion(int row, int column)
        {
            questiontable.Rows[row].Cells[column + 1].Value = "";
        }

        public void refreshpoints(int[] teamsScore)
        {
            team1.Text = teamsScore[0].ToString();
            team2.Text = teamsScore[1].ToString();
            team3.Text = teamsScore[2].ToString();
            team4.Text = teamsScore[3].ToString();
            team5.Text = teamsScore[4].ToString();
            team6.Text = teamsScore[5].ToString();
        }

        public void closeQuestion()
        {
            questionlabel.Visible = false;
            questiontable.Visible = true;
            questionname.Visible = false;
            panel1.Visible = true;
            time.Visible = false;
            time.Text = "60";
        }

        private void userFormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void userLoad(object sender, EventArgs e)
        {
            var a = (Admin) Owner;
            string[] subjects = a.getSubjects();
            questiontable.Rows.Add(subjects[0]);
            questiontable.Rows.Add(subjects[1]);
            questiontable.Rows.Add(subjects[2]);
            questiontable.Rows.Add(subjects[3]);
            questiontable.Rows.Add(subjects[4]);
            questiontable.Rows.Add(subjects[5]);
            int i = 0, j = 0;

            // Заполняем таблицу "ценами" ответов…
            for (; j < 6; j++)
            {
                for (; i < 5; i++)
                    questiontable.Rows[j].Cells[i + 1].Value = costs[i];
                i = 0;
            }
        }

        public void fullscreen()
        {
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            else
                WindowState = FormWindowState.Maximized;

            questionlabel.Size = new Size(Width - 80, questionlabel.Height);
            questionname.Size = new Size(Width, questionname.Size.Height);
            questiontable.Size = new Size(Width, questiontable.Size.Height);
            questiontable.Height = Height;
            time.Location = new Point(Width - 110, time.Location.Y);
        }

        private void fullScreenItemClick(object sender, EventArgs e)
        {
            fullscreen();
        }

        private void fullScreenEvent(object sender, EventArgs e)
        {
            fullscreen();
        }

        public void showCongratulation(int[] teams, int[] places)
        {
            string results = "";
            QuoteChangeTick.Stop();
            questiontable.Visible = false;

            questionname.Text = "Поздравляем участников олимпиады!";
            int i = 0;
            foreach (int place in places)
            {
                results += "\n" + place + " место: Команда №" + (teams[i]+1);
                i++;
            }
            questionlabel.Text = "Места распределились следующим образом:" + results + "\nСпасибо за внимание!";
        }

        public void StartCapRound()
        {
            foreach (Control c in this.Controls)
                if (c.Name != "questionlabel")
                    c.Visible  = false;
            questionlabel.Visible = true;
            questionlabel.Text = "Рабочим платить заработную плату желательно.\n(Е.Гайдар)";
            QuoteChangeTick.Start();
        }

        private void ChangeQuote(object sender, EventArgs e)
        {
            quoteiteration++;
            if (quoteiteration == 15) quoteiteration = 0;
            if (quoteiteration == 0) quote = "Рабочим платить заработную плату желательно.\n(Е.Гайдар)";
            if (quoteiteration == 1) quote = "Судя по прогнозам, экономисты, как и синоптики, опираются на наблюдения из космоса.\n(М.Мамчич)";
            if (quoteiteration == 2) quote = "...экономисты в своих эмпирических исследованиях обязаны опираться на факты. Но многие, особенно лучшие экономисты, любят теоретизировать, оперировать абстрактными категориями.\n(В.Леонтьев)";
            if (quoteiteration == 3) quote = "Спрос рождает предложение.\n(Д.Кейнс)";
            if (quoteiteration == 4) quote = "Западные экономисты часто пытались раскрыть \"принцип\" советского метода планирования. Они так и не добились успеха, т.к. до сих пор такого метода вообще не существует.\n(В.Леонтьев)";
            if (quoteiteration == 5) quote = "Всякая экономия в конечном счете сводится к экономии времени.\n(К.Маркс)";
            if (quoteiteration == 6) quote = "Таким образом, полезность не является мерой меновой стоимости, хотя она абсолютно существенна для этой последней. Если предмет ни на что не годен, другими словами, если он ничем не служит нашим нуждам, он будет лишен меновой стоимости, как бы редок он ни был и каково бы ни было количество труда, необходимое для его получения.\n(Д. Рикардо)";
            if (quoteiteration == 7) quote = "Никакой рост объема продаж продукции не оправдывает инвестиции в ее акции, если на протяжении нескольких лет прибыль компании не будет увеличиваться соответствующими темпами.\n(Ф. Фишер)";
            if (quoteiteration == 8) quote = "Достоинство государства зависит, в конечном счете, от достоинства образующих его личностей.\n(Д.Милль)";
            if (quoteiteration == 9) quote = "Капитал страны является или основным, или оборотным в зависимости от степени своей долговечности.\n(Д.Рикардо)";
            if (quoteiteration == 10) quote = "В том первобытном состоянии общества, которое предшествует присвоению земли в частную собственность и накоплению капитала, весь продукт труда принадлежит работнику. Ему не приходится делиться ни с землевладельцем, ни с хозяином. Если бы такое состояние сохранилось, заработная плата за труд возрастала бы вместе с увеличением производительной силы труда...\n(А.Смит)";
            if (quoteiteration == 11) quote = "...мне кажется, что понять, как работает экономическая система, - это вообще первая задача экономиста.\n(В.Леонтьев)";
            if (quoteiteration == 12) quote = "Экономисты чаще всего опираются на два вида предчувствий: либо ошибочное, либо позднее.\n(М.Мамчич)";
            if (quoteiteration == 13) quote = "Политика есть самое концентрированное выражение экономики.";
            if (quoteiteration == 14) quote = "Экономист, лишенный самостоятельности решения, в лучшем случае бухгалтер, в худшем  кассир.\n(О.Попцов)";
            questionlabel.Text = quote;
        }
    }
}