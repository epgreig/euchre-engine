using System;
using System.Collections.Generic;

namespace euchre.NET
{
    public class Bidder
    {
        public Random RNG;

        public Bidder(Random rng)
            => RNG = rng;

        public bool OrderUp(IEnumerable<Card> hand, Card upcard, int seat)
        {
            var score = ScoreHand(hand, upcard.Suit, upcard, seat);

            if (seat == 1)
            {
                var scores = new Dictionary<char, decimal>();
                foreach (char suit in Constants.SUITS)
                {
                    if (suit != upcard.Suit && ScoreHand(hand, suit, upcard, 1) >= score)
                        return false;
                }
            }

            var fraction = RNG.NextDouble();
            var interval = Constants.THRESHOLD['call'] - Constants.THRESHOLD['pass'];
            var threshold = Constants.THRESHOLD['pass'] + fraction * interval;

            return score > threshold;
        }

        public bool Declare(IEnumerable<Card> hand, Card upcard, int seat, out char? trump)
        {
            trump = null;

            if (isFirstRound)
            {

            }
            else
            {
                decimal bestScore = 0;
                char bestSuit = 'Z';
                foreach (char suit in Constants.SUITS)
                {
                    if (suit != upcard.Suit)
                    {
                        var score = ScoreHand(hand, suit, upcard, seat);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestSuit = suit;
                        }
                    }

                }

                var fraction = RNG.NextDouble();
                var interval = Constants.THRESHOLD['call'] - Constants.THRESHOLD['pass']
                var threshold = Constants.THRESHOLD['pass'] + fraction * interval;

                if (bestScore > threshold)
                {
                    trump = bestSuit;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public decimal ScoreHand(IEnumerable<Card> hand, char suit, Card upcard, int seat)
        {
            if (suit == upcard.Suit)
            {

            }
        }
    }
}
