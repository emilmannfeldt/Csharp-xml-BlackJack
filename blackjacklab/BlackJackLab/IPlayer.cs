using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackLab
{
    interface IPlayer
    {
        void makeBet(int betValue);
        void AddCard(Card card);
        void EmptyHand();
        string Act(int b,List<Card> g, int d);
        List<Card> Hand
        {
            get;
            set;
        }

        int Bet
        {
            get;
            set;
        }

        int Cash
        {
            get; //holla holla get dolla
            set;
        }

        int Score //We're never gonna
        {
            get;
            set;
        }
        string WinStatus
        {
            get;
            set;
        }
        bool Active
        {
            get;
            set;
        }

       
    }
}
