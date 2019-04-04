#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 18:06:42 2019

@author: ethangreig
"""

from constants import next_dict, value_dict, trump_value_dict
from constants import deck, h_deck, d_deck, s_deck, c_deck
from constants import trump_pts, ace_pts, king_pts
from constants import topcard_bonus, topcard_penalty
from constants import next_seat_bonus, green_seat_bonus, seat_bonus_factor
import random

class Deal:
    def __init__(self):
        self.deck = [j + i for i in ['H', 'D', 'S', 'C'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
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
            self.deck = h_deck
        elif(self.trump == 'D'):
            self.deck = d_deck
        elif(self.trump == 'S'):
            self.deck = s_deck
        elif(self.trump == 'C'):
            self.deck = c_deck

        random.Random(self.seed).shuffle(self.deck)
        self.hands = [set(self.deck[i::5]) for i in range(0, 4)]
        self.topcard = self.deck[4]
    
    def dealer_discard(self):
        scores = {}
        hand = self.hands[3]
        scores[self.topcard] = score(hand, self.trump)
        for card in hand:
            new_hand = hand
            new_hand.remove(card)
            new_hand.add(self.topcard)
            scores[card] = score(new_hand, self.trump)
        
        discard = max(scores.iterkeys(), key=(lambda key: scores[key]))
        self.hands[3].add(topcard)
        self.hands[3].remove(discard)
        assert len(self.hands[3]) == 5
        

        
def declare(hand, topcard, seat, rnd):
    if(rnd == 1):
        if(seat == 1 or seat == 3):
            score = score(hand, topcard[1])
            score -= 
        elif(seat == 2):
            
        elif(seat == 0):

def score(hand, suit):
    score = 0
    for card in hand:
        if card[1] == suit:
            