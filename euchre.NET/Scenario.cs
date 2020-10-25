using System;
namespace euchre.NET
{
    public class Scenario
    {
        public Deal Deal;
        public Random RNG;
        public string Encoded;

        public Scenario()
        {
            Deal = new Deal();
            RNG = new Random();
            ExecuteBiddingRound();
        }

        public Scenario(Deal deal)
        {
            Deal = deal;
            RNG = new Random();
            ExecuteBiddingRound();
        }

        public Scenario(Deal deal, int seed)
        {
            Deal = deal;
            RNG = new Random(seed);
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
            var pickup = true;
            if (Caller.Declare(Deal.Hands[0], Deal.Upcard, 1, 1, RNG, out char trump))


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
