#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 17:55:41 2019

@author: ethangreig
"""

import sys

# Next suit
NEXT_DICT = {'H':'D', 'D':'H', 'S':'C', 'C':'S'}

# Trick valuation
VALUE_DICT = {'X':0, 'Q':3, 'K':4, 'A':5}
TRUMP_VALUE_DICT = {'9':0, 'T':1, 'Q':2, 'K':3, 'A':4, 'L':5, 'J':6}

# Decks
DECK = [j + i for i in ['H', 'D', 'S', 'C'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]
# use dictionaries in memory to accelerate the trumpification of the deck
H_DICT = {'JH':'JT', 'JD':'LT', 'AH':'AT', 'KH':'KT', 'QH':'QT', 'TH':'TT', '9H':'9T',
          'AD':'AN', 'KD':'KN', 'QD':'QN', 'TD':'XN', '9D':'XN',
          'AS':'AA', 'KS':'KA', 'QS':'QA', 'JS':'XA', 'TS':'XA', '9S':'XA',
          'AC':'AB', 'KC':'KB', 'QC':'QB', 'JC':'XB', 'TC':'XB', '9C':'XB'}
D_DICT = {'JD':'JT', 'JH':'LT', 'AD':'AT', 'KD':'KT', 'QD':'QT', 'TD':'TT', '9D':'9T',
          'AH':'AN', 'KH':'KN', 'QH':'QN', 'TH':'XN', '9H':'XN',
          'AS':'AA', 'KS':'KA', 'QS':'QA', 'JS':'XA', 'TS':'XA', '9S':'XA',
          'AC':'AB', 'KC':'KB', 'QC':'QB', 'JC':'XB', 'TC':'XB', '9C':'XB'}
S_DICT = {'JS':'JT', 'JC':'LT', 'AS':'AT', 'KS':'KT', 'QS':'QT', 'TS':'TT', '9S':'9T',
          'AC':'AN', 'KC':'KN', 'QC':'QN', 'TC':'XN', '9C':'XN',
          'AH':'AA', 'KH':'KA', 'QH':'QA', 'JH':'XA', 'TH':'XA', '9H':'XA',
          'AD':'AB', 'KD':'KB', 'QD':'QB', 'JD':'XB', 'TD':'XB', '9D':'XB'}
C_DICT = {'JC':'JT', 'JS':'LT', 'AC':'AT', 'KC':'KT', 'QC':'QT', 'TC':'TT', '9C':'9T',
          'AS':'AN', 'KS':'KN', 'QS':'QN', 'TS':'XN', '9S':'XN',
          'AH':'AA', 'KH':'KA', 'QH':'QA', 'JH':'XA', 'TH':'XA', '9H':'XA',
          'AD':'AB', 'KD':'KB', 'QD':'QB', 'JD':'XB', 'TD':'XB', '9D':'XB'}

# Hand Scoring
TRUMP_PTS = {'J': 9.5, 'L': 6, 'A': 5.5, 'K': 5.0, 'Q': 4.75, 'T': 4.5, '9': 4.25}
ACE_PTS = {'N': {'singleton': 1.75, 'paired': 1.5, 'more': 1.5}, 'G': {'singleton': 2.75, 'paired': 1.75, 'more': 1.5}}
KING_PTS = {'singleton': 0.25, 'paired': 0.5, 'more': 0.25}
SUITED_BONUS = {2: 0.5, 4: -2} # 2-suited bonus is multiplied by number of trumps
SEAT_BONUS = {1: 0.5, 2: 0, 3: -1.5, 0: 1.0}
THRESHOLD = {'call': 17.5, 'pass': 13}

# Hand Scoring - First Round
TOPCARD_BONUS = {'J': 2.75, 'A': 2.25, 'K': 2.0, 'Q': 1.75, 'T': 1.75, '9': 1.75}
TOPCARD_PENALTY = {'J': -2.5, 'A': -1.75, 'K': -1.75, 'Q': -1.5, 'T': -1.5, '9': -1.5}

# Hand Scoring - Second Round
NEXT_SEAT_ADJ = {1: 3, 2: -2.0, 3: 1.5, 0: 0.0}
GREEN_SEAT_ADJ = {1: -1.5, 2: 2.0, 3: -0.5, 0: 0.0}
SEAT_ADJ_FACTOR = {'J': 1.0, 'A': 0.65, 'K': 0.6, 'Q': 0.55, 'T': 0.5, '9': 0.5}


# Encoded string:
# {Caller (0,1,2,3)}{1st Hand}{2nd Hand}{3rd Hand}{Dealer Hand}{Upcard}{Discard}

ENCODING_DICT = {'JT': 'A', 'AN': 'H', 'AA': 'M', 'AB': 'R',
                 'LT': 'B', 'KN': 'J', 'KA': 'N', 'KB': 'S',
                 'AT': 'C', 'QN': 'K', 'QA': 'P', 'QB': 'T',
                 'KT': 'D', 'XN': 'L', 'XA': 'Q', 'XB': 'U',
                 'QT': 'E',
                 'TT': 'F',
                 '9T': 'G'}

DECODING_DICT = {v: k for k, v in ENCODING_DICT.items()}