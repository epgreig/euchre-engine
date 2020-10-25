using System;
using System.Collections.Generic;
using System.Linq;

namespace euchre.NET
{
    public static class Constants
    {
        public static IEnumerable<char> RANKS => new List<char>() { 'A', 'K', 'Q', 'J', 'T', '9' };
        public static IEnumerable<char> SUITS => new List<char>() { 'H', 'D', 'S', 'C' };
        public static List<Card> DECK
            => (List<Card>)(from suit in SUITS from rank in RANKS select new Card(rank, suit));

        public static Dictionary<char, char> NEXT_DICT => new Dictionary<char, char>() { { 'H', 'D' }, { 'D', 'H' }, { 'S', 'C' }, { 'C', 'S' } };
        public static Dictionary<char, char> H_TRUMP_DICT => new Dictionary<char, char>() { { 'H', 'T' }, { 'D', 'N' }, { 'S', 'A' }, { 'C', 'B' } };
        public static Dictionary<char, char> D_TRUMP_DICT => new Dictionary<char, char>() { { 'D', 'T' }, { 'H', 'N' }, { 'S', 'A' }, { 'C', 'B' } };
        public static Dictionary<char, char> S_TRUMP_DICT => new Dictionary<char, char>() { { 'S', 'T' }, { 'C', 'N' }, { 'H', 'A' }, { 'D', 'B' } };
        public static Dictionary<char, char> C_TRUMP_DICT => new Dictionary<char, char>() { { 'C', 'T' }, { 'S', 'N' }, { 'H', 'A' }, { 'D', 'B' } };

        public static IEnumerable<Card> TrumpifyDeck/Hands(IEnumerable<Card> deck, char trumpSuit)
        {
            var result = new IEnumerable<Card>();
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
                    result.Append(new Card(card.Rank, 'T'));
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
