#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Mon Oct 19 21:04:39 2020

@author: ethangreig
"""

from scenario import Scenario
from collections import Counter
import pickle

encoded_scenarios = pickle.load(open("scenarios.pickle", "rb"))

scenarios = []
for encoded_scenario in encoded_scenarios:
    scenarios.append(Scenario(encoded_scenario))

def containedInFirst(a, b):
  a_count = Counter(a)
  b_count = Counter(b)
  for key in b_count:
    if key not in a_count:
      return False
    if b_count[key] > a_count[key]:
      return False
  return True

query_cards = [ 'LT', 'TT', 'QN', 'AA', 'KA' ]
#query_results = []
#
#for scenario in scenarios:
#    for hand in scenario.hands:
#        if containedInFirst(hand, query_cards):
#            query_results.append(scenario)

#print([query_result.encoded for query_result in query_results])
#print(len(query_results))
#print(len(scenarios))

call_result_matrix = [[0,0,0,0],
                      [0,0,0,0]]

for enc in encoded_scenarios:
    if (enc[21] in ['A', 'C', 'D', 'E', 'F', 'G']):
        call_result_matrix[0][(int(enc[0])-1)%4] += 1 / len(encoded_scenarios)
    else:
        call_result_matrix[1][(int(enc[0])-1)%4] += 1 / len(encoded_scenarios)

print(call_result_matrix)
print(sum(call_result_matrix[0]), sum(call_result_matrix[1]))
        