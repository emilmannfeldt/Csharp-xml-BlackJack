using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJackLab
{
    class GameEngine
    {
        
        private Deck deckInUse;
        public List<Card> dealerCards;
        private List<IPlayer> players;
        public int dealerscore = 0;
        private const int playerCount = 2;

      

        public GameEngine(string state)
        {
            initialize(state);            
        }
        public void initialize(string state)
        { 
            //Here is where all the fun happens
           

            
            players = new List<IPlayer>();


            //this if start new game
            if (state=="new")
            {
                players.Add(new GuiPlayer());
                players.Add(new AiPlayer());
                deckInUse = new Deck(1);
                dealerCards = new List<Card>();

                foreach (IPlayer p in players)
                {
                    p.Cash = 10000;
                    p.Hand = new List<Card>();
                } 
            }
            else 
            {
                //this if load last played game
                RefreshGameData();
            }
        
             
        }

       
        public bool StartRound(int b, int b2)
        {
            /*
            GuiPlayer checkGp = (GuiPlayer)players[0];
            if (checkGp.Cash < b)
            {
                return false;
            }
            **/
            dealerCards.Clear();
            players[0].Bet = b;
            players[1].Bet = b2;
            foreach (IPlayer p in players)
            {
                p.EmptyHand();
            }
            for (int dealtCards = 0; dealtCards < 2; dealtCards++)
            {
                foreach (IPlayer p in players)
                {
                    p.Active = true;
                    p.WinStatus = "";
                    
                    CardToPlayer(p);
                }
                CardToDealer();
                
            }
            DealerCount();
            return true;
        }

        public bool CardToPlayer(IPlayer p)
        {
            p.AddCard(deckInUse.pickCard());
            checkCardValue(p);
            return p.Active;
        }
        public void CardToDealer()
        {
            dealerCards.Add(deckInUse.pickCard());
         
            if (dealerCards.Count == 2)
            {
                dealerCards.ElementAt(1).Visible = false;
            }
         
        }

        private void checkCardValue(IPlayer currentPlayer)
        {
            if (currentPlayer.Score == 21 && currentPlayer.Hand.Count.Equals(2))
            {
                currentPlayer.Active = false;
                currentPlayer.WinStatus = "Blackjack";
            }
            else if(currentPlayer.Score == 21)
            {
                currentPlayer.Active = false;
            }
            else if (currentPlayer.Score > 21)
            {
                currentPlayer.Active = false;
                currentPlayer.WinStatus = "Busted";
            }
        }
        public List<Card> DealerCards
        {
            get { return dealerCards; }
        }
        public Deck CurrentDeck
        {
            get { return deckInUse; }
        }
        public int ActivePlayers
        {
            get
            {
                int i = 0;
                foreach(IPlayer p in players)
                {
                    if(p.Active) {i++;}
                }
                return i;
            }
        }

        public void endRound()
        {
            
            //sets winstatus for every player
            foreach(IPlayer p in players)
            {
                if (!p.WinStatus.Equals("Blackjack") && !p.WinStatus.Equals("Busted"))
                {
                    if (p.Score > dealerscore || dealerscore>21)
                    {
                        p.WinStatus = "Win";
                    }
                    else
                    {
                        p.WinStatus = "Lose";
                    }

                }
                //counts win/loss
                CountCash(p);
            
            }
            Updatestorage();
        }

        //counts win/loss
        private void CountCash(IPlayer p)
        {
            if (p.WinStatus == "Blackjack")
            {
                p.Cash += p.Bet + p.Bet/2;
            }
         
            if (p.WinStatus == "Win")
            {
                p.Cash += p.Bet;
            }
            else
            {
                p.Cash -= p.Bet; 
            }
        }
        public IPlayer getPlayer(int index)
        {
            return players.ElementAt(index);
        }



        public void DealerAct()
        {
            
            foreach (Card c in dealerCards)
            {
                c.Visible = true;//shows the hidden card
            }
       
            DealerCount();                          
            while (dealerscore<17)    //draws a new card until 17 or more
            {
                CardToDealer();
                DealerCount();
            }
        }

        //counts dealer score
        public void DealerCount()
        {
            dealerscore = 0;
            int aces = 0;
            foreach (Card c in dealerCards)
            {
                if (c.Visible==true)
                {
                    dealerscore += c.CardValue;
                    if (c.Face.Equals("Ace")) { aces++; }
                }     
            }
            while (dealerscore > 21 && aces > 0)
            {
                dealerscore -= 10;
                aces--;
            }
        }
        public int doublebet (int b)
        {
             return b + b;
        }

        //calls for funtions in xmlstorage and stores the objects

        public void Updatestorage()
        {
            Console.WriteLine("updateingstorage from game engine");
            XmlStorage storage = new XmlStorage();
        
            storage.SaveDeck(deckInUse.Cards);
            storage.SaveDealer(dealerCards);
        }

        private IPlayer LoadGuiPlayer()
        {
            
            XmlStorage storage = new XmlStorage();

            
            GuiPlayer guiplayer = storage.LoadGuiPlayer();
            return guiplayer;
        }
        
        private IPlayer LoadAiPlayer()
        {
            XmlStorage storage = new XmlStorage();
           
            AiPlayer aiplayer = storage.LoadAiPlayer();
            return aiplayer;
        }
        private Deck loadDeck()
        {
            XmlStorage storage = new XmlStorage();

            Deck deck = storage.loadDeckInUse();
            return deck;
        }
        private List<Card> loadDealer()
        {
            XmlStorage storage = new XmlStorage();

            List<Card> cards = storage.loadDealer();
            return cards;
        }

        public void RefreshGameData()
        {
            deckInUse = loadDeck();
            dealerCards = loadDealer();
            players.Clear();
            players.Add(LoadGuiPlayer());
            players.Add(LoadAiPlayer());
        }

    }
       
}
