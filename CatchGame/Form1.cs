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
    public enum MyDir { left, right, stopped } //modes of the basket
    public enum SpriteNames { basket, egg, splash } //name of sprite

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

        DateTime lastMovement = DateTime.Now; //Used to give a slight delay in checking for keypress.

        SpriteController spriteController;
        bool left;  //basket is moved to left
        bool right;   //basket is moved to right
        bool move; //whether basket is moved
        int basketSpeed; //speed of the basket
        int eggsSpeed = 10; // speed of the eggs

        int score; //score of the gamer

        Bitmap backBufferBasket;
        Sprite spriteBasket; //spite of the basket
        Graphics graphicsBasket;
        Graphics gBasket;
        int locationBasket;

        Bitmap backBufferEggs;
        Sprite spriteEggs;
        Graphics graphicsEggs;
        Graphics gEggs;

        public Form1()
        {
            InitializeComponent();

            myInIt();
        }

        //design the form
        private void myInIt()
        {
            this.Text = "Catch Game";
            this.AutoSize = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            myMenu();
        }

        //design the form menu
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
        
        //button play
        //event when user click on the play button
        private void btnPlayClick(Object sender, EventArgs e)
        {
            btnPlay.Visible = false;
            btnExit.Visible = false;

            myGame();
        }

        //button exit
        //event when user click on the exit button
        private void btnExitClick(Object sender, EventArgs e)
        {
            this.Close();
        }

        //game play
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

            loadBasketImage();
            loadEggsImage();
        }

        //timer tick event
        private void timerTick(object sender, EventArgs e)
        {
            this.lbCountTime.Text = i.ToString();

            i--;
            if (i % 2 == 0 || i % 5 == 0)
            {
                loadEggsImage();
            }
            if (i < 0)
            {
                this.timer.Enabled = false;
                timeOut();
            }
        }

        //time out
        private void timeOut()
        {
            MessageBox.Show("Your score is: " + lbCountScore.Text.ToString(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnExit.Visible = true;
            btnPlay.Visible = true;

            lbScore.Visible = false;
            lbCountScore.Visible = false;
            lbTime.Visible = false;
            lbCountTime.Visible = false;
            panel.Visible=false;
        }

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

        //render basket image
        private void loadBasketImage()
        {
            spriteBasket = new Sprite(new Point(0, 0), spriteController, Properties.Resources.basket, 95, 50, 1000, 1);
            spriteBasket.SetSize(new Size(120, 100));
            spriteBasket.SetName(SpriteNames.basket.ToString());

            spriteBasket.AddAnimation(new Point(0, 200), Properties.Resources.basket, 95, 50, 150, 1);
            spriteBasket.PutPictureBoxLocation(spriteBasketPoint);

            spriteBasket.CannotMoveOutsideBox = true;
        }

        //render egg image
        private void loadEggsImage()
        {
            int startX = 0;
            int startY = 0;
            Random rand = new Random();

            startX = rand.Next(5, 320);
            startY = 5;

            spriteEggs = new Sprite(new Point(0, 0), spriteController, Properties.Resources.egg, 43, 50, 1000, 1);
            spriteEggs.SetSize(new Size(70, 120));
            spriteEggs.SetName(SpriteNames.egg.ToString());

            spriteEggs.AddAnimation(new Point(0, 200), Properties.Resources.egg, 41, 52, 150, 1);
            spriteEggs.PutPictureBoxLocation(new Point(startX, startY));

            spriteEggs.CannotMoveOutsideBox = true;

            spriteEggs.SetSpriteDirectionDegrees(270);
            spriteEggs.AutomaticallyMoves = true;
            spriteEggs.MovementSpeed = eggsSpeed;

            spriteEggs.SpriteHitsPictureBox += eggsCollision;
            spriteEggs.SpriteHitsSprite += eggHitBasket;
        }

        //control the basket
        private void CheckForKeyPress(object sender, EventArgs e)
        {
            left = false;
            right = false;
            move = false;

            basketSpeed = 10000;

            TimeSpan duration = DateTime.Now - lastMovement;
            if (duration.TotalMilliseconds < 100)
                return;

            lastMovement = DateTime.Now;

            if (spriteController.IsKeyPressed(Keys.Left))
            {
                left = true;
            }
            if (spriteController.IsKeyPressed(Keys.Right))
            {
                right = true;
            }

            if (left && right) return;
            if (left)
            {
                if (lastDirection != MyDir.left)
                {
                    spriteBasket.SetSpriteDirectionDegrees(180);
                    lastDirection = MyDir.left;
                }
                move = true;
                spriteBasket.AutomaticallyMoves = true;
                spriteBasket.MovementSpeed = basketSpeed;
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
            if (!move)
            {
                lastDirection = MyDir.stopped;

                spriteBasket.MovementSpeed = 0;
            }
        }

        //detect collision
        //when egg hit the ground
        private void eggsCollision(object sender, SpriteEventArgs e)
        {
            if (sender == null) return;

            //Sprite egg = (Sprite)sender;
            //Sprite nSprite = spriteController.DuplicateSprite(SpriteNames.splash.ToString());
            //nSprite.PutBaseImageLocation(egg.BaseImageLocation);
            //nSprite.AnimateOnce(0);
            //egg.Destroy();



            //throw new NotImplementedException();
        }
        
        //when egg hit the basket
        private void eggHitBasket(object sender, SpriteEventArgs e)
        {
            Sprite me = (Sprite)sender;

            //SoundPlayer newPlayer = new SoundPlayer(Properties.Resources.Tboom);
            //newPlayer.Play();
            score += 1;
            lbCountScore.Text = score.ToString();

            me.Destroy();
            //e.TargetSprite.Destroy();
        }
    }
}
