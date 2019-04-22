#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sun Apr 21 20:57:15 2019

@author: ethangreig
"""

from generate_hands import Deal
from collections import defaultdict

class StartingScenario:
    def __init__(self, hand, seat, caller, topcard, discard=None, seed=0):
        self.hand = hand
        self.seat = seat
        self.caller = caller
        self.topcard = topcard
        if self.topcard[1] == 'T':
            self.call_round = 1
        else:
            self.call_round = 2

        self.discard = discard
        self.seed = seed
    
    def __eq__(self, other):
        if isinstance(other, self.__class__):
            if (self.hand == other.hand and
                self.seat == other.seat and
                self.caller == other.caller and
                self.topcard == other.topcard and
                self.discard == other.discard):
                return True
        else:
            return False
    
    def __hash__(self):
        return id(self)

starting_scenarios = defaultdict(list)

for i in range(100000):
    d = Deal()
    d.bid()
    caller = d.caller
    for j in range(4):
        seat = (j+1)%4
        discard = None
        if caller == seat == 0 and d.topcard[1] == 'T' and d.topcard[0] != 'L':
            discard = d.discard

        ss = StartingScenario(d.hands[j], seat, caller, d.topcard, discard, d.seed)
        starting_scenarios[ss].append(d.hands)