#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 17:55:41 2019

@author: ethangreig
"""

# Next suit
next_dict = {'H':'D', 'D':'H', 'S':'C', 'C':'S'}

# Trick valuation
value_dict = {'9':0, 'T':1, 'J':2, 'Q':3, 'K':4, 'A':5}
trump_value_dict = {'9':0, 'T':1, 'Q':2, 'K':3, 'A':4, 'L':5, 'J':6}

# Decks
deck = [j + i for i in ['H', 'D', 'S', 'C'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
h_deck = [j + i for i in ['T', 'N', 'A', 'B'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
h_deck[8] = 'LT'
d_deck = [j + i for i in ['N', 'T', 'A', 'B'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
d_deck[2] = 'LT'
s_deck = [j + i for i in ['A', 'B', 'T', 'N'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
s_deck[20] = 'LT'
c_deck = [j + i for i in ['A', 'B', 'N', 'T'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
c_deck[14] = 'LT'

# Hand Scoring
trump_pts = {'J': 9.5, 'L': 7.5, 'A': 6.5, 'K': 5.5, 'Q': 5.0, 'T': 4.5, '9': 4.0}
ace_pts = {'N': {'single': 3.0, 'paired': 1.5, 'more': 0.0}, 'G': {'single': 3.5, 'paired': 2.0, 'more': 0.5}}
king_pts = {'G': {'single': 0.5}}
topcard_bonus = {'J': 7.5, 'A': 5.0, 'K': 4.0, 'Q': 3.5, 'T': 3.5, '9': 3.0}
topcard_penalty = {'J': -6.0, 'A': -4.0, 'K': -3.5, 'Q': -3.5, 'T': -3.0, '9': -3.0}
next_seat_bonus = {1: 2.0, 2: -2.0, 3: 1.0, 0: -1.0}
green_seat_bonus = {1: -2.0, 2: 1.5, 3: -1.0, 4: 0.5}
seat_bonus_factor = {'J': 1.0, 'A': 0.75, 'K': 0.75, 'Q': 0.5, 'T': 0.5, '9': 0.5}