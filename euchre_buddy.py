#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Fri Jul 12 16:29:42 2019

@author: ethangreig
"""

from generate_hands import Deal

# Collect Hand Information
seat = input('your seat (0, 1, 2, or 3): ')
hand_string = input('cards in hand (10 characters): ')
topcard = input('topcard (2 characters): ')
caller = input('seat of caller (0, 1, 2, or 3): ')
trump = input('trump suit (H, D, S, or C): ')

# Process Input
hand = [hand_string[0+i:2+i] for i in range(0, 10, 2)]
seat = int(seat)
seat_index = (seat-1)%4
caller = int(caller)

## Generate Suit Mapping
#if trump == 'H':
#    suit_dict = {'T':'H','N':'D','A':'S','B':'C'}
#elif trump == 'D':
#    suit_dict = {'T':'D','N':'H','A':'S','B':'C'}
#elif trump == 'S':
#    suit_dict = {'T':'S','N':'C','A':'H','B':'D'}
#elif trump == 'C':
#    suit_dict = {'T':'C','N':'S','A':'H','B':'D'}

# Generate Deals
deals = []
N = 10000
for i in range(N):
    d = Deal(fixed_hand=hand, seat=seat, topcard=topcard)
    d.bid()
    if d.caller == caller and d.trump == trump:
        deals.append(d.hands)

M = len(deals)

print('Number of deals found: ' + str(M) + '/' + str(N))

# First Trick
trick = input('first trick (eg. 1AH2QH09S): ')

# Parse Input
seatA = trick[0]
seatA_index = (seatA-1)%4
cardA = trick[1:2]
seatB = trick[0]
seatB_index = (seatB-1)%4
cardB = trick[1:2]
seatC = trick[0]
seatC_index = (seatC-1)%4
cardC = trick[1:2]

# Refine deals
deals_refined = deals
for i in range(M):
    deal = deals[i]
    if cardA in deal[seatA_index]:
        







