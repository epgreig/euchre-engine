using System;
using System.Runtime.Serialization;

namespace Euchre.NET
{
    [Serializable]
    public struct Card
    {
        public char Rank { get; set; }
        public char Suit { get; set; }

        public Card(char rank, char suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override bool Equals(Object obj)
            => obj is Card c && this == c;

        public override int GetHashCode()
            => Rank.GetHashCode() ^ Suit.GetHashCode();

        public static bool operator ==(Card x, Card y)
            => x.Rank == y.Rank && x.Suit == y.Suit;

        public static bool operator !=(Card x, Card y)
            => !(x == y);
    }

    public static class CardExtensions
    {
        public static Card Copy(this Card card)
            => new Card(card.Rank, card.Suit);

        public static string GetString(this Card card)
            => new string(new char[] { card.Rank, card.Suit });

        public static Card Trumpify(this Card card, char trump)
        {
            if (card.Suit == trump)
            {
                card.Suit = 'T';
            }
            else
            {
                if (card.Suit == Constants.NEXT_DICT[trump])
                {
                    card.Suit = 'N';
                    if (card.Rank == 'J')
                    {
                        card.Rank = 'L';
                        card.Suit = 'T';
                    }
                }
                else
                {
                    var GREEN_DICT = Constants.RED_SUITS.Contains(trump)
                        ? Constants.RED_TRUMP_DICT
                        : Constants.BLACK_TRUMP_DICT;

                    card.Suit = GREEN_DICT[card.Suit];
                }

                if (Constants.LOWER_RANKS.Contains(card.Rank))
                    card.Rank = 'X';
            }

            return card;
        }

        public static Card Detrumpify(this Card card, char trump)
        {
            if (card.Suit == 'T')
            {
                card.Suit = trump;
                if (card.Rank == 'L')
                {
                    card.Rank = 'J';
                    card.Suit = Constants.NEXT_DICT[trump];
                }
            }
            else if (card.Suit == 'N')
            {
                card.Suit = Constants.NEXT_DICT[trump];
            }
            else
            {
                var DEGREEN_DICT = Constants.RED_SUITS.Contains(trump)
                    ? Constants.RED_DETRUMP_DICT
                    : Constants.BLACK_DETRUMP_DICT;

                card.Suit = DEGREEN_DICT[card.Suit];
            }

            return card;
        }

        public static char EffectiveSuit(this Card card, char trump)
        {
            if (card.Rank == 'J' && card.Suit == Constants.NEXT_DICT[trump])
                return trump;
            else
                return card.Suit;
        }
    }
}
