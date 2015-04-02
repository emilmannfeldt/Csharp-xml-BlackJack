using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJackLab
{
    public partial class Form1 : Form
    {
        FileSystemWatcher fsw;
        GameEngine game;
        IPlayer GuiPlayer
        {
            get { return game.getPlayer(0); }
        }
        IPlayer AiPlayer
        {
            get { return game.getPlayer(1); }
        }
        int bet;
       
        private string state = "load";
        
        
        public Form1()

        {
            InitializeComponent();

            //messagebox to choose if load or new game

              DialogResult dialogResult = MessageBox.Show("Do you want to start a new game?", "New Game?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                
                state = "new";
                Console.WriteLine("starting new game");
            }
            else if (dialogResult == DialogResult.No)
            {
               
                state="load";
                Console.WriteLine("loading game");
            }
             
         

            game = new GameEngine(state);
            StartFileWatcher();
            
            if (GuiPlayer.Active)
            {
                cip1.Enabled = false;
                cip25.Enabled = false;
                cip100.Enabled = false;
                BetButton.Enabled = false;
                HitButton.Visible = true;
                HitButton.Enabled = true;
                DoubleButton.Visible = true;
                DoubleButton.Enabled = true;
                StopButton.Visible = true;
                StopButton.Enabled = true; 
            }
            else
            {
               
                HitButton.Visible = false;
                DoubleButton.Visible = false;
                StopButton.Visible = false;
                BetButton.Enabled = false;
            }
            Updatestorage();
        }

        private void BetButton_Click(object sender, EventArgs e)
        {
            GuiPlayer.Active = true;
            bet = Convert.ToInt32(BetText.Text);
            AiPlayer.makeBet(bet);
            if (game.StartRound(bet,AiPlayer.Bet))
            {
                cip1.Enabled = false;
                cip25.Enabled = false;
                cip100.Enabled = false;
                BetButton.Enabled = false;
                HitButton.Visible = true;
                HitButton.Enabled = true;
                DoubleButton.Visible = true;
                DoubleButton.Enabled = true;
                StopButton.Visible = true;
                StopButton.Enabled = true;
                Updatestorage();
                Console.WriteLine("updateingstorage calling game");
                
                

            }
            else
            {
                MessageBox.Show("You may not bet more money than you have.", "Cheater alert!");
            }
            if(GuiPlayer.WinStatus == "Blackjack")
            {
                AiPlay();
                Updatestorage();
            }
            
        }

        private void HitButton_Click(object sender, EventArgs e)
        {
            if(!game.CardToPlayer(GuiPlayer)) //draws a card. if player is busted or has blackjack enter statement
            {
               
               //UpdateUiElements();
                //may call for method in Iplayer. needs test
               AiPlay();
            }
            DoubleButton.Enabled = false;
            Updatestorage();
            
        }

        private void AiPlay()
        {
            Updatestorage();
            string aimove = AiPlayer.Act(AiPlayer.Score, AiPlayer.Hand,game.dealerscore);
            //MessageBox.Show("AI says: "+aimove);
            if (aimove.Equals("hit"))
            {
                if (!game.CardToPlayer(AiPlayer))
                {

                    GuiEndRound();
                }
                AiPlay();
            }
            else if (aimove.Equals("double"))
            {
                AiPlayer.Bet=game.doublebet(AiPlayer.Bet);            
                game.CardToPlayer(AiPlayer);
                GuiEndRound();
            }
            else if (aimove.Equals("stop"))
            {
                GuiEndRound();
            }
          
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StopButton.Enabled = false;
            AiPlay();
            Updatestorage();             
        }

        public void UpdateUiElements()
        {

            //fungerande bildvisare. inte så snyggt med bestämt antal boxar men duger tillsvidare
            PictureBox[] playerBoxes = { PlayerCardBox1, PlayerCardBox2, PlayerCardBox3, PlayerCardBox4, PlayerCardBox5, PlayerCardBox6, PlayerCardBox7, PlayerCardBox8 };
            PictureBox[] dealerBoxes = { dealerCardBox1, dealerCardBox2, dealerCardBox3, dealerCardBox4, dealerCardBox5, dealerCardBox6, dealerCardBox7, dealerCardBox8 };
            PictureBox[] aiBoxes = { AiCardBox1, AiCardBox2, AiCardBox3, AiCardBox4, AiCardBox5, AiCardBox6, AiCardBox7, AiCardBox8 };
            foreach (PictureBox p in playerBoxes) //sets all boxes to invisible until changed
            {
                p.Visible = false;
            }
            foreach (PictureBox p in dealerBoxes)
            {
                p.Visible = false;
            }
            foreach (PictureBox p in aiBoxes)
            {
                p.Visible = false;
            }
            for (int i = 0; i < GuiPlayer.Hand.Count;i++)
            {
                Card c = GuiPlayer.Hand[i];
                // use boxes[i] to access each picture box
                playerBoxes[i].Image = Image.FromFile(@"..\\..\\Resources\\" + (c.Visible? c.ImagePath : "cardback.png"));
                playerBoxes[i].Visible = true;
                playerBoxes[i].BringToFront();
            }

            for (int i = 0; i < AiPlayer.Hand.Count; i++)
            {
                Card c = AiPlayer.Hand[i];
                // use boxes[i] to access each picture box
                aiBoxes[i].Image = Image.FromFile(@"..\\..\\Resources\\" + (c.Visible ? c.ImagePath : "cardback.png"));
                aiBoxes[i].Visible = true;
                aiBoxes[i].BringToFront();
            }

            for (int i = 0; i < game.DealerCards.Count;i++)
            {
                Card c = game.DealerCards[i];
                dealerBoxes[i].Image = Image.FromFile(@"..\\..\\Resources\\" + (c.Visible ? c.ImagePath : "cardback.png"));
                dealerBoxes[i].Visible = true;
                dealerBoxes[i].BringToFront();
            }



           
           
            DealerCardValueLabel.Text=game.dealerscore.ToString();
     
            
            GuiPlayerCardValueLabel.Text = GuiPlayer.Score.ToString();
          AiPlayerCardValueLabel.Text = AiPlayer.Score.ToString();

            GuiPlayerStakeLabel.Text = "$"+GuiPlayer.Bet.ToString();
          AiPlayerStakeLabel.Text = "$"+AiPlayer.Bet.ToString();

          GuiPlayerCashLabel.Text = "$"+GuiPlayer.Cash.ToString();
            AiPlayerCashLabel.Text = "$"+AiPlayer.Cash.ToString();

            EndLabel.Text = GuiPlayer.WinStatus;
            EndLabel2.Text = AiPlayer.WinStatus;
        }

        private void BetText_TextChanged(object sender, EventArgs e)
        {
            BetButton.Enabled = betInputIsValid(BetText.Text);
            //BetButton.Enabled = true; //Keep this around for testing purposes or something idk lel
        }

        private bool betInputIsValid(string input)
        {
            try
            {
                int i = Int32.Parse(input);
                return i <= GuiPlayer.Cash;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        //Gui setting after each round is done
        public void GuiEndRound()
        {
            GuiPlayer.Active = false;
            game.DealerAct();
            Updatestorage();
        
            
            HitButton.Enabled = false;
            DoubleButton.Enabled = false;
            StopButton.Enabled = false;
            cip1.Enabled = true;
            cip25.Enabled = true;
            cip100.Enabled = true;
            BetButton.Enabled = true;

            game.endRound();
        }
        private void cip1_Click(object sender, EventArgs e)
        {
            cip1.Width = 1;
            cip1.Height = 1;
            if(string.IsNullOrWhiteSpace(BetText.Text))
            {
                BetText.Text = "1";
            }
            else { 
                int b = Convert.ToInt32(BetText.Text);
                b += 1;
                BetText.Text = b.ToString();

            }
            cip1.Width = 52;
            cip1.Height = 52;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            cip25.Width = 1;
            cip25.Height = 1;
            if (string.IsNullOrWhiteSpace(BetText.Text))
            {
                BetText.Text = "25";
            }
            else
            {
                int b = Convert.ToInt32(BetText.Text);
                b += 25;
                BetText.Text = b.ToString();

            }
            cip25.Width = 52;
            cip25.Height = 52;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            cip100.Width =1;
            cip100.Height=1;
            if (string.IsNullOrWhiteSpace(BetText.Text))
            {
                BetText.Text = "100";
            }
            else
            {
                int b = Convert.ToInt32(BetText.Text);
                b += 100;
                BetText.Text = b.ToString();

            }
            
            cip100.Width = 52;
            cip100.Height = 52;
        }

        private void DoubleButton_Click(object sender, EventArgs e)
        {
            GuiPlayer.Bet = game.doublebet(GuiPlayer.Bet);           
            game.CardToPlayer(GuiPlayer);
            AiPlay();
            Updatestorage();
            
        }

        private void Exit(object sender, FormClosingEventArgs e)
        {
            //game.submitChanges();
        }
        public void Updatestorage()
        {
            Console.WriteLine("updateingstorage from game engine");
            XmlStorage storage = new XmlStorage();
            storage.SaveGuiPlayer(GuiPlayer);
            storage.SaveAiPlayer(AiPlayer);
            game.Updatestorage();
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            game.RefreshGameData();
            UpdateUiElements();
        }   

        private void updateFromFile()
        {
           this.BeginInvoke((MethodInvoker)(() => game.RefreshGameData()));
         
           this.BeginInvoke((MethodInvoker)(() => UpdateUiElements()));
        }

        private void handleChange(object sender, FileSystemEventArgs e)
        {
           Thread.Sleep(100); 
           try
           {
              fsw.EnableRaisingEvents = false;
              updateFromFile();

           }

           finally
           {
                fsw.EnableRaisingEvents = true;
           }
        }


        private void StartFileWatcher()
        {
            fsw = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory, "*.bsf");
            fsw.NotifyFilter = NotifyFilters.LastWrite;
            fsw.Changed += new FileSystemEventHandler(handleChange);
            fsw.EnableRaisingEvents = true;
        }
    }


}

