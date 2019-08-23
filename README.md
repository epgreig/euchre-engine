# euchre-engine

## Purpose
To build a Euchre AI which is able to play any hand of euchre. 

## Details
This engine will be buit using a Reinforced Learning algorithm. Given a standard (non-alone) Eucre hand of 5 cards and some details about who called Trump, the engine will be able to recommend which cards to play 

## Declaring Trump
For now, the process of declaring trump will not be executed by this engine. What I have done is created a relatively simple points system to estimate how likely any bidding situation is to result in Bid or Pass. From this algorithm, I can randomly generate a Euchre deal, and apply the algorithm to each player until the result is a Trump declaration. Note that according to the ruleset I am using, "Stick the Dealer", the dealer must always select a trump suit in the second round of voting. Also note that this points algorithm will also be used to determine the discarded card if the dealer is "ordered up". So my algorithm maps each random deal to a Trump declaration, which in turn generates four realistic Hand Scenarios, one for each seat in the round.

## Hand Scenarios
There are five variables which determine a unique Hand Scenario for a particular player ("Player"):
1. Player's seat
2. Player's 5 cards (relative to trump)
3. The upcard
4. Which seat declared trump
5. Player's discard (if Player is the dealer and picked up the upcard)

Note: There are two notable omissions
a) it doesn't matter which actual suit is trump, because the suits of all cards in the deck (notably, the upcard and your hand) can be permuted without changing the scenario.
b) the information of whether the trump was declared in the first or second is implicit in the upcard (the suit of the upcard is T iff trump was declared in the first round)

### Number of Unique Hand Scenarios

#### Count
- possible # trump suits: 1 (see notes)
- **Case 1: Trump declared in first round**
    - possible # upcards: 6
    - **Case 1.1: Player is not the dealer**
        - possible # seats: 3
        - possible # trump declarers: 4
        - possible # hand combinations: 17,633
    - **Case 1.2: Player is the dealer**
        - possible # seats: 1
        - **Case 1.2.1: Player declared trump**
            - possible # trump declarers: 1
            - possible # hands: 17,633
            - possible # discarded cards: 19
        - **Case 1.2.2: Player did not declare trump**
            - possible # trump declarers: 3
            - possible # hands: 17,633
- **Case 2: Trump declared in second round**
    - possible # seats: 4
    - possible # trump declarers: 4
    - **Case 2.1: Upcard was member of Next suit**
        - possible # upcards: 6
        - possible # hands: 17,633
    - **Case 2.1: Upcard was member of Green suit**
        - possible # upcards: 6
        - possible # hands: 33,649

Which results in a total of **8,502,204**

#### Notes
- The two Green suits (different colour from trump) are interchangeable when considering unique Hand Scenarios, unless the upcard was a member of one of those suits (in which case the extra information differentiates the two Green suits). For example, if Hearts are trump, (JH, JD, KS, QS, TC) and (JH, JD, KC, QC, TS) are identical Hand Scenarios iff the upcard was either a Heart or a Diamond.
- This count includes some Hand Scenarios which are theoretically possible but would never be found in a practical game of Euchre, either because your hand is too strong or too weak to justify the trump declaration result. These types of hands will not be seen by the engine because they'll be filtered out by my trump declaration algorithm.
- 33,649 is the number of possible hands made up of all cards not including the upcard, where the two Green suits are distinguishable. (23 choose 5)
- 17,633 is the number of possible hands made up of all cards not including the upcard, where the two Green suits are non-distinguishable.
- In the implementation of my euchre engine, non-trump Jacks, Tens, and Nines are treated as identical cards. This reduces the effective number of unique Hand Scenarios. Since the specific rank of these cards very rarely affect the outcome of a trick, it is a wothwhile simplification to allow the engine to learn faster by focusing on more important details in the hands.
