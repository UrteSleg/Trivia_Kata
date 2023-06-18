using System;
using UglyTrivia;

namespace Trivia
{
    public class GameRunner
    {

        private static bool notAWinner;

        public static void Main(String[] args)
        {
            Game aGame = new Game();

            aGame.AddPlayer("Chet");
            aGame.AddPlayer("Pat");
            aGame.AddPlayer("Sue");

            if (aGame.isPlayable()) //check if there ar at least two players
            {
                Random rand = new Random();

                do
                {
                    aGame.Roll(rand.Next(5) + 1);

                    if (rand.Next(9) == 7)
                    {
                        notAWinner = aGame.WrongAnswer();
                    }
                    else
                    {
                        notAWinner = aGame.WasCorrectlyAnswered();
                    }

                } while (notAWinner);
            }

            else // if not, display error
            {
                Console.WriteLine("The game cannot be played with less than two players.");
            }
        }
    }
}

