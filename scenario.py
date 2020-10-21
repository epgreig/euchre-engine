#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sun Oct 18 11:30:11 2020

@author: ethangreig
"""

from constants import NEXT_DICT
from constants import DECK, H_DICT, D_DICT, S_DICT, C_DICT
from constants import TRUMP_PTS, ACE_PTS, KING_PTS, SUITED_BONUS, SEAT_BONUS, THRESHOLD
from constants import TOPCARD_BONUS, TOPCARD_PENALTY
from constants import NEXT_SEAT_ADJ, GREEN_SEAT_ADJ, SEAT_ADJ_FACTOR
from constants import ENCODING_DICT, DECODING_DICT
import random


class Deal:
    def __init__(self, seed=None):
        if (seed == None):
            self.seed = random.uniform(0,1)
        else:
            self.seed = seed
        
        self.deck = DECK.copy()
        random.Random(self.seed).shuffle(self.deck)
        self.hands = [self.deck[i:20:4] for i in range(0, 4)]
        self.topcard = self.deck[20]


class Scenario:
    def __init__(self, *args):
        if (len(args) == 0):
            deal = Deal()
        elif isinstance(args[0], str):
            self.decode(args[0])
        elif isinstance(args[0], Deal):
            deal = args[0]
        else:
            raise Exception("input for Scenario must be nothing, an encoded Scenario, a Deal, or a Deal and a seed") 
        
        if (len(args) > 1):
            self.seed = args[1]
        else:
            self.seed = random.uniform(0,1)

        if not hasattr(self, 'encoded'):
            random.Random(self.seed)
            self.bid(deal.hands, deal.topcard)

    def bid(self, suited_hands, topcard):
        pickup = True
        if(declare(suited_hands[0], topcard, 1, 1)):
            trump = topcard[1]
            self.caller = 1
        elif(declare(suited_hands[1], topcard, 2, 1)):
            trump = topcard[1]
            self.caller = 2
        elif(declare(suited_hands[2], topcard, 3, 1)):
            trump = topcard[1]
            self.caller = 3
        elif(declare(suited_hands[3], topcard, 0, 1)):
            trump = topcard[1]
            self.caller = 0
        else:
            pickup = False
            for i in range(4):
                seat = (i+1)%4
                d = declare(suited_hands[i], topcard, seat, 2)
                if (d != False):
                    trump = d
                    self.caller = seat
                    break
        
        discard = topcard
        if (pickup):
            suited_hands[3], discard = dealer_discard(suited_hands[3], topcard)
        
        self.trumpify(trump, suited_hands, topcard, discard)
        self.encode()

    def trumpify(self, trump, hands, topcard, discard):
        if trump == 'H':
            T_DICT = H_DICT
        elif trump == 'D':
            T_DICT = D_DICT
        elif trump == 'S':
            T_DICT = S_DICT
        elif trump == 'C':
            T_DICT = C_DICT

        self.topcard = T_DICT[topcard]
        self.discard = T_DICT[discard]
        self.hands = [None] * 4
        for i in range(4):
            self.hands[i] = [T_DICT[card] for card in hands[i]]

    def encode(self):
        caller = str(self.caller)
        enc_1h = [ENCODING_DICT[card] for card in self.hands[0]]
        enc_1h.sort()
        enc_1h = ''.join(enc_1h)
        enc_2h = [ENCODING_DICT[card] for card in self.hands[1]]
        enc_2h.sort()
        enc_2h = ''.join(enc_2h)
        enc_3h = [ENCODING_DICT[card] for card in self.hands[2]]
        enc_3h.sort()
        enc_3h = ''.join(enc_3h)
        enc_dh = [ENCODING_DICT[card] for card in self.hands[3]]
        enc_dh.sort()
        enc_dh = ''.join(enc_dh)
        enc_tc = ENCODING_DICT[self.topcard]
        enc_dc = ENCODING_DICT[self.discard]
        self.encoded = caller + enc_1h + enc_2h + enc_3h + enc_dh + enc_tc + enc_dc
        assert len(self.encoded) == 23
        
    def decode(self, enc):
        self.encoded = enc
        self.caller = enc[0]
        self.hands = [None] * 4
        self.hands[0] = [DECODING_DICT[card] for card in enc[1:6]]
        self.hands[1] = [DECODING_DICT[card] for card in enc[6:11]]
        self.hands[2] = [DECODING_DICT[card] for card in enc[11:16]]
        self.hands[3] = [DECODING_DICT[card] for card in enc[16:21]]
        self.topcard = DECODING_DICT[enc[21]]
        self.discard = DECODING_DICT[enc[22]]


def declare(hand, topcard, seat, rnd, seed=None):
    if (seed == None):
        seed = random.uniform(0,1)

    if rnd == 1:
        score = score_hand_scenario(hand, topcard[1], topcard, seat)
        if seat == 1:
            # Check is better off waiting until next turn
            scores = {}
            for suit in ['H', 'D', 'S', 'C']:
                if suit != topcard[1]:
                    scores[suit] = score_hand_scenario(hand, suit, topcard, seat)

            if max(scores.values()) > score:
                return False
            
        if score > THRESHOLD['call']:
            return topcard[1]
        else:
            interval = THRESHOLD['call'] - THRESHOLD['pass']
            threshold = THRESHOLD['pass'] + random.uniform(0,1) * interval
            if score > threshold:
                return topcard[1]
            else:
                return False

    elif rnd == 2:
        scores = {}
        for suit in ['H', 'D', 'S', 'C']:
            if suit != topcard[1]:
                scores[suit] = score_hand_scenario(hand, suit, topcard, seat)
        
        best_suit = max(scores.keys(), key=(lambda key: scores[key]))
        score = scores[best_suit]
        if seat == 0 or score > THRESHOLD['call']:
            return best_suit
        else:
            interval = THRESHOLD['call'] - THRESHOLD['pass']
            threshold = THRESHOLD['pass'] + random.uniform(0,1) * interval
            if score > threshold:
                return best_suit
            else:
                return False


def score_hand_scenario(hand, suit, topcard, seat):
    if suit == topcard[1]:
        if seat == 1 or seat == 3:
            score = score_hand(hand, suit)
            score += TOPCARD_PENALTY[topcard[0]]
        elif seat == 2:
            score = score_hand(hand, suit)
            score += TOPCARD_BONUS[topcard[0]]
        elif seat == 0:
            discard, score = best_discard(hand, topcard)
    elif suit == NEXT_DICT[topcard[1]]:
        score = score_hand(hand, suit, topcard[1])
        score += NEXT_SEAT_ADJ[seat] * SEAT_ADJ_FACTOR[topcard[0]]
    else:
        score = score_hand(hand, suit, topcard[1])
        score += GREEN_SEAT_ADJ[seat] * SEAT_ADJ_FACTOR[topcard[0]]
    
    score += SEAT_BONUS[seat]
    return score


def score_hand(hand, suit, discarded_suit=None):
    next_suit = NEXT_DICT[suit]
    hand = ['L'+suit if card == 'J'+next_suit else card for card in hand]
    score = 0
    suits_in_hand = [card[1] for card in hand]
    suit_counts = dict([(suit, suits_in_hand.count(suit)) for suit in set(suits_in_hand)])
    for card in hand:
        if card[1] == suit:
            score += TRUMP_PTS[card[0]]
        else:
            suit_count = suit_counts[card[1]]
            if discarded_suit == card[1]:
                suit_count += 1

            if card[0] == 'A':
                if card[1] == next_suit:
                    if suit_count == 1:
                        score += ACE_PTS['N']['singleton']
                    elif suit_count == 2:
                        score += ACE_PTS['N']['paired']
                    else:
                        score += ACE_PTS['N']['more']
                else:
                    if suit_count == 1:
                        score += ACE_PTS['G']['singleton']
                    elif suit_count == 2:
                        score += ACE_PTS['G']['paired']
                    else:
                        score += ACE_PTS['G']['more']
            elif card[0] == 'K':
                if suit_count == 1:
                    score += KING_PTS['singleton']
                elif suit_count == 2:
                    score += KING_PTS['paired']
                else:
                    score += KING_PTS['more']
    
    if len(suit_counts) < 3:
        if suit in suit_counts:
            score += SUITED_BONUS[2]*suit_counts[suit]
    elif len(suit_counts) == 4:
        score += SUITED_BONUS[4]
    
    return score


def dealer_discard(hand, topcard):
    discard, _ = best_discard(hand, topcard)
    if discard in hand:
        hand.remove(discard)
        hand.append(topcard)
    
    assert len(hand) == 5
    return hand, discard


def best_discard(hand, topcard):
    scores = {}
    scores[topcard] = score_hand(hand, topcard[1])
    for card in hand:
        new_hand = hand.copy()
        new_hand.remove(card)
        new_hand.append(topcard)
        scores[card] = score_hand(new_hand, topcard[1], card[1])
        
    discard = max(scores.keys(), key=(lambda key: scores[key]))
    return discard, scores[discard]