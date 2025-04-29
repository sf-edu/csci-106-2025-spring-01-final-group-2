using System;
using System.Collections.Generic;

namespace ConsoleBlackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Blackjack!\n");

            bool playAgain = true;
            while (playAgain)
            {
                Deck deck = new Deck();
                deck.Shuffle();

                Hand player = new Hand();
                Hand dealer = new Hand();

                // Initial deal
                player.AddCard(deck.Draw());
                dealer.AddCard(deck.Draw());
                player.AddCard(deck.Draw());
                dealer.AddCard(deck.Draw());

                // Player turn
                bool playerBusted = false;
                while (true)
                {
                    Console.WriteLine($"Dealer shows: {dealer.Cards[0]} and (Hidden)");
                    Console.WriteLine($"Your hand ({player.Value}): {player}\n");

                    if (player.Value > 21)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You busted!");
                        Console.ResetColor();
                        playerBusted = true;
                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Hit or Stand? (h/s): ");
                    Console.ResetColor();
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.H)
                    {
                        var card = deck.Draw();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\nYou draw: {card}\n");
                        Console.ResetColor();
                        player.AddCard(card);
                    }
                    else if (key == ConsoleKey.S)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nYou stand.\n");
                        Console.ResetColor();
                        break;
                    }
                }

                // Dealer turn
                if (!playerBusted)
                {
                    Console.WriteLine($"Dealer reveals: {dealer.Cards[1]}. Dealer's hand: {dealer} ({dealer.Value})");
                    while (dealer.Value < 17)
                    {
                        var card = deck.Draw();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Dealer draws: {card}");
                        Console.ResetColor();
                        dealer.AddCard(card);
                    }
                    Console.WriteLine($"Dealer stands with {dealer.Value}.\n");
                }

                // Determine result
                Console.WriteLine("Final Hands:");
                Console.WriteLine($"  Dealer ({dealer.Value}): {dealer}");
                Console.WriteLine($"  You    ({player.Value}): {player}\n");

                if (playerBusted)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Dealer wins.");
                    Console.ResetColor();
                }
                else if (dealer.Value > 21 || player.Value > dealer.Value)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You win!");
                    Console.ResetColor();
                }
                else if (player.Value < dealer.Value)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Dealer wins.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Push (tie).");
                    Console.ResetColor();
                }

                // Play again?
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nPlay again? (y/n): ");
                Console.ResetColor();
                playAgain = Console.ReadKey(true).Key == ConsoleKey.Y;
                Console.WriteLine();
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("Thanks for playing!");
        }
    }

    class Card
    {
        public string Rank { get; }
        public string Suit { get; }
        public int Value
        {
            get
            {
                if (Rank == "A") return 11;
                if (Rank == "K" || Rank == "Q" || Rank == "J") return 10;
                return int.Parse(Rank);
            }
        }

        public Card(string rank, string suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString() => $"{Rank}{Suit}";
    }

    class Deck
    {
        private readonly List<Card> cards;
        // Now using letters instead of icons
        private static readonly string[] Suits = { "", "", "", "" };
        private static readonly string[] Ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        private readonly Random rng = new Random();

        public Deck()
        {
            cards = new List<Card>();
            foreach (var s in Suits)
                foreach (var r in Ranks)
                    cards.Add(new Card(r, s));
        }

        public void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = cards[i];
                cards[i] = cards[j];
                cards[j] = tmp;
            }
        }

        public Card Draw()
        {
            if (cards.Count == 0) throw new InvalidOperationException("The deck is empty.");
            var top = cards[0];
            cards.RemoveAt(0);
            return top;
        }
    }

    class Hand
    {
        public List<Card> Cards { get; } = new List<Card>();

        public int Value
        {
            get
            {
                int total = 0;
                int aceCount = 0;
                foreach (var c in Cards)
                {
                    total += c.Value;
                    if (c.Rank == "A") aceCount++;
                }
                // Downgrade Aces from 11 to 1 as needed
                while (total > 21 && aceCount > 0)
                {
                    total -= 10;
                    aceCount--;
                }
                return total;
            }
        }

        public void AddCard(Card card) => Cards.Add(card);
        public override string ToString() => string.Join(", ", Cards);
    }
}
