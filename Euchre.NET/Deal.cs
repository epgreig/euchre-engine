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
            ShuffleAndDeal(false);
        }

        public Deal(IList<IList<Card>> knownCards, Card upcard, int? seed = null)
        {
            KnownCards = knownCards;
            Upcard = upcard;
            Seed = seed ?? (new Random()).Next();
            ShuffleAndDeal(true);
        }

        private void ShuffleAndDeal(bool someCardsKnown)
        {
            Deck = new List<Card>(Constants.DECK);

            if (someCardsKnown)
                ShuffleDeckWithKnownCards();
            else
                ShuffleDeck();

            DealHands();
        }

        private void ShuffleDeck()
        {
            var random = new Random(Seed);
            int n = Deck.Count();
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card card = Deck[k];
                Deck[k] = Deck[n];
                Deck[n] = card;
            }
        }

        private void DealHands()
        {
            Hands = new List<IList<Card>>();
            for (int i = 0; i <= 3; i++)
                Hands.Add(Deck.GetRange(5 * i, 5));

            Upcard = Deck[20];
        }

        private void ShuffleDeckWithKnownCards()
        {
            ShuffleDeckWithKnownCards();
        }
    }
}
