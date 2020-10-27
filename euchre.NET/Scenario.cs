using System;
using System.Collections.Generic;
using System.Linq;

namespace euchre.NET
{
    public class Scenario
    {
        public Deal Deal;
        public int Caller;
        public char TrumpSuit;
        public List<IEnumerable<Card>> Hands;
        public Card Upcard;
        public Card Downcard;
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
            bool dealerPickup = false;

            for (int i = 0; i <= 3; i++)
            {
                int seat = (i + 1) % 4;
                if (bidder.OrderUp(Deal.Hands[i], Deal.Upcard, seat, out Card? discard))
                {
                    Caller = seat;
                    TrumpSuit = Deal.Upcard.Suit;
                    pickup = true;
                    if (seat == 0)
                    {
                        Downcard = (Card)discard;
                        dealerPickup = true;
                    }

                    break;
                }
            }

            if (!pickup)
            {
                for (int i = 0; i <= 3; i++)
                {
                    int seat = (i + 1) % 4;
                    if (bidder.Declare(Deal.Hands[i], Deal.Upcard, seat, out char? trump))
                    {
                        TrumpSuit = (char)trump;
                        Caller = seat;
                        break;
                    }
                }
            }

            if (pickup && !dealerPickup)
                bidder.SelectDiscard(Deal.Hands[3], Deal.Upcard, out Downcard);
            else if (!pickup)
                Downcard = Deal.Upcard;
        }

        private void TrumpifyDeck()
        {
            Downcard.Trumpify(TrumpSuit);
            Upcard = Upcard.Copy().Trumpify(TrumpSuit);
            Hands = new List<IEnumerable<Card>>();
            foreach (var hand in Deal.Hands)
                Hands.Append(hand.Select(c => c.Copy().Trumpify(TrumpSuit)));
        }

        private void Encode()
        {

        }

        private void Decode(string encoded)
        {

        }
    }
}
