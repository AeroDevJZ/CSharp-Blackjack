using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

public class Program
{
    public static void Main(string[] args)
    {
        // Variables for money and betting
        int playerMoney = 0;
        int initialBettingAmount = 0;
        int currentBettingAmount = 0;
        int indexGameIsOn = 0;
        int lastHighPlayersMoney = 0;
        int startingPlayersMoney = 1;
        int count = 0;
        bool valid = false;

        // Arrays to hold cards and their respective worth
        int[] worth = new int[] { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };
        string[] name = new string[] { "Ace of Hearts", "2 of Hearts", "3 of Hearts", "4 of Hearts", "5 of Hearts", "6 of Hearts", "7 of Hearts", "8 of Hearts", "9 of Hearts", "10 of Hearts", "Jack of Hearts", "Queen of Hearts", "King of Hearts", "Ace of Diamonds", "2 of Diamonds", "3 of Diamonds", "4 of Diamonds", "5 of Diamonds", "6 of Diamonds", "7 of Diamonds", "8 of Diamonds", "9 of Diamonds", "10 of Diamonds", "Jack of Diamonds", "Queen of Diamonds", "King of Diamonds", "Ace of Clubs", "2 of Clubs", "3 of Clubs", "4 of Clubs", "5 of Clubs", "6 of Clubs", "7 of Clubs", "8 of Clubs", "9 of Clubs", "10 of Clubs", "Jack of Clubs", "Queen of Clubs", "King of Clubs", "Ace of Spades", "2 of Spades", "3 of Spades", "4 of Spades", "5 of Spades", "6 of Spades", "7 of Spades", "8 of Spades", "9 of Spades", "10 of Spades", "Jack of Spades", "Queen of Spades", "King of Spades" };

        // Lists to hold the total decks being used and both player's hands
        List<int> poolOfCardsWorth = new List<int>();
        List<string> poolOfCardsName = new List<string>();
        List<int> dealersWorth = new List<int>();
        List<string> dealersHand = new List<string>();
        List<int> playersWorth = new List<int>();
        List<string> playersHand = new List<string>();

        // ------------------------------------------------------------ Start of executable code ------------------------------------------------------------ \\

        Console.WriteLine("                                           Welcome to Blackjack\nThis is a card game where you play against the dealer and try to get as close to 21 as possible without going over.\n\nEnter how much inital money you want to have:");
        while (!valid)
        {
            valid = int.TryParse(Console.ReadLine(), out playerMoney);
            if (!valid)
            {
                Console.WriteLine("\nPlease enter a valid number: ");
            }
            else if (playerMoney < 1)
            {
                valid = false;
                Console.WriteLine("\n\nYou need to have some money to play the game. Enter a higher amount please:");
            }
            startingPlayersMoney = playerMoney;
        }
        valid = false;
        Console.WriteLine("\nEnter your initial betting amount or enter 0 if you want it to be auto calculated:");
        while (!valid)
        {
            valid = int.TryParse(Console.ReadLine(), out initialBettingAmount);
            if (!valid || initialBettingAmount < 0)
            {
                Console.WriteLine("\nPlease enter a valid number: ");
                valid = false;
            }
            else if (initialBettingAmount > 0 && initialBettingAmount > playerMoney)
            {
                Console.WriteLine("\nPlease enter a number lower than " + playerMoney + " or enter 0 if you want it to be auto calculated.");
                valid = false;
            }
            else if (initialBettingAmount == 0)
            {
                Console.WriteLine("\nHow safe do you want to play?\nEnter a number 1 - 10 with 1 being not safe and 10 being extremely safe: ");
                bool valid1 = false;
                int input1 = 0;
                while (!valid1)
                {
                    valid1 = int.TryParse(Console.ReadLine(), out input1);
                    if (!valid1 || input1 < 0 || input1 > 10)
                    {
                        Console.WriteLine("\nPlease enter a valid number: ");
                        valid1 = false;
                    }
                }
                initialBettingAmount = (int)(playerMoney / (Math.Pow(2.0, input1) - 1));
            }
            Console.WriteLine("\nYour inital betting amount is " + initialBettingAmount + ".");
        }
        currentBettingAmount = initialBettingAmount;
        valid = false;
        Console.WriteLine("\nEnter the number of decks that you want to play with from 1-15:");
        while (!valid)
        {
            int input1 = 0;
            valid = int.TryParse(Console.ReadLine(), out input1);
            if (!valid || input1 < 1 || input1 > 16)
            {
                Console.WriteLine("\nPlease enter a number between 1 and 15: ");
                valid = false;
            }
            else
            {
                AddCards(input1);
                Console.WriteLine("\n" + input1 + " decks are currently in use.");
            }
        }
        valid = false;
        Console.WriteLine("\nWould you like to play a manual game or have an algorithm\nsimulate a game for you? Enter 0 for manual or 1 for an algorithm:");
        while (!valid)
        {
            int input1 = 0;
            valid = int.TryParse(Console.ReadLine(), out input1);
            if (!valid || input1 < 0 || input1 > 1)
            {
                Console.WriteLine("\nPlease enter a either a 0 or a 1: ");
                valid = false;
            }
            else if (input1 == 0)
            {
                valid = false;
                PlayGameManual();
                valid = true;
            }
            else
            {
                valid = false;
                PlayAutoGame();
                valid = true;
            }
        }
        Console.WriteLine("\n\nGoodbye World!");
        // ------------------------------------------------------------ End of executable code ------------------------------------------------------------ \\

        void AddCards(int numberOfDecks)
        {
            for (int i = 0; i < numberOfDecks; i++)
            {
                for (int j = 0; j < worth.Length; j++)
                {
                    poolOfCardsWorth.Add(worth[j]);
                    poolOfCardsName.Add(name[j]);
                }
            }
        }

        void ShuffleCards()
        {
            Random random = new Random();
            int[] tempWorth = new int[poolOfCardsWorth.Count];
            string[] tempName = new string[poolOfCardsName.Count];
            for (int i = 0; i < poolOfCardsName.Count; i++)
            {
                int r = random.Next(0, (poolOfCardsName.Count - i));
                tempName[i] = poolOfCardsName[r];
                tempWorth[i] = poolOfCardsWorth[r];
                for (int j = r; j < (poolOfCardsName.Count - 1 - i); j++)
                {
                    poolOfCardsName[j] = poolOfCardsName[j + 1];
                    poolOfCardsWorth[j] = poolOfCardsWorth[j + 1];
                }
            }
            poolOfCardsName.Clear();
            poolOfCardsName.AddRange(tempName);
            poolOfCardsWorth.Clear();
            poolOfCardsWorth.AddRange(tempWorth);
            indexGameIsOn = 0;
            Console.WriteLine("\n------------------------------Cards have been shuffled------------------------------");
        }

        void PlayGameManual()
        {
            int max = poolOfCardsName.Count - 12;
            while (!valid)
            {
                ShuffleCards();
                while (indexGameIsOn < max && !valid)
                {
                    if (playerMoney >= currentBettingAmount)
                    {
                        bool won = true;
                        Console.WriteLine("\n\nStarting New Round: ");
                        AddCardPlayer();
                        AddCardDealer();
                        AddCardPlayer();
                        AddCardDealer();
                        bool valid1 = false;
                        if (ReturnWorth(playersWorth) == 21)
                        {
                            TellUserHands();
                            playerMoney += (int)(currentBettingAmount * 1.5);
                            Console.WriteLine("\nCongrats, you won by getting a BlackJack! You gained " + (int)(currentBettingAmount * 1.5) + " dollars for a total of " + playerMoney + " dollars");
                            won = true;
                        }
                        else
                        {
                            TellUserHands();
                            Console.WriteLine("\nInput h to hit or s to stand:");
                            while (!valid1)
                            {
                                string? input1 = Console.ReadLine();
                                if (input1 == "h")
                                {
                                    AddCardPlayer();
                                    valid1 = false;
                                    if (ReturnWorth(playersWorth) > 21)
                                    {
                                        TellUserHands();
                                        playerMoney -= currentBettingAmount;
                                        Console.WriteLine("\n\nSorry you have busted! You lost " + currentBettingAmount + " dollars for a total of " + playerMoney + " dollars");
                                        valid1 = true;
                                        won = false;
                                    }
                                    else
                                    {
                                        TellUserHands();
                                        Console.WriteLine("\nInput h to hit or s to stand:");
                                    }

                                }
                                else if (input1 == "s")
                                {
                                    Console.WriteLine("You chose to stand, the dealer will play now.\n\n--------------------------------- Dealer's Turn --------------------------------- \n");
                                    valid1 = true;
                                }
                                else
                                {
                                    TellUserHands();
                                    Console.WriteLine("\nPlease enter either s for stand or h for hit:");
                                    valid1 = false;
                                }
                            }
                            valid1 = false;
                            if (ReturnWorth(playersWorth) < 22)
                            {
                                Console.WriteLine("\nDealer's hand:");
                                Console.WriteLine(PrintStringList(dealersHand) + " which is worth " + ReturnWorth(dealersWorth));
                                while (ReturnWorth(dealersWorth) < 17)
                                {
                                    AddCardDealer();
                                    Console.WriteLine("\nDealer's hand:");
                                    Console.WriteLine(PrintStringList(dealersHand) + " which is worth " + ReturnWorth(dealersWorth));
                                }
                            }
                            if ((ReturnWorth(dealersWorth) > ReturnWorth(playersWorth)) && ReturnWorth(dealersWorth) < 22)
                            {
                                playerMoney -= currentBettingAmount;
                                Console.WriteLine("\n\nSorry, the dealer won. Better luck next time! You lost " + currentBettingAmount + " dollars for a total of " + playerMoney + " dollars");
                                won = false;
                            }
                            else if (won)
                            {
                                playerMoney += currentBettingAmount;
                                Console.WriteLine("\n\nCongrats, you won that round! You gained " + currentBettingAmount + " dollars for a total of " + playerMoney + " dollars");
                            }
                        }
                        Console.WriteLine("\n\nDo you want to play another game? Enter y for yes or n for no:");
                        while (!valid1)
                        {
                            string? input1 = Console.ReadLine();
                            if (input1 == "y")
                            {
                                valid1 = true;
                                valid = false;
                                bool valid2 = false;
                                Console.WriteLine("\nWhat do you want your next betting amount to be? You have " + playerMoney + " dollars. Enter 0 to auto decide, 1 to double your current betting amount, or any number above 1 to bet that amount.");
                                while (!valid2)
                                {
                                    int input2 = 0;
                                    valid2 = int.TryParse(Console.ReadLine(), out input2);
                                    if (valid2 && input2 == 0)
                                    {
                                        if (won)
                                        {
                                            currentBettingAmount = initialBettingAmount;
                                            Console.WriteLine("\nYour current betting amount is now: " + currentBettingAmount);
                                        }
                                        else if (currentBettingAmount * 2 <= playerMoney)
                                        {
                                            currentBettingAmount *= 2;
                                            if (currentBettingAmount == playerMoney)
                                            {
                                                Console.WriteLine("\nYou are one game away from being broke. Your current betting amount is now: " + currentBettingAmount);
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nYour current betting amount is now: " + currentBettingAmount);
                                            }
                                        }
                                        else if (playerMoney != 0)
                                        {
                                            currentBettingAmount = playerMoney;
                                            Console.WriteLine("\nYou are getting close to being broke. Your current betting amount is now: " + currentBettingAmount);
                                        }
                                    }
                                    else if (valid2 && input2 == 1)
                                    {
                                        if (currentBettingAmount * 2 <= playerMoney)
                                        {
                                            currentBettingAmount *= 2;
                                            Console.WriteLine("\nYour current betting amount is now: " + currentBettingAmount);
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nYou do not have sufficent funds to double your bet.");
                                            valid2 = false;
                                        }
                                    }
                                    else if (valid2 && input2 > 1)
                                    {
                                        if (input2 > playerMoney)
                                        {
                                            Console.WriteLine("\nYou do not have sufficent funds to bet" + input2 + ".");
                                            valid2 = false;
                                        }
                                        else
                                        {
                                            currentBettingAmount = input2;
                                            Console.WriteLine("\nYour current betting amount is now: " + currentBettingAmount);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nPlease enter 0, 1, or any number lower than how much money you have.");
                                    }
                                }
                            }
                            else if (input1 == "n")
                            {
                                valid1 = true;
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("\nPlease enter either y for yes or n for no:");
                                valid1 = false;
                            }
                        }
                        playersHand.Clear();
                        playersWorth.Clear();
                        dealersHand.Clear();
                        dealersWorth.Clear();
                    }
                    else
                    {
                        Console.WriteLine("\nYou ran out of money, maybe don't gamble it all next time.");
                        valid = true;
                    }
                }
            }
        }

        void PlayAutoGame()
        {
            int max = poolOfCardsName.Count - 12;
            while (!valid)
            {
                ShuffleCards();
                while (indexGameIsOn < max && !valid)
                {
                    if (playerMoney >= currentBettingAmount && !(playerMoney / startingPlayersMoney >= 100))
                    {
                        Console.WriteLine("\n\nStarting Round #" + count);
                        AddCardPlayer();
                        AddCardDealer();
                        AddCardPlayer();
                        AddCardDealer();
                        if (ReturnWorth(playersWorth) == 21)
                        {
                            playerMoney += (int)(currentBettingAmount * 1.5);
                            Console.WriteLine("\nCongrats, you won by getting a BlackJack! You gained " + (int)(currentBettingAmount * 1.5) + " dollars for a total of " + playerMoney + " dollars");
                            if (playerMoney > lastHighPlayersMoney)
                            {
                                lastHighPlayersMoney = playerMoney;
                            }
                        }
                        else
                        {
                            while (ReturnWorth(playersWorth) < 17)
                            {
                                AddCardPlayer();
                            }
                            if (ReturnWorth(playersWorth) < 22)
                            {
                                while (ReturnWorth(dealersWorth) < 17)
                                {
                                    AddCardDealer();
                                }
                            }
                            if (((ReturnWorth(dealersWorth) > ReturnWorth(playersWorth)) && ReturnWorth(dealersWorth) < 22) || ReturnWorth(playersWorth) > 21)
                            {
                                playerMoney -= currentBettingAmount;
                                TellUserAllHands();
                                Console.WriteLine("\n\nSorry, the dealer won. Better luck next time! You lost " + currentBettingAmount + " dollars for a total of " + playerMoney + " dollars");
                                if (currentBettingAmount * 2 <= playerMoney)
                                {
                                    currentBettingAmount *= 2;
                                }
                                else if (playerMoney != 0)
                                {
                                    currentBettingAmount = playerMoney;
                                }
                            }
                            else if (ReturnWorth(dealersWorth) == ReturnWorth(playersWorth))
                            {
                                TellUserAllHands();
                                Console.WriteLine("\n\nTie, no one wins any money you still have " + playerMoney + " dollars");
                            }
                            else
                            {
                                playerMoney += currentBettingAmount;
                                TellUserAllHands();
                                Console.WriteLine("\n\nCongrats, you won that round! You gained " + currentBettingAmount + " dollars for a total of " + playerMoney + " dollars");
                                if (playerMoney > lastHighPlayersMoney)
                                {
                                    lastHighPlayersMoney = playerMoney;
                                }
                                currentBettingAmount = initialBettingAmount;
                            }
                        }
                        playersHand.Clear();
                        playersWorth.Clear();
                        dealersHand.Clear();
                        dealersWorth.Clear();
                        count++;
                    }
                    else if (playerMoney / startingPlayersMoney >= 100)
                    {
                        Console.WriteLine("\nYou 100X your money after " + count + " rounds, earning " + lastHighPlayersMoney + " on the last game when you started with " + startingPlayersMoney + " for a total of " + (lastHighPlayersMoney - startingPlayersMoney) + " gained.");
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("\nYou ran out of money after " + count + " rounds, earning " + lastHighPlayersMoney + " on the last game when you started with " + startingPlayersMoney + " for a total of " + (lastHighPlayersMoney - startingPlayersMoney) + " gained.");
                        valid = true;
                    }
                }
            }
        }

        void AddCardDealer()
        {
            dealersHand.Add(poolOfCardsName[indexGameIsOn]);
            dealersWorth.Add(poolOfCardsWorth[indexGameIsOn]);
            indexGameIsOn++;
        }

        void TellUserHands()
        {
            Console.WriteLine("\n\nDealer's card: ");
            if (dealersWorth[0] == 11)
            {
                Console.WriteLine(dealersHand[0] + " which is worth either 1 or 11");
            }
            else
            {
                Console.WriteLine(dealersHand[0] + " which is worth " + dealersWorth[0]);
            }
            Console.WriteLine("\nPlayer's hand:");
            Console.WriteLine(PrintStringList(playersHand) + " which is worth " + ReturnWorth(playersWorth));
        }

        void TellUserAllHands()
        {
            Console.WriteLine("\n\nDealer's hand: ");
            Console.WriteLine(PrintStringList(dealersHand) + " which is worth " + ReturnWorth(dealersWorth));
            Console.WriteLine("\nPlayer's hand:");
            Console.WriteLine(PrintStringList(playersHand) + " which is worth " + ReturnWorth(playersWorth));
        }

        void AddCardPlayer()
        {
            playersHand.Add(poolOfCardsName[indexGameIsOn]);
            playersWorth.Add(poolOfCardsWorth[indexGameIsOn]);
            indexGameIsOn++;
        }

        int ReturnWorth(List<int> list)
        {
            int total = 0;
            int aces = 0;
            for (int i = 0; i < list.Count; i++)
            {
                total += list[i];
                if (list[i] == 11)
                {
                    aces++;
                }
            }
            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }
            return total;
        }

        string PrintStringList(List<string> list)
        {
            string total = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == (list.Count - 1))
                {
                    total += "and " + list[i];
                }
                else
                {
                    total += list[i] + ", ";
                }
            }
            return total;
        }
    }
}
