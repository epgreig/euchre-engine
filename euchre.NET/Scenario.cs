using System;
namespace euchre.NET
{
    public class Scenario
    {
        public Deal Deal;
        public int Caller;
        public char? TrumpSuit;
        public Even
        public readonly int Seed;
        public string Encoded;

        public Scenario()
        {
            Deal = new Deal();
            Seed = (new Random()).Next();
            ExecuteBiddingRound();
        }

        public Scenario(Deal deal)
        {
            Deal = deal;
            Seed = (new Random()).Next();
            ExecuteBiddingRound();
        }

        public Scenario(Deal deal, int seed)
        {
            Deal = deal;
            Seed = seed;
            ExecuteBiddingRound();
        }

        public Scenario(string encoded)
            => Decode(encoded);

        private void ExecuteBiddingRound()
        {
            DetermineBid();
            TrumpifyDeck();
            Encode();
        }

        private void DetermineBid()
        {
            var bidder = new Bidder(new Random(Seed));
            bool pickup = false;

            for (int i = 0; i <= 3; i++)
            {
                int seat = (i + 1) % 4;
                if (bidder.OrderUp(Deal.Hands[i], Deal.Upcard, seat))
                {
                    Caller = seat;
                    TrumpSuit = Deal.Upcard.Suit;
                    pickup = true;
                    break;
                }
            }

            if (!pickup)
            {
                for (int i = 0; i <= 3; i++)
                {
                    int seat = (i + 1) % 4;
                    if (bidder.Declare(Deal.Hands[i], Deal.Upcard, seat, out TrumpSuit)
                    {
                        Caller = seat;
                        break;
                    }
                }
            }

            if (pickup)

        }

        private void TrumpifyDeck()
        {

        }

        private void Encode()
        {

        }

        private void Decode(string encoded)
        {

        }
    }
}
