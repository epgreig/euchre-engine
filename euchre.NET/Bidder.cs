using System;
using System.Collections.Generic;
using System.Linq;

namespace Euchre.NET
{
    public class Bidder
    {
        public Random RNG;

        public Bidder(Random rng)
            => RNG = rng;

        public bool OrderUp(IList<Card> hand, Card upcard, int seat, out Card? discard)
        {
            var score = ScoreHand(hand, upcard.Suit, upcard, seat, out discard);

            if (seat == 1)
            {
                var scores = new Dictionary<char, float>();
                foreach (char suit in Constants.SUITS)
                {
                    if (suit != upcard.Suit && ScoreHand(hand, suit, upcard, 1, out discard) >= score)
                        return false;
                }
            }

            var fraction = RNG.NextDouble();
            var interval = Constants.THRESHOLD["call"] - Constants.THRESHOLD["pass"];
            var threshold = Constants.THRESHOLD["pass"] + fraction * interval;

            return score > threshold;
        }

        public bool Declare(IList<Card> hand, Card upcard, int seat, out char? trump)
        {
            trump = null;
            float bestScore = 0;
            char bestSuit = 'Z';
            foreach (char suit in Constants.SUITS)
            {
                if (suit != upcard.Suit)
                {
                    var score = ScoreHand(hand, suit, upcard, seat, out _);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestSuit = suit;
                    }
                }

            }

            var fraction = RNG.NextDouble();
            var interval = Constants.THRESHOLD["call"] - Constants.THRESHOLD["pass"];
            var threshold = Constants.THRESHOLD["pass"] + fraction * interval;

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

        public float ScoreHand(IList<Card> hand, char suit, Card upcard, int seat, out Card? discard)
        {
            float score = 0;
            discard = null;
            if (suit == upcard.Suit)
            {
                if (seat == 1 || seat == 3)
                {
                    score += ScoreHighCards(hand, suit);
                    score += Constants.UPCARD_PENALTY[upcard.Rank];
                }
                else if (seat == 2)
                {
                    score += ScoreHighCards(hand, suit);
                    score += Constants.UPCARD_BONUS[upcard.Rank];
                }
                else if (seat == 0)
                {
                    score += SelectDiscard(hand, upcard, out Card discardResult);
                    discard = discardResult;
                }
            }
            else if (suit == Constants.NEXT_DICT[upcard.Suit])
            {
                score += ScoreHighCards(hand, suit, upcard.Suit);
                score += Constants.NEXT_ADJ[seat] * Constants.UPCARD_ADJ_FACTOR[upcard.Rank];
            }
            else
            {
                score += ScoreHighCards(hand, suit, upcard.Suit);
                score += Constants.GREEN_ADJ[seat] * Constants.UPCARD_ADJ_FACTOR[upcard.Rank];
            }

            score += Constants.SEAT_BONUS[seat];
            return score;
        }

        public float ScoreHighCards(IList<Card> hand, char trump, char? discardedSuit = null)
        {
            char nextSuit = Constants.NEXT_DICT[trump];
            var handCopy = new List<Card>(hand);
            for (int i = 0; i < handCopy.Count; i++)
            {
                Card card = handCopy[i];
                if (card.Suit == nextSuit && card.Rank == 'J')
                {
                    card.Suit = trump;
                    card.Rank = 'L';
                }
            }

            float score = 0;

            var suits = handCopy.Select(c => c.Suit);
            var suitsGrouped = suits.GroupBy(s => s);
            var suitCounts = new Dictionary<char, int>();
            foreach (var group in suitsGrouped)
                suitCounts.Add(group.Key, group.Count());
        
            foreach (Card card in handCopy)
            {
                if (card.Suit == trump)
                {
                    score += Constants.TRUMP_PTS[card.Rank];
                }
                else
                {
                    int suitCount = suitCounts[card.Suit];
                    if (card.Suit == discardedSuit)
                        suitCount++;

                    if (card.Rank == 'A')
                    {
                        if (card.Suit == nextSuit)
                        {
                            if (suitCount == 1)
                                score += Constants.ACE_PTS['N']["singleton"];
                            else if (suitCount == 2)
                                score += Constants.ACE_PTS['N']["paired"];
                            else
                                score += Constants.ACE_PTS['N']["more"];
                        }
                        else
                        {
                            if (suitCount == 1)
                                score += Constants.ACE_PTS['G']["singleton"];
                            else if (suitCount == 2)
                                score += Constants.ACE_PTS['G']["paired"];
                            else
                                score += Constants.ACE_PTS['G']["more"];
                        }
                    }
                    else if (card.Rank == 'K')
                    {
                        if (suitCount == 1)
                            score += Constants.KING_PTS["singleton"];
                        else if (suitCount == 2)
                            score += Constants.KING_PTS["paired"];
                        else
                            score += Constants.KING_PTS["more"];
                    }
                }
            }

            if (suitCounts.Count() < 3 && suitCounts.ContainsKey(trump))
                score += Constants.SUITED_BONUS[2] * suitCounts[trump];
            else if (suitCounts.Count() == 4)
                score += Constants.SUITED_BONUS[4];

            return score;
        }

        public float SelectDiscard(IList<Card> hand, Card upcard, out Card downcard)
        {
            char trump = upcard.Suit;
            downcard = upcard.Copy();
            float bestScore = ScoreHighCards(hand, trump);

            for (int i = 0; i < hand.Count(); i++)
            {
                var handCopy = new List<Card>(hand)
                {
                    [i] = upcard
                };
                float score = ScoreHighCards(handCopy, trump, hand[i].Suit);
                if (score > bestScore)
                {
                    bestScore = score;
                    downcard = hand[i].Copy();
                }
            }

            return bestScore;
        }
    }
}
