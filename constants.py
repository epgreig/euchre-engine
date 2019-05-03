#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 17:55:41 2019

@author: ethangreig
"""

# Next suit
NEXT_DICT = {'H':'D', 'D':'H', 'S':'C', 'C':'S'}

# Trick valuation
VALUE_DICT = {'X':0, 'Q':3, 'K':4, 'A':5}
TRUMP_VALUE_DICT = {'9':0, 'T':1, 'Q':2, 'K':3, 'A':4, 'L':5, 'J':6}

# Decks
DECK = [j + i for i in ['H', 'D', 'S', 'C'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
H_DECK = [j + i for i in ['T'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']] + [j + i for i in ['N', 'A', 'B'] for j in ['X', 'X', 'X', 'Q', 'K', 'A']]
H_DECK[8] = 'LT'
D_DECK = [j + i for i in ['N'] for j in ['X', 'X', 'X', 'Q', 'K', 'A']] + [j + i for i in ['T'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']] + [j + i for i in ['A', 'B'] for j in ['X', 'X', 'X', 'Q', 'K', 'A']]
D_DECK[2] = 'LT'
S_DECK = [j + i for i in ['A', 'B'] for j in ['X', 'X', 'X', 'Q', 'K', 'A']] + [j + i for i in ['T'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']] + [j + i for i in ['N'] for j in ['X', 'X', 'X', 'Q', 'K', 'A']]
S_DECK[20] = 'LT'
C_DECK = [j + i for i in ['A', 'B', 'N'] for j in ['X', 'X', 'X', 'Q', 'K', 'A']] + [j + i for i in ['T'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
C_DECK[14] = 'LT'

# Hand Scoring
TRUMP_PTS = {'J': 9.5, 'L': 7.0, 'A': 6.0, 'K': 5.0, 'Q': 4.5, 'T': 4.0, '9': 4.0}
ACE_PTS = {'N': {'singleton': 2.0, 'paired': 1.0, 'more': 1.0}, 'G': {'singleton': 2.5, 'paired': 2.0, 'more': 1.0}}
KING_PTS = {'G': {'singleton': 0.5}, 'other': 0.5}
SUITED_BONUS = {2: 0.5, 4: -2.5} # 2-suited bonus is multiplied by number of trumps
SEAT_BONUS = {1: 0.0, 2: -0.5, 3: -1.5, 0: 0.0}
THRESHOLD = {'call': 17, 'pass': 12.5}

# Hand Scoring - First Round
TOPCARD_BONUS = {'J': 4.0, 'A': 2.5, 'K': 2.0, 'Q': 1.5, 'T': 1.5, '9': 1.0}
TOPCARD_PENALTY = {'J': -5.0, 'A': -4.0, 'K': -3.5, 'Q': -3.0, 'T': -3.0, '9': -3.0}

# Hand Scoring - Second Round
NEXT_SEAT_ADJ = {1: 2.5, 2: -2.0, 3: 1.5, 0: -1.0}
GREEN_SEAT_ADJ = {1: -1.5, 2: 1.0, 3: -0.5, 0: 0.5}
SEAT_ADJ_FACTOR = {'J': 1.0, 'A': 0.5, 'K': 0.45, 'Q': 0.35, 'T': 0.3, '9': 0.25}