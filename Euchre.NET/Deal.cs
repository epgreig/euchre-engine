using System;
using System.Collections.Generic;
using System.Linq;

namespace Euchre.NET
{
    public class Deal
    {
        public readonly Random RNG;
        public List<Card> Deck;
        public IList<IList<Card>> Hands;
        public readonly IList<IList<Card>> KnownCards;
        public Card Upcard;
        public int Attempts = 0;

        private readonly char _trump;
        private readonly IList<IList<char>> _knownVoids;

        public Deal(int? seed = null)
        {
            RNG = seed != null ? new Random((int)seed) : new Random();
            ShuffleAndDeal();
        }

        public Deal(IList<IList<Card>> knownCards, Card upcard, IList<IList<char>> knownVoids, char trump, int seed, Card? downcard = null)
        {
            KnownCards = knownCards;
            _trump = trump;
            _knownVoids = knownVoids ?? new List<IList<char>>(4);
            Upcard = upcard;
            RNG = new Random((int)seed);
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
            int n = deck.Count();
            while (n > 1)
            {
                n--;
                int k = RNG.Next(n + 1);
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

            // fill dealer hand, allowing 1 buffer card from void suits
            var dealerHand = Hands[0];
            int dealerCount = dealerHand.Count;
            var voidCardsAdded = 0;
            for (int j = 0; j < remainingDeck.Count; j++)
            {
                if (dealerCount == 5)
                    break;

                if (!_knownVoids[0].Contains(remainingDeck[j].EffectiveSuit(_trump)) || (voidCardsAdded++ == 0))
                {
                    dealerHand.Add(remainingDeck[j]);
                    remainingDeck.RemoveAt(j);
                    j--;
                    dealerCount++;
                }
            }

            // fill remaining hands
            for (int i = 1; i < Hands.Count; i++)
            {
                var hand = Hands[i];
                int count = hand.Count;
                for (int j = 0; j < remainingDeck.Count; j++)
                {
                    if (count == 5)
                        break;

                    if (!_knownVoids[i].Contains(remainingDeck[j].EffectiveSuit(_trump)))
                    {
                        hand.Add(remainingDeck[j]);
                        remainingDeck.RemoveAt(j);
                        j--;
                        count++;
                    }
                }

                if (count != 5)
                    break;
            }

            if (Hands.Any(h => h.Count != 5))
            {
                if (Attempts > 1000)
                    throw new Exception("Could not find an appropriate deal");

                Attempts++;
                ShuffleAndDealWithKnownCards();
            }
        }
    }
}