using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyTrivia
{
    public class Game
    {
        private List<string> players = new List<string>(); // change to private fields
        private int[] places = new int[6];
        private int[] purses = new int[6];
        private bool[] inPenaltyBox = new bool[6];

        private Dictionary<string, LinkedList<string>> questionCategories = new Dictionary<string, LinkedList<string>>(); //Dictionary, introduce new categories of questions and ensure the distribution is correct

        int currentPlayer = 0;
        bool isGettingOutOfPenaltyBox;

        public Game()
        {
            InitializeQuestionCategories(); // call a method instead
        }

        private void InitializeQuestionCategories() // method for initializing
        {
            questionCategories["Pop"] = new LinkedList<string>();
            questionCategories["Science"] = new LinkedList<string>();
            questionCategories["Sports"] = new LinkedList<string>();
            questionCategories["Rock"] = new LinkedList<string>();

            for (int i = 0; i < 50; i++)
            {
                questionCategories["Pop"].AddLast("Pop Question " + i);
                questionCategories["Science"].AddLast("Science Question " + i);
                questionCategories["Sports"].AddLast("Sports Question " + i);
                questionCategories["Rock"].AddLast("Rock Question " + i);
            }
        }

        public bool isPlayable() // add a condition to check if there are any questions remaining in the questionCategories
        {
            return HowManyPlayers() >= 2 && questionCategories.Any(category => category.Value.Any());
        }

        public bool AddPlayer(string playerName) //use appropriate method name, check for player count
        {
            if (players.Count >= 6) // dont allow more than 6
            {
                Console.WriteLine("Cannot add more than six players.");
                return false;
            }

            players.Add(playerName);
            places[HowManyPlayers()] = 0;
            purses[HowManyPlayers()] = 0;
            inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        public int HowManyPlayers() // method should start with capital
        {
            return players.Count;
        }

        public void Roll(int roll) // method should start with capital
        {
            Console.WriteLine(players[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0) 
                {
                    isGettingOutOfPenaltyBox = true; // player rolled an even number and is out of the penalty box

                    Console.WriteLine(players[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] += roll; // changed to operator +=
                    if (places[currentPlayer] > 11) places[currentPlayer] -= 12; // changed to operator -=

                    Console.WriteLine(players[currentPlayer]
                            + "'s new location is "
                            + places[currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false; // an odd number sends player to jail
                }

            }
            else
            {

                places[currentPlayer] += roll; // changed to operator +=
                if (places[currentPlayer] > 11) places[currentPlayer] -= 12; //changed to operator -=

                Console.WriteLine(players[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }

        }

        private void AskQuestion() // method name should start with a capital, edited method to allow more categories
        {
            string category = CurrentCategory();
            if (questionCategories.ContainsKey(category) && questionCategories[category].Any())
            {
                string question = questionCategories[category].First();
                Console.WriteLine(question);
                questionCategories[category].RemoveFirst();
            }
            else // added an else statement to notify if there are no more cards
            {
                Console.WriteLine("No more questions in the " + category + " category!");
            }
        }

        private String CurrentCategory() // method name should start with a capital, changed to switch
        {
            int currentPosition = places[currentPlayer];
            switch (currentPosition)
            {
                case 0:
                case 4:
                case 8:
                    return "Pop";
                case 1:
                case 5:
                case 9:
                    return "Science";
                case 2:
                case 6:
                case 10:
                    return "Sports";
                default:
                    return "Rock";
            }
        }

        public bool WasCorrectlyAnswered() // method name should start with a capital
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    purses[currentPlayer]++;
                    Console.WriteLine(players[currentPlayer]
                            + " now has "
                            + purses[currentPlayer]
                            + " Gold Coins.");

                    bool winner = DidPlayerWin();
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;

                    return winner;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;
                    return true;
                }
            }
            else
            {

                Console.WriteLine("Answer was corrent!!!!");
                purses[currentPlayer]++;
                Console.WriteLine(players[currentPlayer]
                        + " now has "
                        + purses[currentPlayer]
                        + " Gold Coins.");

                bool winner = DidPlayerWin();
                currentPlayer++;
                if (currentPlayer == players.Count) currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswer() // 
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;
            isGettingOutOfPenaltyBox = false; // Resetting the variable when the player is sent to the penalty box

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }


        private bool DidPlayerWin() //start with capital and check if there are any questions remaining in the questionCategories
        {
            return !(purses[currentPlayer] == 6) && questionCategories.Any(category => category.Value.Any());
        }
    }
}