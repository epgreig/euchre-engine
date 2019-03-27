/* constants.cpp */
using namespace std;
#ifndef MY_CONSTANTS
#define MY_CONSTANTS

namespace MyConstants
{
    map<char, char> next_dict;
    next_dict.insert(pair<char, char>('H', 'D'));
    next_dict.insert(pair<char, char>('D', 'H'));
    next_dict.insert(pair<char, char>('S', 'C'));
    next_dict.insert(pair<char, char>('C', 'S'));

    map<char, int> trump_value_dict;
    trump_value_dict.insert(pair<char, int>('J', 6));
    trump_value_dict.insert(pair<char, int>('L', 5));
    trump_value_dict.insert(pair<char, int>('A', 4));
    trump_value_dict.insert(pair<char, int>('K', 3));
    trump_value_dict.insert(pair<char, int>('Q', 2));
    trump_value_dict.insert(pair<char, int>('T', 1));
    trump_value_dict.insert(pair<char, int>('9', 0));

    map<char, int> value_dict;
    trump_value_dict.insert(pair<char, int>('A', 5));
    trump_value_dict.insert(pair<char, int>('K', 4));
    trump_value_dict.insert(pair<char, int>('Q', 3));
    trump_value_dict.insert(pair<char, int>('J', 2));
    trump_value_dict.insert(pair<char, int>('T', 1));
    trump_value_dict.insert(pair<char, int>('9', 0));
}

#endif