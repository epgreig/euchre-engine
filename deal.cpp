/* deal.cpp */
using namespace std;
#include <constants.cpp>

class Card {
  public:
    char rank;
    char suit;

    Card(char rk, char st)
        : rank(rk), suit(st)
    {}

    int Rank(char trump)
    {
        char next = next_dict[trump];
        if( suit == trump )
        {
            return trump_value_dict[rank];
        }
        else if( suit == next && rank == 'J' )
        {
            return trump_value_dict['L'];
        }
        else
        {
            return value_dict[rank];
        }
    }
    char Suit(char trump)
    {
        char next = next_dict[trump]
        if( rank == 'J' && suit == next )
        {
            return trump;
        }
        else
        {
            return suit;
        }
    }
}

class Deck {
    const vector<Card> cards;

  public:
    Deck() {}
}