using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Shoot_Out_Game_MOO_ICT
{
    public partial class DungeonDelvers : Form
    {

        //Constants
        const int MAX_RAND = 10;
        const int BOSS_FLOOR = 5;
        //Variables
        bool goLeft, goRight, goUp, goDown, gameOver;
        string lockedRoom = "None";
        string facing = "up";
        int playerHealth = 100;
        int speed = 8;
        int stairsChance = 0;
        int enemyNum;
        double enemyHealthMultiplier = 1;
        int floor = 1;
        int roomNum = 0;
        bool firstRoom = true;
        bool allLocked = false;
        bool stairsVisible = false;
        Random randNum = new Random();

        //Enemy speeds
        int zombieSpeed = 3;

        //lists
        List<float> enemyHealth = new List<float>();



        public DungeonDelvers()
        {
            InitializeComponent();
            RestartGame();
            stairs.Hide();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            labelFloor.Text = ($"Floor: {floor.ToString()} /  {BOSS_FLOOR}");
            labelRoom.Text = ($"Room: {roomNum.ToString()}");
            NewFloor();
            //Doors
            //Door Interaction
            //make it so your movement is reversed when touching a locked door
            if (enemyNum == 0)
            {
                allLocked = false;
            }
            else
            {
                allLocked = true; //when you kill an enemy I need to make it reduce enemynum by 1
            }
            if (((PictureBox)player).Bounds.IntersectsWith(basicDoor.Bounds) && !allLocked && lockedRoom != "Left") //left door
            {
                //go to right
                player.Left = 750;
                player.Top = 270;
                //lock right
                lockedRoom = "Right";
                LoadRoom();

            }
            if (((PictureBox)player).Bounds.IntersectsWith(basicDoor1.Bounds) && !allLocked && lockedRoom != "Right") //right door
            {
                //go to left
                player.Left = 100;
                player.Top = 270;
                //lock left
                lockedRoom = "Left";
                LoadRoom();
            }
            if (((PictureBox)player).Bounds.IntersectsWith(basicDoor2.Bounds) && !allLocked && lockedRoom != "Bottom") //bottom door
            {
                //go to top
                player.Left = 420;
                player.Top = 100;
                //lock top
                lockedRoom = "Top";
                LoadRoom();
            }
            if (((PictureBox)player).Bounds.IntersectsWith(basicDoor3.Bounds) && !allLocked && lockedRoom != "Top") //top door
            {
                //go to bottom
                player.Left = 420;
                player.Top = 450;
                //lock bottom
                lockedRoom = "Bottom";
                LoadRoom();
            }

            if (enemyNum == 0)
            {
                allLocked = false;
                //Change sprite on all doors except "lockedDoor"
                //Spawn Chest


            }
            if (playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                player.Image = Properties.Resources.dead;
                GameTimer.Stop();
            }

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }
            if (goUp == true && player.Top > 45)
            {
                player.Top -= speed;
            }
            if (goDown == true && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }



            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();

                    }
                }


                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie") //make them need multiple hits and include different enemy types
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            enemyNum -= 1;
                            if (enemyNum == 0)
                            {
                                SpawnChest();
                            }
                        }
                    }
                }


            }


        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if (gameOver == true)
            {
                return;
            }

            if (e.KeyCode == Keys.A)
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }

            if (e.KeyCode == Keys.W)
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }

            if (e.KeyCode == Keys.S)
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }



        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = false;
            }

            if (e.KeyCode == Keys.W)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.S)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && gameOver == false)
            {
                ShootBullet(facing);


            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }

        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet(this);
        }


        private void RestartGame() //edit this
        {
            player.Image = Properties.Resources.up;

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;

            playerHealth = 100;

            GameTimer.Start();
        }

        private void LoadRoom()
        {
            //Declare Variables
            int stairs;



            //create layout
            roomNum += 1;
            firstRoom = false;
            //Check for stairs
            stairsChance += randNum.Next(roomNum, MAX_RAND + roomNum); //Change MAX_RAND to increase the odds of rolling a staircase each time
            if (stairsChance >= 100)
            {
                stairsChance = 100;
            }
            //random num to set room
            stairs = randNum.Next(stairsChance, 100);
            if (stairs == 100)
            {
                SpawnStairs();
            }
            //Lock doors
            if (firstRoom == true)
            {
                //Unlock all doors
                allLocked = false;
                lockedRoom = "None";
                //Spawn weapon

                //Spawn Treasure
            }
            else
            {
                // Lock all rooms
                allLocked = true;
            }
            //Spawn enemies
            SpawnEnemies();


            
            //PictureBox pictureBox = new PictureBox();
            //enemies multi-array
            //items multi-array


            //create room
            


        }

        private void SpawnChest()
        {

        }

        private void DungeonDelvers_Load(object sender, EventArgs e)
        {
            stairs.Hide();
        }

        private void SpawnEnemies()
        {

            //Choose enemy type


            //Choose number of enemies
            enemyNum = randNum.Next(1, 3) + floor; //make list with enemy health in each part of it
            for (int i = 0; i < enemyNum; i++)
            {
                PictureBox zombie = new PictureBox(); // create a new picture box called zombie
                zombie.Tag = "zombie"; // add a tag to it called zombie
                zombie.Image = Properties.Resources.zdown; // the default picture for the zombie is zdown
                zombie.Left = randNum.Next(200, 700); // generate a number between 0 and 900 and assignment that to the new zombies left 
                zombie.Top = randNum.Next(200, 350); // generate a number between 0 and 800 and assignment that to the new zombies top
                zombie.SizeMode = PictureBoxSizeMode.AutoSize; // set auto size for the new picture box
                this.Controls.Add(zombie); // add the picture box to the screen
                player.BringToFront(); // bring the player to the front
            }
            //ignore enemies in bonus room
            //set enemy health to enemy health * multiplier
            
            //Up to here
            //Zombies
            
        }
    

        private void SpawnStairs()
        {
            stairs.Top = randNum.Next(120, 500);
            stairs.Left = randNum.Next(50, 800);
            stairs.Show();
            stairsVisible = true;
        }

        private void NewFloor()
        {
            if (((PictureBox)player).Bounds.IntersectsWith(stairs.Bounds) && stairsVisible) //checks if you are touching the stairs
            {   //up to here
                floor += 1; //increase floor by 1
                roomNum = -1;
                stairsChance = 0;
                firstRoom = true;
                enemyHealthMultiplier += 1.15; //boost enemy health
                LoadRoom();
                if (floor == BOSS_FLOOR) //if the floor is the boss floor
                {
                    BossFloor();
                }
                stairs.Hide();
                stairsVisible = false;
            }
        }

        private void BossFloor()
        {

        }



    }


}
