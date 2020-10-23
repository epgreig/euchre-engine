using System;
using System.Collections.Generic;
using System.Linq;

namespace euchre.NET
{
    public static class Constants
    {
        private static IEnumerable<char> RANKS => new[] { 'A', 'K', 'Q', 'J', 'T', '9' };
        private static IEnumerable<char> SUITS => new[] { 'H', 'D', 'S', 'C' };
        public static IEnumerable<Card> DECK
            => from suit in SUITS from rank in RANKS select new Card(rank, suit);

        public static Dictionary<char, char> NEXT_DICT => new Dictionary<char, char>() { { 'H', 'D' }, { 'D', 'H' }, { 'S', 'C' }, { 'C', 'S' } };
        public static Dictionary<char, char> H_TRUMP_DICT => new Dictionary<char, char>() { { 'H', 'T' }, { 'D', 'N' }, { 'S', 'A' }, { 'C', 'B' } };
        public static Dictionary<char, char> D_TRUMP_DICT => new Dictionary<char, char>() { { 'D', 'T' }, { 'H', 'N' }, { 'S', 'A' }, { 'C', 'B' } };
        public static Dictionary<char, char> S_TRUMP_DICT => new Dictionary<char, char>() { { 'S', 'T' }, { 'C', 'N' }, { 'H', 'A' }, { 'D', 'B' } };
        public static Dictionary<char, char> C_TRUMP_DICT => new Dictionary<char, char>() { { 'C', 'T' }, { 'S', 'N' }, { 'H', 'A' }, { 'D', 'B' } };

        public static IEnumerable<Card> deck TrumpifyDeck(IEnumerable<Card> deck, char trumpSuit)
        {
            var nextSuit = NEXT_DICT[trumpSuit];

            Dictionary<char, char> suitMapping;
            switch (trumpSuit)
            {
                case 'H':
                    suitMapping = H_TRUMP_DICT;
                    break;
                case 'D':
                    suitMapping = D_TRUMP_DICT;
                    break;
                case 'S':
                    suitMapping = S_TRUMP_DICT;
                    break;
                case 'C':
                    suitMapping = C_TRUMP_DICT;
                    break;
                default:
                    throw new Exception($"{trumpSuit} is not a valid suit.");
            }

            foreach (var card in deck)
            {
                if (card.Suit == trumpSuit)
                {
                    card.Suit = 'T';
                }
                if ((card.Rank == 'J') && (card.Suit == nextSuit))
                {
                    card.Rank = 'L';
                    card.Suit = 'T';
                }
                else if ((card.Rank))
            }
        }
    }

}
