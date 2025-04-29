using System;
using System.Collections.Generic;

class BlackjackGame
{
    static Random rand = new Random();

    static void Main()
    {
        Console.WriteLine("=== Welcome to Number Blackjack! ===");

        bool keepPlaying = true;

        while (keepPlaying)
        {
            PlayRound();

            Console.Write("\nPlay again? (y/n): ");
            string input = Console.ReadLine().ToLower();
            keepPlaying = input == "y";
            Console.Clear(); // optional: clears screen for a fresh round
        }

        Console.WriteLine("Thanks for playing! Goodbye!");
    }

    static void PlayRound()
    {
        List<int> playerHand = new List<int>();
        List<int> dealerHand = new List<int>();

        // Initial two cards
        playerHand.Add(DrawCard());
        playerHand.Add(DrawCard());
        dealerHand.Add(DrawCard());
        dealerHand.Add(DrawCard());

        Console.WriteLine("\nYour cards: " + string.Join(", ", playerHand) + " (Total: " + HandValue(playerHand) + ")");
        Console.WriteLine("Dealer shows: " + dealerHand[0]);

        // Player turn
        while (true)
        {
            Console.Write("Hit or Stand? (h/s): ");
            string choice = Console.ReadLine().ToLower();

            if (choice == "h")
            {
                int newCard = DrawCard();
                playerHand.Add(newCard);
                Console.WriteLine("You drew a " + newCard);
                Console.WriteLine("Your hand: " + string.Join(", ", playerHand) + " (Total: " + HandValue(playerHand) + ")");

                if (HandValue(playerHand) > 21)
                {
                    Console.WriteLine("Bust! You lose.");
                    return;
                }
            }
            else if (choice == "s")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'h' or 's'.");
            }
        }

        // Dealer turn
        Console.WriteLine("\nDealer's turn...");
        Console.WriteLine("Dealer's hand: " + string.Join(", ", dealerHand) + " (Total: " + HandValue(dealerHand) + ")");

        while (HandValue(dealerHand) < 17)
        {
            int newCard = DrawCard();
            dealerHand.Add(newCard);
            Console.WriteLine("Dealer draws a " + newCard);
        }

        int playerTotal = HandValue(playerHand);
        int dealerTotal = HandValue(dealerHand);

        Console.WriteLine("\nFinal Hands:");
        Console.WriteLine("Your total: " + playerTotal);
        Console.WriteLine("Dealer total: " + dealerTotal);

        // Outcome
        if (dealerTotal > 21 || playerTotal > dealerTotal)
            Console.WriteLine("You win!");
        else if (playerTotal < dealerTotal)
            Console.WriteLine("Dealer wins!");
        else
            Console.WriteLine("It's a tie!");
    }

    static int DrawCard()
    {
        return rand.Next(2, 12); // Cards between 2 and 11
    }

    static int HandValue(List<int> hand)
    {
        int total = 0;
        foreach (int card in hand)
        {
            total += card;
        }
        return total;
    }
}
