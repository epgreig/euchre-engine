#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 17:54:58 2019

@author: ethangreig
"""

from constants import next_dict, value_dict, trump_value_dict
from random import shuffle


class Round:
    def __init__(self, deal):
        self.hands = deal.hands
        self.played = []
        self.tricks = 0
        self.select_trump()
        self.hand = self.hands[0]
    
    def select_trump(self, trump='H'):
        self.trump = trump
        self.hands = trumpify(self.hands, self.trump)
        
    def play(self, trick=None):
        if trick==None:
            t = Trick()

def trumpify(hands, trump):
    next_suit = next_dict[trump]
    for i in range(len(hands)):
        hands[i] = ['L'+trump if card=='J'+next_suit else card for card in hands[i]]

class Trick:
    def __init__(self, card, trump='H', players=4, leader=1):
        self.trump = trump
        self.num_players = players
        self.leader = leader
        self.winner = None
        self.follow = card[1]
        self.num_cards = 1
        self.cards = [card]
    
    def add(self, card):
        self.cards.append(card)
        self.num_cards += 1
        if self.num_cards==self.num_players:
            self.decide_trick()
    
    def decide_trick(self):
        ranks = [card[0] for card in t.cards]
        suits = [card[1] for card in t.cards]
        if t.trump in suits:
            trump_cards = [i for i in range(t.num_cards) if suits[i]==t.trump]
            trump_rank_values = [trump_value_dict[ranks[i]] for i in trump_cards]
            t.winner = trump_ranks.index(max(trump_ranks))
    
    def compare_trump(self, card_a, card_b):
        card_a

ranks = [card[0] for card in t.cards]
suits = [card[1] for card in t.cards]
if t.trump in suits:
    trump_rank_values = [trump_value_dict[ranks[i]] if suits[i]==t.trump else -1 for i in range(t.num_cards)]
    t.winner = trump_rank_values.index(max(trump_rank_values))
else:
    follow_rank_values = [value_dict[ranks[i]] if suits[i]==t.follow else -1 for i in range(t.num_cards)]
    t.winner = follow_rank_values.index(max(follow_rank_values))