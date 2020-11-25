using System;
using System.Collections.Generic;
using System.Linq;

namespace Euchre.NET
{
    public class Deal
    {
        public readonly int Seed;
        public List<Card> Deck;
        public IList<IList<Card>> Hands;
        public readonly IList<IList<Card>> KnownCards;
        public Card Upcard;

        private readonly char _trump;
        private readonly IList<IList<char>> _knownVoids;

        public Deal(int? seed = null)
        {
            Seed = seed ?? (new Random()).Next();
            ShuffleAndDeal();
        }

        public Deal(IList<IList<Card>> knownCards, Card upcard, IList<IList<char>> knownVoids, char trump, int seed, Card? downcard = null)
        {
            KnownCards = knownCards;
            _trump = trump;
            _knownVoids = knownVoids ?? new List<IList<char>>(4);
            Upcard = upcard;
            Seed = seed;
            ShuffleAndDealWithKnownCards();
        }

        private void ShuffleAndDeal()
        {
            Deck = new List<Card>(Constants.DECK);
            Shuffle(Deck);
            DealHands();
        }

        private void Shuffle(IList<Card> deck)
        {
            var random = new Random(Seed);
            int n = deck.Count();
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card card = deck[k];
                deck[k] = deck[n];
                deck[n] = card;
            }
        }

        private void DealHands()
        {
            Hands = new List<IList<Card>>(4);
            for (int i = 0; i <= 3; i++)
                Hands.Add(Deck.GetRange(5 * i, 5));

            Upcard = Deck[20];
        }

        private void ShuffleAndDealWithKnownCards()
        {
            var allKnownCards = KnownCards.SelectMany(c => c).ToList();
            allKnownCards.Add(Upcard);
            var remainingDeck = new List<Card>(Constants.DECK).Where(c => !allKnownCards.Contains(c)).ToList();
            Shuffle(remainingDeck);

            Hands = new List<IList<Card>>() { KnownCards[0].ToList(), KnownCards[1].ToList(), KnownCards[2].ToList(), KnownCards[3].ToList() };
            for (int i = 0; i < Hands.Count; i++)
            {
                var hand = Hands[i];
                int count = hand.Count;
                for (int j = 0; j < remainingDeck.Count; j++)
                {
                    if (count == 5)
                        break;

                    var a = _knownVoids[i];
                    var b = remainingDeck[j].EffectiveSuit(_trump);
                    var c = a.Contains(b);
                    if (!_knownVoids[i].Contains(remainingDeck[j].EffectiveSuit(_trump)))
                    {
                        hand.Add(remainingDeck[j]);
                        remainingDeck.RemoveAt(j);
                        j--;
                        count++;
                    }
                }
            }
        }
    }
}
