#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sun Apr 21 20:57:15 2019

@author: ethangreig
"""

from generate_hands import Deal
from collections import defaultdict

starting_scenarios = defaultdict(list)

for i in range(250000):
    d = Deal()
    d.bid()
    caller = d.caller
    for j in range(4):
        seat = (j+1)%4
        discard = None
        if caller == seat == 0 and d.topcard[1] == 'T' and d.topcard[0] != 'L':
            discard = d.discard

        ss = (tuple(sorted(d.hands[j])), seat, caller, d.topcard, discard)
        starting_scenarios[ss].append(d.hands)