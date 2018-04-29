using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpriteLibrary;

namespace CatchGame
{
    public enum MyDir { left, right, stopped }

    public partial class Form1 : Form
    {
        //khai bao bien
        private Button btnPlay = new Button(); //button play to start the game
        private Button btnExit = new Button(); //button exit to exit the game

        private Label lbScore = new Label(); //label to display the score
        private Label lbCountScore = new Label();
        private Label lbTime = new Label(); //label to display the timer
        private Label lbCountTime = new Label();

        private Timer timer = new Timer(); //the timer to count the time
        private int i = 60; //the time to play is 60 seconds

        private PictureBox panel = new PictureBox(); //picturebox to display the sprite

        private Point spriteBasketPoint = new Point(100, 230); //where the basket first appear

        MyDir lastDirection = MyDir.stopped;

        SpriteController spriteController;
        bool left;  //basket is moved to left
        bool right;   //basket is moved to right
        bool move;
        int basketSpeed = 20;

        Bitmap backBufferBasket;
        Sprite spriteBasket;
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

            this.BackgroundImage = Properties.Resources.bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        //thiet ke form hien thi game
        public void myGame()
        {
            lbScore.Text = "Score";
            lbScore.Location = new Point(10, 10);
            lbScore.Size = new Size(40,20);
            lbScore.BackColor = Color.Transparent;
            this.Controls.Add(lbScore);

            lbTime.Text = "Time";
            lbTime.Location = new Point(210, 10);
            lbTime.Size = new Size(30, 20);
            lbTime.BackColor = Color.Transparent;
            this.Controls.Add(lbTime);

            lbCountScore.Text = "0";
            lbCountScore.Location = new Point(50, 10);
            lbCountScore.BackColor = Color.Transparent;
            this.Controls.Add(lbCountScore);

            lbCountTime.Text = "0";
            lbCountTime.Location = new Point(240, 10);
            lbCountTime.BackColor = Color.Transparent;
            this.Controls.Add(lbCountTime);

            panel.Size = new Size(this.Width, this.Height - 35);
            panel.Location = new Point(0, 35);
            panel.BackColor = Color.White;
            this.Controls.Add(panel);

            timer.Tick += new EventHandler(timerTick);
            timer.Enabled = true;
            timer.Interval = 1000;

            loadBackground();

            spriteController = new SpriteController(panel);
            spriteController.DoTick += CheckForKeyPress;

            backBufferBasket = new Bitmap(panel.Width, panel.Height);

            backBufferEggs = new Bitmap(panel.Width, panel.Height);

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
        //ve lai cua so khi resize
        private void DemoWindow_ResizeEnd(object sender, EventArgs e)
        {
            panel.Invalidate();
        }

        //set the background of the game
        private void loadBackground()
        {
            panel.BackgroundImage = Properties.Resources.bg;
            panel.BackgroundImageLayout = ImageLayout.Stretch;
        }

        //load hinh cai gio
        private void loadBasketImage()
        {
            spriteBasket = new Sprite(new Point(0, 0), spriteController, Properties.Resources.basket, 95, 50, 1000, 1);
            spriteBasket.SetSize(new Size(120, 100));

            spriteBasket.AddAnimation(new Point(0, 200), Properties.Resources.basket, 95, 50, 150, 1);
            spriteBasket.PutPictureBoxLocation(spriteBasketPoint);

            spriteBasket.CannotMoveOutsideBox = true;
        }

        //load hinh con ga
        private void loadChickenImage()
        {

        }

        //load hinh qua trung
        private void loadEggsImage()
        {
            //spriteEggs = new Bitmap(Properties.Resources.egg);
            //graphicsEggs = panel.CreateGraphics();
            //gEggs = Graphics.FromImage(backBufferEggs);

            ////gEggs.Clear(Color.White);
            //gEggs.DrawImage(spriteEggs, 20, 20, new Rectangle(0, 0, spriteEggs.Width, spriteEggs.Height), GraphicsUnit.Pixel);
            //gEggs.Dispose();

            //graphicsEggs.DrawImageUnscaled(backBufferEggs, 20, 20);
        }
        #endregion

        #region dieu khien gio
        //dieu khien ban phim
        private void CheckForKeyPress(object sender, EventArgs e)
        {
            left = false;
            right = false;
            move = false;

            if (spriteController.IsKeyPressed(Keys.Left))
            {
                left = true;
            }
            if (spriteController.IsKeyPressed(Keys.Right))
            {
                right = true;
            }

            if (left)
            {
                if (lastDirection != MyDir.left)
                {
                    spriteBasket.SetSpriteDirectionDegrees(180);
                    lastDirection = MyDir.left;
                }
                move = true;
                spriteBasket.MovementSpeed = basketSpeed;
                spriteBasket.AutomaticallyMoves = true;
            }
            if (right)
            {
                if (lastDirection != MyDir.right)
                {
                    spriteBasket.SetSpriteDirectionDegrees(0);
                    lastDirection = MyDir.right;
                }
                move = true;
                spriteBasket.AutomaticallyMoves = true;
                spriteBasket.MovementSpeed = basketSpeed;
            }
            //if (!move)
            //{
            //    lastDirection = MyDir.stopped;
            //    spriteBasket.MovementSpeed = 0;
            //}
        }
        #endregion
    }
}
