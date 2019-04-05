#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 17:55:41 2019

@author: ethangreig
"""

# Next suit
NEXT_DICT = {'H':'D', 'D':'H', 'S':'C', 'C':'S'}

# Trick valuation
VALUE_DICT = {'9':0, 'T':1, 'J':2, 'Q':3, 'K':4, 'A':5}
TRUMP_VALUE_DICT = {'9':0, 'T':1, 'Q':2, 'K':3, 'A':4, 'L':5, 'J':6}

# Decks
DECK = [j + i for i in ['H', 'D', 'S', 'C'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
H_DECK = [j + i for i in ['T', 'N', 'A', 'B'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
H_DECK[8] = 'LT'
D_DECK = [j + i for i in ['N', 'T', 'A', 'B'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
D_DECK[2] = 'LT'
S_DECK = [j + i for i in ['A', 'B', 'T', 'N'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
S_DECK[20] = 'LT'
C_DECK = [j + i for i in ['A', 'B', 'N', 'T'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
C_DECK[14] = 'LT'

# Hand Scoring
TRUMP_PTS = {'J': 9.5, 'L': 7.5, 'A': 6.5, 'K': 5.5, 'Q': 5.0, 'T': 4.5, '9': 4.0}
ACE_PTS = {'N': {'singleton': 3.0, 'paired': 1.5, 'more': 0.0}, 'G': {'singleton': 3.5, 'paired': 2.0, 'more': 0.5}}
KING_PTS = {'G': {'singleton': 0.5}}
SUITED_BONUS = {2: 1, 4: -2.5}

# Hand Scoring - First Round
TOPCARD_BONUS = {'J': 7.5, 'A': 5.0, 'K': 4.0, 'Q': 3.5, 'T': 3.5, '9': 3.0}
TOPCARD_PENALTY = {'J': -6.0, 'A': -4.0, 'K': -3.5, 'Q': -3.5, 'T': -3.0, '9': -3.0}

# Hand Scoring - Second Round
NEXT_SEAT_BONUS = {1: 2.0, 2: -2.0, 3: 1.0, 0: -1.0}
GREEN_SEAT_BONUS = {1: -2.0, 2: 1.5, 3: -1.0, 4: 0.5}
SEAT_BONUS_FACTOR = {'J': 1.0, 'A': 0.75, 'K': 0.75, 'Q': 0.5, 'T': 0.5, '9': 0.5}