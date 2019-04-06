#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 18:06:42 2019

@author: ethangreig
"""

from constants import NEXT_DICT, VALUE_DICT, TRUMP_VALUE_DICT
from constants import DECK, H_DECK, D_DECK, S_DECK, C_DECK
from constants import TRUMP_PTS, ACE_PTS, KING_PTS, SUITED_BONUS, THRESHOLD
from constants import TOPCARD_BONUS, TOPCARD_PENALTY
from constants import NEXT_SEAT_BONUS, GREEN_SEAT_BONUS, SEAT_BONUS_FACTOR
import random

class Deal:
    def __init__(self):
        self.deck = DECK
        self.seed = random.uniform(0,1)
        random.Random(self.seed).shuffle(self.deck)
        self.hands = [set(self.deck[i::5]) for i in range(0, 4)]
        self.topcard = self.deck[4]
    
    def bid(self):
        if(declare(self.hands[0], self.topcard, 1, 1)):
            self.trump = self.topcard[1]
            self.caller = 1
            self.hands[3] = best_five(self.hands[3], self.topcard)
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
            for i in range(4):
                seat = (i+1)%4
                d = declare(self.hands[i], self.topcard, seat, 2)
                if(d):
                    self.trump = d
                    self.caller = seat
                    break

        if(self.trump == self.topcard[1]):
            self.dealer_discard()
        
        self.trumpify()
        
    
    def trumpify(self):
        if(self.trump == 'H'):
            self.deck = H_DECK
        elif(self.trump == 'D'):
            self.deck = D_DECK
        elif(self.trump == 'S'):
            self.deck = S_DECK
        elif(self.trump == 'C'):
            self.deck = C_DECK

        random.Random(self.seed).shuffle(self.deck)
        self.hands = [set(self.deck[i::5]) for i in range(0, 4)]
        self.topcard = self.deck[4]
    
    def dealer_discard(self):
        discard = best_discard(self.hands[3], self.topcard)
        self.hands[3].add(self.topcard)
        self.hands[3].remove(discard)
        assert len(self.hands[3]) == 5


def best_discard(hand, topcard):
    scores = {}
    scores[topcard] = score_hand(hand, topcard[1])
    for card in hand:
        new_hand = hand
        new_hand.remove(card)
        new_hand.add(topcard)
        scores[card] = score_hand(new_hand, topcard[1])
        
    discard = max(scores.iterkeys(), key=(lambda key: scores[key]))
    return discard, scores[discard]


def declare(hand, topcard, seat, rnd):
    if rnd == 1:
        score = score_hand_scenario(hand, topcard[1], topcard, seat)
        if seat == 1:
            scores = {}
            for suit in ['H', 'D', 'S', 'C']:
                if suit != topcard[1]:
                    hypothetical_score = score_hand_scenario(hand, suit, topcard, seat)
                    scores[suit] = hypothetical_score
            
            if max(scores.values()) > score:
                return False
            
        if score > THRESHOLD:
            return topcard[1]
        else:
            return False
    elif rnd == 2:
        scores = {}
        for suit in ['H', 'D', 'S', 'C']:
            if suit != topcard[1]:
                hypothetical_score = score_hand_scenario(hand, suit, topcard, seat)
                scores[suit] = hypothetical_score
        
        if seat == 0 or max(scores.values()) > THRESHOLD:
            return max(scores.iterkeys(), key=(lambda key: scores[key]))
        else:
            return False


def score_hand_scenario(hand, suit, topcard, seat):
    if suit == topcard[1]:
        if seat == 1 or seat == 3:
            score = score_hand(hand, suit)
            score += TOPCARD_PENALTY[suit]
        elif seat == 2:
            score = score_hand(hand, suit)
            score += TOPCARD_BONUS[suit]
        elif seat == 0:
            discard, score = best_discard(hand, topcard)
    elif suit == NEXT_DICT[topcard[1]]:
        score = score_hand(hand, suit)
        score += NEXT_SEAT_BONUS[seat] * SEAT_BONUS_FACTOR[topcard[0]]
    else:
        score = score_hand(hand, suit)
        score += GREEN_SEAT_BONUS[seat] * SEAT_BONUS_FACTOR[topcard[0]]
    
    return score


def score_hand(hand, suit):
    next_suit = NEXT_DICT[suit]
    hand = ['L'+suit if card == 'J'+next_suit else card for card in hand]
    score = 0
    suits_in_hand = [card[1] for card in hand]
    suit_counts = dict([(suit, suits_in_hand.count(suit)) for suit in set(suits_in_hand)])
    for card in hand:
        if card[1] == suit:
            score += TRUMP_PTS[card[0]]
        elif card[0] == 'A':
            suit_count = suit_counts[card[1]]
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
            suit_count = suit_counts[card[1]]
            if suit_count == 1 and card[1] != next_suit:
                score += KING_PTS['G']['singleton']
    
    if len(suit_counts) < 3:
        score += SUITED_BONUS[2]*suit_counts[suit]
    elif len(suit_counts) == 4:
        score += SUITED_BONUS[4]
    
    return score
            