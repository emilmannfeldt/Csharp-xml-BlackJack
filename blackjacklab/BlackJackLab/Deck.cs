using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BlackJackLab.Properties;

namespace BlackJackLab
{
    class Deck
    {
        private Stack<Card> cards;

        public Stack<Card> Cards
        {
            get { return cards; }
        }
        
        public Deck(int numberOfDecks)
        {
            cards = new Stack<Card>();
            initialize(numberOfDecks);
        }

        public Deck(List<Card> listOfCards)
        {
            cards = new Stack<Card>(listOfCards);
        }
        
        private void initialize(int numberOfDecks)
        {
            for(int i = 0; i<numberOfDecks;i++)
            {
                addFullDeck();
            }
            Shuffle();
        }

        private void Shuffle()
        {
            // Seeds a Random object with the current unix time, making it sufficiently unpredictable for the purpose. Then uses OrderBy to reshuffle the deck.
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Card[] deck = cards.ToArray();

            for (int n = deck.Length - 1; n > 0; --n)
            {
                int k = rnd.Next(n + 1);
                Card temp = deck[n];
                deck[n] = deck[k];
                deck[k] = temp;
            }
            cards.Clear();
            cards = new Stack<Card>(deck);
        }
        

        private void addFullDeck()
        {
            string[] colors = new string[] {"Hearts", "Spades", "Diamonds", "Clubs"};
            string[] faces = new string[] { "Jack", "Queen", "King", "Ace" }; //Dummied out until we implement proper face card handling
            foreach(String color in colors)
            {
                for(int i = 2;i <= 10; i++)
                {
                    string path = i + "_of_" + color;
                    cards.Push(new Card(i, color," ", i+"_of_"+color+".png",true));
                }
                foreach(string face in faces) 
                {
                    if(face=="Ace") //for aces
                    {
                        cards.Push(new Card(11, color, face, face + "_of_" + color + "2.png", true)); 
                    }
                    else //for face cards
                    {
                        cards.Push(new Card(10, color, face, face + "_of_" + color + "2.png", true));
                    }
                    
                }
            }
            
        }

        public Card pickCard()//Of the USS Enterprise
        {
            if(cards.Count < 6)
            {
                initialize(1);
            }
            return cards.Pop(); //Required addition: Check whether cards are available and handle if they are not.
        }
        
    }
}
