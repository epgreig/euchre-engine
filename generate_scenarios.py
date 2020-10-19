#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sun Oct 18 19:09:34 2020

@author: ethangreig
"""

from scenario import Scenario
import pickle

# to start a new file from scratch, run:
#starting_scenarios = defaultdict(list)

try:
    scenarios = pickle.load(open("scenarios.pickle", "rb"))
except (OSError, IOError):
    print("pickle not found, starting a new pickle file")
    scenarios = list()

for i in range(50000):
    s = Scenario()
    scenarios.append(s.encoded)

with open("scenarios.pickle", "wb") as f:
    pickle.dump(scenarios, f)
