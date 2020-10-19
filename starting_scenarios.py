#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sun Apr 21 20:57:15 2019

@author: ethangreig
"""

from generate_hands import Deal
from collections import defaultdict
import pickle

# to start a new file from scratch, run:
#starting_scenarios = defaultdict(list)

try:
    starting_scenarios = pickle.load(open("starting_scenarios.pickle", "rb"))
except (OSError, IOError) as e:
    print("pickle not found, starting a new pickle file")
    starting_scenarios = defaultdict(list)

for i in range(5000):
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

with open("starting_scenarios.pickle", "wb") as f:
    pickle.dump(starting_scenarios, f)