using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Casino casino = new Casino();

            casino.PlayGame();
        }
    }

    class Casino
    {
        private Player _player = new Player();
        public void PlayGame()
        {
            const string CommandGetCard = "1";
            const string CommandShowInfoCard = "2";
            const string CommandExitGame = "3";

            bool isWork = true;

            Console.WriteLine($"{CommandGetCard} взять карту ");
            Console.WriteLine($"{CommandShowInfoCard} посмотреть какие карты в руке");
            Console.WriteLine($"{CommandExitGame} выход из игры ");

            while (isWork)
            {
                switch (Console.ReadLine())
                {
                    case CommandGetCard:
                        _player.GetCard();
                        break;

                    case CommandShowInfoCard:
                        _player.ShowInfoCard();
                        break;

                    case CommandExitGame:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("");
                        break;
                }
            }
        }
    }
    class CardDeck
    {
        private static Random random = new Random();

        private List<Card> _deck = new List<Card>();

        public Card GiveCard()
        {
            Card temporaryCard = _deck.Last();
            _deck.Remove(temporaryCard);
            return temporaryCard;
        }

        public CardDeck()
        {
            FillDeck();
        }

        private void FillDeck()
        {
            List<string> cardsValue = new List<string> { "T", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "D", "K" };
            List<string> cardsSuit = new List<string> { "♣", "♠", "♥", "♦" };

            for (int i = 0; i < cardsValue.Count; i++)
            {
                for (int j = 0; j < cardsSuit.Count; j++)
                {
                    Card card = new Card(cardsSuit[j], cardsValue[i]);
                    _deck.Add(card);
                }
            }

            Shuffle(_deck);
        }

        public void Shuffle(List<Card> cards)
        {
            for (int i = 0; i < _deck.Count; i++)
            {
                int indexCard = random.Next(_deck.Count);
                (_deck[i], _deck[indexCard]) = (_deck[indexCard], _deck[i]);

            }
        }
    }

    class Card
    {
        private string _cardSuit;
        private string _cardValue;

        public Card(string cardSuit, string cardValue)
        {
            _cardSuit = cardSuit;
            _cardValue = cardValue;
        }

        public void ShowInfo() 
        {
            Console.WriteLine($"Масть карты {_cardSuit}, значение карты {_cardValue}");
        }
    }

    class Player
    {
        private List<Card> _hand = new List<Card>();
        private CardDeck _deck = new CardDeck();

        public void GetCard()
        {
            _hand.Add(_deck.GiveCard());
        }

        public void ShowInfoCard()
        {
            foreach (Card card in _hand)
            {
                card.ShowInfo();
            }
        }
    }
}