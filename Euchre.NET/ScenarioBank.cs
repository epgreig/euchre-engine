using System;
using System.Collections.Generic;
using System.Linq;

namespace Euchre.NET
{
    public class ScenarioBank
    {
        private bool _paused;
        private IList<Scenario> _relevantScenarios;
        private IList<Card> _firstSeatCards;
        private IList<Card> _secondSeatCards;
        private IList<Card> _thirdSeatCards;
        private IList<Card> _dealerCards;
        private IEnumerable<char> _firstSeatVoids;
        private IEnumerable<char> _secondSeatVoids;
        private IEnumerable<char> _thirdSeatVoids;
        private IEnumerable<char> _dealerVoids;
        private Card _upcard;
        private Card? _downcard;
        private int _perspective;
        private int _caller;
        private char _trump;

        private const int CAPACITY = 200;

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
            _relevantScenarios = new List<Scenario>();

            _perspective = seat;
            _caller = caller;
            _trump = trump;

            _firstSeatCards = seat == 1 ? hand.Select(c => c.Trumpify(_trump)).ToList() : new List<Card>();
            _secondSeatCards = seat == 2 ? hand.Select(c => c.Trumpify(_trump)).ToList() : new List<Card>();
            _thirdSeatCards = seat == 3 ? hand.Select(c => c.Trumpify(_trump)).ToList() : new List<Card>();
            _dealerCards = seat == 0 ? hand.Select(c => c.Trumpify(_trump)).ToList() : new List<Card>();
            _upcard = upcard.Trumpify(_trump);
            _downcard = downcard?.Trumpify(_trump);

            GenerateScenarios();
        }

        public RevealRound(int seatStart, List<Card> round)
        {
            _firstSeatCards.Add(round[Math.Abs(seatStart - 1)]); 1 -> 0, 2 -> 3, 3 -> 2, 0 -> 1
            _secondSeatCards.Add(round[Math.Abs(seatStart - 2)]); 1 -> 1, 2 -> 0, 3 -> 3, 0 -> 2
            _thirdSeatCards.Add(round[Math.Abs(seatStart - 3)]); 1 -> 2, 2 -> 1, 3 -> 0, 0 -> 3
            _dealerCards.Add(round[Math.Abs(seatStart - 1)]); 1 -> 3, 2 -> 2, 3 -> 1, 0 -> 0
        }

        private void GenerateScenarios()
        {
            while (!_paused && _relevantScenarios.Count() <= CAPACITY)
            {
                //var deal = new Deal(...);
                //var scenario = new Scenario(deal);
                //_relevantScenarios.Add(scenario);
                OnRelevantScenariosChanged(EventArgs.Empty);
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
