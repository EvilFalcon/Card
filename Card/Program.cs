using System;
using System.Collections.Generic;
using System.Linq;

namespace Card
{
    internal class Program
    {
        static void Main()
        {
            BlackJack casino = new BlackJack();

            casino.StartNewRound();
            casino.PlayGame();
        }
    }

    class BlackJack
    {
        private int _decksCount;
        private Player _player = new Player();
        private Dealer _dealer = new Dealer();
        private Deck _deck = new Deck();
        private Dictionary<string, int> _blackJackPoints;
        private int _playerPositionLeft = 40;
        private int _dealerPositionLeft = 0;
        private int _maxPointCards = 21;

        public BlackJack()
        {
            _blackJackPoints = CreateBlackJackPoints();
        }

        public void PlayGame()
        {
            const string CommandGetCard = "1";
            const string CommandPassTheMove = "2";
            const string CommandExitGame = "3";

            int positionLeftMenu = 0;
            int positionTopMenu = 10;

            bool isWork = true;

            while (isWork)
            {
                bool isRunPlayer = false;

                Console.Clear();
                _player.ShowAllCardsInfo(_playerPositionLeft, _player.Name);
                _dealer.ShowCardDealerInfo(_dealerPositionLeft, _dealer.Name);
                Console.SetCursorPosition(positionLeftMenu, positionTopMenu);
                Console.WriteLine($"{CommandGetCard}) взять карту ");
                Console.WriteLine($"{CommandPassTheMove}) Передать ход");
                Console.WriteLine($"{CommandExitGame} выход из игры ");

                switch (Console.ReadLine())
                {
                    case CommandGetCard:
                        isRunPlayer = RunThePlayer();
                        break;

                    case CommandPassTheMove:
                        isRunPlayer = RunTheDealer();
                        break;

                    case CommandExitGame:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }

                if (isRunPlayer)
                {
                    AssignVictory();
                    StartNextRound();
                }

                Console.WriteLine("Нажмите любую кнопку для продолжения");
                Console.ReadLine();
            }
        }

        private bool RunThePlayer()
        {
            _player.GetCard(_deck.GiveCard());
            GetAllPoints(_player);

            if (_player.PointCards > _maxPointCards)
            {
                return true;
            }

            return false;
        }

        private void AssignVictory()
        {
            int positionTop = 5;
            int positionLeftInfoVictory = 1;
            int positopTopInfoVictory = 1;

            Console.Clear();
            _player.ShowAllCardsInfo(_playerPositionLeft, _player.Name, positionTop);
            _dealer.ShowAllCardsInfo(_dealerPositionLeft, _dealer.Name, positionTop);
            Console.SetCursorPosition(positionLeftInfoVictory, positopTopInfoVictory);

            if (_player.PointCards > _dealer.PointCards && _player.PointCards <= _maxPointCards)
            {
                Console.WriteLine("Победил игрок");
            }
            else if (_dealer.PointCards > _player.PointCards && _dealer.PointCards <= _maxPointCards)
            {
                Console.WriteLine(" Победило казино");
            }
            else if (_player.PointCards == _dealer.PointCards && _player.PointCards <= _maxPointCards)
            {
                Console.WriteLine("Ничья");
            }
        }

        public void StartNewRound()
        {
            SetDecksCount();
            GiveNewDecks();
            GiveStarterCardSet();
            GetAllPoints(_player);
            GetAllPoints(_dealer);
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

        private void GetAllPoints(Person person)
        {
            int points = 0;
            int cardsCount = person.CardsCount;
            bool isAce = false;

            for (int i = 0; i < cardsCount; i++)
            {
                string value = person.GetCardValue(i);

                if (value != "T")
                {
                    points += _blackJackPoints[value];
                }
                else if (value == "T" && isAce == false)
                {
                    points += _blackJackPoints[value];
                    isAce = true;
                }
                else if (value == "T" && isAce == true)
                {
                    points += 1;
                }
            }

            person.ScorePoints(points);
        }

        private bool RunTheDealer()
        {
            const int minThreshold = 17;

            while (_dealer.PointCards < minThreshold)
            {
                _dealer.GetCard(_deck.GiveCard());
                GetAllPoints(_dealer);
            }

            return true;
        }

        private void StartNextRound()
        {
            int ratio = 3;
            int lowerLimitDecks = _deck.GetCardsCount / ratio;

            if (_deck.GetCardsCount <= lowerLimitDecks)
            {
                GiveNewDecks();
            }

            _dealer.FoldCards();
            _player.FoldCards();

            GiveStarterCardSet();
        }

        private void GiveNewDecks()
        {
            _deck = new Deck(_decksCount);
        }

        private void GiveStarterCardSet()
        {
            int kitCard = 2;

            for (int i = 0; i < kitCard; i++)
            {
                _player.GetCard(_deck.GiveCard());
                _dealer.GetCard(_deck.GiveCard());
            }

            GetAllPoints(_player);
            GetAllPoints(_dealer);
        }

        private void SetDecksCount()
        {
            int tmpCount = GetDeckCount();
            _decksCount = tmpCount;
        }

        private int GetDeckCount()
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
                Console.Clear();
                ShowInfoDeckCount(tmpCount, inputKeyCommand, increaseKeyCommand, decreaseKeyCommand);

                ConsoleKeyInfo pressedKey = Console.ReadKey();

                if (decreaseKeyCommand == pressedKey.Key && tmpCount > minCount)
                {
                    tmpCount--;
                }
                else if (increaseKeyCommand == pressedKey.Key && tmpCount < maxCount)
                {
                    tmpCount++;
                }
                else if (inputKeyCommand == pressedKey.Key && minCount <= tmpCount && tmpCount <= maxCount)
                {
                    isSelectedGameMode = true;
                }
            }

