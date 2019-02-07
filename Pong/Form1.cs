﻿/*
 * Description:     A basic PONG simulator
 * Author:          Cameron Johnson 
 * Date:            Feb. 4, 2019
 */

#region libraries

using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Threading;
#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean upKeyDown,downKeyDown, leftKeyDown, rightKeyDown, wKeyDown, sKeyDown, aKeyDown, dKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        int BALL_SPEED = 4;
        Rectangle ball;

        //paddle speeds and rectangles
        const int PADDLE_SPEED = 4;
        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 5;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.I:
                    upKeyDown = true;
                    break;
                case Keys.K:
                    downKeyDown = true;
                    break;
                case Keys.J:
                    leftKeyDown = true;
                    break;
                case Keys.L:
                    rightKeyDown = true;
                    break;


                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.D:
                    dKeyDown = true;
                    break;

                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }
        
        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.I:
                   upKeyDown = false;
                    break;
                case Keys.K:
                    downKeyDown = false;
                    break;
                case Keys.J:
                    leftKeyDown = false;
                    break;
                case Keys.L:
                    rightKeyDown = false;
                    break;


                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.D:
                    dKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            //set Width and Height of ball
            ball.Height = ball.Width = 10;
            //set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2 - ball.Width / 2;
            //set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2 - ball.Height / 2;

        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            //create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight)
            {
                ball.X += BALL_SPEED;
            }
            else
            {
                ball.X -= BALL_SPEED;
            }

            //create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            if (ballMoveDown)
            {
                ball.Y += BALL_SPEED;
            }
            else
            {
                ball.Y -= BALL_SPEED;
            }

            #endregion

            #region update paddle positions

            if (wKeyDown == true && p1.Y > 0)
            {
                //create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
                p1.Y -= PADDLE_SPEED;
            }

            //create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED
            if (sKeyDown == true && p1.Y < this.Height - p1.Height)
            {
                //create code to move player 1 paddle down using p1.Y and PADDLE_SPEED
                p1.Y += PADDLE_SPEED;
            }
            if (aKeyDown == true && p1.X > 0)
            {
                //create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
                p1.X -= PADDLE_SPEED;
            }

            //create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED
            if (dKeyDown == true && p1.X < this.Width - p1.Width)
            {
                //create code to move player 1 paddle down using p1.Y and PADDLE_SPEED
                p1.X += PADDLE_SPEED;
            }




            if (upKeyDown == true && p2.Y > 0)
            {
                //create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED
                p2.Y -= PADDLE_SPEED;

            }
            if (downKeyDown == true && p2.Y < this.Height - p2.Height)
            {
                //create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED
                p2.Y += PADDLE_SPEED;
            }
            if (leftKeyDown == true && p2.X > 0)
            {
                //create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED
                p2.X -= PADDLE_SPEED;

            }
            if (rightKeyDown == true && p2.X < this.Width - p2.Width)
            {
                //create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED
                p2.X += PADDLE_SPEED;
            }




            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y + ball.Height >= this.Height || ball.Y < 0) // if ball hits top or bottom line
            {
                //use ballMoveDown boolean to change direction
                ballMoveDown = !ballMoveDown;
                //play a collision sound
                collisionSound.Play();
            }

            #endregion

            #region ball collision with paddles

            //create if statment that checks p1 or p2 collides with ball and if it does
            if (p1.IntersectsWith(ball) || p2.IntersectsWith(ball))
            {
                ballMoveRight = !ballMoveRight;
                collisionSound.Play();
                BALL_SPEED = BALL_SPEED + 1;
            }
                             
            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
               
                // --- play score sound
                // --- update player 2 score
                scoreSound.Play();
                player2Score++;
                BALL_SPEED = 4;

                // use if statement to check to see if player 2 has won the game. If true run 
                // GameOver method. Else change direction of ball and call SetParameters method.
                if (player2Score == gameWinScore)
                {
                    GameOver("Player 2");
                }

                else
                {
                    SetParameters();
                    ballMoveRight = false;
                }

            }

            if (ball.X > this.Width - ball.Width)
            {
                scoreSound.Play();
                player1Score++;
                BALL_SPEED = 4;


                if (player1Score == gameWinScore)
                {
                    GameOver("Player 1");
                }

                else
                {
                    SetParameters();
                    ballMoveRight = false;
                }
            }
            //same as above but this time check for collision with the right wall

            #endregion
            
            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            // TODO create game over logic

            // --- stop the gameUpdateLoop
            gameUpdateLoop.Stop();
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).
            startLabel.Text = winner + " wins!";
            startLabel.Visible = true;
            this.Refresh();

            // --- pause for two seconds 
            Thread.Sleep(2000);
            // --- use the startLabel to ask the user if they want to play again
            startLabel.Text = "Do you want to play again?\n" +
                "Press space to begin or N to cancel";

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw paddles using FillRectangle
            e.Graphics.FillRectangle(drawBrush, p1);
            e.Graphics.FillRectangle(drawBrush, p2);

            // TODO draw ball using FillRectangle
            e.Graphics.FillEllipse(drawBrush, ball);

            // TODO draw scores to the screen using DrawString
            e.Graphics.DrawString("Player 1 score: " + player1Score, drawFont, drawBrush, 10, 5);
            e.Graphics.DrawString("Player 2 score: " + player2Score, drawFont, drawBrush, this.Width - 150, 5);
        }

    }
}
