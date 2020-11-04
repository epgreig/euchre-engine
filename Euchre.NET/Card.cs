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
    }
}
