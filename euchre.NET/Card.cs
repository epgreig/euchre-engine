using System;
namespace euchre.NET
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
    }
}