            return tmpCount;
        }

        private void ShowInfoDeckCount(int count, ConsoleKey inputKey, ConsoleKey increaseKey, ConsoleKey decreaseKey)
        {

            int maxCountCard = _deck.GetCardsCountInOnePack * count;

            Console.WriteLine($"Управление количеством колод!\nУменьшить количество колод :{decreaseKey}\nУвеличить количество колод {increaseKey}\nПодтвердить выбор режима {increaseKey}");
            Console.WriteLine($"Режим с {maxCountCard} катры/ми, или {count}: колода/ми! ");
        }
    }

    class Deck
    {

        private int _decksCount;
        private List<Card> _cards = new List<Card>();
        private List<string> _cardsValue = new List<string> { "T", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "D", "K" };
        private List<string> _cardsSuit = new List<string> { "♣", "♠", "♥", "♦" };

        public Deck(int deckCount = 1)
        {
            _decksCount = deckCount;
            Fill();
            Shuffle();
        }

        public int GetCardsCountInOnePack => _cardsValue.Count * _cardsSuit.Count;

        public int GetCardsCount => _cards.Count();

        public Card GiveCard()
        {
            if (GetCardsCount != 0)
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
            for (int k = 0; k < _decksCount; k++)
            {
                CreateDeck();
            }
        }

        private void CreateDeck()
        {
            for (int i = 0; i < _cardsValue.Count; i++)
            {
                for (int j = 0; j < _cardsSuit.Count; j++)
                {
                    Card card = new Card(_cardsSuit[j], _cardsValue[i]);
                    _cards.Add(card);
                }
            }
        }

        private void Shuffle()
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

        public string CardValue()
        {
            string value = _value;
            return value;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"|{_suit}{_value}|");
        }
    }

    abstract class Person
    {
        private int _pointCards = 0;

        private List<Card> _cardsOfHand = new List<Card>();

        public int CardsCount => _cardsOfHand.Count;

        private bool HaveNotCards
        {
            get
            {
                return _cardsOfHand.Count() == 0;
            }
        }

        public int PointCards => _pointCards;

        public void ScorePoints(int pointsCards)
        {
            _pointCards = pointsCards;
        }

        public void GetCard(Card card)
        {
            Card temporaryCard = card;
            if (temporaryCard != null)
            {
                _cardsOfHand.Add(temporaryCard);
            }
        }

        public string GetCardValue(int index)
        {
            string valueCardOfHand = _cardsOfHand[index].CardValue();
            return valueCardOfHand;
        }

        public void FoldCards()
        {
            if (_cardsOfHand.Count != 0)
            {
                _cardsOfHand.Clear();
            }
            else
            {
                Console.WriteLine("У вас нет кард");
            }
        }

        public void ShowAllCardsInfo(int positionLeft, string name, int positionTopInfoCard=1)
        {

            if (HaveNotCards == false)
            {
                int positionInfoCard = 3+name.Length;
                int positionString = 1;
                Console.SetCursorPosition(positionLeft, positionTopInfoCard - positionString);
                Console.WriteLine($"|{name}|карты|Кол/во очков: {_pointCards}|");
               

                foreach (var card in _cardsOfHand)
                {
                    Console.SetCursorPosition(positionLeft + positionInfoCard, positionTopInfoCard);
                    card.ShowInfo();
                    positionTopInfoCard++;
                }

                Console.SetCursorPosition(0, 0);
            }
        }

        public void ShowCardDealerInfo(int positionLeft, string name)
        {
            if (HaveNotCards == false)
            {
                Console.WriteLine($"|Карты {name}|");
                _cardsOfHand[0].ShowInfo();
            }

            Console.SetCursorPosition(0, 0);
        }
    }

    class Player : Person
    {
        private string _name;

        public Player()
        {
            Console.WriteLine("Введите ваше имя ");
            _name = Console.ReadLine();
        }

        public string Name => _name;
    }

    class Dealer : Person
    {
        private string name = "Dealer";

        public string Name => name;
    }
}



//////if (temporaryCard.Value == "T")
//сделать  начисление очков [ok]
//добавить условия победы, поражения и сделать ничью[ok]
//добавить ставки и сделать страховку []
//сделать условие хода дилера[ok] 
//////
