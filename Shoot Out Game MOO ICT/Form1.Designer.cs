namespace Shoot_Out_Game_MOO_ICT
{
    partial class DungeonDelvers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DungeonDelvers));
            this.label1 = new System.Windows.Forms.Label();
            this.healthBar = new System.Windows.Forms.ProgressBar();
            this.GameTimer = new System.Windows.Forms.Timer(this.components);
            this.labelRoom = new System.Windows.Forms.Label();
            this.labelFloor = new System.Windows.Forms.Label();
            this.labelGold = new System.Windows.Forms.Label();
            this.rat3 = new System.Windows.Forms.PictureBox();
            this.rat2 = new System.Windows.Forms.PictureBox();
            this.rat1 = new System.Windows.Forms.PictureBox();
            this.bat3 = new System.Windows.Forms.PictureBox();
            this.bat2 = new System.Windows.Forms.PictureBox();
            this.bat1 = new System.Windows.Forms.PictureBox();
            this.stairs = new System.Windows.Forms.PictureBox();
            this.basicDoor3 = new System.Windows.Forms.PictureBox();
            this.basicDoor2 = new System.Windows.Forms.PictureBox();
            this.basicDoor1 = new System.Windows.Forms.PictureBox();
            this.basicDoor = new System.Windows.Forms.PictureBox();
            this.player = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.rat3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rat2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rat1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bat3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bat2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bat1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stairs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 626);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Health: ";
            // 
            // healthBar
            // 
            this.healthBar.Location = new System.Drawing.Point(88, 626);
            this.healthBar.Name = "healthBar";
            this.healthBar.Size = new System.Drawing.Size(187, 23);
            this.healthBar.TabIndex = 1;
            this.healthBar.Value = 100;
            // 
            // GameTimer
            // 
            this.GameTimer.Enabled = true;
            this.GameTimer.Interval = 20;
            this.GameTimer.Tick += new System.EventHandler(this.MainTimerEvent);
            // 
            // labelRoom
            // 
            this.labelRoom.AutoSize = true;
            this.labelRoom.BackColor = System.Drawing.Color.White;
            this.labelRoom.Location = new System.Drawing.Point(15, 12);
            this.labelRoom.Name = "labelRoom";
            this.labelRoom.Size = new System.Drawing.Size(38, 13);
            this.labelRoom.TabIndex = 10;
            this.labelRoom.Text = "Room:";
            // 
            // labelFloor
            // 
            this.labelFloor.AutoSize = true;
            this.labelFloor.BackColor = System.Drawing.Color.White;
            this.labelFloor.Location = new System.Drawing.Point(15, 26);
            this.labelFloor.Name = "labelFloor";
            this.labelFloor.Size = new System.Drawing.Size(33, 13);
            this.labelFloor.TabIndex = 11;
            this.labelFloor.Text = "Floor:";
            // 
            // labelGold
            // 
            this.labelGold.AutoSize = true;
            this.labelGold.BackColor = System.Drawing.Color.White;
            this.labelGold.Location = new System.Drawing.Point(15, 40);
            this.labelGold.Name = "labelGold";
            this.labelGold.Size = new System.Drawing.Size(32, 13);
            this.labelGold.TabIndex = 12;
            this.labelGold.Text = "Gold:";
            // 
            // rat3
            // 
            this.rat3.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DD_Rat1;
            this.rat3.Location = new System.Drawing.Point(584, 440);
            this.rat3.Name = "rat3";
            this.rat3.Size = new System.Drawing.Size(83, 37);
            this.rat3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rat3.TabIndex = 18;
            this.rat3.TabStop = false;
            // 
            // rat2
            // 
            this.rat2.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DD_Rat1;
            this.rat2.Location = new System.Drawing.Point(625, 286);
            this.rat2.Name = "rat2";
            this.rat2.Size = new System.Drawing.Size(83, 37);
            this.rat2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rat2.TabIndex = 17;
            this.rat2.TabStop = false;
            // 
            // rat1
            // 
            this.rat1.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DD_Rat1;
            this.rat1.Location = new System.Drawing.Point(603, 354);
            this.rat1.Name = "rat1";
            this.rat1.Size = new System.Drawing.Size(83, 37);
            this.rat1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rat1.TabIndex = 16;
            this.rat1.TabStop = false;
            // 
            // bat3
            // 
            this.bat3.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DD_BatDown4;
            this.bat3.Location = new System.Drawing.Point(250, 270);
            this.bat3.Name = "bat3";
            this.bat3.Size = new System.Drawing.Size(89, 36);
            this.bat3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bat3.TabIndex = 15;
            this.bat3.TabStop = false;
            // 
            // bat2
            // 
            this.bat2.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DD_BatDown4;
            this.bat2.Location = new System.Drawing.Point(360, 295);
            this.bat2.Name = "bat2";
            this.bat2.Size = new System.Drawing.Size(89, 36);
            this.bat2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bat2.TabIndex = 14;
            this.bat2.TabStop = false;
            // 
            // bat1
            // 
            this.bat1.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DD_BatDown4;
            this.bat1.Location = new System.Drawing.Point(499, 286);
            this.bat1.Name = "bat1";
            this.bat1.Size = new System.Drawing.Size(89, 36);
            this.bat1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bat1.TabIndex = 13;
            this.bat1.TabStop = false;
            // 
            // stairs
            // 
            this.stairs.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DD_Staircase;
            this.stairs.Location = new System.Drawing.Point(426, 189);
            this.stairs.Name = "stairs";
            this.stairs.Size = new System.Drawing.Size(100, 100);
            this.stairs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.stairs.TabIndex = 7;
            this.stairs.TabStop = false;
            // 
            // basicDoor3
            // 
            this.basicDoor3.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DoorClosedTop;
            this.basicDoor3.InitialImage = ((System.Drawing.Image)(resources.GetObject("basicDoor3.InitialImage")));
            this.basicDoor3.Location = new System.Drawing.Point(432, 1);
            this.basicDoor3.Name = "basicDoor3";
            this.basicDoor3.Size = new System.Drawing.Size(65, 100);
            this.basicDoor3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.basicDoor3.TabIndex = 6;
            this.basicDoor3.TabStop = false;
            // 
            // basicDoor2
            // 
            this.basicDoor2.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DoorClosedTop;
            this.basicDoor2.InitialImage = ((System.Drawing.Image)(resources.GetObject("basicDoor2.InitialImage")));
            this.basicDoor2.Location = new System.Drawing.Point(432, 560);
            this.basicDoor2.Name = "basicDoor2";
            this.basicDoor2.Size = new System.Drawing.Size(65, 100);
            this.basicDoor2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.basicDoor2.TabIndex = 5;
            this.basicDoor2.TabStop = false;
            // 
            // basicDoor1
            // 
            this.basicDoor1.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DoorClosedTop;
            this.basicDoor1.InitialImage = ((System.Drawing.Image)(resources.GetObject("basicDoor1.InitialImage")));
            this.basicDoor1.Location = new System.Drawing.Point(858, 270);
            this.basicDoor1.Name = "basicDoor1";
            this.basicDoor1.Size = new System.Drawing.Size(65, 100);
            this.basicDoor1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.basicDoor1.TabIndex = 4;
            this.basicDoor1.TabStop = false;
            // 
            // basicDoor
            // 
            this.basicDoor.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.DoorClosedTop;
            this.basicDoor.InitialImage = ((System.Drawing.Image)(resources.GetObject("basicDoor.InitialImage")));
            this.basicDoor.Location = new System.Drawing.Point(1, 270);
            this.basicDoor.Name = "basicDoor";
            this.basicDoor.Size = new System.Drawing.Size(65, 100);
            this.basicDoor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.basicDoor.TabIndex = 3;
            this.basicDoor.TabStop = false;
            // 
            // player
            // 
            this.player.Image = global::Shoot_Out_Game_MOO_ICT.Properties.Resources.up;
            this.player.Location = new System.Drawing.Point(426, 440);
            this.player.Name = "player";
            this.player.Size = new System.Drawing.Size(71, 100);
            this.player.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.player.TabIndex = 2;
            this.player.TabStop = false;
            // 
            // DungeonDelvers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(924, 661);
            this.Controls.Add(this.player);
            this.Controls.Add(this.rat3);
            this.Controls.Add(this.rat2);
            this.Controls.Add(this.rat1);
            this.Controls.Add(this.bat3);
            this.Controls.Add(this.bat2);
            this.Controls.Add(this.bat1);
            this.Controls.Add(this.labelGold);
            this.Controls.Add(this.labelFloor);
            this.Controls.Add(this.labelRoom);
            this.Controls.Add(this.stairs);
            this.Controls.Add(this.basicDoor3);
            this.Controls.Add(this.basicDoor2);
            this.Controls.Add(this.basicDoor1);
            this.Controls.Add(this.basicDoor);
            this.Controls.Add(this.healthBar);
            this.Controls.Add(this.label1);
            this.Name = "DungeonDelvers";
            this.Text = "Dungeon Delvers";
            this.Load += new System.EventHandler(this.DungeonDelvers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyIsDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyIsUp);
            ((System.ComponentModel.ISupportInitialize)(this.rat3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rat2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rat1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bat3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bat2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bat1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stairs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basicDoor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar healthBar;
        private System.Windows.Forms.PictureBox player;
        private System.Windows.Forms.Timer GameTimer;
        private System.Windows.Forms.PictureBox basicDoor;
        private System.Windows.Forms.PictureBox basicDoor1;
        private System.Windows.Forms.PictureBox basicDoor2;
        private System.Windows.Forms.PictureBox basicDoor3;
        private System.Windows.Forms.PictureBox stairs;
        private System.Windows.Forms.Label labelRoom;
        private System.Windows.Forms.Label labelFloor;
        private System.Windows.Forms.Label labelGold;
        private System.Windows.Forms.PictureBox bat1;
        private System.Windows.Forms.PictureBox bat2;
        private System.Windows.Forms.PictureBox bat3;
        private System.Windows.Forms.PictureBox rat1;
        private System.Windows.Forms.PictureBox rat2;
        private System.Windows.Forms.PictureBox rat3;
    }
}

