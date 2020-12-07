using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Euchre.NET
{
    [Serializable]
    public class Scenario
    {
        public Deal Deal;
        public int Caller;
        public char TrumpSuit;
        public List<List<Card>> Hands;
        public Card Upcard;
        public Card Downcard;
        public readonly int Seed;
        public string Serialized;

        public Scenario(Deal deal = null, int? seed = null)
        {
            Deal = deal ?? new Deal();
            Seed = seed ?? (new Random()).Next();
            ExecuteBiddingRound();
        }

        public string Serialize()
        {
            if (string.IsNullOrEmpty(Serialized))
            {
                StringBuilder serialized = new StringBuilder(Caller.ToString(), 23);
                foreach (var hand in Hands)
                {
                    foreach (var card in hand)
                        serialized.Append(card.GetString());
                }

                serialized.Append(Upcard.GetString());
                serialized.Append(Downcard.GetString());
                Serialized = serialized.ToString();
            }

            return Serialized;
        }

        private void ExecuteBiddingRound()
        {
            DetermineBid();
            TrumpifyDeck();
        }

        private void DetermineBid()
        {
            var bidder = new Bidder(new Random(Seed));
            bool pickup = false;
            bool dealerPickup = false;

            foreach (int seat in new[] { 1, 2, 3, 0 })
            {
                if (bidder.OrderUp(Deal.Hands[seat], Deal.Upcard, seat, out Card? discard))
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
                foreach (int seat in new[] { 1, 2, 3, 0 })
                {
                    if (bidder.Declare(Deal.Hands[seat], Deal.Upcard, seat, out char? trump))
                    {
                        TrumpSuit = (char)trump;
                        Caller = seat;
                        break;
                    }
                }
            }

            if (pickup && !dealerPickup)
                bidder.SelectDiscard(Deal.Hands[0], Deal.Upcard, out Downcard);
            else if (!pickup)
                Downcard = Deal.Upcard;
        }

        private void TrumpifyDeck()
        {
            Downcard = Downcard.Trumpify(TrumpSuit);
            Upcard = Deal.Upcard.Trumpify(TrumpSuit);
            Hands = new List<List<Card>>(4);
            foreach (var hand in Deal.Hands)
                Hands.Add(hand.Select(c => c.Copy().Trumpify(TrumpSuit)).ToList());
        }

        public bool DealerHasVoids(IList<Card> knownCards, IList<char> voids)
        {
            var dealerSuits = new List<char>();
            foreach (var card in Deal.Hands[0])
            {
                if (!knownCards.Contains(card))
                    dealerSuits.Add(card.Suit);
            }

            return !voids.Any(s => dealerSuits.Contains(s));
        }
    }
}
