using System;
using System.Collections.Generic;
using System.Linq;

namespace euchre.NET
{
    public static class Constants
    {
        // Cards
        public static IEnumerable<char> RANKS => new List<char>() { 'A', 'K', 'Q', 'J', 'T', '9' };
        public static IEnumerable<char> LOWER_RANKS => new List<char>() { 'J', 'T', '9' };
        public static IEnumerable<char> SUITS => new List<char>() { 'H', 'D', 'S', 'C' };
        public static List<Card> DECK
            => (List<Card>)(from suit in SUITS from rank in RANKS select new Card(rank, suit));

        // Suit Mapping
        public static Dictionary<char, char> NEXT_DICT => new Dictionary<char, char>() { { 'H', 'D' }, { 'D', 'H' }, { 'S', 'C' }, { 'C', 'S' } };
        public static Dictionary<char, char> H_TRUMP_DICT => new Dictionary<char, char>() { { 'H', 'T' }, { 'D', 'N' }, { 'S', 'A' }, { 'C', 'B' } };
        public static Dictionary<char, char> D_TRUMP_DICT => new Dictionary<char, char>() { { 'D', 'T' }, { 'H', 'N' }, { 'S', 'A' }, { 'C', 'B' } };
        public static Dictionary<char, char> S_TRUMP_DICT => new Dictionary<char, char>() { { 'S', 'T' }, { 'C', 'N' }, { 'H', 'A' }, { 'D', 'B' } };
        public static Dictionary<char, char> C_TRUMP_DICT => new Dictionary<char, char>() { { 'C', 'T' }, { 'S', 'N' }, { 'H', 'A' }, { 'D', 'B' } };

        // Hand Scoring
        public static Dictionary<string, float> THRESHOLD
            => new Dictionary<string, float>()
            { { "pass", 13.0f },
              { "call", 17.5f } };
        public static Dictionary<char, float> TRUMP_PTS
            => new Dictionary<char, float>()
            { { 'J', 9.50f },
              { 'L', 6.00f },
              { 'A', 5.50f },
              { 'K', 5.00f },
              { 'Q', 4.75f },
              { 'T', 4.50f },
              { '9', 4.25f } };
        public static Dictionary<char, Dictionary<string, float>> ACE_PTS
            => new Dictionary<char, Dictionary<string, float>>()
            {
                { 'N', new Dictionary<string, float>()
                    { { "singleton", 1.75f },
                      { "paired", 1.50f },
                      { "more", 1.50f } } },
                { 'G', new Dictionary<string, float>()
                    { { "singleton", 2.75f },
                      { "paired", 1.75f },
                      { "more", 1.50f } } }
            };
        public static Dictionary<string, float> KING_PTS
            => new Dictionary<string, float>()
            { { "singleton", 0.25f },
              { "paired", 0.50f },
              { "more", 0.25f } };
        public static Dictionary<int, float> SUITED_BONUS
            => new Dictionary<int, float>()
            { { 2,  0.5f },
              { 4, -2.0f } }; // 2-suited bonus is multiplied by number of trumps
        public static Dictionary<int, float> SEAT_BONUS
            => new Dictionary<int, float>()
            { { 1,  0.5f },
              { 2,  0.0f },
              { 3, -1.5f },
              { 0,  1.0f } }; // 2-suited bonus is multiplied by number of trumps

        // Hand Scoring - First Round
        public static Dictionary<char, float> UPCARD_BONUS
            => new Dictionary<char, float>()
            { { 'J', 3.25f },
              { 'A', 2.25f },
              { 'K', 2.00f },
              { 'Q', 1.75f },
              { 'T', 1.75f },
              { '9', 1.75f } };
        public static Dictionary<char, float> UPCARD_PENALTY
            => new Dictionary<char, float>()
            { { 'J', -2.50f },
              { 'A', -1.75f },
              { 'K', -1.75f },
              { 'Q', -1.50f },
              { 'T', -1.50f },
              { '9', -1.50f } };

        // Hand Scoring - Second Round
        public static Dictionary<int, float> NEXT_ADJ
            => new Dictionary<int, float>()
            { { 1,  3.0f },
              { 2, -2.0f },
              { 3,  1.5f },
              { 0, -0.5f } };
        public static Dictionary<int, float> GREEN_ADJ
            => new Dictionary<int, float>()
            { { 1, -2.00f },
              { 2,  2.00f },
              { 3, -0.75f },
              { 0,  0.50f } };
        public static Dictionary<char, float> UPCARD_ADJ_FACTOR
            => new Dictionary<char, float>()
            { { 'J', 1.00f },
              { 'A', 0.65f },
              { 'K', 0.60f },
              { 'Q', 0.55f },
              { 'T', 0.50f },
              { '9', 0.50f } };

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
