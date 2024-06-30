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
using Shoot_Out_Game_MOO_ICT.Properties;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Shoot_Out_Game_MOO_ICT
{
    public partial class DungeonDelvers : Form
    {

        //Constants
        const int MAX_RAND = 10;
        const int BOSS_FLOOR = 5;
        const int CHARGE_PERIOD = -15;
        const int GOBLIN_GUN_DISTANCE = 150; //make larger to increase range for the goblin gun
        //Variables
        bool goLeft, goRight, goUp, goDown;
        bool keyUp, keyDown, keyLeft, keyRight;
        string lockedRoom = "None";
        string facing = "up";
        string bloodType;
        bool gunDropVisible = false;
        int playerHealth = 30;
        int maxHealth = 30;
        int maxiFrames = 5; //need to apply this properly
        int iFrames = 0; //change to increase the base num of iFrames the player has (5)
        int speed = 8;
        int stairsChance = 0;
        int enemyNum;
        int flipPlayerRight = 0;
        int flipPlayerLeft = 0;
        double enemyHealthMultiplier = 1;
        int floor = 1;
        int coinDrops = 0;
        int enemyCoinDrops;
        int healthDrops = 0;
        int roomNum = 0;
        int souls = 0;
        int floorLootSpawned = 0;
        int soulDropChance = 0;
        int enemyHit = 0;
        int gold = 0;
        int stopRemoveLoot = 0;
        int animCounter = 0;
        int slimeAnimCounter = 0;
        int skeletonAnimCounter = 0;
        int moveFrameUpdate = 0;
        bool haveGun = false;
        bool firstRoom = true;
        bool allLocked = false;
        bool stairsVisible = false;
        bool chestVisible = false;
        bool chestLooted = false;
        bool heartsSpawned = false;
        bool stairsWillSpawn = false;
        bool respawn = false;
        //public bool bulletHitsPlayer = false;
        bool removeLoot = false;
        int enemyDropTop;
        int gunHeight;
        int enemyDropLeft;
        int smallBloodImage, bigBloodImage;
        int weaponLuck = 0;
        bool rat1FacingRight = true;
        bool rat2FacingRight = true;
        bool rat3FacingRight = true;
        bool slime1FacingRight = false;
        bool slime2FacingRight = false;
        bool slime3FacingRight = false;
        bool skeleton1FacingRight = true;
        bool skeleton2FacingRight = true;
        bool skeleton3FacingRight = true;
        bool goblin1FacingRight = true;
        bool goblin2FacingRight = true;
        bool goblin3FacingRight = true;
        bool finishedDeathSequence = false;
        bool windowClosed = false;
        bool quit = false;
        int skeleton1Charge = 30;
        int skeleton2Charge = 25;
        int skeleton3Charge = 20;
        int chargeLeft1, chargeTop1, chargeLeft2, chargeTop2, chargeLeft3, chargeTop3;
        Random randNum = new Random();
        double damage = 30; //damage from gun
        double damageMultiplier = 1;
        int cooldown = 50; //change based on gun type
        int chooseEnemy;
        int goblin1Cooldown = 100;
        int goblin2Cooldown = 100;
        int goblin3Cooldown = 100;
        //enemy nums
        int batNum = 0;
        int ratNum = 0;
        int slimeNum = 0;
        int goblinNum = 0;
        int skeletonNum = 0;
        int flipGunRight = 0;
        int flipGunLeft = 0;

        //PB sizes
        Size coinSize = new Size(30, 30);
        Size potionSize = new Size(40, 50);
        Size soulSize = new Size(38, 44);
        Size smallBloodSplatter = new Size(20, 40);
        Size bigBloodSplatter = new Size(50, 50);
        Size mainSpaceGunSize = new Size(63, 43); //69, 44
        Size SpaceGunUpSize = new Size(43, 63);


        //Enemy speeds
        int bat1Speed = 3;
        int bat2Speed = 3;
        int bat3Speed = 3;
        int rat1Speed = 2;
        int rat2Speed = 2;
        int rat3Speed = 2;
        int slime1Speed = 10;
        int slime2Speed = 10;
        int slime3Speed = 10;
        int goblin1Speed = 2;
        int goblin2Speed = 2;
        int goblin3Speed = 2;
        int skeleton1Speed = 2;
        int skeleton2Speed = 2;
        int skeleton3Speed = 2;



        //visibility
        bool bat1Visible = false;
        bool bat2Visible = false;
        bool bat3Visible = false;
        bool rat1Visible = false;
        bool rat2Visible = false;
        bool rat3Visible = false;
        bool slime1Visible = false;
        bool slime2Visible = false;
        bool slime3Visible = false;
        bool goblin1Visible = false;
        bool goblin2Visible = false;
        bool goblin3Visible = false;
        bool skeleton1Visible = false;
        bool skeleton2Visible = false;
        bool skeleton3Visible = false;

        //lists
        List<double> enemyHealth = new List<double>();
        List<string> enemyTypes = new List<string>();
        List<string> chestLoot = new List<string>();
        List<string> upgrades = new List<string>();


        public DungeonDelvers()
        {
            InitializeComponent();
            //RestartGame();
            stairs.Hide();
            bat1.Hide();
            bat2.Hide();
            bat3.Hide();
            rat1.Hide();
            rat2.Hide();
            rat3.Hide();
            slime1.Hide();
            slime2.Hide();
            slime3.Hide();
            skeleton1.Hide();
            skeleton2.Hide();
            skeleton3.Hide();
            continueBtn.Hide();
            deathScreen.Hide();
            goblin1.Hide();
            goblin1Gun.Hide();
            goblin2.Hide();
            goblin2Gun.Hide();
            goblin3.Hide();
            goblin3Gun.Hide();
            if (maxHealth > 30 && maxHealth <= 40)
            {
                heart4.Show();
                heart5.Hide();
            }
            else if (maxHealth > 40)
            {
                heart4.Show();
                heart5.Show();
            }
            else
            {
                heart4.Hide();
                heart5.Hide();
            }
            StartRun();
        }


        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (windowClosed)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                using (StreamReader reader = new StreamReader("respawn.csv"))
                {

                    respawn = bool.Parse(reader.ReadToEnd());
                }
                if (respawn)
                {
                    //reset game maximise window - dont actually maximise just set to normal size
                    windowClosed = false;
                    this.WindowState = FormWindowState.Normal;
                    StartRun();
                }
                using (StreamReader reader2 = new StreamReader("quit.csv")) //quits the game if instructed to by main menu
                {
                    quit = bool.Parse(reader2.ReadToEnd());
                    if (quit)
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.Close();
                    }
                }

            }

            if (!finishedDeathSequence)
            {
                //resize window
                this.MinimumSize = new Size(940, 700);
                this.MaximumSize = new Size(940, 700);
                //sets weapon damage
                damage = Bullet.damage;
                if (haveGun)
                {
                    gun.Show();
                }
                else
                {
                    gun.Hide();
                }
                if (facing == "right" && flipGunRight == 0 && flipGunLeft == 0 && flipPlayerRight == 0 && flipPlayerLeft == 0)
                {
                    gun.Image = Image.FromFile("images/SpaceGun.png");
                }
                if (facing == "right" && flipGunRight == 0 && flipGunLeft == 0 && flipPlayerRight == 0 && flipPlayerLeft == 0)
                {
                    gun.Image = Image.FromFile("images/SpaceGun.png");
                }
                gun.Top = player.Top + 20;
                //fix anim and add right facing anim
                GoblinShoot();
                if (iFrames > 0)
                {
                    iFrames--;
                }
                if (flipPlayerRight > 1)
                {
                    flipPlayerRight--;
                }
                if (flipPlayerLeft > 1)
                {
                    flipPlayerLeft--;
                }
                if (flipGunRight > 1)
                {
                    flipGunRight--;
                }
                if (flipGunLeft > 1)
                {
                    flipGunLeft--;
                }
                if (flipPlayerRight == 1)
                {
                    player.Image = Image.FromFile("images/PlayerRight1.png");
                    gun.Image = Image.FromFile("images/SpaceGun.png");
                    flipPlayerRight = 0;
                    gun.Left = player.Left + 65;
                }
                if (flipPlayerLeft == 1)
                {
                    player.Image = Image.FromFile("images/PlayerLeft1.png");
                    gun.Image = Image.FromFile("images/SpaceGunL.png");
                    flipPlayerLeft = 0;
                    gun.Left = player.Left - 65;

                }
                if (flipGunRight == 1)
                {
                    gun.Image = Image.FromFile("images/SpaceGun.png");
                    gun.Size = mainSpaceGunSize;
                    flipGunRight = 0;
                }
                if (flipGunLeft == 1)
                {
                    gun.Image = Image.FromFile("images/SpaceGunL.png");
                    gun.Size = mainSpaceGunSize;
                    flipGunLeft = 0;
                }
                if (cooldown > 0)
                {
                    cooldown--;
                }
                if (goblin1Cooldown > 0)
                {
                    goblin1Cooldown--;
                }
                if (goblin2Cooldown > 0)
                {
                    goblin2Cooldown--;
                }
                if (goblin3Cooldown > 0)
                {
                    goblin3Cooldown--;
                }
                if (stopRemoveLoot > 0)
                {
                    stopRemoveLoot--;
                }
                if (stopRemoveLoot == 0)
                {
                    removeLoot = false;
                }
                if (skeleton1Charge > 0)
                {
                    skeleton1Charge--;
                    chargeLeft1 = player.Left;
                    chargeTop1 = player.Top;

                }
                if (skeleton2Charge > 0)
                {
                    skeleton2Charge--;
                    chargeLeft2 = player.Left;
                    chargeTop2 = player.Top;

                }
                if (skeleton3Charge > 0)
                {
                    skeleton1Charge--;
                    chargeLeft3 = player.Left;
                    chargeTop3 = player.Top;

                }
                //spawn gun
                goblin1Gun.Top = goblin1.Top + 10;
                //gun.Top = gunHeight;
                if (goLeft && flipPlayerLeft == 0 && flipPlayerRight == 0)
                {
                    gun.Left = player.Left - 65;
                }
                else if (goRight && flipPlayerLeft == 0 && flipPlayerRight == 0)
                {
                    gun.Left = player.Left + 70;
                }
                if (goblin1FacingRight)
                {
                    goblin1Gun.Left = goblin1.Left + 55;
                    goblin1Gun.Image = Image.FromFile("images/spaceGun.png");
                }
                else if (!goblin1FacingRight)
                {
                    goblin1Gun.Left = goblin1.Left - 55;
                    goblin1Gun.Image = Image.FromFile("images/spaceGunL.png");
                }
                //spawn gun2
                goblin2Gun.Top = goblin2.Top + 10;
                if (goblin2FacingRight)
                {
                    goblin2Gun.Left = goblin2.Left + 55;
                    goblin2Gun.Image = Image.FromFile("images/spaceGun.png");
                }
                else if (!goblin2FacingRight)
                {
                    goblin2Gun.Left = goblin2.Left - 55;
                    goblin2Gun.Image = Image.FromFile("images/spaceGunL.png");
                }
                //spawn gun2
                goblin3Gun.Top = goblin3.Top + 10;
                if (goblin3FacingRight)
                {
                    goblin3Gun.Left = goblin3.Left + 55;
                    goblin3Gun.Image = Image.FromFile("images/spaceGun.png");
                }
                else if (!goblin3FacingRight)
                {
                    goblin3Gun.Left = goblin3.Left - 55;
                    goblin3Gun.Image = Image.FromFile("images/spaceGunL.png");
                }
                RefreshHealth();
                OpenChest();
                EnemyAnims();
                EnemyMove();
                labelFloor.Text = ($"Floor: {floor} /  {BOSS_FLOOR}");
                labelRoom.Text = ($"Room: {roomNum}");
                labelGold.Text = (gold.ToString());
                labelSouls.Text = (souls.ToString());
                NewFloor();
                //Doors
                //Door Interaction
                if (chestVisible)
                {
                    chest.Show();
                }
                else
                {
                    chest.Hide();
                }
                if (enemyNum == 0)
                {
                    allLocked = false;
                    if (lockedRoom == "Right")
                    {
                        basicDoor.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor2.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor3.Image = Image.FromFile("images/DoorOpen.png");
                    }
                    else if (lockedRoom == "Left")
                    {
                        basicDoor1.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor2.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor3.Image = Image.FromFile("images/DoorOpen.png");
                    }
                    else if (lockedRoom == "Top")
                    {
                        basicDoor.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor2.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor1.Image = Image.FromFile("images/DoorOpen.png");
                    }
                    else if (lockedRoom == "Bottom")
                    {
                        basicDoor.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor1.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor3.Image = Image.FromFile("images/DoorOpen.png");
                    }
                    else
                    {
                        basicDoor.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor1.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor2.Image = Image.FromFile("images/DoorOpen.png");
                        basicDoor3.Image = Image.FromFile("images/DoorOpen.png");
                    }

                }
                else
                {
                    allLocked = true; //when you kill an enemy I need to make it reduce enemynum by 1
                    basicDoor.Image = Image.FromFile("images/DoorClosed.png");
                    basicDoor1.Image = Image.FromFile("images/DoorClosed.png");
                    basicDoor2.Image = Image.FromFile("images/DoorClosed.png");
                    basicDoor3.Image = Image.FromFile("images/DoorClosed.png");
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
                    enemyHealth.Clear();


                }
                if (playerHealth <= 0)
                {
                    Die();
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

        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if (!finishedDeathSequence)
            {
                if (facing == "left")
                {
                    if (moveFrameUpdate == 10)
                    {
                        moveFrameUpdate = 0;
                    }
                    else if (moveFrameUpdate < 5)
                    {
                        player.Image = Image.FromFile("images/PlayerLeft1.png");
                    }
                    else if (moveFrameUpdate >= 5)
                    {
                        player.Image = Image.FromFile("images/PlayerLeft2.png");
                    }
                    moveFrameUpdate++;
                }
                if (facing == "right")
                {
                    if (moveFrameUpdate == 10)
                    {
                        moveFrameUpdate = 0;
                    }
                    else if (moveFrameUpdate < 5)
                    {
                        player.Image = Image.FromFile("images/PlayerRight1.png");
                    }
                    else if (moveFrameUpdate >= 5)
                    {
                        player.Image = Image.FromFile("images/PlayerRight2.png");
                    }
                    moveFrameUpdate++;
                }

                if (e.KeyCode == Keys.A)
                {
                    if (facing == "right")
                    {
                        player.Image = Image.FromFile("images/PlayerLeft1.png");
                        moveFrameUpdate = 0;
                    }
                    goLeft = true;
                    facing = "left";
                    gun.Image = Image.FromFile("images/spaceGunL.png");

                }
                if (e.KeyCode == Keys.D)
                {
                    if (facing == "left")
                    {
                        moveFrameUpdate = 0;
                        player.Image = Image.FromFile("images/PlayerRight1.png");
                    }
                    goRight = true;
                    facing = "right";
                    gun.Image = Image.FromFile("images/spaceGun.png");
                }

                if (e.KeyCode == Keys.W)
                {
                    goUp = true;
                }

                if (e.KeyCode == Keys.S)
                {
                    goDown = true;
                }

                if (e.KeyCode == Keys.Up)
                {
                    keyUp = true;
                }

                if (e.KeyCode == Keys.Right)
                {
                    keyRight = true;
                }
                if (e.KeyCode == Keys.Down)
                {
                    keyDown = true;
                }
                if (e.KeyCode == Keys.Left)
                {
                    keyLeft = true;
                }
                if (e.KeyCode == Keys.NumPad0) //placeholder dev things
                {
                    souls += 10;
                }
                if (e.KeyCode == Keys.NumPad1)
                {
                    souls = 0;
                }

                if ((e.KeyCode == Keys.NumPad2))
                {
                    floor += 1;
                }
                if (((e.KeyCode == Keys.NumPad3)))
                {
                    playerHealth = maxHealth;
                }
            }


        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (!finishedDeathSequence)
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


                if (cooldown == 0 && haveGun)
                {
                    if (keyRight && !keyUp && !keyDown && !keyLeft)
                    {
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "right";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        keyRight = false;
                        cooldown = Bullet.weaponCooldown;
                        gun.Size = mainSpaceGunSize;
                        gun.Image = Image.FromFile("images/spaceGun.png");
                        //flip player
                        if (facing == "left")
                        {
                            player.Image = Image.FromFile("images/PlayerRight1.png");
                            moveFrameUpdate = 0;
                            flipPlayerLeft = 10;
                            if (goLeft)
                            {
                                gun.Left = player.Left + 20;
                            }
                            else
                            {
                                gun.Left = player.Left + 65;
                            }

                        }
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "rightup";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "rightdown";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }
                    }
                    else if (keyLeft && !keyRight && !keyUp && !keyDown)
                    {
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "left";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        keyLeft = false;
                        cooldown = Bullet.weaponCooldown;
                        gun.Size = mainSpaceGunSize;
                        gun.Image = Image.FromFile("images/spaceGunL.png");
                        //flip player
                        if (facing == "right")
                        {
                            player.Image = Image.FromFile("images/PlayerLeft1.png");
                            moveFrameUpdate = 0;
                            flipPlayerRight = 10;
                            if (goRight)
                            {
                                gun.Left = player.Left - 20;
                            }
                            else
                            {
                                gun.Left = player.Left - 65;
                            }

                        }
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "leftup";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "leftdown";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }
                    }
                    else if (!keyLeft && !keyRight && keyUp && !keyDown)
                    {
                        if (facing == "right")
                        {
                            gun.Size = SpaceGunUpSize;
                            gun.Image = Image.FromFile("images/spaceGunUp.png");
                            flipGunRight = 10;
                        }
                        else
                        {
                            gun.Size = SpaceGunUpSize;
                            gun.Image = Image.FromFile("images/spaceGunUpL.png");
                            flipGunLeft = 10;
                        }
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "up";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        keyUp = false;
                        cooldown = Bullet.weaponCooldown;
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "leftup";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "rightup";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }
                    }
                    else if (!keyLeft && !keyRight && !keyUp && keyDown)
                    {
                        if (facing == "right")
                        {
                            gun.Size = SpaceGunUpSize;
                            gun.Image = Image.FromFile("images/spaceGunDown.png");
                            gunHeight = player.Top + 50;
                            flipGunRight = 5;
                        }
                        else
                        {
                            gun.Size = SpaceGunUpSize;
                            gun.Image = Image.FromFile("images/spaceGunDownL.png");
                            gunHeight = player.Top + 50;
                            flipGunLeft = 5;
                        }
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "down";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        keyDown = false;
                        cooldown = Bullet.weaponCooldown;
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "leftdown";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "rightdown";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }
                    }
                    else if (keyRight && keyUp && !keyLeft && !keyDown)
                    {
                        if (facing == "left")
                        {
                            player.Image = Image.FromFile("images/PlayerRight1.png");
                            moveFrameUpdate = 0;
                            flipPlayerLeft = 10;
                            if (goLeft)
                            {
                                gun.Left = player.Left + 20;
                            }
                            else
                            {
                                gun.Left = player.Left + 65;
                            }

                        }
                        else
                        {
                            flipGunRight = 5;
                        }
                        gun.Image = Image.FromFile("images/spaceGunUp45.png");
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "rightup";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "up";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "right";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }

                        keyRight = false;
                        keyUp = false;
                        cooldown = Bullet.weaponCooldown;
                    }
                    else if (keyLeft && keyUp && !keyRight && !keyDown)
                    {
                        if (facing == "right")
                        {
                            player.Image = Image.FromFile("images/PlayerLeft1.png");
                            moveFrameUpdate = 0;
                            flipPlayerLeft = 10;
                            if (goLeft)
                            {
                                gun.Left = player.Left - 20;
                            }
                            else
                            {
                                gun.Left = player.Left - 65;
                            }

                        }
                        else
                        {
                            flipGunLeft = 5;
                        }
                        gun.Image = Image.FromFile("images/spaceGunUpL45.png");
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "leftup";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        keyLeft = false;
                        keyUp = false;
                        cooldown = Bullet.weaponCooldown;
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "up";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "left";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }
                    }
                    else if (keyRight && keyDown && !keyLeft && !keyUp)
                    {
                        if (facing == "left")
                        {
                            player.Image = Image.FromFile("images/PlayerRight1.png");
                            moveFrameUpdate = 0;
                            flipPlayerLeft = 10;
                            if (goLeft)
                            {
                                gun.Left = player.Left + 20;
                            }
                            else
                            {
                                gun.Left = player.Left + 65;
                            }

                        }
                        else
                        {
                            flipGunRight = 5;
                        }
                        gun.Image = Image.FromFile("images/spaceGunDown45.png");
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "rightdown";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        keyRight = false;
                        keyDown = false;
                        cooldown = Bullet.weaponCooldown;
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "down";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "right";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }
                    }
                    else if (keyLeft && keyDown && !keyUp && !keyRight)
                    {
                        if (facing == "right")
                        {
                            player.Image = Image.FromFile("images/PlayerLeft1.png");
                            moveFrameUpdate = 0;
                            flipPlayerLeft = 10;
                            if (goLeft)
                            {
                                gun.Left = player.Left - 20;
                            }
                            else
                            {
                                gun.Left = player.Left - 65;
                            }

                        }
                        else
                        {
                            flipGunLeft = 5;
                        }
                        gun.Image = Image.FromFile("images/spaceGunDownL45.png");
                        Bullet shootBullet = new Bullet();
                        shootBullet.shooter = "player";
                        shootBullet.direction = "leftdown";
                        shootBullet.bulletLeft = gun.Left + (gun.Width / 2);
                        shootBullet.bulletTop = gun.Top + (gun.Height / 2);
                        shootBullet.MakeBullet(this);
                        keyLeft = false;
                        keyDown = false;
                        cooldown = Bullet.weaponCooldown;
                        if (Bullet.gunType == "spread")
                        {
                            Bullet shootBullet2 = new Bullet();
                            shootBullet2.shooter = "player";
                            shootBullet2.direction = "down";
                            Bullet shootBullet3 = new Bullet();
                            shootBullet3.shooter = "player";
                            shootBullet3.direction = "left";
                            shootBullet2.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet2.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet2.MakeBullet(this);
                            shootBullet3.bulletLeft = gun.Left + (gun.Width / 2);
                            shootBullet3.bulletTop = gun.Top + (gun.Height / 2);
                            shootBullet3.MakeBullet(this);
                        }
                    }
                    else
                    {
                        keyRight = false;
                        keyUp = false;
                        keyLeft = false;
                        keyDown = false;
                    }
                }
                else
                {
                    keyRight = false;
                    keyUp = false;
                    keyLeft = false;
                    keyDown = false;
                }
            }

        }
        private void LoadRoom()
        {
            if (!finishedDeathSequence)
            {


                //Declare Variables
                int stairs;

                //give player iFrames
                iFrames = maxiFrames;
                chestVisible = false;
                //also hide loot
                removeLoot = true;
                stopRemoveLoot = 10;
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

                    stairsWillSpawn = true;
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


        }
        private void SpawnChest()
        {
            if (!finishedDeathSequence)
            {
                //chestLoot.Clear();
                //spawn chest
                chest.Image = Image.FromFile("images/ClosedChest.png");
                chestVisible = true;
                //choose loot
                chestLooted = false;
                if (firstRoom && floor == 1)
                {
                    int randomWeapon = randNum.Next(weaponLuck, 100 + 1);
                    if (randomWeapon <= 50 || weaponLuck == 0)
                    {
                        //space gun
                        chestLoot.Add("gun");
                    }
                    else
                    {
                        randomWeapon = randNum.Next(1, 4 + 1);
                        if (randomWeapon == 1)
                        {
                            chestLoot.Add("gun");
                            Bullet.gunType = "beeg";
                        }
                        else if (randomWeapon == 2)
                        {
                            chestLoot.Add("gun");
                            Bullet.gunType = "spread";
                        }
                        else if (randomWeapon == 3)
                        {
                            chestLoot.Add("gun");
                            Bullet.gunType = "pierce";
                        }
                        else if (randomWeapon == 4)
                        {
                            chestLoot.Add("gun");
                            Bullet.gunType = "SPEED";
                        }
                    }

                }
                else
                {
                    if (floor == 1)
                    {
                        //health and coins
                        coinDrops = randNum.Next(1, 3);
                        healthDrops = randNum.Next(0, 6);
                        for (int i = 0; i < coinDrops; i++)
                        {
                            chestLoot.Add("coin");
                        }
                        if (healthDrops == 1)
                        {
                            chestLoot.Add("potion");
                        }
                    }
                    else if (floor == 2)
                    {
                        //health coins and slight chance at weapon
                    }
                    else if (floor == 3)
                    {
                        //health coins higher chance at weapon slight chance at souls
                    }
                    else if (floor == 4)
                    {
                        //everything
                    }
                }
            }
        }
        private void OpenChest()
        {
            if (!finishedDeathSequence)
            {
                if (((PictureBox)player).Bounds.IntersectsWith(chest.Bounds) && chestVisible && !chestLooted)
                {
                    chestLooted = true;
                    chest.Image = Image.FromFile("images/OpenChest.png");
                    //spawn loot
                    for (int i = 0; i < chestLoot.Count; i++)
                    {
                        if (chestLoot[i] == "coin")
                        {
                            //it does this code but coin doesn't spawn for some reason
                            PictureBox coin = new PictureBox
                            {
                                Image = Image.FromFile("images/Coin.png"),
                                Top = player.Top,
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Size = coinSize,
                                Tag = "coin",
                                Left = randNum.Next(300, 600)
                            };
                            coin.BringToFront();
                            this.Controls.Add(coin);
                            player.BringToFront();
                            gun.BringToFront();
                            //next time find a way to reference coin so player can pick it up outside of this bit of code
                        }
                        else if (chestLoot[i] == "potion")
                        {
                            PictureBox potion = new PictureBox
                            {
                                Image = Image.FromFile("images/Potion.png"),
                                Top = player.Top,
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Size = potionSize,
                                Tag = "potion",
                                Left = randNum.Next(300, 600)
                            };
                            potion.BringToFront();
                            this.Controls.Add(potion);
                            player.BringToFront();
                            gun.BringToFront();
                        }
                        else if (chestLoot[i] == "gun")
                        {
                            gunDrop.BringToFront();
                            gunDrop.Show();
                            gunDrop.Top = player.Top;
                            gunDrop.Left = randNum.Next(300, 600);
                            player.BringToFront();
                            gun.BringToFront();
                            gunDropVisible = true;
                        }
                        //add more loot later
                    }
                    chestLoot.Clear();
                }
            }
        }
        private void DungeonDelvers_Load(object sender, EventArgs e)
        {
            stairs.Hide();
        }
        private void SpawnEnemies()
        {
            if (!finishedDeathSequence)
            {
                //Choose number of enemies
                //Choose enemies
                if (floor == 1)
                {

                    //bats and rats
                    enemyNum = randNum.Next(2, 3);
                    for (int i = 0; i < enemyNum; i++)
                    {
                        chooseEnemy = randNum.Next(0, 2);
                        if (batNum == 3 && ratNum != 3)
                        {
                            RatSpawn();
                        }
                        else if (ratNum == 3 && batNum != 3)
                        {
                            BatSpawn();
                        }
                        else if (chooseEnemy == 0 && !(batNum >= 3))
                        {
                            //spawn bat
                            BatSpawn();
                        }
                        else if (chooseEnemy == 1 && !(ratNum >= 3))
                        {
                            //spawn rat
                            RatSpawn();
                        }
                        else
                        {
                            enemyNum--;
                        }

                    }

                }
                else if (floor == 2)
                {
                    //bats rats slimes
                    enemyNum = randNum.Next(3, 4); //add enemy health in each part of list
                    for (int i = 0; i < enemyNum; i++)
                    {
                        chooseEnemy = randNum.Next(0, 3);
                        if (chooseEnemy == 0 && !(batNum >= 3))
                        {
                            //spawn bat
                            BatSpawn();
                        }
                        else if (chooseEnemy == 1 && !(ratNum >= 3))
                        {
                            //spawn rat
                            RatSpawn();
                        }
                        else if (!(slimeNum >= 3))
                        {
                            //spawn slime
                            SlimeSpawn();
                        }
                        else
                        {
                            enemyNum--;
                        }
                    }
                }
                else if (floor == 3)
                {
                    //bats rats slimes goblins
                    enemyNum = randNum.Next(4, 5); //add enemy health in each part of list
                    for (int i = 0; i < enemyNum; i++)
                    {
                        chooseEnemy = randNum.Next(0, 4);
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
                            SlimeSpawn();
                        }
                        else if (!(goblinNum >= 3))
                        {
                            //spawn goblin
                            GoblinSpawn();
                        }
                        else
                        {
                            enemyNum--;
                        }
                    }

                }
                else if (floor == 4)
                {
                    //bats rats slimes goblins skeletons
                    enemyNum = randNum.Next(5, 7); //add enemy health in each part of list
                    for (int i = 0; i < enemyNum; i++)
                    {
                        chooseEnemy = randNum.Next(0, 5);
                        if (chooseEnemy == 0 && !(batNum >= 3))
                        {
                            //spawn bat
                            BatSpawn();

                        }
                        else if (chooseEnemy == 1 && !(ratNum >= 3))
                        {
                            //spawn rat
                            RatSpawn();
                        }
                        else if (chooseEnemy == 2 && !(slimeNum >= 3))
                        {
                            //spawn slime
                            SlimeSpawn();
                        }
                        else if (chooseEnemy == 3 && !(goblinNum >= 3))
                        {
                            //spawn goblin
                            GoblinSpawn();
                        }
                        else if (!(skeletonNum >= 3))
                        {
                            //spawn skeleton
                            SkeletonSpawn();
                        }
                        else
                        {
                            enemyNum--;
                        }
                    }

                }
                //floor 5 is boss floor
            }
        }
        private void SpawnStairs()
        {
            if (!finishedDeathSequence)
            {
                stairsWillSpawn = false;
                stairs.Top = randNum.Next(120, 500);
                stairs.Left = randNum.Next(50, 800);
                stairs.Show();
                stairsVisible = true;
            }
        }
        private void NewFloor()
        {
            if (!finishedDeathSequence)
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
        }
        private void BossFloor()
        {

        }

        private void continueBtn_Click(object sender, EventArgs e)
        {
            //hide button
            continueBtn.Hide();
            //open souls form

            //save souls and close form
            string filePath = "souls.csv";

            // Open the file for writing, overwriting existing content
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                // Write the variable content to the file
                writer.Write(souls);
            }
            soulsMenu form2 = new soulsMenu();
            form2.Show();
            using (StreamWriter writer = new StreamWriter("respawn.csv"))
            {
                writer.Write(string.Empty); // This clears the file
                writer.WriteLine("false");
            }
            windowClosed = true;
            //finishdeathsequence
        }

        private void BatSpawn()
        {
            if (!finishedDeathSequence)
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
                else if (batNum == 1)
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
        }
        private void RatSpawn()
        {
            if (!finishedDeathSequence)
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
        }
        private void SkeletonSpawn()
        {
            if (!finishedDeathSequence)
            {
                if (skeletonNum == 0)
                {

                    skeleton1.Show();
                    skeleton1.Tag = "enemy";
                    skeleton1.Left = randNum.Next(200, 700);
                    skeleton1.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(150, 200) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("skeleton1");
                    this.Controls.Add(skeleton1);
                    skeleton1Visible = true;

                }
                else if (skeletonNum == 1)
                {
                    skeleton2.Show();
                    skeleton2.Tag = "enemy";
                    skeleton2.Left = randNum.Next(200, 700);
                    skeleton2.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(150, 200) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("skeleton2");
                    this.Controls.Add(skeleton2);
                    skeleton2Visible = true;
                }
                else //change to skeleton
                {
                    skeleton3.Show();
                    skeleton3.Tag = "enemy";
                    skeleton3.Left = randNum.Next(200, 700);
                    skeleton3.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(150, 200) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("skeleton3");
                    this.Controls.Add(skeleton3);
                    skeleton3Visible = true;
                }
                skeletonNum++;
            }
        }
        private void GoblinSpawn()
        {
            if (!finishedDeathSequence)
            {
                if (goblinNum == 0)
                {

                    goblin1.Show();
                    goblin1Gun.Show();
                    goblin1.Tag = "enemy";
                    goblin1.Left = randNum.Next(200, 700);
                    goblin1.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(150, 200) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("goblin1");
                    this.Controls.Add(goblin1);
                    goblin1Visible = true;

                }
                else if (goblinNum == 1)
                {
                    goblin2.Show();
                    goblin2Gun.Show();
                    goblin2.Tag = "enemy";
                    goblin2.Left = randNum.Next(200, 700);
                    goblin2.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(150, 200) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("goblin2");
                    this.Controls.Add(goblin2);
                    goblin2Visible = true;
                }
                else
                {
                    goblin3.Show();
                    goblin3Gun.Show();
                    goblin3.Tag = "enemy";
                    goblin3.Left = randNum.Next(200, 700);
                    goblin3.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(150, 200) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("goblin3");
                    this.Controls.Add(goblin3);
                    goblin3Visible = true;
                }
                goblinNum++;
            }
        }
        private void SlimeSpawn()
        {
            if (!finishedDeathSequence)
            {
                if (slimeNum == 0)
                {

                    slime1.Show();
                    slime1.Tag = "enemy";
                    slime1.Left = randNum.Next(200, 700);
                    slime1.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(80, 100) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("slime1");
                    this.Controls.Add(slime1);
                    slime1Visible = true;

                }
                else if (slimeNum == 1) //change this part once slime 2 and 3 added
                {
                    slime2.Show();
                    slime2.Tag = "enemy";
                    slime2.Left = randNum.Next(200, 700);
                    slime2.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(80, 100) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("slime2");
                    this.Controls.Add(slime2);
                    slime2Visible = true;
                }
                else
                {
                    slime3.Show();
                    slime3.Tag = "enemy";
                    slime3.Left = randNum.Next(200, 700);
                    slime3.Top = randNum.Next(200, 350);
                    enemyHealth.Add(randNum.Next(80, 100) * enemyHealthMultiplier); //adjust values if necessary
                    enemyTypes.Add("slime3");
                    this.Controls.Add(slime3);
                    slime3Visible = true;
                }
                slimeNum++;
            }
        }
        private void Collision()
        {
            if (!finishedDeathSequence)
            {
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (string)x.Tag == "idkihavetoputsomethinghereforsomereason")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();

                        }
                    }
                    if (player.Bounds.IntersectsWith(gunDrop.Bounds) && gunDropVisible)
                    {
                        gunDropVisible = false;
                        gunDrop.Hide();
                        haveGun = true;
                    }
                    //Detects if the player collides with the coins and potions
                    if (x is PictureBox && (string)x.Tag == "potion")
                    {
                        //if next room remove
                        if (removeLoot)
                        {
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        }
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            //((PictureBox)x).Dispose();
                            if (playerHealth <= maxHealth - 10)
                            {
                                playerHealth = maxHealth;
                            }
                            else
                            {
                                playerHealth += 10;  //change when switching to hearts system
                            }
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        }
                    }
                    if (x is PictureBox && (string)x.Tag == "coin")
                    {
                        if (removeLoot)
                        {
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        }
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gold += 1;
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        }
                    }
                    if (x is PictureBox && (string)x.Tag == "soul")
                    {
                        if (removeLoot)
                        {
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        }
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            souls += 1;
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        }
                    }
                    if (x is PictureBox && (string)x.Tag == "blood")
                    {
                        if (removeLoot)
                        {
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        }
                    }



                    foreach (Control j in this.Controls)
                    {
                        if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && ((string)x.Tag == "enemy" || Bullet.BulletHitsPlayer))
                        {
                            //if enemy bullet hits player
                            if (((PictureBox)player).Bounds.IntersectsWith(j.Bounds) && Bullet.BulletHitsPlayer && iFrames == 0) //may need to bypass x.tag = "enemy"
                            {
                                this.Controls.Remove(j);
                                ((PictureBox)j).Dispose();
                                playerHealth -= 5;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                iFrames = maxiFrames;
                                //test
                                //move enemy away
                                if (((PictureBox)player).Left > bat1.Left)
                                {
                                    ((PictureBox)player).Left += speed * 3;
                                }
                                if (((PictureBox)player).Top > bat1.Top)
                                {
                                    ((PictureBox)player).Top += speed * 3;
                                }
                                if (((PictureBox)player).Left < bat1.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 3;
                                }
                                if (((PictureBox)player).Top < bat1.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 3;
                                }

                            }

                            if (((PictureBox)bat1).Bounds.IntersectsWith(j.Bounds) && bat1Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "bat1")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)bat1).Left > player.Left)
                                {
                                    ((PictureBox)bat1).Left += bat1Speed * 5;
                                }
                                if (((PictureBox)bat1).Top > player.Top)
                                {
                                    ((PictureBox)bat1).Top += bat1Speed * 5;
                                }
                                if (((PictureBox)bat1).Left < player.Left)
                                {
                                    ((PictureBox)bat1).Left -= bat1Speed * 5;
                                }
                                if (((PictureBox)bat1).Top < player.Top)
                                {
                                    ((PictureBox)bat1).Top -= bat1Speed * 5;
                                }

                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    //big blood splatter
                                    enemyDropTop = ((PictureBox)bat1).Top;
                                    enemyDropLeft = ((PictureBox)bat1).Left;
                                    bloodType = "big";
                                    SpawnBlood();
                                    bat1.Hide();
                                    EnemyDrops();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    bat1Visible = false;
                                    batNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)bat1).Top;
                                    enemyDropLeft = ((PictureBox)bat1).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }
                            if (((PictureBox)bat2).Bounds.IntersectsWith(j.Bounds) && bat2Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "bat2")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)bat2).Left > player.Left)
                                {
                                    ((PictureBox)bat2).Left += bat2Speed * 5;
                                }
                                if (((PictureBox)bat2).Top > player.Top)
                                {
                                    ((PictureBox)bat2).Top += bat2Speed * 5;
                                }
                                if (((PictureBox)bat2).Left < player.Left)
                                {
                                    ((PictureBox)bat2).Left -= bat2Speed * 5;
                                }
                                if (((PictureBox)bat2).Top < player.Top)
                                {
                                    ((PictureBox)bat2).Top -= bat2Speed * 5;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    //big blood splatter
                                    enemyDropTop = ((PictureBox)bat2).Top;
                                    enemyDropLeft = ((PictureBox)bat2).Left;
                                    bloodType = "big";
                                    SpawnBlood();
                                    EnemyDrops();
                                    bat2.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    bat2Visible = false;
                                    batNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)bat2).Top;
                                    enemyDropLeft = ((PictureBox)bat2).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }
                            if (((PictureBox)bat3).Bounds.IntersectsWith(j.Bounds) && bat3Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "bat3")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)bat3).Left > player.Left)
                                {
                                    ((PictureBox)bat3).Left += bat3Speed * 5;
                                }
                                if (((PictureBox)bat3).Top > player.Top)
                                {
                                    ((PictureBox)bat3).Top += bat3Speed * 5;
                                }
                                if (((PictureBox)bat3).Left < player.Left)
                                {
                                    ((PictureBox)bat3).Left -= bat3Speed * 5;
                                }
                                if (((PictureBox)bat3).Top < player.Top)
                                {
                                    ((PictureBox)bat3).Top -= bat3Speed * 5;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)bat3).Top;
                                    enemyDropLeft = ((PictureBox)bat3).Left;
                                    bloodType = "big";
                                    SpawnBlood();
                                    bat3.Hide();
                                    EnemyDrops();
                                    enemyNum -= 1;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    bat3Visible = false;
                                    batNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)bat3).Top;
                                    enemyDropLeft = ((PictureBox)bat3).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }


                            if (((PictureBox)rat1).Bounds.IntersectsWith(j.Bounds) && rat1Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "rat1")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)rat1).Left > player.Left)
                                {
                                    ((PictureBox)rat1).Left += rat1Speed * 5;
                                }
                                if (((PictureBox)rat1).Top > player.Top)
                                {
                                    ((PictureBox)rat1).Top += rat1Speed * 5;
                                }
                                if (((PictureBox)rat1).Left < player.Left)
                                {
                                    ((PictureBox)rat1).Left -= rat1Speed * 5;
                                }
                                if (((PictureBox)rat1).Top < player.Top)
                                {
                                    ((PictureBox)rat1).Top -= rat1Speed * 5;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    //big splatter
                                    rat1.Hide();
                                    enemyDropTop = ((PictureBox)rat1).Top;
                                    enemyDropLeft = ((PictureBox)rat1).Left;
                                    bloodType = "big";
                                    SpawnBlood();
                                    EnemyDrops();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    rat1Visible = false;
                                    ratNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)rat1).Top;
                                    enemyDropLeft = ((PictureBox)rat1).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }
                            if (((PictureBox)rat2).Bounds.IntersectsWith(j.Bounds) && rat2Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "rat2")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)rat2).Left > player.Left)
                                {
                                    ((PictureBox)rat2).Left += rat2Speed * 5;
                                }
                                if (((PictureBox)rat2).Top > player.Top)
                                {
                                    ((PictureBox)rat2).Top += rat2Speed * 5;
                                }
                                if (((PictureBox)rat2).Left < player.Left)
                                {
                                    ((PictureBox)rat2).Left -= rat2Speed * 5;
                                }
                                if (((PictureBox)rat2).Top < player.Top)
                                {
                                    ((PictureBox)rat2).Top -= rat2Speed * 5;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)rat2).Top;
                                    enemyDropLeft = ((PictureBox)rat2).Left;
                                    bloodType = "big";
                                    SpawnBlood();
                                    rat2.Hide();
                                    EnemyDrops();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    rat2Visible = false;
                                    ratNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)rat2).Top;
                                    enemyDropLeft = ((PictureBox)rat2).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }
                            if (((PictureBox)rat3).Bounds.IntersectsWith(j.Bounds) && rat3Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "rat3")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)rat3).Left > player.Left)
                                {
                                    ((PictureBox)rat3).Left += rat3Speed * 5;
                                }
                                if (((PictureBox)rat3).Top > player.Top)
                                {
                                    ((PictureBox)rat3).Top += rat3Speed * 5;
                                }
                                if (((PictureBox)rat3).Left < player.Left)
                                {
                                    ((PictureBox)rat3).Left -= rat3Speed * 5;
                                }
                                if (((PictureBox)rat3).Top < player.Top)
                                {
                                    ((PictureBox)rat3).Top -= rat3Speed * 5;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)rat3).Top;
                                    enemyDropLeft = ((PictureBox)rat3).Left;
                                    bloodType = "big";
                                    SpawnBlood();
                                    EnemyDrops();
                                    rat3.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    rat3Visible = false;
                                    ratNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)rat3).Top;
                                    enemyDropLeft = ((PictureBox)rat3).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }

                            //slimes
                            if (((PictureBox)slime1).Bounds.IntersectsWith(j.Bounds) && slime1Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "slime1")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)slime1).Left > player.Left)
                                {
                                    ((PictureBox)slime1).Left += slime1Speed;
                                }
                                if (((PictureBox)slime1).Top > player.Top)
                                {
                                    ((PictureBox)slime1).Top += slime1Speed;
                                }
                                if (((PictureBox)slime1).Left < player.Left)
                                {
                                    ((PictureBox)slime1).Left -= slime1Speed;
                                }
                                if (((PictureBox)slime1).Top < player.Top)
                                {
                                    ((PictureBox)slime1).Top -= slime1Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)slime1).Top;
                                    enemyDropLeft = ((PictureBox)slime1).Left;
                                    bloodType = "bigslime";
                                    SpawnBlood();
                                    EnemyDrops();
                                    slime1.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    slime1Visible = false;
                                    slimeNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)slime1).Top;
                                    enemyDropLeft = ((PictureBox)slime1).Left;
                                    bloodType = "smallslime";
                                    SpawnBlood();
                                }
                            }

                            if (((PictureBox)slime2).Bounds.IntersectsWith(j.Bounds) && slime2Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "slime2")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)slime2).Left > player.Left)
                                {
                                    ((PictureBox)slime2).Left += slime2Speed;
                                }
                                if (((PictureBox)slime2).Top > player.Top)
                                {
                                    ((PictureBox)slime2).Top += slime2Speed;
                                }
                                if (((PictureBox)slime2).Left < player.Left)
                                {
                                    ((PictureBox)slime2).Left -= slime2Speed;
                                }
                                if (((PictureBox)slime2).Top < player.Top)
                                {
                                    ((PictureBox)slime2).Top -= slime2Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)slime2).Top;
                                    enemyDropLeft = ((PictureBox)slime2).Left;
                                    bloodType = "bigslime";
                                    SpawnBlood();
                                    EnemyDrops();
                                    slime2.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    slime2Visible = false;
                                    slimeNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)slime2).Top;
                                    enemyDropLeft = ((PictureBox)slime2).Left;
                                    bloodType = "smallslime";
                                    SpawnBlood();
                                }
                            }
                            if (((PictureBox)slime3).Bounds.IntersectsWith(j.Bounds) && slime3Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "slime3")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)slime3).Left > player.Left)
                                {
                                    ((PictureBox)slime3).Left += slime3Speed;
                                }
                                if (((PictureBox)slime3).Top > player.Top)
                                {
                                    ((PictureBox)slime3).Top += slime3Speed;
                                }
                                if (((PictureBox)slime3).Left < player.Left)
                                {
                                    ((PictureBox)slime3).Left -= slime3Speed;
                                }
                                if (((PictureBox)slime3).Top < player.Top)
                                {
                                    ((PictureBox)slime3).Top -= slime3Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)slime3).Top;
                                    enemyDropLeft = ((PictureBox)slime3).Left;
                                    bloodType = "bigslime";
                                    SpawnBlood();
                                    EnemyDrops();
                                    slime3.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    slime3Visible = false;
                                    slimeNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)slime3).Top;
                                    enemyDropLeft = ((PictureBox)slime3).Left;
                                    bloodType = "smallslime";
                                    SpawnBlood();
                                }
                            }

                            //skeletons
                            if (((PictureBox)skeleton1).Bounds.IntersectsWith(j.Bounds) && skeleton1Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "skeleton1")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)skeleton1).Left > player.Left)
                                {
                                    ((PictureBox)skeleton1).Left += skeleton1Speed;
                                }
                                if (((PictureBox)skeleton1).Top > player.Top)
                                {
                                    ((PictureBox)skeleton1).Top += skeleton1Speed;
                                }
                                if (((PictureBox)skeleton1).Left < player.Left)
                                {
                                    ((PictureBox)skeleton1).Left -= skeleton1Speed;
                                }
                                if (((PictureBox)skeleton1).Top < player.Top)
                                {
                                    ((PictureBox)skeleton1).Top -= skeleton1Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)skeleton1).Top;
                                    enemyDropLeft = ((PictureBox)skeleton1).Left;
                                    bloodType = "bones";
                                    SpawnBlood();
                                    EnemyDrops();
                                    skeleton1.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    skeleton1Visible = false;
                                    skeletonNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                            }
                            if (((PictureBox)skeleton2).Bounds.IntersectsWith(j.Bounds) && skeleton2Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "skeleton2")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)skeleton2).Left > player.Left)
                                {
                                    ((PictureBox)skeleton2).Left += skeleton2Speed;
                                }
                                if (((PictureBox)skeleton2).Top > player.Top)
                                {
                                    ((PictureBox)skeleton2).Top += skeleton2Speed;
                                }
                                if (((PictureBox)skeleton2).Left < player.Left)
                                {
                                    ((PictureBox)skeleton2).Left -= skeleton2Speed;
                                }
                                if (((PictureBox)skeleton2).Top < player.Top)
                                {
                                    ((PictureBox)skeleton2).Top -= skeleton2Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)skeleton2).Top;
                                    enemyDropLeft = ((PictureBox)skeleton2).Left;
                                    bloodType = "bones";
                                    SpawnBlood();
                                    EnemyDrops();
                                    skeleton2.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    skeleton2Visible = false;
                                    skeletonNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                            }
                            if (((PictureBox)skeleton3).Bounds.IntersectsWith(j.Bounds) && skeleton3Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "skeleton3")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)skeleton3).Left > player.Left)
                                {
                                    ((PictureBox)skeleton3).Left += skeleton3Speed;
                                }
                                if (((PictureBox)skeleton3).Top > player.Top)
                                {
                                    ((PictureBox)skeleton3).Top += skeleton3Speed;
                                }
                                if (((PictureBox)skeleton3).Left < player.Left)
                                {
                                    ((PictureBox)skeleton3).Left -= skeleton3Speed;
                                }
                                if (((PictureBox)skeleton3).Top < player.Top)
                                {
                                    ((PictureBox)skeleton3).Top -= skeleton3Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)skeleton3).Top;
                                    enemyDropLeft = ((PictureBox)skeleton3).Left;
                                    bloodType = "bones";
                                    SpawnBlood();
                                    EnemyDrops();
                                    skeleton3.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    skeleton3Visible = false;
                                    skeletonNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                            }

                            //goblins
                            if (((PictureBox)goblin1).Bounds.IntersectsWith(j.Bounds) && goblin1Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "goblin1")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)goblin1).Left > player.Left)
                                {
                                    ((PictureBox)goblin1).Left += goblin1Speed;
                                }
                                if (((PictureBox)goblin1).Top > player.Top)
                                {
                                    ((PictureBox)goblin1).Top += goblin1Speed;
                                }
                                if (((PictureBox)goblin1).Left < player.Left)
                                {
                                    ((PictureBox)goblin1).Left -= goblin1Speed;
                                }
                                if (((PictureBox)goblin1).Top < player.Top)
                                {
                                    ((PictureBox)goblin1).Top -= goblin1Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)goblin1).Top;
                                    enemyDropLeft = ((PictureBox)goblin1).Left;
                                    bloodType = "big"; //change to goblin blood if have time
                                    SpawnBlood();
                                    EnemyDrops();
                                    goblin1.Hide();
                                    goblin1Gun.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    goblin1Visible = false;
                                    goblinNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)goblin1).Top;
                                    enemyDropLeft = ((PictureBox)goblin1).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }
                            //goblin2
                            if (((PictureBox)goblin2).Bounds.IntersectsWith(j.Bounds) && goblin2Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "goblin2")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)goblin2).Left > player.Left)
                                {
                                    ((PictureBox)goblin2).Left += goblin2Speed;
                                }
                                if (((PictureBox)goblin2).Top > player.Top)
                                {
                                    ((PictureBox)goblin2).Top += goblin2Speed;
                                }
                                if (((PictureBox)goblin2).Left < player.Left)
                                {
                                    ((PictureBox)goblin2).Left -= goblin2Speed;
                                }
                                if (((PictureBox)goblin2).Top < player.Top)
                                {
                                    ((PictureBox)goblin2).Top -= goblin2Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)goblin2).Top;
                                    enemyDropLeft = ((PictureBox)goblin2).Left;
                                    bloodType = "big"; //change to goblin blood if have time
                                    SpawnBlood();
                                    EnemyDrops();
                                    goblin2.Hide();
                                    goblin2Gun.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    goblin2Visible = false;
                                    goblinNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)goblin2).Top;
                                    enemyDropLeft = ((PictureBox)goblin2).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }
                            //goblin3
                            if (((PictureBox)goblin3).Bounds.IntersectsWith(j.Bounds) && goblin3Visible && !Bullet.BulletHitsPlayer) //j is bullet
                            {
                                if (Bullet.gunType != "pierce")
                                {
                                    this.Controls.Remove(j);
                                    ((PictureBox)j).Dispose();
                                }
                                for (int i = 0; i < enemyHealth.Count; i++)
                                {
                                    if (enemyTypes[i] == "goblin3")
                                    {
                                        enemyHit = i;
                                    }
                                }
                                enemyHealth[enemyHit] -= damage;
                                //move enemy away
                                if (((PictureBox)goblin3).Left > player.Left)
                                {
                                    ((PictureBox)goblin3).Left += goblin3Speed;
                                }
                                if (((PictureBox)goblin3).Top > player.Top)
                                {
                                    ((PictureBox)goblin3).Top += goblin3Speed;
                                }
                                if (((PictureBox)goblin3).Left < player.Left)
                                {
                                    ((PictureBox)goblin3).Left -= goblin3Speed;
                                }
                                if (((PictureBox)goblin3).Top < player.Top)
                                {
                                    ((PictureBox)goblin3).Top -= goblin3Speed;
                                }
                                if (enemyHealth[enemyHit] <= 0)
                                {
                                    //die
                                    enemyDropTop = ((PictureBox)goblin3).Top;
                                    enemyDropLeft = ((PictureBox)goblin3).Left;
                                    bloodType = "big"; //change to goblin blood if have time
                                    SpawnBlood();
                                    EnemyDrops();
                                    goblin3.Hide();
                                    goblin3Gun.Hide();
                                    enemyNum--;
                                    enemyHealth.RemoveAt(enemyHit);
                                    enemyTypes.RemoveAt(enemyHit);
                                    goblin3Visible = false;
                                    goblinNum--;
                                    if (enemyNum == 0)
                                    {
                                        SpawnChest();
                                        if (stairsWillSpawn == true)
                                        {
                                            SpawnStairs();
                                        }
                                    }
                                }
                                else
                                {
                                    //small blood splatter
                                    enemyDropTop = ((PictureBox)goblin3).Top;
                                    enemyDropLeft = ((PictureBox)goblin3).Left;
                                    bloodType = "small";
                                    SpawnBlood();
                                }
                            }

                        }
                    }
                    //if the enemy hits the player
                    if (x is PictureBox && x.Tag == "enemy")
                    {
                        // below is the if statement thats checking the bounds of the player and the enemy
                        if (((PictureBox)bat1).Bounds.IntersectsWith(player.Bounds) && bat1Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 2;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                iFrames = maxiFrames;
                                //test
                                //move enemy away
                                if (((PictureBox)player).Left > bat1.Left)
                                {
                                    ((PictureBox)player).Left += speed * 3;
                                }
                                if (((PictureBox)player).Top > bat1.Top)
                                {
                                    ((PictureBox)player).Top += speed * 3;
                                }
                                if (((PictureBox)player).Left < bat1.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 3;
                                }
                                if (((PictureBox)player).Top < bat1.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 3;
                                }
                            }
                        }
                        if (((PictureBox)bat2).Bounds.IntersectsWith(player.Bounds) && bat2Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 2;
                                iFrames = maxiFrames;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > bat2.Left)
                                {
                                    ((PictureBox)player).Left += speed * 3;
                                }
                                if (((PictureBox)player).Top > bat2.Top)
                                {
                                    ((PictureBox)player).Top += speed * 3;
                                }
                                if (((PictureBox)player).Left < bat2.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 3;
                                }
                                if (((PictureBox)player).Top < bat2.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 3;
                                }
                            }
                        }
                        if (((PictureBox)bat3).Bounds.IntersectsWith(player.Bounds) && bat3Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 2;
                                iFrames = maxiFrames;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > bat3.Left)
                                {
                                    ((PictureBox)player).Left += speed * 3;
                                }
                                if (((PictureBox)player).Top > bat3.Top)
                                {
                                    ((PictureBox)player).Top += speed * 3;
                                }
                                if (((PictureBox)player).Left < bat3.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 3;
                                }
                                if (((PictureBox)player).Top < bat3.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 3;
                                }
                            }
                        }
                        if (((PictureBox)rat1).Bounds.IntersectsWith(player.Bounds) && rat1Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 5;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > rat1.Left)
                                {
                                    ((PictureBox)player).Left += speed * 4;
                                }
                                if (((PictureBox)player).Top > rat1.Top)
                                {
                                    ((PictureBox)player).Top += speed * 4;
                                }
                                if (((PictureBox)player).Left < rat1.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 4;
                                }
                                if (((PictureBox)player).Top < rat1.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 4;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)rat2).Bounds.IntersectsWith(player.Bounds) && rat2Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 5;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > rat2.Left)
                                {
                                    ((PictureBox)player).Left += speed * 4;
                                }
                                if (((PictureBox)player).Top > rat2.Top)
                                {
                                    ((PictureBox)player).Top += speed * 4;
                                }
                                if (((PictureBox)player).Left < rat2.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 4;
                                }
                                if (((PictureBox)player).Top < rat2.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 4;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)rat3).Bounds.IntersectsWith(player.Bounds) && rat3Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 5;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > rat3.Left)
                                {
                                    ((PictureBox)player).Left += speed * 4;
                                }
                                if (((PictureBox)player).Top > rat3.Top)
                                {
                                    ((PictureBox)player).Top += speed * 4;
                                }
                                if (((PictureBox)player).Left < rat3.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 4;
                                }
                                if (((PictureBox)player).Top < rat3.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 4;
                                }
                                iFrames = maxiFrames;
                            }
                        }

                        //slimes
                        if (((PictureBox)slime1).Bounds.IntersectsWith(player.Bounds) && slime1Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 5;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > slime1.Left)
                                {
                                    ((PictureBox)player).Left += speed * 6;
                                }
                                if (((PictureBox)player).Top > slime1.Top)
                                {
                                    ((PictureBox)player).Top += speed * 6;
                                }
                                if (((PictureBox)player).Left < slime1.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 6;
                                }
                                if (((PictureBox)player).Top < slime1.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 6;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)slime2).Bounds.IntersectsWith(player.Bounds) && slime2Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 5;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > slime2.Left)
                                {
                                    ((PictureBox)player).Left += speed * 6;
                                }
                                if (((PictureBox)player).Top > slime2.Top)
                                {
                                    ((PictureBox)player).Top += speed * 6;
                                }
                                if (((PictureBox)player).Left < slime2.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 6;
                                }
                                if (((PictureBox)player).Top < slime2.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 6;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)slime3).Bounds.IntersectsWith(player.Bounds) && slime3Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 5;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > slime3.Left)
                                {
                                    ((PictureBox)player).Left += speed * 6;
                                }
                                if (((PictureBox)player).Top > slime3.Top)
                                {
                                    ((PictureBox)player).Top += speed * 6;
                                }
                                if (((PictureBox)player).Left < slime3.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 6;
                                }
                                if (((PictureBox)player).Top < slime3.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 6;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)skeleton1).Bounds.IntersectsWith(player.Bounds) && skeleton1Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 10;
                                bloodType = "big";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > skeleton1.Left)
                                {
                                    ((PictureBox)player).Left += speed * 6;
                                }
                                if (((PictureBox)player).Top > skeleton1.Top)
                                {
                                    ((PictureBox)player).Top += speed * 6;
                                }
                                if (((PictureBox)player).Left < skeleton1.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 6;
                                }
                                if (((PictureBox)player).Top < skeleton1.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 6;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)skeleton2).Bounds.IntersectsWith(player.Bounds) && skeleton2Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 10;
                                bloodType = "big";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > skeleton2.Left)
                                {
                                    ((PictureBox)player).Left += speed * 6;
                                }
                                if (((PictureBox)player).Top > skeleton2.Top)
                                {
                                    ((PictureBox)player).Top += speed * 6;
                                }
                                if (((PictureBox)player).Left < skeleton2.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 6;
                                }
                                if (((PictureBox)player).Top < skeleton2.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 6;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)skeleton3).Bounds.IntersectsWith(player.Bounds) && skeleton3Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 10;
                                bloodType = "big";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > skeleton3.Left)
                                {
                                    ((PictureBox)player).Left += speed * 6;
                                }
                                if (((PictureBox)player).Top > skeleton3.Top)
                                {
                                    ((PictureBox)player).Top += speed * 6;
                                }
                                if (((PictureBox)player).Left < skeleton3.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 6;
                                }
                                if (((PictureBox)player).Top < skeleton3.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 6;
                                }
                                iFrames = maxiFrames;
                            }
                        }

                        if (((PictureBox)goblin1).Bounds.IntersectsWith(player.Bounds) && goblin1Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 2;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > goblin1.Left)
                                {
                                    ((PictureBox)player).Left += speed * 2;
                                }
                                if (((PictureBox)player).Top > goblin1.Top)
                                {
                                    ((PictureBox)player).Top += speed * 2;
                                }
                                if (((PictureBox)player).Left < goblin1.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 2;
                                }
                                if (((PictureBox)player).Top < goblin1.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 2;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)goblin2).Bounds.IntersectsWith(player.Bounds) && goblin2Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 2;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > goblin2.Left)
                                {
                                    ((PictureBox)player).Left += speed * 2;
                                }
                                if (((PictureBox)player).Top > goblin2.Top)
                                {
                                    ((PictureBox)player).Top += speed * 2;
                                }
                                if (((PictureBox)player).Left < goblin2.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 2;
                                }
                                if (((PictureBox)player).Top < goblin2.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 2;
                                }
                                iFrames = maxiFrames;
                            }
                        }
                        if (((PictureBox)goblin3).Bounds.IntersectsWith(player.Bounds) && goblin3Visible)
                        {
                            if (iFrames == 0)
                            {
                                playerHealth -= 2;
                                bloodType = "small";
                                enemyDropTop = ((PictureBox)player).Top;
                                enemyDropLeft = ((PictureBox)player).Left;
                                SpawnBlood();
                                if (((PictureBox)player).Left > goblin3.Left)
                                {
                                    ((PictureBox)player).Left += speed * 2;
                                }
                                if (((PictureBox)player).Top > goblin3.Top)
                                {
                                    ((PictureBox)player).Top += speed * 2;
                                }
                                if (((PictureBox)player).Left < goblin3.Left)
                                {
                                    ((PictureBox)player).Left -= speed * 2;
                                }
                                if (((PictureBox)player).Top < goblin3.Top)
                                {
                                    ((PictureBox)player).Top -= speed * 2;
                                }
                                iFrames = maxiFrames;
                            }
                        }


                    }


                }
            }

        }
        private void EnemyAnims() //make enemies have different anim speeds so they aren't in sync with each other
        {
            if (!finishedDeathSequence)
            {
                if (skeleton1Visible || skeleton2Visible || skeleton3Visible)
                {
                    if (skeletonAnimCounter >= 20)
                    {
                        skeletonAnimCounter = 0;
                    }
                    if (skeletonAnimCounter <= 10)
                    {
                        if (skeleton1Visible)
                        {
                            if (skeleton1FacingRight)
                            {
                                skeleton1.Image = Image.FromFile("images/SkeletonUpR.png");
                            }
                            else
                            {
                                skeleton1.Image = Image.FromFile("images/SkeletonUpL.png");
                            }
                        }

                        if (skeleton2Visible)
                        {
                            if (skeleton2FacingRight)
                            {
                                skeleton2.Image = Image.FromFile("images/SkeletonUpR.png");
                            }
                            else
                            {
                                skeleton2.Image = Image.FromFile("images/SkeletonUpL.png");
                            }
                        }
                        if (skeleton3Visible)
                        {
                            if (skeleton3FacingRight)
                            {
                                skeleton3.Image = Image.FromFile("images/SkeletonUpR.png");
                            }
                            else
                            {
                                skeleton3.Image = Image.FromFile("images/SkeletonUpL.png");
                            }
                        }


                    }
                    if (skeletonAnimCounter >= 10)
                    {
                        if (skeleton1Visible)
                        {
                            if (skeleton1FacingRight)
                            {
                                skeleton1.Image = Image.FromFile("images/SkeletonDownR.png");
                            }
                            else
                            {
                                skeleton1.Image = Image.FromFile("images/SkeletonDownL.png");
                            }
                        }

                        if (skeleton2Visible)
                        {
                            if (skeleton2FacingRight)
                            {
                                skeleton2.Image = Image.FromFile("images/SkeletonDownR.png");
                            }
                            else
                            {
                                skeleton2.Image = Image.FromFile("images/SkeletonDownL.png");
                            }
                        }
                        if (skeleton3Visible)
                        {
                            if (skeleton3FacingRight)
                            {
                                skeleton3.Image = Image.FromFile("images/SkeletonDownR.png");
                            }
                            else
                            {
                                skeleton3.Image = Image.FromFile("images/SkeletonDownL.png");
                            }
                        }


                    }


                }
                if (animCounter >= 30)
                {
                    animCounter = 0;
                }
                if (animCounter <= 15)
                {
                    if (bat1Visible)
                    {
                        bat1.Image = Image.FromFile("images/BatUp.png");
                    }
                    if (bat2Visible)
                    {
                        bat2.Image = Image.FromFile("images/BatUp.png");
                    }
                    if (bat3Visible)
                    {
                        bat3.Image = Image.FromFile("images/BatUp.png");
                    }
                    if (rat1Visible)
                    {
                        if (rat1FacingRight)
                        {
                            rat1.Image = Image.FromFile("images/rat2.png");
                        }
                        else
                        {
                            rat1.Image = Image.FromFile("images/rat2L.png");
                        }
                    }
                    if (rat2Visible)
                    {
                        if (rat2FacingRight)
                        {
                            rat2.Image = Image.FromFile("images/rat2.png");
                        }
                        else
                        {
                            rat2.Image = Image.FromFile("images/rat2L.png");
                        }
                    }
                    if (rat3Visible)
                    {
                        if (rat3FacingRight)
                        {
                            rat3.Image = Image.FromFile("images/rat2.png");
                        }
                        else
                        {
                            rat3.Image = Image.FromFile("images/rat2L.png");
                        }
                    }
                }
                if (animCounter >= 15)
                {
                    if (bat1Visible)
                    {
                        bat1.Image = Image.FromFile("images/BatDown.png");
                    }
                    if (bat2Visible)
                    {
                        bat2.Image = Image.FromFile("images/BatDown.png");
                    }
                    if (bat3Visible)
                    {
                        bat3.Image = Image.FromFile("images/BatDown.png");
                    }
                    if (rat1Visible)
                    {
                        if (rat1FacingRight)
                        {
                            rat1.Image = Image.FromFile("images/rat1.png");
                        }
                        else
                        {
                            rat1.Image = Image.FromFile("images/rat1L.png");
                        }
                    }
                    if (rat2Visible)
                    {
                        if (rat2FacingRight)
                        {
                            rat2.Image = Image.FromFile("images/rat1.png");
                        }
                        else
                        {
                            rat2.Image = Image.FromFile("images/rat1L.png");
                        }
                    }
                    if (rat3Visible)
                    {
                        if (rat3FacingRight)
                        {
                            rat3.Image = Image.FromFile("images/rat1.png");
                        }
                        else
                        {
                            rat3.Image = Image.FromFile("images/rat1L.png");
                        }
                    }
                    //slime anim is in EnemyMove()
                }
                //temporary goblin anim
                if (goblin1Visible)
                {
                    if (animCounter > 15)
                    {
                        if (goblin1FacingRight)
                        {
                            goblin1.Image = Image.FromFile("images/goblin1.png");
                        }
                        else
                        {
                            goblin1.Image = Image.FromFile("images/goblin1L.png");
                        }
                    }
                    else
                    {
                        if (goblin1FacingRight)
                        {
                            goblin1.Image = Image.FromFile("images/goblin2.png");
                        }
                        else
                        {
                            goblin1.Image = Image.FromFile("images/goblin2L.png");
                        }
                    }
                }
                if (goblin2Visible)
                {
                    if (animCounter > 15)
                    {
                        if (goblin2FacingRight)
                        {
                            goblin2.Image = Image.FromFile("images/goblin1.png");
                        }
                        else
                        {
                            goblin2.Image = Image.FromFile("images/goblin1L.png");
                        }
                    }
                    else
                    {
                        if (goblin2FacingRight)
                        {
                            goblin2.Image = Image.FromFile("images/goblin2.png");
                        }
                        else
                        {
                            goblin2.Image = Image.FromFile("images/goblin2L.png");
                        }
                    }
                }
                if (goblin3Visible)
                {
                    if (animCounter > 15)
                    {
                        if (goblin3FacingRight)
                        {
                            goblin3.Image = Image.FromFile("images/goblin1.png");
                        }
                        else
                        {
                            goblin3.Image = Image.FromFile("images/goblin1L.png");
                        }
                    }
                    else
                    {
                        if (goblin3FacingRight)
                        {
                            goblin3.Image = Image.FromFile("images/goblin2.png");
                        }
                        else
                        {
                            goblin3.Image = Image.FromFile("images/goblin2L.png");
                        }
                    }
                }
                animCounter++;
                skeletonAnimCounter++;
            }

        }
        private void EnemyMove()
        {
            if (!finishedDeathSequence)
            {
                //move bat1 toward player
                if (bat1Visible)
                {
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
                        bat1Speed = -1;
                    }
                    else
                    {
                        bat1Speed = 3;
                    }

                }
                if (bat2Visible)
                {
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
                        bat2Speed = -1;
                    }
                    else
                    {
                        bat2Speed = 3;
                    }
                }
                if (bat3Visible)
                {
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
                        bat3Speed = -1;
                    }
                    else
                    {
                        bat3Speed = 3;
                    }
                }
                //rats
                if (rat1Visible)
                {
                    //move rat1 toward player
                    if (((PictureBox)rat1).Left > player.Left)
                    {
                        ((PictureBox)rat1).Left -= rat1Speed;
                        rat1FacingRight = false;
                    }
                    if (((PictureBox)rat1).Top > player.Top)
                    {
                        ((PictureBox)rat1).Top -= rat1Speed;
                    }
                    if (((PictureBox)rat1).Left < player.Left)
                    {
                        ((PictureBox)rat1).Left += rat1Speed;
                        rat1FacingRight = true;
                    }
                    if (((PictureBox)rat1).Top < player.Top)
                    {
                        ((PictureBox)rat1).Top += rat1Speed;
                    }
                    //Stop overlap
                    if ((((PictureBox)rat1).Bounds.IntersectsWith(rat2.Bounds) && rat1Visible && rat2Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible && rat1Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible))
                    {
                        rat1Speed = -1;
                    }
                    else
                    {
                        rat1Speed = 3;
                    }
                }
                if (rat2Visible)
                {
                    //move rat2 toward player
                    if (((PictureBox)rat2).Left > player.Left)
                    {
                        ((PictureBox)rat2).Left -= rat2Speed;
                        rat2FacingRight = false;
                    }
                    if (((PictureBox)rat2).Top > player.Top)
                    {
                        ((PictureBox)rat2).Top -= rat2Speed;
                    }
                    if (((PictureBox)rat2).Left < player.Left)
                    {
                        ((PictureBox)rat2).Left += rat2Speed;
                        rat2FacingRight = true;
                    }
                    if (((PictureBox)rat2).Top < player.Top)
                    {
                        ((PictureBox)rat2).Top += rat2Speed;
                    }
                    //Stop overlap
                    if ((((PictureBox)rat2).Bounds.IntersectsWith(rat3.Bounds) && rat2Visible && rat3Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible && rat1Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible))
                    {
                        rat2Speed = -1;
                    }
                    else
                    {
                        rat2Speed = 3;
                    }
                }
                if (rat3Visible)
                {
                    //move rat3 toward player
                    if (((PictureBox)rat3).Left > player.Left)
                    {
                        ((PictureBox)rat3).Left -= rat3Speed;
                        rat3FacingRight = false;
                    }
                    if (((PictureBox)rat3).Top > player.Top)
                    {
                        ((PictureBox)rat3).Top -= rat3Speed;
                    }
                    if (((PictureBox)rat3).Left < player.Left)
                    {
                        ((PictureBox)rat3).Left += rat3Speed;
                        rat3FacingRight = true;
                    }
                    if (((PictureBox)rat3).Top < player.Top)
                    {
                        ((PictureBox)rat3).Top += rat3Speed;
                    }
                    if ((((PictureBox)rat3).Bounds.IntersectsWith(rat1.Bounds) && rat3Visible && rat1Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible && rat1Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(rat1.Bounds) && rat1Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(rat2.Bounds) && rat2Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(rat3.Bounds) && rat3Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(slime1.Bounds) && slime1Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(slime2.Bounds) && slime2Visible) || (((PictureBox)rat1).Bounds.IntersectsWith(slime3.Bounds) && slime3Visible))
                    {
                        rat3Speed = -1;
                    }
                    else
                    {
                        rat3Speed = 3;
                    }
                }

                //goblins
                if (goblin1Visible)
                {
                    //move goblin1 toward player
                    if (((PictureBox)goblin1).Left > player.Left)
                    {
                        ((PictureBox)goblin1).Left -= goblin1Speed;
                        goblin1FacingRight = false;
                    }
                    if (((PictureBox)goblin1).Top > player.Top)
                    {
                        ((PictureBox)goblin1).Top -= goblin1Speed;
                    }
                    if (((PictureBox)goblin1).Left < player.Left)
                    {
                        ((PictureBox)goblin1).Left += goblin1Speed;
                        goblin1FacingRight = true;
                    }
                    if (((PictureBox)goblin1).Top < player.Top)
                    {
                        ((PictureBox)goblin1).Top += goblin1Speed;
                    }
                    if ((((PictureBox)goblin1).Bounds.IntersectsWith(goblin2.Bounds) && goblin2Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(goblin3.Bounds) && goblin3Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(rat2.Bounds) && rat2Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(rat3.Bounds) && rat3Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(slime1.Bounds) && slime1Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(slime2.Bounds) && slime2Visible) || (((PictureBox)goblin1).Bounds.IntersectsWith(slime3.Bounds) && slime3Visible))
                    {
                        goblin1Speed = -1;
                    }
                    else
                    {
                        goblin1Speed = 2;
                    }
                }
                if (goblin2Visible)
                {
                    //move goblin2 toward player
                    if (((PictureBox)goblin2).Left > player.Left)
                    {
                        ((PictureBox)goblin2).Left -= goblin2Speed;
                        goblin2FacingRight = false;
                    }
                    if (((PictureBox)goblin2).Top > player.Top)
                    {
                        ((PictureBox)goblin2).Top -= goblin2Speed;
                    }
                    if (((PictureBox)goblin2).Left < player.Left)
                    {
                        ((PictureBox)goblin2).Left += goblin2Speed;
                        goblin2FacingRight = true;
                    }
                    if (((PictureBox)goblin2).Top < player.Top)
                    {
                        ((PictureBox)goblin2).Top += goblin2Speed;
                    }
                    if ((((PictureBox)goblin2).Bounds.IntersectsWith(goblin3.Bounds) && goblin3Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(rat2.Bounds) && rat2Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(rat3.Bounds) && rat3Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(slime1.Bounds) && slime1Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(slime2.Bounds) && slime2Visible) || (((PictureBox)goblin2).Bounds.IntersectsWith(slime3.Bounds) && slime3Visible))
                    {
                        goblin2Speed = -1;
                    }
                    else
                    {
                        goblin2Speed = 2;
                    }
                    if (goblin3Visible)
                    {
                        //move goblin3 toward player
                        if (((PictureBox)goblin3).Left > player.Left)
                        {
                            ((PictureBox)goblin3).Left -= goblin3Speed;
                            goblin3FacingRight = false;
                        }
                        if (((PictureBox)goblin3).Top > player.Top)
                        {
                            ((PictureBox)goblin3).Top -= goblin3Speed;
                        }
                        if (((PictureBox)goblin3).Left < player.Left)
                        {
                            ((PictureBox)goblin3).Left += goblin3Speed;
                            goblin3FacingRight = true;
                        }
                        if (((PictureBox)goblin3).Top < player.Top)
                        {
                            ((PictureBox)goblin3).Top += goblin3Speed;
                        }
                        if ((((PictureBox)goblin3).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible) || (((PictureBox)goblin3).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || (((PictureBox)goblin3).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible) || (((PictureBox)goblin3).Bounds.IntersectsWith(rat2.Bounds) && rat2Visible) || (((PictureBox)goblin3).Bounds.IntersectsWith(rat3.Bounds) && rat3Visible) || (((PictureBox)goblin3).Bounds.IntersectsWith(slime1.Bounds) && slime1Visible) || (((PictureBox)goblin3).Bounds.IntersectsWith(slime2.Bounds) && slime2Visible) || (((PictureBox)goblin3).Bounds.IntersectsWith(slime3.Bounds) && slime3Visible))
                        {
                            goblin3Speed = -1;
                        }
                        else
                        {
                            goblin3Speed = 2;
                        }
                    }
                }
                if (skeleton1Visible)
                {

                    //skeleton1
                    if (skeleton1Charge <= 0)
                    {
                        skeleton1Charge--;
                        if (((PictureBox)skeleton1).Left > chargeLeft1)
                        {
                            ((PictureBox)skeleton1).Left -= skeleton1Speed * 5;
                            skeleton1FacingRight = false;
                        }
                        if (((PictureBox)skeleton1).Top > chargeTop1)
                        {
                            ((PictureBox)skeleton1).Top -= skeleton1Speed * 5;
                        }
                        if (((PictureBox)skeleton1).Left < chargeLeft1)
                        {
                            ((PictureBox)skeleton1).Left += skeleton1Speed * 5;
                            skeleton1FacingRight = true;
                        }
                        if (((PictureBox)skeleton1).Top < chargeTop1)
                        {
                            ((PictureBox)skeleton1).Top += skeleton1Speed * 5;
                        }
                        if (skeleton1Charge <= CHARGE_PERIOD)
                        {
                            skeleton1Charge = randNum.Next(20, 50); //cancel charge
                        }
                    }
                    else
                    {
                        if (((PictureBox)skeleton1).Left > player.Left)
                        {
                            ((PictureBox)skeleton1).Left -= skeleton1Speed;
                            skeleton1FacingRight = false;
                        }
                        if (((PictureBox)skeleton1).Top > player.Top)
                        {
                            ((PictureBox)skeleton1).Top -= skeleton1Speed;
                        }
                        if (((PictureBox)skeleton1).Left < player.Left)
                        {
                            ((PictureBox)skeleton1).Left += skeleton1Speed;
                            skeleton1FacingRight = true;
                        }
                        if (((PictureBox)skeleton1).Top < player.Top)
                        {
                            ((PictureBox)skeleton1).Top += skeleton1Speed;
                        }
                        if (((((PictureBox)skeleton1).Bounds.IntersectsWith(rat1.Bounds) && rat1Visible && skeleton1Visible) || (((PictureBox)skeleton1).Bounds.IntersectsWith(rat2.Bounds) && skeleton1Visible && rat2Visible) || (((PictureBox)skeleton1).Bounds.IntersectsWith(rat3.Bounds) && bat2Visible) || (((PictureBox)skeleton1).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible) || ((PictureBox)skeleton1).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || ((PictureBox)skeleton1).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible)
                        {
                            skeleton1Speed = -1;
                        }
                        else
                        {
                            skeleton1Speed = 2;
                        }
                    }
                }

                if (skeleton2Visible)
                {
                    //skeleton2
                    if (skeleton2Charge <= 0)
                    {
                        skeleton2Charge--;
                        if (((PictureBox)skeleton2).Left > chargeLeft2)
                        {
                            ((PictureBox)skeleton2).Left -= skeleton2Speed * 5;
                            skeleton2FacingRight = false;
                        }
                        if (((PictureBox)skeleton2).Top > chargeTop2)
                        {
                            ((PictureBox)skeleton2).Top -= skeleton2Speed * 5;
                        }
                        if (((PictureBox)skeleton2).Left < chargeLeft2)
                        {
                            ((PictureBox)skeleton2).Left += skeleton2Speed * 5;
                            skeleton2FacingRight = true;
                        }
                        if (((PictureBox)skeleton2).Top < chargeTop2)
                        {
                            ((PictureBox)skeleton2).Top += skeleton2Speed * 5;
                        }
                        if (skeleton2Charge <= CHARGE_PERIOD)
                        {
                            skeleton2Charge = randNum.Next(20, 50); //cancel charge
                        }
                    }
                    else
                    {
                        if (((PictureBox)skeleton2).Left > player.Left)
                        {
                            ((PictureBox)skeleton2).Left -= skeleton2Speed;
                            skeleton2FacingRight = false;
                        }
                        if (((PictureBox)skeleton2).Top > player.Top)
                        {
                            ((PictureBox)skeleton2).Top -= skeleton2Speed;
                        }
                        if (((PictureBox)skeleton2).Left < player.Left)
                        {
                            ((PictureBox)skeleton2).Left += skeleton2Speed;
                            skeleton2FacingRight = true;
                        }
                        if (((PictureBox)skeleton2).Top < player.Top)
                        {
                            ((PictureBox)skeleton2).Top += skeleton2Speed;
                        }
                        if (((((PictureBox)skeleton2).Bounds.IntersectsWith(rat1.Bounds) && rat1Visible) || (((PictureBox)skeleton2).Bounds.IntersectsWith(rat2.Bounds) && rat2Visible) || (((PictureBox)skeleton3).Bounds.IntersectsWith(rat3.Bounds) && bat2Visible) || (((PictureBox)skeleton3).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible) || ((PictureBox)skeleton2).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || ((PictureBox)skeleton2).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible || ((PictureBox)skeleton2).Bounds.IntersectsWith(skeleton1.Bounds) && skeleton1Visible)
                        {
                            skeleton2Speed = -1;
                        }
                        else
                        {
                            skeleton2Speed = 2;
                        }
                    }
                }
                if (skeleton3Visible)
                {
                    //skeleton3
                    if (skeleton3Charge <= 0)
                    {
                        skeleton3Charge--;
                        if (((PictureBox)skeleton3).Left > chargeLeft3)
                        {
                            ((PictureBox)skeleton3).Left -= skeleton3Speed * 5;
                            skeleton3FacingRight = false;
                        }
                        if (((PictureBox)skeleton3).Top > chargeTop3)
                        {
                            ((PictureBox)skeleton3).Top -= skeleton3Speed * 5;
                        }
                        if (((PictureBox)skeleton3).Left < chargeLeft3)
                        {
                            ((PictureBox)skeleton3).Left += skeleton3Speed * 5;
                            skeleton3FacingRight = true;
                        }
                        if (((PictureBox)skeleton3).Top < chargeTop3)
                        {
                            ((PictureBox)skeleton3).Top += skeleton3Speed * 5;
                        }
                        if (skeleton3Charge <= CHARGE_PERIOD)
                        {
                            skeleton3Charge = randNum.Next(20, 50); //cancel charge
                        }
                    }
                    else
                    {
                        if (((PictureBox)skeleton3).Left > player.Left)
                        {
                            ((PictureBox)skeleton3).Left -= skeleton3Speed;
                            skeleton3FacingRight = false;
                        }
                        if (((PictureBox)skeleton3).Top > player.Top)
                        {
                            ((PictureBox)skeleton3).Top -= skeleton3Speed;
                        }
                        if (((PictureBox)skeleton3).Left < player.Left)
                        {
                            ((PictureBox)skeleton3).Left += skeleton3Speed;
                            skeleton3FacingRight = true;
                        }
                        if (((PictureBox)skeleton3).Top < player.Top)
                        {
                            ((PictureBox)skeleton3).Top += skeleton3Speed;
                        }
                        if (((((PictureBox)skeleton3).Bounds.IntersectsWith(rat1.Bounds) && rat1Visible) || (((PictureBox)skeleton3).Bounds.IntersectsWith(rat2.Bounds) && rat2Visible) || (((PictureBox)skeleton3).Bounds.IntersectsWith(rat3.Bounds) && bat2Visible) || (((PictureBox)skeleton2).Bounds.IntersectsWith(bat3.Bounds) && bat3Visible) || ((PictureBox)skeleton3).Bounds.IntersectsWith(bat2.Bounds) && bat2Visible) || ((PictureBox)skeleton3).Bounds.IntersectsWith(bat1.Bounds) && bat1Visible || ((PictureBox)skeleton3).Bounds.IntersectsWith(skeleton2.Bounds) && skeleton2Visible || ((PictureBox)skeleton3).Bounds.IntersectsWith(skeleton1.Bounds) && skeleton1Visible)
                        {
                            skeleton3Speed = -1;
                        }
                        else
                        {
                            skeleton3Speed = 2;
                        }
                    }

                }
                //slimes
                if (slime1Visible || slime2Visible || slime3Visible)
                {
                    if (slimeAnimCounter <= 10)
                    {
                        if (slime1Visible)
                        {
                            if ((((PictureBox)slime1).Bounds.IntersectsWith(slime2.Bounds) && slime2Visible) || (((PictureBox)slime1).Bounds.IntersectsWith(slime3.Bounds)) && slime3Visible)
                            {
                                slime1Speed = -3;
                            }
                            else
                            {
                                slime1Speed = 10;
                            }
                        }
                        if (slime2Visible)
                        {
                            if (((PictureBox)slime2).Bounds.IntersectsWith(slime3.Bounds) && slime3Visible)
                            {
                                slime2Speed = -3;
                            }
                            else
                            {
                                slime2Speed = 10;
                            }
                        }
                        if (slime1Visible)
                        {
                            if (!slime1FacingRight)
                            {
                                slime1.Image = Image.FromFile("images/NeutralSlimeBlueL.png");
                            }
                            else
                            {
                                slime1.Image = Image.FromFile("images/NeutralSlimeBlueR.png");
                            }
                        }
                        if (slime2Visible)
                        {
                            if (!slime2FacingRight)
                            {
                                slime2.Image = Image.FromFile("images/NeutralSlimeBlueL.png");
                            }
                            else
                            {
                                slime2.Image = Image.FromFile("images/NeutralSlimeBlueR.png");
                            }
                        }
                        if (slime3Visible)
                        {
                            if (!slime3FacingRight)
                            {
                                slime3.Image = Image.FromFile("images/NeutralSlimeBlueL.png");
                            }
                            else
                            {
                                slime3.Image = Image.FromFile("images/NeutralSlimeBlueR.png");
                            }
                        }

                    }
                    else if (slimeAnimCounter <= 20)
                    {

                        if (slime1Visible)
                        {
                            if (!slime1FacingRight)
                            {
                                slime1.Image = Image.FromFile("images/StretchedSlimeBlueL.png");
                            }
                            else
                            {
                                slime1.Image = Image.FromFile("images/StretchedSlimeBlueR.png");
                            }
                        }
                        if (slime2Visible)
                        {
                            if (!slime2FacingRight)
                            {
                                slime2.Image = Image.FromFile("images/StretchedSlimeBlueL.png");
                            }
                            else
                            {
                                slime2.Image = Image.FromFile("images/StretchedSlimeBlueR.png");
                            }
                        }
                        if (slime3Visible)
                        {
                            if (!slime3FacingRight)
                            {
                                slime3.Image = Image.FromFile("images/StretchedSlimeBlueL.png");
                            }
                            else
                            {
                                slime3.Image = Image.FromFile("images/StretchedSlimeBlueR.png");
                            }
                        }
                        if (slime1Visible)
                        {
                            //slime1
                            if (((PictureBox)slime1).Left > player.Left)
                            {
                                //jump left
                                ((PictureBox)slime1).Left -= slime1Speed;
                                slime1FacingRight = false;
                            }
                            if (((PictureBox)slime1).Top > player.Top)
                            {
                                //jump down
                                ((PictureBox)slime1).Top -= slime1Speed;
                            }
                            if (((PictureBox)slime1).Left < player.Left)
                            {
                                //jump right
                                slime1FacingRight = true;
                                ((PictureBox)slime1).Left += slime1Speed;
                            }
                            if (((PictureBox)slime1).Top < player.Top)
                            {
                                //jump up
                                ((PictureBox)slime1).Top += slime1Speed;
                            }
                        }
                        if (slime2Visible)
                        {
                            //slime2
                            if (((PictureBox)slime2).Left > player.Left)
                            {
                                //jump left
                                ((PictureBox)slime2).Left -= slime2Speed;
                                slime2FacingRight = false;
                            }
                            if (((PictureBox)slime2).Top > player.Top)
                            {
                                //jump down
                                ((PictureBox)slime2).Top -= slime2Speed;
                            }
                            if (((PictureBox)slime2).Left < player.Left)
                            {
                                //jump right
                                slime2FacingRight = true;
                                ((PictureBox)slime2).Left += slime2Speed;
                            }
                            if (((PictureBox)slime2).Top < player.Top)
                            {
                                //jump up
                                ((PictureBox)slime2).Top += slime2Speed;
                            }
                        }
                        if (slime3Visible)
                        {
                            //slime3
                            if (((PictureBox)slime3).Left > player.Left)
                            {
                                //jump left
                                ((PictureBox)slime3).Left -= slime3Speed;
                                slime3FacingRight = false;
                            }
                            if (((PictureBox)slime3).Top > player.Top)
                            {
                                //jump down
                                ((PictureBox)slime3).Top -= slime3Speed;
                            }
                            if (((PictureBox)slime3).Left < player.Left)
                            {
                                //jump right
                                slime3FacingRight = true;
                                ((PictureBox)slime3).Left += slime3Speed;
                            }
                            if (((PictureBox)slime3).Top < player.Top)
                            {
                                //jump up
                                ((PictureBox)slime3).Top += slime3Speed;
                            }
                        }
                    }
                    else if (slimeAnimCounter <= 25)
                    {
                        if (slime1Visible)
                        {
                            if (!slime1FacingRight)
                            {
                                slime1.Image = Image.FromFile("images/FlatSlimeBlueL.png");
                            }
                            else
                            {
                                slime1.Image = Image.FromFile("images/FlatSlimeBlueR.png");
                            }
                        }
                        if (slime2Visible)
                        {
                            if (!slime2FacingRight)
                            {
                                slime2.Image = Image.FromFile("images/FlatSlimeBlueL.png");
                            }
                            else
                            {
                                slime2.Image = Image.FromFile("images/FlatSlimeBlueR.png");
                            }
                        }
                        if (slime3Visible)
                        {
                            if (!slime3FacingRight)
                            {
                                slime3.Image = Image.FromFile("images/FlatSlimeBlueL.png");
                            }
                            else
                            {
                                slime3.Image = Image.FromFile("images/FlatSlimeBlueR.png");
                            }
                        }
                    }
                    else
                    {
                        slimeAnimCounter = 0;
                    }
                    slimeAnimCounter++;
                }
            }
        }
        private void GoblinShoot()
        {
            if (goblin1Cooldown == 0 && goblin1Visible)
            {
                //make bullet
                Bullet shootBullet = new Bullet();
                shootBullet.shooter = "goblin";
                //choose direction
                // Check if the player is diagonally up-left, up-right, down-right or down-left from the goblin within GOBLIN_GUN_DISTANCE units
                if (((PictureBox)goblin1).Top > player.Top && ((PictureBox)goblin1Gun).Left > player.Left && ((PictureBox)goblin1Gun).Top - player.Top <= GOBLIN_GUN_DISTANCE && ((PictureBox)goblin1Gun).Left - player.Left <= GOBLIN_GUN_DISTANCE)
                {
                    // leftup
                    shootBullet.direction = "leftup";
                }
                else if (((PictureBox)goblin1Gun).Top > player.Top && ((PictureBox)goblin1Gun).Left < player.Left && ((PictureBox)goblin1Gun).Top - player.Top <= GOBLIN_GUN_DISTANCE && player.Left - ((PictureBox)goblin1Gun).Left <= GOBLIN_GUN_DISTANCE)
                {
                    // rightup
                    shootBullet.direction = "rightup";
                }
                else if (((PictureBox)goblin1Gun).Top < player.Top && ((PictureBox)goblin1Gun).Left > player.Left && player.Top - ((PictureBox)goblin1Gun).Top <= GOBLIN_GUN_DISTANCE && ((PictureBox)goblin1Gun).Left - player.Left <= GOBLIN_GUN_DISTANCE)
                {
                    // leftdown
                    shootBullet.direction = "leftdown";
                }
                else if (((PictureBox)goblin1Gun).Top < player.Top && ((PictureBox)goblin1Gun).Left < player.Left && player.Top - ((PictureBox)goblin1Gun).Top <= GOBLIN_GUN_DISTANCE && player.Left - ((PictureBox)goblin1Gun).Left <= GOBLIN_GUN_DISTANCE)
                {
                    // rightdown
                    shootBullet.direction = "rightdown";
                }
                else
                {
                    // Check if horizontal distance is greater than vertical distance
                    if (Math.Abs(((PictureBox)goblin1Gun).Left - player.Left) > Math.Abs(((PictureBox)goblin1Gun).Top - player.Top))
                    {
                        // horizontal direction
                        if (((PictureBox)goblin1Gun).Left > player.Left)
                        {
                            // left
                            shootBullet.direction = "left";
                        }
                        else
                        {
                            // right
                            shootBullet.direction = "right";
                        }
                    }
                    else
                    {
                        // vertical direction
                        if (((PictureBox)goblin1Gun).Top > player.Top)
                        {
                            // up
                            shootBullet.direction = "up";
                        }
                        else
                        {
                            // down
                            shootBullet.direction = "down";
                        }
                    }
                }

                shootBullet.bulletLeft = goblin1Gun.Left + (goblin1Gun.Width / 2);
                shootBullet.bulletTop = goblin1Gun.Top + (goblin1Gun.Height / 2);
                shootBullet.MakeBullet(this);
                //make a variable in bullet class to tell the code it is an enemy bullet which can hurt the player
                goblin1Cooldown = randNum.Next(30, 70);
            }
            if (goblin2Cooldown == 0 && goblin2Visible) //have not finished this part
            {
                //make bullet
                Bullet shootBullet = new Bullet();
                shootBullet.shooter = "goblin";
                //choose direction
                // Check if the player is diagonally up-left, up-right, down-right or down-left from the goblin within GOBLIN_GUN_DISTANCE units
                if (((PictureBox)goblin2).Top > player.Top && ((PictureBox)goblin2Gun).Left > player.Left && ((PictureBox)goblin2Gun).Top - player.Top <= GOBLIN_GUN_DISTANCE && ((PictureBox)goblin2Gun).Left - player.Left <= GOBLIN_GUN_DISTANCE)
                {
                    // leftup
                    shootBullet.direction = "leftup";
                }
                else if (((PictureBox)goblin2Gun).Top > player.Top && ((PictureBox)goblin2Gun).Left < player.Left && ((PictureBox)goblin2Gun).Top - player.Top <= GOBLIN_GUN_DISTANCE && player.Left - ((PictureBox)goblin2Gun).Left <= GOBLIN_GUN_DISTANCE)
                {
                    // rightup
                    shootBullet.direction = "rightup";
                }
                else if (((PictureBox)goblin2Gun).Top < player.Top && ((PictureBox)goblin2Gun).Left > player.Left && player.Top - ((PictureBox)goblin2Gun).Top <= GOBLIN_GUN_DISTANCE && ((PictureBox)goblin2Gun).Left - player.Left <= GOBLIN_GUN_DISTANCE)
                {
                    // leftdown
                    shootBullet.direction = "leftdown";
                }
                else if (((PictureBox)goblin2Gun).Top < player.Top && ((PictureBox)goblin2Gun).Left < player.Left && player.Top - ((PictureBox)goblin2Gun).Top <= GOBLIN_GUN_DISTANCE && player.Left - ((PictureBox)goblin2Gun).Left <= GOBLIN_GUN_DISTANCE)
                {
                    // rightdown
                    shootBullet.direction = "rightdown";
                }
                else
                {
                    // Check if horizontal distance is greater than vertical distance
                    if (Math.Abs(((PictureBox)goblin2Gun).Left - player.Left) > Math.Abs(((PictureBox)goblin2Gun).Top - player.Top))
                    {
                        // horizontal direction
                        if (((PictureBox)goblin2Gun).Left > player.Left)
                        {
                            // left
                            shootBullet.direction = "left";
                        }
                        else
                        {
                            // right
                            shootBullet.direction = "right";
                        }
                    }
                    else
                    {
                        // vertical direction
                        if (((PictureBox)goblin2Gun).Top > player.Top)
                        {
                            // up
                            shootBullet.direction = "up";
                        }
                        else
                        {
                            // down
                            shootBullet.direction = "down";
                        }
                    }
                }

                shootBullet.bulletLeft = goblin2Gun.Left + (goblin2Gun.Width / 2);
                shootBullet.bulletTop = goblin2Gun.Top + (goblin2Gun.Height / 2);
                shootBullet.MakeBullet(this);
                //make a variable in bullet class to tell the code it is an enemy bullet which can hurt the player
                goblin2Cooldown = randNum.Next(30, 70);
            }
            if (goblin3Cooldown == 0 && goblin3Visible) //have not finished this part
            {
                //make bullet
                Bullet shootBullet = new Bullet();
                shootBullet.shooter = "goblin";
                //choose direction
                // Check if the player is diagonally up-left, up-right, down-right or down-left from the goblin within GOBLIN_GUN_DISTANCE units
                if (((PictureBox)goblin3).Top > player.Top && ((PictureBox)goblin3Gun).Left > player.Left && ((PictureBox)goblin3Gun).Top - player.Top <= GOBLIN_GUN_DISTANCE && ((PictureBox)goblin3Gun).Left - player.Left <= GOBLIN_GUN_DISTANCE)
                {
                    // leftup
                    shootBullet.direction = "leftup";
                }
                else if (((PictureBox)goblin3Gun).Top > player.Top && ((PictureBox)goblin3Gun).Left < player.Left && ((PictureBox)goblin3Gun).Top - player.Top <= GOBLIN_GUN_DISTANCE && player.Left - ((PictureBox)goblin3Gun).Left <= GOBLIN_GUN_DISTANCE)
                {
                    // rightup
                    shootBullet.direction = "rightup";
                }
                else if (((PictureBox)goblin3Gun).Top < player.Top && ((PictureBox)goblin3Gun).Left > player.Left && player.Top - ((PictureBox)goblin3Gun).Top <= GOBLIN_GUN_DISTANCE && ((PictureBox)goblin3Gun).Left - player.Left <= GOBLIN_GUN_DISTANCE)
                {
                    // leftdown
                    shootBullet.direction = "leftdown";
                }
                else if (((PictureBox)goblin3Gun).Top < player.Top && ((PictureBox)goblin3Gun).Left < player.Left && player.Top - ((PictureBox)goblin3Gun).Top <= GOBLIN_GUN_DISTANCE && player.Left - ((PictureBox)goblin3Gun).Left <= GOBLIN_GUN_DISTANCE)
                {
                    // rightdown
                    shootBullet.direction = "rightdown";
                }
                else
                {
                    // Check if horizontal distance is greater than vertical distance
                    if (Math.Abs(((PictureBox)goblin3Gun).Left - player.Left) > Math.Abs(((PictureBox)goblin3Gun).Top - player.Top))
                    {
                        // horizontal direction
                        if (((PictureBox)goblin3Gun).Left > player.Left)
                        {
                            // left
                            shootBullet.direction = "left";
                        }
                        else
                        {
                            // right
                            shootBullet.direction = "right";
                        }
                    }
                    else
                    {
                        // vertical direction
                        if (((PictureBox)goblin3Gun).Top > player.Top)
                        {
                            // up
                            shootBullet.direction = "up";
                        }
                        else
                        {
                            // down
                            shootBullet.direction = "down";
                        }
                    }
                }

                shootBullet.bulletLeft = goblin3Gun.Left + (goblin3Gun.Width / 2);
                shootBullet.bulletTop = goblin3Gun.Top + (goblin3Gun.Height / 2);
                shootBullet.MakeBullet(this);
                //make a variable in bullet class to tell the code it is an enemy bullet which can hurt the player
                goblin3Cooldown = randNum.Next(30, 70);
            }


        }

        private void RefreshHealth()
        {
            if (!finishedDeathSequence)
            {
                if (playerHealth == 50)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/heart.png");
                    heart3.Image = Image.FromFile("images/heart.png");
                    heart4.Image = Image.FromFile("images/heart.png");
                    heart5.Image = Image.FromFile("images/heart.png");
                }
                else if (playerHealth >= 45)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/heart.png");
                    heart3.Image = Image.FromFile("images/heart.png");
                    heart4.Image = Image.FromFile("images/heart.png");
                    heart5.Image = Image.FromFile("images/halfHeart.png");
                }
                else if (playerHealth >= 40)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/heart.png");
                    heart3.Image = Image.FromFile("images/heart.png");
                    heart4.Image = Image.FromFile("images/heart.png");
                    heart5.Image = Image.FromFile("images/noHeart.png");
                }
                else if (playerHealth >= 35)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/heart.png");
                    heart3.Image = Image.FromFile("images/heart.png");
                    heart4.Image = Image.FromFile("images/halfHeart.png");
                    heart5.Image = Image.FromFile("images/noHeart.png");
                }
                else if (playerHealth >= 30)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/heart.png");
                    heart3.Image = Image.FromFile("images/heart.png");
                    heart4.Image = Image.FromFile("images/noHeart.png");

                }
                else if (playerHealth >= 25)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/heart.png");
                    heart3.Image = Image.FromFile("images/Halfheart.png");
                }
                else if (playerHealth >= 20)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/heart.png");
                    heart3.Image = Image.FromFile("images/Noheart.png");
                }
                else if (playerHealth >= 15)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/halfHeart.png");
                    heart3.Image = Image.FromFile("images/Noheart.png");
                }
                else if (playerHealth >= 10)
                {
                    heart1.Image = Image.FromFile("images/heart.png");
                    heart2.Image = Image.FromFile("images/NoHeart.png");
                    heart3.Image = Image.FromFile("images/Noheart.png");
                }
                else if (playerHealth >= 5)
                {
                    heart1.Image = Image.FromFile("images/halfheart.png");
                    heart2.Image = Image.FromFile("images/NoHeart.png");
                    heart3.Image = Image.FromFile("images/Noheart.png");
                }
                else
                {
                    heart1.Image = Image.FromFile("images/Noheart.png");
                    heart2.Image = Image.FromFile("images/NoHeart.png");
                    heart3.Image = Image.FromFile("images/Noheart.png");
                }
            }




        }
        private void EnemyDrops()
        {
            if (!finishedDeathSequence)
            {
                //drop souls
                soulDropChance = randNum.Next(0, 6);
                if (soulDropChance == 0)
                {
                    //drop a soul
                    for (int i = 0; i < floor; i++)
                    {
                        PictureBox soul = new PictureBox
                        {
                            Image = Image.FromFile("images/Soul.png"),
                            Top = enemyDropTop,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = soulSize,
                            Tag = "soul",
                            Left = enemyDropLeft,
                        };
                        soul.BringToFront();
                        this.Controls.Add(soul);
                        player.BringToFront();
                        gun.BringToFront();
                    }
                }
                //drop gold
                enemyCoinDrops = randNum.Next(0, 6);
                if (enemyCoinDrops == 0)
                {
                    //drop a coin
                    PictureBox coin = new PictureBox
                    {
                        Image = Image.FromFile("images/Coin.png"),
                        Top = enemyDropTop,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = coinSize,
                        Tag = "coin",
                        Left = enemyDropLeft
                    };
                    coin.BringToFront();
                    this.Controls.Add(coin);
                    player.BringToFront();
                    gun.BringToFront();
                }
            }
        }


        private void SpawnBlood()
        {
            if (!finishedDeathSequence)
            {

                if (bloodType == "small")
                {
                    //small blood splatter
                    PictureBox smallBlood = new PictureBox()
                    {
                        Top = enemyDropTop + randNum.Next(-5, 5),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = smallBloodSplatter,
                        Tag = "blood",
                        Left = enemyDropLeft + randNum.Next(-5, 5),

                    };
                    smallBloodImage = randNum.Next(0, 3 + 1);
                    if (smallBloodImage == 0)
                    {
                        smallBlood.Image = Image.FromFile("images/smallBlood1.png");
                    }
                    else if (smallBloodImage == 1)
                    {
                        smallBlood.Image = Image.FromFile("images/smallBlood2.png");
                    }
                    else if (smallBloodImage == 2)
                    {
                        smallBlood.Image = Image.FromFile("images/smallBlood3.png");
                    }
                    else if (smallBloodImage == 3)
                    {
                        smallBlood.Image = Image.FromFile("images/smallBlood4.png");
                    }


                    smallBlood.SendToBack();
                    this.Controls.Add(smallBlood);
                    player.BringToFront();
                    gun.BringToFront();
                }
                else if (bloodType == "big")
                {
                    //big blood splatter
                    PictureBox bigBlood = new PictureBox()
                    {
                        Top = enemyDropTop + randNum.Next(-5, 5),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = bigBloodSplatter,
                        Tag = "blood",
                        Left = enemyDropLeft + randNum.Next(-5, 5),

                    };
                    bigBloodImage = randNum.Next(0, 4 + 1);
                    if (bigBloodImage == 0)
                    {
                        bigBlood.Image = Image.FromFile("images/BloodSplatter1.png");
                    }
                    else if (bigBloodImage == 1)
                    {
                        bigBlood.Image = Image.FromFile("images/BloodSplatter2.png");
                    }
                    else if (bigBloodImage == 2)
                    {
                        bigBlood.Image = Image.FromFile("images/BloodSplatter3.png");
                    }
                    else if (bigBloodImage == 3)
                    {
                        bigBlood.Image = Image.FromFile("images/BloodSplatter4.png");
                    }
                    else if (bigBloodImage == 4)
                    {
                        bigBlood.Image = Image.FromFile("images/BloodSplatter5.png");
                    }
                    bigBlood.SendToBack();
                    this.Controls.Add(bigBlood);
                    player.BringToFront();
                    gun.BringToFront();
                }
                else if (bloodType == "smallslime")
                {
                    //small slime splatter
                    PictureBox smallSlime = new PictureBox()
                    {
                        Top = enemyDropTop + randNum.Next(-5, 5),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = smallBloodSplatter,
                        Tag = "blood",
                        Left = enemyDropLeft + randNum.Next(-5, 5),

                    };
                    smallBloodImage = randNum.Next(0, 3 + 1);
                    if (smallBloodImage == 0)
                    {
                        smallSlime.Image = Image.FromFile("images/smallSlime1.png");
                    }
                    else if (smallBloodImage == 1)
                    {
                        smallSlime.Image = Image.FromFile("images/smallSlime2.png");
                    }
                    else if (smallBloodImage == 2)
                    {
                        smallSlime.Image = Image.FromFile("images/smallSlime3.png");
                    }
                    else if (smallBloodImage == 3)
                    {
                        smallSlime.Image = Image.FromFile("images/smallSlime4.png");
                    }
                    smallSlime.SendToBack();
                    this.Controls.Add(smallSlime);
                    player.BringToFront();
                    gun.BringToFront();
                }
                else if (bloodType == "bigslime")
                {
                    //big slime splatter
                    PictureBox bigSlime = new PictureBox()
                    {
                        Top = enemyDropTop + randNum.Next(-5, 5),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = bigBloodSplatter,
                        Tag = "blood",
                        Left = enemyDropLeft + randNum.Next(-5, 5),

                    };
                    bigBloodImage = randNum.Next(0, 3 + 1);
                    if (bigBloodImage == 0)
                    {
                        bigSlime.Image = Image.FromFile("images/SlimeSplatter1.png");
                    }
                    else if (bigBloodImage == 1)
                    {
                        bigSlime.Image = Image.FromFile("images/SlimeSplatter2.png");
                    }
                    else if (bigBloodImage == 2)
                    {
                        bigSlime.Image = Image.FromFile("images/SlimeSplatter3.png");
                    }
                    else if (bigBloodImage == 3)
                    {
                        bigSlime.Image = Image.FromFile("images/SlimeSplatter4.png");
                    }
                    bigSlime.SendToBack();
                    this.Controls.Add(bigSlime);
                    player.BringToFront();
                    gun.BringToFront();
                }
                else if (bloodType == "bones")
                {
                    //bones
                    PictureBox bones = new PictureBox()
                    {
                        Top = enemyDropTop + randNum.Next(-5, 5),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = bigBloodSplatter, //change to bones
                        Tag = "blood",
                        Left = enemyDropLeft + randNum.Next(-5, 5),
                        Image = Image.FromFile("images/skeletonHead.png")
                    };
                    bones.SendToBack();
                    this.Controls.Add(bones);
                    player.BringToFront();
                    gun.BringToFront();
                }
            }
        }
        private void Die()
        {
            if (!finishedDeathSequence)
            {


                //death effects
                bloodType = "big";
                enemyDropTop = ((PictureBox)player).Top;
                enemyDropLeft = ((PictureBox)player).Left;
                SpawnBlood();
                //death animation
                //have a slight delay before brining up death screen
                //Show you died with a button to continue
                deathScreen.Show();
                deathScreen.BringToFront();
                continueBtn.Show();
                continueBtn.BringToFront();
                //wait til user clicks button
                //GameTimer.Stop();
                finishedDeathSequence = true;
                rat1.Hide();
                rat1Visible = false;
                rat2.Hide();
                rat2Visible = false;
                rat3.Hide();
                rat3Visible = false;
                bat1.Hide();
                bat1Visible = false;
                bat2.Hide();
                bat2Visible = false;
                bat3.Hide();
                bat3Visible = false;
                slime1.Hide();
                slime1Visible = false;
                slime2.Hide();
                slime2Visible = false;
                slime3.Hide();
                slime3Visible = false;
                goblin1.Hide();
                goblin1Visible = false;
                goblin2.Hide();
                goblin2Visible = false;
                goblin3.Hide();
                goblin3Visible = false;
                skeleton1.Hide();
                skeleton1Visible = false;
                skeleton2.Hide();
                skeleton2Visible = false;
                skeleton3.Hide();
                skeleton3Visible = false;
                goblin1Gun.Hide();
                goblin2Gun.Hide();
                goblin3Gun.Hide();
            }
        }

        private void StartRun()
        {
            gunDropVisible = false;
            gunDrop.Hide();
            haveGun = false;
            maxiFrames = 5;
            Bullet.damage = 30; //edit this to check which weapon you have
            goLeft = false;
            goRight = false;
            goUp = false;
            goDown = false;
            player.Top = 380;
            player.Left = 440;
            maxHealth = 30;
            souls = 0;
            gold = 0;
            enemyNum = 0;
            batNum = 0;
            ratNum = 0;
            goblinNum = 0;
            skeletonNum = 0;
            slimeNum = 0;
            stopRemoveLoot = 10;
            floor = 1;
            roomNum = 0;
            firstRoom = true;
            deathScreen.Hide();
            finishedDeathSequence = false;
            allLocked = false;
            lockedRoom = "none";
            enemyHealth.Clear();
            enemyHit = 0;
            weaponLuck = 0;
            using (StreamWriter writer = new StreamWriter("quit.csv"))
            {
                writer.Write(string.Empty); // This clears the file
                writer.WriteLine("false");
            }
            using (StreamReader reader = new StreamReader("upgrades.csv"))
            {
                string items = reader.ReadToEnd();
                for (int i = 0; i < 5; i++)
                {
                    if (items.Contains($"lvl{i}healthBoost"))
                    {
                        upgrades.Add($"lvl{i}healthBoost");
                        maxHealth += (i * 5);
                    }
                    if (items.Contains($"lvl{i}attackBoost"))
                    {
                        upgrades.Add($"lvl{i}attackBoost");
                        damageMultiplier = ((i * 5) / 100) + 1;
                    }
                    if (items.Contains($"lvl{i}luckyStart"))
                    {
                        upgrades.Add($"lvl{i}luckyStart");
                        weaponLuck += 5;
                    }
                    if (items.Contains($"lvl{i}invincibility"))
                    {
                        upgrades.Add($"lvl{i}invincibility");
                        maxiFrames += (i * 2);
                    }
                }
            }
            if (maxHealth > 30 && maxHealth <= 40)
            {
                heart4.Show();
                heart5.Hide();
            }
            if (maxHealth > 40)
            {
                heart4.Show();
                heart5.Show();
            }
            playerHealth = maxHealth; //add more health based on upgrades
            damage *= damageMultiplier;
            //remove all blood
            //spawn wapon chest
            SpawnChest();
        }
    }


}