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

        public Deal(int? seed = null)
        {
            Seed = seed ?? (new Random()).Next();
            ShuffleAndDeal();
        }

        public Deal(IList<IList<Card>> knownCards, Card upcard, Card? downcard = null)
        {
            KnownCards = knownCards;
            Upcard = upcard;
            Seed = (new Random()).Next();
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

            Hands = new List<IList<Card>>(KnownCards);
            foreach (var hand in Hands)
            {
                int count = hand.Count;
                while (count < 5)
                {
                    count++;
                    hand.Add(remainingDeck[0]);
                    remainingDeck.RemoveAt(0);
                }
            }
        }
    }
}
