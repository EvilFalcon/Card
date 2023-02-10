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
        private int _deckCount;
        private Player _player = new Player();
        private Dealer _dealer = new Dealer();
        private Deck _deck;
        private Dictionary<string, int> _blackJackPoints;

        public BlackJack()
        {
            _blackJackPoints = CreateBlackJackPoints();
        }

        public void PlayGame()
        {
            const string CommandGetCard = "1";
            const string CommandPassTheMove = "2";
            const string CommandExitGame = "4";

            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                _player.ShowCardsPlayerInfo();
                _dealer.ShowCardDealerInfo();
                Console.WriteLine($"{CommandGetCard} взять карту ");
                Console.WriteLine($"{CommandPassTheMove} Передать ход");
                Console.WriteLine($"{CommandExitGame} выход из игры ");

                switch (Console.ReadLine())
                {
                    case CommandGetCard:
                        RunThePlayer();
                        break;

                    case CommandPassTheMove:
                        RunTheDealer();
                        break;

                    case CommandExitGame:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }


                if (AssignVictory())
                {
                StartNextRound();

                }
                Console.WriteLine("Нажмите любую кнопку для продолжения");
                Console.ReadLine();
            }
        }

        private void RunThePlayer()
        {
            _player.GetCard(_deck.GiveCard());
            GetAllPoints(_player);
        }

        private bool AssignVictory()
        {

            return;
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

        private void RunTheDealer()
        {
            _dealer.ShowCardDealerInfo();

            const int minThreshold = 17;

            while (_dealer.PointCards < minThreshold)
            {

                _dealer.GetCard(_deck.GiveCard());
                GetAllPoints(_dealer);
            }

        }

        private void StartNextRound()
        {
            int lowerLimitDecks = _deck.GetCardsCount / 3;

            if (_deck.GetCardsCount <= lowerLimitDecks)
            {
                _dealer.FoldCards();
                _player.FoldCards();
                GiveNewDecks();

            }
        }

        private void GivePoints()
        {

        }

        private void GiveNewDecks()
        {
            _deck = new Deck(_deckCount);

        }

        private void GiveStarterCardSet()
        {
            int kitCard = 2;

            for (int i = 0; i < kitCard; i++)
            {
                _player.GetCard(_deck.GiveCard());
                _dealer.GetCard(_deck.GiveCard());
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
        private int _deckCount;

        private List<Card> _cards = new List<Card>();

        public Deck(int deckCount)
        {
            _deckCount = deckCount;
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

            for (int k = 0; k < _deckCount; k++)
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

            Shuffle();
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

            Console.WriteLine($"|  {_suit}          |       {_value}      |");
        }
    }

    abstract class Person
    {

        private int _pointCards = 0;

        private List<Card> _cardsOfHand = new List<Card>();

        public int CardsCount => _cardsOfHand.Count;

        public bool HaveNotCards    ///надо оправдать
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

        public void ShowCardsPlayerInfo()
        {
            if (HaveNotCards == false)
            {
                Console.SetCursorPosition(40, 1);
                Console.WriteLine(_pointCards);
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("|Масть карты  |Значение карты|Количество очков|");

                foreach (var card in _cardsOfHand)
                {
                    card.ShowInfo();
                }
            }
        }

        public void ShowCardDealerInfo()
        {
            if (HaveNotCards == false)
            {
                Console.WriteLine("Масть карты   |Значение карты");

                _cardsOfHand[0].ShowInfo();
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
//сделать  начисление очков [ok]
//добавить условия победы, поражения и сделать ничью
//добавить ставки и сделать страховку 
//сделать условие хода дилера[ok] 
//////
