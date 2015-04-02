using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackLab
{
    class AiPlayer: IPlayer
    {
        private List<Card> hand;
        private int cash;
        private int bet;
        private int score;
        private bool active;
        private string winstatus;

        public AiPlayer():base()
        {
            hand = new List<Card>();
        }

        public void AddCard(Card card)
        {
            hand.Add(card);   
        }

        public void EmptyHand()
        {
            hand = new List<Card>();
        }

        public string Act(int score, List<Card> cards, int dealerscore)
        { //make smarter
            if (Score > 16)
            {
                return "stop";
            }
            else if(Score==11 || Score==10)
            {
                if((cards.Count == 2) && dealerscore!=11)
                {
                    return "double";
                }
                else
                {
                    return "hit";
                }
            }
            else if(score>11)
            {
                if(dealerscore <=6)
                {
                    return "stop";
                }
                return "hit";
            }
            else
            {
                return "hit";
            }
        }

        public List<Card> Hand
        {
            get { return hand; }
            set { hand = value; }
        }

        public int Cash
        {
            get { return (int)cash; }
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
            set{winstatus = value;}
        }

        public bool Active
        {
            get{return active;}
            set{active = value;}
        }

        public int Bet
        {
            get
            {
               
                return bet;
            }
            set { bet = value;}
        }

        public void makeBet(int otherBet)
        {
            if (otherBet > 100)
            {
                bet = otherBet - 50;
            }
            else if (otherBet<100)
            {
                bet = otherBet + 50;
            }
            else
            {
                bet = otherBet - 10;
            }

        }
    }
}
