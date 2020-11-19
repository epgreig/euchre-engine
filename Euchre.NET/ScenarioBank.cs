using System;
using System.Collections.Generic;
using System.Linq;

namespace Euchre.NET
{
    public class ScenarioBank
    {
        private bool _paused;
        private Random _random;
        public IList<Scenario> _relevantScenarios;
        private IList<IList<Card>> _knownCards;
        private IList<IList<char>> _knownVoids;
        private Card _upcard;
        private Card? _downcard;
        private int _perspective;
        private int _caller;
        private char _trump;

        private const int CAPACITY = 16;

        public event EventHandler RelevantScenariosChanged;
        public event EventHandler PauseChanged;

        public bool Paused
        {
            get => _paused;
            set
            {
                _paused = value;
                OnPauseChanged(EventArgs.Empty);
            }
        }

        public ScenarioBank(int seat, List<Card> hand, Card upcard, int caller, char trump, Card? downcard = null)
        {
            _paused = false;
            _random = new Random();
            _relevantScenarios = new List<Scenario>();

            _perspective = seat;
            _caller = caller;
            _trump = trump;

            _knownVoids = new List<IList<char>>(4) { new List<char>(3), new List<char>(3), new List<char>(3), new List<char>(3) };
            _knownCards = new List<IList<Card>>(4) { new List<Card>(5), new List<Card>(5), new List<Card>(5), new List<Card>(5) };
            //_knownCards[seat] = hand.Select(c => c.Trumpify(_trump)).ToList();
            _knownCards[seat] = hand;

            _upcard = upcard;
            _downcard = downcard;

            GenerateScenarios();
        }

        public void RevealRound(int seatStart, List<Card> round)
        {
            var followSuit = round[0].Suit;
            for (int seat = 0; seat < 4; seat++)
            {
                if (seat != _perspective)
                {
                    Card seatCard = round[(((seat - seatStart) % 4) + 4) % 4];
                    _knownCards[seat].Add(seatCard);

                    if (seat != seatStart)
                        _knownVoids[seat].Add(seatCard.Suit);
                }
            }

            GenerateScenarios();
        }

        private void GenerateScenarios()
        {
            int attempts = 0;
            while (!_paused && attempts < 10000 && _relevantScenarios.Count() <= CAPACITY)
            {
                var d = new Deal(_knownCards, _upcard, _random.Next());
                var s = new Scenario(d);
                if (s.Caller == _caller && s.TrumpSuit == _trump)
                    _relevantScenarios.Add(s);
                //OnRelevantScenariosChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnRelevantScenariosChanged(EventArgs e)
        {
            EventHandler handler = RelevantScenariosChanged;
            handler?.Invoke(this, e);
        }

        protected virtual void OnPauseChanged(EventArgs e)
        {
            EventHandler handler = PauseChanged;
            handler?.Invoke(this, e);
        }
    }
}
