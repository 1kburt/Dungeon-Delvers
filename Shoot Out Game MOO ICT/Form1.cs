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
        int enemyHit = 0;
        int gold = 0;
        int animCounter = 0;
        bool firstRoom = true;
        bool allLocked = false;
        bool stairsVisible = false;
        Random randNum = new Random();
        int damage = 20;
        //enemy nums
        int batNum = 0;
        int ratNum = 0;
        int slimeNum = 0;
        int goblinNum = 0;
        int skeletonNum = 0;

        //Enemy speeds
        int enemySpeed = 3;
        int bat1Speed = 3;
        int bat2Speed = 3;
        int bat3Speed = 3;
        int rat1Speed = 2;
        int rat2Speed = 2;
        int rat3Speed = 2;

        //visibility
        bool bat1Visible = false;
        bool bat2Visible = false;
        bool bat3Visible = false;
        bool rat1Visible = false;
        bool rat2Visible = false;
        bool rat3Visible = false;

        //lists
        List<double> enemyHealth = new List<double>();
        List<string> enemyTypes = new List<string>();



        public DungeonDelvers()
        {
            InitializeComponent();
            RestartGame();
            stairs.Hide();
            bat1.Hide();
            bat2.Hide();
            bat3.Hide();
            rat1.Hide();
            rat2.Hide();
            rat3.Hide();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            enemyAnims();
            enemyMove();
            labelFloor.Text = ($"Floor: {floor} /  {BOSS_FLOOR}");
            labelRoom.Text = ($"Room: {roomNum}");
            labelRoom.Text = ($"Gold: {gold}");
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
                enemyHealth.Clear();


            }
            if (playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                player.Image = Properties.Resources.dead; //reset
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
            Collision();

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

        private void SpawnEnemies() //new enemy spawner
        {
            int chooseEnemy;
            //Choose number of enemies
            //Choose enemies
            if (floor == 1)
            {

                //bats and rats
                enemyNum = randNum.Next(1, 3); 
                for (int i = 0; i < enemyNum; i++)
                {
                    chooseEnemy = randNum.Next(0, 1);
                    if (chooseEnemy == 0 && !(batNum > 3))
                    {
                        //spawn bat
                        BatSpawn();
                    }
                    else if (!(ratNum > 3))
                    {
                        //spawn rat
                        RatSpawn();
                    }
                    else if (batNum != 3 && ratNum != 3)
                    {
                        i--; //decrease i by one to redo the loop
                    }
                }

            }
            else if (floor == 2)
            {
                //bats rats slimes
                enemyNum = randNum.Next(1, 3); //add enemy health in each part of list
                for (int i = 0; i < enemyNum; i++)
                {
                    chooseEnemy = randNum.Next(0, 2);
                    if (chooseEnemy == 0 && !(batNum > 3))
                    {
                        //spawn bat
                        BatSpawn();
                    }
                    else if (chooseEnemy == 1 && !(ratNum > 3))
                    {
                        //spawn rat
                        RatSpawn();
                    }
                    else if (!(slimeNum > 3))
                    {
                        //spawn slime
                    }
                }
            }
            else if (floor == 3)
            {
                //bats rats slimes goblins
                enemyNum = randNum.Next(1, 3); //add enemy health in each part of list
                for (int i = 0; i < enemyNum; i++)
                {
                    chooseEnemy = randNum.Next(0, 3);
                    if (chooseEnemy == 0 && !(batNum > 3))
                    {
                        //spawn bat
                        BatSpawn();
                    }
                    else if (chooseEnemy == 1 && !(ratNum > 3))
                    {
                        //spawn rat
                        RatSpawn();
                    }
                    else if (chooseEnemy == 2 && !(slimeNum > 3))
                    {
                        //spawn slime
                    }
                    else if (!(goblinNum > 3))
                    {
                        //spawn goblin
                    }
                }
                
            }
            else if (floor == 4)
            {
                //bats rats slimes goblins skeletons
                enemyNum = randNum.Next(1, 3); //add enemy health in each part of list
                for (int i = 0; i < enemyNum; i++)
                {
                    chooseEnemy = randNum.Next(0, 2);
                    if (chooseEnemy == 0 && !(batNum > 3))
                    {
                        //spawn bat
                        BatSpawn();

                    }
                    else if (chooseEnemy == 1 && !(ratNum > 3))
                    {
                        //spawn rat
                        RatSpawn();
                    }
                    else if (chooseEnemy == 2 && !(slimeNum > 3))
                    {
                        //spawn slime
                    }
                    else if (chooseEnemy == 3 && !(goblinNum > 3))
                    {
                        //spawn goblin
                    }
                    else if (!(skeletonNum > 3))
                    {
                        //spawn skeleton
                    }
                }
                
            }
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

        private void BatSpawn()
        {
            if (batNum == 0)
            {

                bat1.Show();
                bat1.Tag = "enemy";
                bat1.Left = randNum.Next(200, 700);
                bat1.Top = randNum.Next(200, 350);
                enemyHealth.Add(randNum.Next(100, 150) * enemyHealthMultiplier); //adjust values if necessary
                enemyTypes.Add("bat1");
                this.Controls.Add(bat1);
                bat1Visible = true;

            }
            else if(batNum == 1)
            {
                bat2.Show();
                bat2.Tag = "enemy";
                bat2.Left = randNum.Next(200, 700);
                bat2.Top = randNum.Next(200, 350);
                enemyHealth.Add(randNum.Next(100, 150) * enemyHealthMultiplier); //adjust values if necessary
                enemyTypes.Add("bat2");
                this.Controls.Add(bat2);
                bat2Visible = true;
            }
            else
            {
                bat3.Show();
                bat3.Tag = "enemy";
                bat3.Left = randNum.Next(200, 700);
                bat3.Top = randNum.Next(200, 350);
                enemyHealth.Add(randNum.Next(100, 150) * enemyHealthMultiplier); //adjust values if necessary
                enemyTypes.Add("bat3");
                this.Controls.Add(bat3);
                bat3Visible = true;
            }
            batNum++;
        }

        private void RatSpawn()
        {
            if (ratNum == 0)
            {

                rat1.Show();
                rat1.Tag = "enemy";
                rat1.Left = randNum.Next(200, 700);
                rat1.Top = randNum.Next(200, 350);
                enemyHealth.Add(randNum.Next(100, 150) * enemyHealthMultiplier); //adjust values if necessary
                enemyTypes.Add("rat1");
                this.Controls.Add(rat1);
                rat1Visible = true;

            }
            else if (ratNum == 1)
            {
                rat2.Show();
                rat2.Tag = "enemy";
                rat2.Left = randNum.Next(200, 700);
                rat2.Top = randNum.Next(200, 350);
                enemyHealth.Add(randNum.Next(100, 150) * enemyHealthMultiplier); //adjust values if necessary
                enemyTypes.Add("rat2");
                this.Controls.Add(rat2);
                rat2Visible = true;
            }
            else
            {
                rat3.Show();
                rat3.Tag = "enemy";
                rat3.Left = randNum.Next(200, 700);
                rat3.Top = randNum.Next(200, 350);
                enemyHealth.Add(randNum.Next(100, 150) * enemyHealthMultiplier); //adjust values if necessary
                enemyTypes.Add("rat3");
                this.Controls.Add(rat3);
                rat3Visible = true;
            }
            ratNum++;
        }


        private void Collision()
        {
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
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "enemy") //If bullet hits an enemy (specifically a zombie rn) - make them need multiple hits and include different enemy types
                    {
                        if (((PictureBox)bat1).Bounds.IntersectsWith(j.Bounds) && bat1Visible) //j is bullet
                        {
                            this.Controls.Remove(j);
                            
                            ((PictureBox)j).Dispose();
                            if (enemyNum == 0)
                            {
                                SpawnChest();
                            }
                            for (int i = 0; i < enemyHealth.Count; i++)
                            {
                                if (enemyTypes[i] == "bat1")
                                {
                                    enemyHit = i;
                                }
                            }
                            enemyHealth[enemyHit] -= damage;
                            //out of range error
                            if (enemyHealth[enemyHit] <= 0)
                            {
                                //die
                                bat1.Hide();
                                enemyNum -= 1;
                                enemyHealth.RemoveAt(enemyHit);
                                enemyTypes.RemoveAt(enemyHit);
                                bat1Visible = false;
                            }
                        }
                        if (((PictureBox)bat2).Bounds.IntersectsWith(j.Bounds) && bat2Visible) //j is bullet
                        {
                            this.Controls.Remove(j);

                            ((PictureBox)j).Dispose();
                            if (enemyNum == 0)
                            {
                                SpawnChest();
                            }
                            for (int i = 0; i < enemyHealth.Count; i++)
                            {
                                if (enemyTypes[i] == "bat2")
                                {
                                    enemyHit = i;
                                }
                            }
                            enemyHealth[enemyHit] -= damage; //out of range error
                            if (enemyHealth[enemyHit] <= 0)
                            {
                                //die
                                bat2.Hide();
                                enemyNum -= 1;
                                enemyHealth.RemoveAt(enemyHit);
                                enemyTypes.RemoveAt(enemyHit);
                                bat2Visible = false;
                            }
                        }
                        if (((PictureBox)bat3).Bounds.IntersectsWith(j.Bounds) && bat3Visible) //j is bullet
                        {
                            this.Controls.Remove(j);

                            ((PictureBox)j).Dispose();
                            if (enemyNum == 0)
                            {
                                SpawnChest();
                            }
                            for (int i = 0; i < enemyHealth.Count; i++)
                            {
                                if (enemyTypes[i] == "bat3")
                                {
                                    enemyHit = i;
                                }
                            }
                            enemyHealth[enemyHit] -= damage; //out of range error
                            if (enemyHealth[enemyHit] <= 0)
                            {
                                //die
                                bat3.Hide();
                                enemyNum -= 1;
                                enemyHealth.RemoveAt(enemyHit);
                                enemyTypes.RemoveAt(enemyHit);
                                bat3Visible = false;
                            }
                        }


                        if (((PictureBox)rat1).Bounds.IntersectsWith(j.Bounds) && rat1Visible) //j is bullet
                        {
                            this.Controls.Remove(j);

                            ((PictureBox)j).Dispose();
                            if (enemyNum == 0)
                            {
                                SpawnChest();
                            }
                            for (int i = 0; i < enemyHealth.Count; i++)
                            {
                                if (enemyTypes[i] == "rat1")
                                {
                                    enemyHit = i;
                                }
                            }
                            enemyHealth[enemyHit] -= damage;
                            //out of range error
                            if (enemyHealth[enemyHit] <= 0)
                            {
                                //die
                                rat1.Hide();
                                enemyNum -= 1;
                                enemyHealth.RemoveAt(enemyHit);
                                enemyTypes.RemoveAt(enemyHit);
                                bat1Visible = false;
                            }
                        }
                        if (((PictureBox)rat2).Bounds.IntersectsWith(j.Bounds) && rat2Visible) //j is bullet
                        {
                            this.Controls.Remove(j);

                            ((PictureBox)j).Dispose();
                            if (enemyNum == 0)
                            {
                                SpawnChest();
                            }
                            for (int i = 0; i < enemyHealth.Count; i++)
                            {
                                if (enemyTypes[i] == "rat2")
                                {
                                    enemyHit = i;
                                }
                            }
                            enemyHealth[enemyHit] -= damage;
                            //out of range error
                            if (enemyHealth[enemyHit] <= 0)
                            {
                                //die
                                rat2.Hide();
                                enemyNum -= 1;
                                enemyHealth.RemoveAt(enemyHit);
                                enemyTypes.RemoveAt(enemyHit);
                                bat1Visible = false;
                            }
                        }
                        if (((PictureBox)rat3).Bounds.IntersectsWith(j.Bounds) && rat3Visible) //j is bullet
                        {
                            this.Controls.Remove(j);

                            ((PictureBox)j).Dispose();
                            if (enemyNum == 0)
                            {
                                SpawnChest();
                            }
                            for (int i = 0; i < enemyHealth.Count; i++)
                            {
                                if (enemyTypes[i] == "rat3")
                                {
                                    enemyHit = i;
                                }
                            }
                            enemyHealth[enemyHit] -= damage;
                            //out of range error
                            if (enemyHealth[enemyHit] <= 0)
                            {
                                //die
                                rat1.Hide();
                                enemyNum -= 1;
                                enemyHealth.RemoveAt(enemyHit);
                                enemyTypes.RemoveAt(enemyHit);
                                bat1Visible = false;
                            }
                        }

                    }
                }
                //if the player hits an enemy
                if (x is PictureBox && x.Tag == "enemy")
                {
                    // below is the if statement thats checking the bounds of the player and the enemy
                    if (((PictureBox)bat1).Bounds.IntersectsWith(player.Bounds) && bat1Visible)
                    {
                        playerHealth -= 1;
                    }
                    if (((PictureBox)bat2).Bounds.IntersectsWith(player.Bounds) && bat2Visible)
                    {
                        playerHealth -= 1;
                    }
                    if (((PictureBox)bat3).Bounds.IntersectsWith(player.Bounds) && bat3Visible)
                    {
                        playerHealth -= 1;
                    }
                    if (((PictureBox)bat3).Bounds.IntersectsWith(player.Bounds) && rat1Visible)
                    {
                        playerHealth -= 1;
                    }
                    if (((PictureBox)bat3).Bounds.IntersectsWith(player.Bounds) && rat2Visible)
                    {
                        playerHealth -= 1;
                    }
                    if (((PictureBox)bat3).Bounds.IntersectsWith(player.Bounds) && rat3Visible)
                    {
                        playerHealth -= 1;
                    }

                }


            }


        }

        private void enemyAnims() //make enemies have different anim speed
        {
            if (animCounter >= 30)
            {
                animCounter = 0;
            }
            else if (animCounter <= 15)
            {
                bat1.Image = Properties.Resources.DD_BatUp2;
                bat2.Image = Properties.Resources.DD_BatUp2;
                bat3.Image = Properties.Resources.DD_BatUp2;
            }
            else if (animCounter >= 15)
            {
                bat1.Image = Properties.Resources.DD_BatDown4;
                bat2.Image = Properties.Resources.DD_BatDown4;
                bat3.Image = Properties.Resources.DD_BatDown4;
            }
            animCounter++;

        }

        private void enemyMove()
        {
            //move bat1 toward player
            if (((PictureBox)bat1).Left > player.Left)
            {
                ((PictureBox)bat1).Left -= bat1Speed;
            }
            if (((PictureBox)bat1).Top > player.Top)
            {
                ((PictureBox)bat1).Top -= bat1Speed;
            }
            if (((PictureBox)bat1).Left < player.Left)
            {
                ((PictureBox)bat1).Left += bat1Speed;
            }
            if (((PictureBox)bat1).Top < player.Top)
            {
                ((PictureBox)bat1).Top += bat1Speed;
            }
            //Stop overlap
            if (((PictureBox)bat1).Bounds.IntersectsWith(bat2.Bounds) && bat1Visible && bat2Visible)
            {
                //set bat1 speed to 0
                bat1Speed = 0;
            }
            else
            {
                bat1Speed = 3;
            }

            //move bat2 toward player
            if (((PictureBox)bat2).Left > player.Left)
            {
                ((PictureBox)bat2).Left -= bat2Speed;
            }
            if (((PictureBox)bat2).Top > player.Top)
            {
                ((PictureBox)bat2).Top -= bat2Speed;
            }
            if (((PictureBox)bat2).Left < player.Left)
            {
                ((PictureBox)bat2).Left += bat2Speed;
            }
            if (((PictureBox)bat2).Top < player.Top)
            {
                ((PictureBox)bat2).Top += bat2Speed;
            }
            //Stop overlap
            if (((PictureBox)bat2).Bounds.IntersectsWith(bat3.Bounds) && bat2Visible && bat3Visible)
            {
                //set bat1 speed to 0
                bat2Speed = 0;
            }
            else
            {
                bat2Speed = 3;
            }

            //move bat3 toward player
            if (((PictureBox)bat3).Left > player.Left)
            {
                ((PictureBox)bat3).Left -= bat3Speed;
            }
            if (((PictureBox)bat3).Top > player.Top)
            {
                ((PictureBox)bat3).Top -= bat3Speed;
            }
            if (((PictureBox)bat3).Left < player.Left)
            {
                ((PictureBox)bat3).Left += bat3Speed;
            }
            if (((PictureBox)bat3).Top < player.Top)
            {
                ((PictureBox)bat3).Top += bat3Speed;
            }
            if (((PictureBox)bat3).Bounds.IntersectsWith(bat1.Bounds) && bat3Visible && bat1Visible)
            {
                //set bat3 speed to 0
                bat3Speed = 0;
            }
            else
            {
                bat3Speed = 3;
            }



            //rats
            //move rat1 toward player
            if (((PictureBox)rat1).Left > player.Left)
            {
                ((PictureBox)rat1).Left -= rat1Speed;
            }
            if (((PictureBox)rat1).Top > player.Top)
            {
                ((PictureBox)rat1).Top -= rat1Speed;
            }
            if (((PictureBox)rat1).Left < player.Left)
            {
                ((PictureBox)rat1).Left += rat1Speed;
            }
            if (((PictureBox)rat1).Top < player.Top)
            {
                ((PictureBox)rat1).Top += rat1Speed;
            }
            //Stop overlap
            if (((PictureBox)rat1).Bounds.IntersectsWith(rat2.Bounds) && rat1Visible && rat2Visible)
            {
                //set bat1 speed to 0
                rat1Speed = 0;
            }
            else
            {
                rat1Speed = 3;
            }

            //move rat2 toward player
            if (((PictureBox)rat2).Left > player.Left)
            {
                ((PictureBox)rat2).Left -= rat2Speed;
            }
            if (((PictureBox)rat2).Top > player.Top)
            {
                ((PictureBox)rat2).Top -= rat2Speed;
            }
            if (((PictureBox)rat2).Left < player.Left)
            {
                ((PictureBox)rat2).Left += rat2Speed;
            }
            if (((PictureBox)rat2).Top < player.Top)
            {
                ((PictureBox)rat2).Top += rat2Speed;
            }
            //Stop overlap
            if (((PictureBox)rat2).Bounds.IntersectsWith(rat3.Bounds) && rat2Visible && rat3Visible)
            {
                //set bat1 speed to 0
                rat2Speed = 0;
            }
            else
            {
                rat2Speed = 3;
            }

            //move bat3 toward player
            if (((PictureBox)rat3).Left > player.Left)
            {
                ((PictureBox)rat3).Left -= bat1Speed;
            }
            if (((PictureBox)rat3).Top > player.Top)
            {
                ((PictureBox)rat3).Top -= rat3Speed;
            }
            if (((PictureBox)rat3).Left < player.Left)
            {
                ((PictureBox)rat3).Left += rat3Speed;
            }
            if (((PictureBox)rat3).Top < player.Top)
            {
                ((PictureBox)rat3).Top += bat1Speed;
            }
            if (((PictureBox)rat3).Bounds.IntersectsWith(rat1.Bounds) && rat3Visible && rat1Visible)
            {
                //set bat3 speed to 0
                rat3Speed = 0;
            }
            else
            {
                rat3Speed = 3;
            }


        }

    }


}
