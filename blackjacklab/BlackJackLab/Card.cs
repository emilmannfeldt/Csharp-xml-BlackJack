using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BlackJackLab
{
    partial class Card
    {
        private string color;
        private string face;
        private string imagepath;
        private bool visible;
        private int cardValue;
            public Card(){

    }
     

        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                List<string> allowedValues = new List<String> { "Hearts", "Spades", "Clubs", "Diamonds" };
                if (allowedValues.Contains(value, StringComparer.OrdinalIgnoreCase))
                {
                    color = value;
                }
                else
                {
                    color = "none";
                }
            }
        }


        public string Face
        {
            get
            {
                return face;
            }
            set
            {
                List<string> allowedValues = new List<String> { "Jack", "Queen", "King", "Ace" }; //Has not been tested against false input, as the "else" clause is not implemented.
                if (allowedValues.Contains(value, StringComparer.OrdinalIgnoreCase))
                {
                    face = value;
                }
                else
                {
                    face = " ";
                }
            }
        }
        public int CardValue
        {
            get
            {
                return cardValue;
            }
            set
            {
                if (value < 1 || value > 11)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    cardValue = value;
                }
            }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public Card(int _value, string _color, string _face, string _img, bool _visible)
        {
            try
            {
                CardValue = _value;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Value out of range, must be >0<12. Input was: " + _value);
            }
            color = _color; //Lägg in kontrollmetod för Hearts/Clubs/Spades/Diamonds
            face = _face;
            visible = _visible;
            imagepath = _img;
            //          image = getImageByPath(value, face); -Ej implementerat
        }
        public override string ToString()
        {
            if ((bool)visible)
            {
                if (face == " ") //for number cards
                {
                    return (CardValue + " of " + Color);
                }
                else //for cards with face
                {
                    return (face + " of " + Color);
                }
            }
            else
            {
                return "It's a secret!";
            }
        }
        public string ImagePath
        {
            get
            {
                 return imagepath;
            }
            set
            {
                
                imagepath = value;
            }
        }
    }
}