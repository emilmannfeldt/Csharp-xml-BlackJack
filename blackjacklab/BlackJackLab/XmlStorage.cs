using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlackJackLab
{
    class XmlStorage
    {
        public XmlStorage() { }

        public void SaveGuiPlayer(IPlayer player)
        {
            Console.WriteLine("Start saving");
            XDocument document = new XDocument(
                       new XDeclaration("1.0", "utf-8", "yes"),
                       new XComment("Players"),
                   
                                 new XElement("Player",
                                     new XElement("Active", player.Active),
                                     new XElement("Score", player.Score),
                                     new XElement("Cash", player.Cash),
                                     new XElement("Bet", player.Bet),
                                     new XElement("WinStatus", player.WinStatus),

                                     new XElement("Hand", player.Hand.Select(Card =>
                                         new XElement("Card",
                                             new XElement("Color", Card.Color),
                                             new XElement("Face", Card.Face),
                                             new XElement("Visible", Card.Visible),
                                             new XElement("ImagePath", Card.ImagePath),
                                             new XElement("CardValue", Card.CardValue)
                                         )

                                     )
                                 )
                             )
                         
                         
                     );

            document.Save("GuiPlayer.bsf");

        }
        public void SaveAiPlayer(IPlayer player)
        {
            Console.WriteLine("Start saving");
            XDocument document = new XDocument(
                       new XDeclaration("1.0", "utf-8", "yes"),
                       new XComment("Players"),

                                 new XElement("Player",
                                     new XElement("Active", player.Active),
                                     new XElement("Score", player.Score),
                                     new XElement("Cash", player.Cash),
                                     new XElement("Bet", player.Bet),
                                     new XElement("WinStatus", player.WinStatus),

                                     new XElement("Hand", player.Hand.Select(Card =>
                                         new XElement("Card",
                                             new XElement("Color", Card.Color),
                                             new XElement("Face", Card.Face),
                                             new XElement("ImagePath", Card.ImagePath),
                                             new XElement("Visible", Card.Visible),
                                             new XElement("CardValue", Card.CardValue)
                                         )

                                     )
                                 )
                             )


                     );

            document.Save("AiPlayer.bsf");

        }





        public void SaveDeck(Stack<Card> deck)
        {

            XDocument document = new XDocument(
                       new XDeclaration("1.0", "utf-8", "yes"),
                       new XComment("deck"),
                       new XElement("cards",
                           deck.Select(card =>
                                 new XElement("card",
                                 new XElement("CardValue", card.CardValue),
                                 new XElement("Color", card.Color),
                                 new XElement("Face", card.Face),
                                 new XElement("ImagePath", card.ImagePath),
                                 new XElement("Visible", card.Visible)

                             )
                         )
                         )
                     );

            document.Save("Deck.bsf");

        }
        public void SaveDealer(List<Card> cards)
        {
            XDocument document = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),
                        new XComment("Dealer"),
                        new XElement("cards",
                            cards.Select(card =>
                                  new XElement("card",
                                  new XElement("CardValue", card.CardValue),
                                  new XElement("Color", card.Color),
                                  new XElement("Face", card.Face),
                                  new XElement("Visible", card.Visible),
                                  new XElement("ImagePath", card.ImagePath)

                              )
                          )
                          )
                      );

            document.Save("Dealer.bsf");

        }




              public  GuiPlayer LoadGuiPlayer()
        {
            //array with player and computer to return
            GuiPlayer player = new GuiPlayer();
                  
           

            string fileName = "GuiPlayer.bsf";
            XDocument doc = XDocument.Load(fileName);
            var data = from item in doc.Descendants("Player")
                       select new
                       {                        
                           Active = item.Element("Active").Value,
                           Score = item.Element("Score").Value,
                           Cash = item.Element("Cash").Value,
                           Bet = item.Element("Bet").Value,
                           WinStatus = item.Element("WinStatus").Value,
                           Hand = item.Element("Hand").Elements("Card").Select(Card => new Card
                                     {
                                         Color = Card.Element("Color").Value,
                                         Face = Card.Element("Face").Value,
                                         ImagePath = Card.Element("ImagePath").Value,
                                         CardValue = int.Parse(Card.Element("CardValue").Value),
                                         Visible = bool.Parse(Card.Element("Visible").Value)
                                     }).ToList<Card>()
                       };

            foreach (var p in data)
            {
                //first read players data
              
                    player.Active = bool.Parse(p.Active);
                    player.Score = Int32.Parse(p.Score);
                    player.Cash = Int32.Parse(p.Cash);
                    player.Bet = Int32.Parse(p.Bet);
                    player.WinStatus = (p.WinStatus);
                    List <Card> hand = new List<Card>();
                    
                    foreach (Card c in p.Hand)
                    {
                        hand.Add(c);                      
                    }
                    player.Hand=hand;
                }
            return player;   
        }



        public AiPlayer LoadAiPlayer()
              {
                  //array with player and computer to return
                  AiPlayer player = new AiPlayer();



                  string fileName = "AiPlayer.bsf";
                  XDocument doc = XDocument.Load(fileName);
                  var data = from item in doc.Descendants("Player")
                             select new
                             {
                                 Active = item.Element("Active").Value,
                                 Score = item.Element("Score").Value,
                                 Cash = item.Element("Cash").Value,
                                 Bet = item.Element("Bet").Value,
                                 WinStatus = item.Element("WinStatus").Value,
                                 Hand = item.Element("Hand").Elements("Card").Select(Card => new Card
                                 {
                                     Color = Card.Element("Color").Value,
                                     Face = Card.Element("Face").Value,
                                     ImagePath = Card.Element("ImagePath").Value,
                                     CardValue = int.Parse(Card.Element("CardValue").Value),
                                     Visible = bool.Parse(Card.Element("Visible").Value)
                                 }).ToList<Card>()
                             };

                  foreach (var p in data)
                  {
                      //first read players data

                      player.Active = bool.Parse(p.Active);
                      player.Score = Int32.Parse(p.Score);
                      player.Cash = Int32.Parse(p.Cash);
                      player.Bet = Int32.Parse(p.Bet);
                      player.WinStatus = (p.WinStatus);
                      List<Card> hand = new List<Card>();

                      foreach (Card c in p.Hand)
                      {
                          hand.Add(c);
                      }
                      player.Hand = hand;
                  }
                  return player;
              }



        public Deck loadDeckInUse()
        {
            //array with player and computer to return
            List<Card> cardsInDeck = new List<Card>();
            



            string fileName = "Deck.bsf";
            XDocument doc = XDocument.Load(fileName);
            var data = from item in doc.Descendants("card")
                       select new
                       {
                           Color = item.Element("Color").Value,
                           Face = item.Element("Face").Value,
                           ImagePath = item.Element("ImagePath").Value,
                           CardValue = int.Parse(item.Element("CardValue").Value),
                           Visible = bool.Parse(item.Element("Visible").Value)

                       };

          
                //first read players data           
                

                foreach (var c in data)
                {
                    Card card = new Card();

                    card.Color = (c.Color);
                    card.Face = (c.Face);
                    card.ImagePath = (c.ImagePath);
                    card.CardValue = (c.CardValue);
                    card.Visible = (c.Visible);

                    cardsInDeck.Add(card);
                }
                
            
            Deck deck = new Deck(cardsInDeck);
            return deck;
        }



        public List<Card> loadDealer()
        {
            //array with player and computer to return
            List<Card> dealercards = new List<Card>();




            string fileName = "Dealer.bsf";
            XDocument doc = XDocument.Load(fileName);
            var data = from item in doc.Descendants("card")
                       select new
                       {
                           Color = item.Element("Color").Value,
                           Face = item.Element("Face").Value,
                           ImagePath = item.Element("ImagePath").Value,
                           CardValue = int.Parse(item.Element("CardValue").Value),
                           Visible = bool.Parse(item.Element("Visible").Value)

                       };


            //first read players data           


            foreach (var c in data)
            {
                Card card = new Card();

                card.Color = (c.Color);
                card.Face = (c.Face);
                card.ImagePath = (c.ImagePath);
                card.CardValue = (c.CardValue);
                card.Visible = (c.Visible);

                dealercards.Add(card);
            }


            
            return dealercards;
        }




        }

       



   
    }

