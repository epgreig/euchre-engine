#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 18:06:42 2019

@author: ethangreig
"""

from constants import NEXT_DICT
from constants import DECK, H_DICT, D_DICT, S_DICT, C_DICT
from constants import TRUMP_PTS, ACE_PTS, KING_PTS, SUITED_BONUS, SEAT_BONUS, THRESHOLD
from constants import TOPCARD_BONUS, TOPCARD_PENALTY
from constants import NEXT_SEAT_ADJ, GREEN_SEAT_ADJ, SEAT_ADJ_FACTOR
import random

class Deal:
    def __init__(self, fixed_hand=False, seat=0, topcard='00', seed=0):
        if seed == 0:
            self.seed = random.uniform(0,1)
        else:
            self.seed = seed
        self.fixed_hand = fixed_hand
        if self.fixed_hand != False:
            self.fixed_hand_ids = [i for i in range(24) if DECK[i] in self.fixed_hand]
            self.deck = [card for card in DECK if card not in fixed_hand + [topcard]]
            random.Random(self.seed).shuffle(self.deck)
            self.hands = []
            indicator = 0
            for i in range(4):
                if (i+1)%4 == seat:
                    self.hands.append(fixed_hand)
                    indicator = 1
                else:
                    self.hands.append(self.deck[(i-indicator):15:3])
            assert len(self.hands) == 4
            self.topcard = topcard
        else:
            self.deck = DECK.copy()
            random.Random(self.seed).shuffle(self.deck)
            self.hands = [self.deck[i:20:4] for i in range(0, 4)]
            self.topcard = self.deck[20]
    
    def bid(self):
        self.pickup = True
        if(declare(self.hands[0], self.topcard, 1, 1)):
            self.trump = self.topcard[1]
            self.caller = 1
        elif(declare(self.hands[1], self.topcard, 2, 1)):
            self.trump = self.topcard[1]
            self.caller = 2
        elif(declare(self.hands[2], self.topcard, 3, 1)):
            self.trump = self.topcard[1]
            self.caller = 3
        elif(declare(self.hands[3], self.topcard, 0, 1)):
            self.trump = self.topcard[1]
            self.caller = 0
        else:
            self.pickup = False
            for i in range(4):
                seat = (i+1)%4
                d = declare(self.hands[i], self.topcard, seat, 2)
                if(d != False):
                    self.trump = d
                    self.caller = seat
                    break

        if(self.pickup):
            self.dealer_discard()
        
        self.trumpify()
    
    def dealer_discard(self):
        self.discard = best_discard(self.hands[3], self.topcard)[0]
        self.hands[3].remove(self.discard)
        self.hands[3].append(self.topcard)
        assert len(self.hands[3]) == 5
    
    def trumpify(self):
        if self.trump == 'H':
            T_DICT = H_DICT
        elif self.trump == 'D':
            T_DICT = D_DICT
        elif self.trump == 'S':
            T_DICT = S_DICT
        elif self.trump == 'C':
            T_DICT = C_DICT
        
        self.topcard = T_DICT[self.topcard]
        for i in range(4):
            self.hands[i] = [T_DICT[card] for card in self.hands[i]]
        
        if self.pickup:
            self.discard = T_DICT[self.discard]


def best_discard(hand, topcard):
    scores = {}
    scores[topcard] = score_hand(hand, topcard[1])
    for card in hand:
        new_hand = hand.copy()
        new_hand.remove(card)
        new_hand.append(topcard)
        scores[card] = score_hand(new_hand, topcard[1], card[1])
        
    discard = max(scores.keys(), key=(lambda key: scores[key]))
    return discard, scores[discard]


def declare(hand, topcard, seat, rnd):
    if rnd == 1:
        score = score_hand_scenario(hand, topcard[1], topcard, seat)
        if seat == 1:
            # Check is better off waiting until next turn
            scores = {}
            for suit in ['H', 'D', 'S', 'C']:
                if suit != topcard[1]:
                    scores[suit] = score_hand_scenario(hand, suit, topcard, seat)

            if max(scores.values()) > score:
                return False
            
        if score > THRESHOLD['call']:
            return topcard[1]
        else:
            interval = THRESHOLD['call'] - THRESHOLD['pass']
            threshold = THRESHOLD['pass'] + random.uniform(0,1) * interval
            if score > threshold:
                return topcard[1]
            else:
                return False

    elif rnd == 2:
        scores = {}
        for suit in ['H', 'D', 'S', 'C']:
            if suit != topcard[1]:
                scores[suit] = score_hand_scenario(hand, suit, topcard, seat)
        
        best_suit = max(scores.keys(), key=(lambda key: scores[key]))
        score = scores[best_suit]
        if seat == 0 or score > THRESHOLD['call']:
            return best_suit
        else:
            interval = THRESHOLD['call'] - THRESHOLD['pass']
            threshold = THRESHOLD['pass'] + random.uniform(0,1) * interval
            if score > threshold:
                return best_suit
            else:
                return False


def score_hand_scenario(hand, suit, topcard, seat):
    if suit == topcard[1]:
        if seat == 1 or seat == 3:
            score = score_hand(hand, suit)
            score += TOPCARD_PENALTY[topcard[0]]
        elif seat == 2:
            score = score_hand(hand, suit)
            score += TOPCARD_BONUS[topcard[0]]
        elif seat == 0:
            discard, score = best_discard(hand, topcard)
    elif suit == NEXT_DICT[topcard[1]]:
        score = score_hand(hand, suit, topcard[1])
        score += NEXT_SEAT_ADJ[seat] * SEAT_ADJ_FACTOR[topcard[0]]
    else:
        score = score_hand(hand, suit, topcard[1])
        score += GREEN_SEAT_ADJ[seat] * SEAT_ADJ_FACTOR[topcard[0]]
    
    score += SEAT_BONUS[seat]
    return score


def score_hand(hand, suit, discarded_suit=None):
    next_suit = NEXT_DICT[suit]
    hand = ['L'+suit if card == 'J'+next_suit else card for card in hand]
    score = 0
    suits_in_hand = [card[1] for card in hand]
    suit_counts = dict([(suit, suits_in_hand.count(suit)) for suit in set(suits_in_hand)])
    for card in hand:
        if card[1] == suit:
            score += TRUMP_PTS[card[0]]
        else:
            suit_count = suit_counts[card[1]]
            if discarded_suit == card[1]:
                suit_count += 1

            if card[0] == 'A':
                if card[1] == next_suit:
                    if suit_count == 1:
                        score += ACE_PTS['N']['singleton']
                    elif suit_count == 2:
                        score += ACE_PTS['N']['paired']
                    else:
                        score += ACE_PTS['N']['more']
                else:
                    if suit_count == 1:
                        score += ACE_PTS['G']['singleton']
                    elif suit_count == 2:
                        score += ACE_PTS['G']['paired']
                    else:
                        score += ACE_PTS['G']['more']
            elif card[0] == 'K':
                if suit_count == 1 and card[1] != next_suit:
                    score += KING_PTS['G']['singleton']
                else:
                    score += KING_PTS['other']
    
    if len(suit_counts) < 3:
        if suit in suit_counts:
            score += SUITED_BONUS[2]*suit_counts[suit]
    elif len(suit_counts) == 4:
        score += SUITED_BONUS[4]
    
    return score
            

d=Deal(seed=0.7307849952015257)
d.hands
d.topcard
d.bid()