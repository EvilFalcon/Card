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

            casino.StartNewRound();
            casino.PlayGame();
        }
    }

    class Casino
    {
        private int _deckCount;
        private Player _player = new Player();
        private Dealer _dealer = new Dealer();
        private Deck _deck;
        private Dictionary<string, int> _blackJackPoints;

        public Casino()
        {
            _blackJackPoints = CreateBlackJackPoints();
        }

        public void PlayGame()
        {
            const string CommandGetCard = "1";
            const string CommandShowInfoCard = "2";
            const string CommandExitGame = "3";

            bool isWork = true;


            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"{CommandGetCard} взять карту ");
                Console.WriteLine($"{CommandShowInfoCard} посмотреть какие карты в руке");
                Console.WriteLine($"{CommandExitGame} выход из игры ");

                switch (Console.ReadLine())
                {
                    case CommandGetCard:
                        _player.GetCard(_deck.GiveCard());
                        break;

                    case CommandShowInfoCard:
                        _player.ShowCardsPlayerInfo();
                        break;

                    case CommandExitGame:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }

                WalksTheDealer();
            }
        }

        public void StartNewRound()
        {
            SetDecksCount();

            GiveNewDecks();

            GiveStarterCardSet();
        }

        private Dictionary<string, int> CreateBlackJackPoints()
        {
            Dictionary<string, int> blackJackPoints = new Dictionary<string, int>
            {
                ["T"] = 11,
                ["2"] = 2,
                ["3"] = 3,
                ["4"] = 4,
                ["5"] = 5,
                ["6"] = 6,
                ["7"] = 7,
                ["8"] = 8,
                ["9"] = 9,
                ["10"] = 10,
                ["J"] = 10,
                ["D"] = 10,
                ["K"] = 10

            };

            return blackJackPoints;
        }

        private void WalksTheDealer()
        {
            _dealer.ShowCardDealerInfo();

            if (_dealer.PointCards <= 16)
            {
                _dealer.GetCard(_deck.GiveCard());
            }

            Console.WriteLine("Нажмите любую кнопку для продолжения");
            Console.ReadLine();
        }

        private void StartNextRound()
        {
            int lowerLimitDecks = _deck.GetCardsCount / 3;

            if (_deck.GetCardsCount <= lowerLimitDecks)
            {
                _player.ReturnCards();
                GiveNewDecks();

            }
        }

        private void GivePoints()
        {

        }

        private void GiveNewDecks()
        {
            for (int i = 0; i < _deckCount; i++)
            {
                _deck = new Deck();
            }
        }

        private void GiveStarterCardSet()
        {
            int kitCard = 2;

            for (int i = kitCard; i == 0; i--)
            {
                _player.GetCard(_deck.GiveCard());
            }
        }

        private void SetDecksCount()
        {

            int tmpCount = GetDeckCount();

            _deckCount = tmpCount;
        }

        private static int GetDeckCount()
        {
            ConsoleKey decreaseKeyCommand = ConsoleKey.LeftArrow;
            ConsoleKey increaseKeyCommand = ConsoleKey.RightArrow;
            ConsoleKey inputKeyCommand = ConsoleKey.Enter;

            int maxCount = 8;
            int minCount = 1;
            int tmpCount = 1;
            bool isSelectedGameMode = false;

            while (isSelectedGameMode == false)
            {
                ShowInfoDeckCount(tmpCount, inputKeyCommand, increaseKeyCommand, decreaseKeyCommand);

                ConsoleKeyInfo pressedKey = Console.ReadKey();

                if (decreaseKeyCommand == pressedKey.Key && tmpCount < minCount)
                {
                    tmpCount--;
                }
                else if (increaseKeyCommand == pressedKey.Key && tmpCount > maxCount)
                {
                    tmpCount++;
                }
                else if (inputKeyCommand == pressedKey.Key && minCount <= tmpCount || tmpCount <= maxCount)
                {
                    isSelectedGameMode = true;
                }

            }

            return tmpCount;
        }

        private static void ShowInfoDeckCount(int count, ConsoleKey inputKey, ConsoleKey increaseKey, ConsoleKey decreaseKey)
        {
            int maxCountCard = 52 * count;

            Console.WriteLine($"Управление количеством колод!\nУменьшить количество колод :{decreaseKey}\nУвеличить количество колод {increaseKey}\nПодтвердить выбор режима {increaseKey}");
            Console.WriteLine($"Режим с {maxCountCard} катры/ми, или {count}: колода/ми! ");


        }

    }

    class Deck
    {
        private List<string> _cardsValue = new List<string> { "T", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "D", "K" };
        private List<string> _cardsSuit = new List<string> { "♣", "♠", "♥", "♦" };

        private List<Card> _cards = new List<Card>();

        public Deck()
        {
            Fill();
        }

        public List<string> CardsValue { get; private set; }

        public int GetCardsCount => _cards.Count();

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

        private void Fill()
        {

            for (int i = 0; i < _cardsValue.Count; i++)
            {
                for (int j = 0; j < _cardsSuit.Count; j++)
                {
                    Card card = new Card(_cardsSuit[j], _cardsValue[i]);
                    _cards.Add(card);
                }
            }

            Shuffle();
        }

        public void Shuffle()
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

        public string Value => _value;

        public void ShowInfo()
        {

            Console.WriteLine($"  {_suit}         |       {_value}       ");
        }
    }

    abstract class Person
    {
        private int _pointCards = 0;

        private List<Card> _hand = new List<Card>();

        public bool HaveNotCards
        {
            get
            {
                return _hand.Count() == 0;
            }
        }

        public int PointCards => _pointCards;

        public void ScorePoints(int pointsCards)
        {
            _pointCards += pointsCards;
        }

        public void GetCard(Card card)
        {
            Card temporaryCard = card;
            if (temporaryCard != null)
            {
                _hand.Add(temporaryCard);
            }
        }

        public void ReturnCards()
        {
            if (_hand.Count != 0)
            {
                _hand.Clear();
            }
            else
            {
                Console.WriteLine("У вас нет кард");
            }
        }

        public void ShowCardsPlayerInfo()
        {
            if (HaveNotCards == false)
            {
                Console.WriteLine("Масть карты  |Значение карты");

                foreach (var card in _hand)
                {
                    card.ShowInfo();
                }
            }
        }

        public void ShowCardDealerInfo()
        {
            if (HaveNotCards==false)
            {
                Console.WriteLine("Масть карты   |Значение карты");

                _hand[0].ShowInfo();
            }
        }


    }

    class Player : Person
    {

    }

    class Dealer : Person
    {

    }
}
//////if (temporaryCard.Value == "T")
//сделать  начисление очков 
//добавить условия победы, поражения и сделать ничью
//добавить ставки и сделать страховку 
//сделать условие хода дилера 
/////