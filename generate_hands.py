#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 18:06:42 2019

@author: ethangreig
"""

import numpy as np
from constants import deck, next_dict, value_dict, trump_value_dict
from random import shuffle

class Deal:
    def __init__(self):
        shuffle(deck)
        self.hands = [self.deck[i::5] for i in range(0, 4)]
        self.topcard = self.deck[4]
    
    def bid(self):
        if(declare(self.hands[0], self.topcard, 1, 1)):
            self.trump = self.topcard[1]
    
    def trumpify(self, suit):
        self.trump = suit
        next_suit = next_dict[trump]
        for i in range(24):
            deck[i] = ['L'+trump if card=='J'+next_suit else card for card in deck]