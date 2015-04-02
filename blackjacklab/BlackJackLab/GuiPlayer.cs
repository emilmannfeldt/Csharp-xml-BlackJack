using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJackLab
{
    class GuiPlayer: IPlayer
    {
        private List<Card> hand;
        private int cash;
        private int bet;
        private int score;
        private bool active;
        private string winstatus;

      
        public GuiPlayer(){

    }

        public GuiPlayer(bool _active,int _score, int _cash, int _bet, string _winstatus, List<Card> _hand)
        {
            active = _active;
            score = _score;
            cash = _cash;
            bet = _bet;
            winstatus = _winstatus;
            hand = _hand;
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        public void EmptyHand()
        {
            hand = new List<Card>();
        }

        public string Act(int b, List<Card> f, int c)
        {
            return "testAct";
        }

        public List<Card> Hand
        {
            get { return hand; }
            set { hand = value; }
        }

        public int Cash
        {
            get { return cash; }
            set { cash = value; }
        }

        public int Score
        {
            get
            {
                int i = 0;
                int aces = 0;
                foreach (Card c in hand)
                {
                    i += c.CardValue;
                   if (c.Face.Equals("Ace")) { aces++; }
                }
                while (i > 21 && aces > 0)
                {
                    i -= 10;
                    aces--;
                }
                return i;
            }
            set
            {
                score = value;
            }
        }

        public string WinStatus
        {
            get { return winstatus; }
            set { winstatus = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public void makeBet(int betValue)
        {
            Console.Write("Yay");
           
        }

        public int Bet
        {
            get 
            {
              
                return bet;
            }
            set { bet = value; }
        }

    }
}
