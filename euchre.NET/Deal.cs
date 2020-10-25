using System;
using System.Collections.Generic;
using System.Linq;

namespace euchre.NET
{
    public class Deal
    {
        public readonly int Seed;
        public List<Card> Deck;
        public IList<IEnumerable<Card>> Hands;
        public Card Upcard;

        public Deal()
        {
            Seed = (new Random()).Next();
            ShuffleAndDeal();
        }

        public Deal(int seed)
        {
            Seed = seed;
            ShuffleAndDeal();
        }

        private void ShuffleAndDeal()
        {
            Deck = new List<Card>(Constants.DECK);
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
            Hands = new List<IEnumerable<Card>>();
            for (int i = 0; i <= 3; i++)
                Hands.Append(Deck.GetRange(5 * i, 5)));

            Upcard = Deck[20];
        }
    }
}
