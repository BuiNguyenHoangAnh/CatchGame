using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatchGame
{
    public partial class Form1 : Form
    {
        //khai bao bien
        private Button btnPlay = new Button();
        private Button btnExit = new Button();

        private Label lbScore = new Label();
        private Label lbTime = new Label();
        private Label lbCountTime = new Label();
        private Label lbCountScore = new Label();

        private Timer timer = new Timer();
        private int i = 60;

        private Panel panel = new Panel();

        Bitmap backBufferBasket;
        Bitmap spriteBasket;
        Graphics graphicsBasket;
        Graphics gBasket;
        int locationBasket;

        Bitmap backBufferEggs;
        Bitmap spriteEggs;
        Graphics graphicsEggs;
        Graphics gEggs;

        public Form1()
        {
            InitializeComponent();

            myInIt();
        }

        #region thiet ke form
        //thiet ke form
        private void myInIt()
        {
            this.Text = "Catch Game";
            this.AutoSize = false;

            myMenu();
        }

        //thiet ke form menu
        private void myMenu()
        {
            btnPlay.Text = "Play";
            btnPlay.Location = new Point(110, 70);
            this.Controls.Add(btnPlay);
            btnPlay.Click += new EventHandler(btnPlayClick);

            btnExit.Text = "Exit";
            btnExit.Location = new Point(110, 150);
            this.Controls.Add(btnExit);
            btnExit.Click += new EventHandler(btnExitClick);
        }

        //thiet ke form hien thi game
        public void myGame()
        {
            lbScore.Text = "Score";
            lbScore.Location = new Point(10, 10);
            lbScore.Size = new Size(40,20);
            this.Controls.Add(lbScore);

            lbTime.Text = "Time";
            lbTime.Location = new Point(210, 10);
            lbTime.Size = new Size(30, 20);
            this.Controls.Add(lbTime);

            lbCountScore.Text = "0";
            lbCountScore.Location = new Point(50, 10);
            this.Controls.Add(lbCountScore);

            lbCountTime.Text = "0";
            lbCountTime.Location = new Point(240, 10);
            this.Controls.Add(lbCountTime);

            panel.Size = new Size(this.Width, this.Height - 35);
            panel.Location = new Point(0, 35);
            panel.BackColor = Color.White;
            this.Controls.Add(panel);

            timer.Tick += new EventHandler(timerTick);
            timer.Enabled = true;
            timer.Interval = 1000;

            backBufferBasket = new Bitmap(panel.Width, panel.Height);

            backBufferEggs = new Bitmap(panel.Width, panel.Height);

            loadBackground();
            loadBasketImage();
            loadEggsImage();
            
        }
        #endregion

        #region su kien cac button tren man hinh menu
        //su kien khi click vao button Play
        private void btnPlayClick(Object sender, EventArgs e)
        {
            btnPlay.Visible = false;
            btnExit.Visible = false;

            myGame();
        }

        //su kien khi click vao button Exit
        private void btnExitClick(Object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region timer
        //su kien cua timer
        private void timerTick(object sender, EventArgs e)
        {
            this.lbCountTime.Text = i.ToString();

            i--;
            if (i < 0)
            {
                this.timer.Enabled = false;
            }
        }
        #endregion

        #region load hinh anh
        //set the background of the game
        private void loadBackground()
        {
            panel.BackgroundImage = Properties.Resources.bg;
            panel.BackgroundImageLayout = ImageLayout.Stretch;
        }

        //load hinh cai gio
        private void loadBasketImage()
        {
            spriteBasket =new Bitmap(Properties.Resources.basket);
            graphicsBasket = panel.CreateGraphics();
            gBasket = Graphics.FromImage(backBufferBasket);

            //gBasket.Clear(Color.White);
            gBasket.DrawImage(spriteBasket, panel.Width / 2 - spriteBasket.Width, panel.Height - spriteBasket.Height - 60, new Rectangle(0, 0, spriteBasket.Width, spriteBasket.Height), GraphicsUnit.Pixel);
            gBasket.Dispose();

            graphicsBasket.DrawImageUnscaled(backBufferBasket, 20, 20);
        }

        //load hinh con ga
        private void loadChickenImage()
        {

        }

        //load hinh qua trung
        private void loadEggsImage()
        {
            spriteEggs = new Bitmap(Properties.Resources.egg);
            graphicsEggs = panel.CreateGraphics();
            gEggs = Graphics.FromImage(backBufferEggs);

            //gEggs.Clear(Color.White);
            gEggs.DrawImage(spriteEggs, 20, 20, new Rectangle(0, 0, spriteEggs.Width, spriteEggs.Height), GraphicsUnit.Pixel);
            gEggs.Dispose();

            graphicsEggs.DrawImageUnscaled(backBufferEggs, 20, 20);
        }
        #endregion

        #region dieu khien gio
        //dieu khien ban phim
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                locationBasket -= 1;
                spriteBasket = new Bitmap(Properties.Resources.basket);
                graphicsBasket = panel.CreateGraphics();
                gBasket = Graphics.FromImage(backBufferBasket);

                gBasket.Clear(Color.White);
                gBasket.DrawImage(spriteBasket, panel.Width / 2 - spriteBasket.Width + 2 * locationBasket, panel.Height - spriteBasket.Height - 60, new Rectangle(0, 0, spriteBasket.Width, spriteBasket.Height), GraphicsUnit.Pixel);
                gBasket.Dispose();

                graphicsBasket.DrawImageUnscaled(backBufferBasket, 20, 20);
                return true;
            }
            if (keyData == Keys.Right)
            {
                locationBasket += 1;
                spriteBasket = new Bitmap(Properties.Resources.basket);
                graphicsBasket = panel.CreateGraphics();
                gBasket = Graphics.FromImage(backBufferBasket);

                gBasket.Clear(Color.White);
                gBasket.DrawImage(spriteBasket, panel.Width / 2 - spriteBasket.Width + 2 * locationBasket, panel.Height - spriteBasket.Height - 60, new Rectangle(0, 0, spriteBasket.Width, spriteBasket.Height), GraphicsUnit.Pixel);
                gBasket.Dispose();

                graphicsBasket.DrawImageUnscaled(backBufferBasket, 20, 20);
                return true;
            }
            return false;
        }
        #endregion
    }
}
