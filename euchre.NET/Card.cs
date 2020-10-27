using System;

namespace Euchre.NET
{
    public struct Card
    {
        public char Rank;
        public char Suit;

        public Card(char rank, char suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }

    public static class CardExtensions
    {
        public static Card Copy(this Card card)
            => new Card(card.Rank, card.Suit);

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
