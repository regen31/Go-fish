﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go_fish
{
    class Card
    {
        public Suits Suit;
        public Values Value;
        public string Name { get { return Value.ToString() + " of " + Suit.ToString(); } }

        public Card (Suits suit, Values value)
        {
            this.Suit = suit;
            this.Value = value;
        }

        public static string Plural (Values value)
        {
            if (value == Values.Six)
                return "Sixes";
            else
                return value.ToString() + "s";
        }
    }
}
