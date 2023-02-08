using System;
using System.Collections.Generic;
using System.Linq;  

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
        private Deck _deck = new Deck();
        public void PlayGame()
        {
            const string CommandGetCard = "1";
            const string CommandShowInfoCard = "2";
            const string CommandTakeCard = "3";
            const string CommandExitGame = "4";

            bool isWork = true;


            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"{CommandGetCard} взять карту ");
                Console.WriteLine($"{CommandShowInfoCard} посмотреть какие карты в руке");
                Console.WriteLine($"{CommandTakeCard} вернуть карты в колоду");
                Console.WriteLine($"{CommandExitGame} выход из игры ");

                switch (Console.ReadLine())
                {
                    case CommandGetCard:
                        _player.GetCard();
                        break;

                    case CommandShowInfoCard:
                        _player.ShowInfoCard();
                        break;

                    case CommandTakeCard:
                        _player.ReturnCards();
                        break;

                    case CommandExitGame:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }

                Console.WriteLine("Нажмите любую кнопку для продолжения");
                Console.ReadLine();
            }
        }
    }

    class Deck
    {
       

        private List<Card> _cards = new List<Card>();

        public Deck()
        {
            FillDeck();
        }

        public Card GiveCard()
        {
            if (_cards.Count != 0)
            {
                Card temporaryCard = _cards.Last();
                _cards.Remove(temporaryCard);
                return temporaryCard;
            }
            else
            {
                Console.WriteLine("карты закончились в колоде! верните карты в колоду ");
                return null;
            }
        }

        public void TakeCards(Card card)
        {
            _cards.Add(card);
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
                    _cards.Add(card);
                }
            }

            Shuffle(_cards);
        }

        private void Shuffle(List<Card> cards)
        {
            Random random = new Random();

            for (int i = 0; i < _cards.Count; i++)
            {
                int indexCard = random.Next(_cards.Count);
                (_cards[i], _cards[indexCard]) = (_cards[indexCard], _cards[i]);
            }
        }
    }

    class Card
    {
        private string _suit;
        private string _value;

        public Card(string suit, string value)
        {
            _suit = suit;
            _value = value;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Масть карты {_suit}, значение карты {_value}");
        }
    }

    class Player
    {
        private List<Card> _hand = new List<Card>();
        private Deck _deck = new Deck();

        public void GetCard()
        {
            Card temporaryCard = _deck.GiveCard();
            if (temporaryCard != null)
            {
                _hand.Add(temporaryCard);
            }
        }

        public void ReturnCards()
        {
            Card[] cards = new Card[_hand.Count];

            if (_hand.Count != 0)
            {
                foreach (Card card in _hand)
                {
                    _deck.TakeCards(card);

                }

                for (int i = 0; i < _hand.Count; i++)
                {
                    _hand.RemoveAt(i);
                }
            }
            else
            {
                Console.WriteLine("у вас нет кард");
            }
        }

        public void ShowInfoCard()
        {
            foreach (var card in _hand)
            {
                card.ShowInfo();
            }
        }
    }
}