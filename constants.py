#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Mar 30 17:55:41 2019

@author: ethangreig
"""

deck = [j + i for i in ['H', 'D', 'S', 'C'] for j in ['9', 'T', 'J', 'Q', 'K', 'A']]

next_dict = {'H':'D', 'D':'H', 'S':'C', 'C':'S'}

value_dict = {'9':0, 'T':1, 'J':2, 'Q':3, 'K':4, 'A':5}
trump_value_dict = {'9':0, 'T':1, 'Q':2, 'K':3, 'A':4, 'L':5, 'J':6}