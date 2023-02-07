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
    class CardDeck
    {
        private static Random random = new Random();

        private List<Card> _deck = new List<Card>();

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

        public Card GiveCard()
        {
            if (_deck.Count != 0)
            {
                Card temporaryCard = _deck.Last();
                _deck.Remove(temporaryCard);
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
            _deck.Add(card);
        }

        private void Shuffle(List<Card> cards)
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