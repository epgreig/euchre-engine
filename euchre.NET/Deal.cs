using System;
namespace euchre.NET
{
    public class Deal
    {
        double? Seed;

        public Deal()
        {
            var random = new Random();
            Seed = random.NextDouble();

        self.deck = DECK.copy()
        random.Random(self.seed).shuffle(self.deck)
        self.hands = [self.deck[i: 20:4] for i in range(0, 4)]
        self.topcard = self.deck[20]

        }
        
        public Deal(decimal seed)
        {
            Seed = seed;

        }
    }
}
