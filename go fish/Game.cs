﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace go_fish
{
    class Game
    {
        private List<Player> players;
        private Dictionary<Values, Player> books;
        private Deck stock;
        private TextBox textboxonform;

        public Game(string playername, IEnumerable<string> opponentnames, TextBox textboxonform) {
            Random random = new Random();
         this.textboxonform = textboxonform;
            players = new List<Player>();
            players.Add(new Player(playername, random, textboxonform));
            foreach (string player in opponentnames)
                players.Add(new Player(player, random, textboxonform));
            books = new Dictionary<Values, Player>();
            stock = new Deck();
            Deal();
            players[0].SortHand();
        }

        private void Deal()
        {
            stock.Shuffle();
            for (int i = 0; i<5; i++)
            {
                foreach (Player player in players)
                    player.TakeCard(stock.Deal());
                foreach (Player player in players)
                    PullOutBooks(player);
            }
        }
        public bool PlayOneRound (int selectedPlayerCard)
        {
            Values cardToAskFor = players[0].Peek(selectedPlayerCard).Value;
            for(int i = 0; i < players.Count; i++)
            {
                if (i == 0)
                    players[0].AskForACard(players, 0, stock, cardToAskFor);
                else
                    players[i].AskForACard(players, i, stock);
                if (PullOutBooks(players[i]))
                {
                    textboxonform.Text += players[i].Name + " drew a new hand" + Environment.NewLine;
                    int card = 1;
                    while (card<5 && stock.Count > 0)
                    {
                        players[i].TakeCard(stock.Deal());
                        card++;
                    }
                }
                players[0].SortHand();
                if (stock.Count == 0)
                {
                    textboxonform.Text = "The stock is out of cards. Game over!" + Environment.NewLine;
                    return true;
                }
            }
            return false;
        }

        public bool PullOutBooks(Player player)
        {
            IEnumerable<Values> bookspulled = player.PullOutOfBooks();
            foreach (Values value in bookspulled)
                books.Add(value, player);
            if (player.CardCount == 0)
                return true;
            return false;
        }

        public string DescribeBooks()
        {
            string whoHasWhichBooks = "";
            foreach (Values value in books.Keys)
                whoHasWhichBooks += books[value].Name + " has a book of " + Card.Plural(value) + Environment.NewLine;
            return whoHasWhichBooks;
        }

        public string GetWinnerName()
        {
            Dictionary<string, int> winners = new Dictionary<string, int>();
            foreach (Values value in books.Keys)
            {
                string name = books[value].Name;
                if (winners.ContainsKey(name))
                    winners[name]++;
                else
                    winners.Add(name, 1);
            }
            int mostbooks = 0;
            foreach (string name in winners.Keys)
                if (winners[name] > mostbooks)
                    mostbooks = winners[name];
            bool tie = false;
            string winnerlist = "";
            foreach (string name in winners.Keys)
                if (winners[name] == mostbooks)
                {
                    if (!String.IsNullOrEmpty(winnerlist))
                    {
                        winnerlist += " and ";
                        tie = true;
                    }
                    winnerlist += name;
                }
            winnerlist += " with " + mostbooks + " books";
            if (tie)
                return "A tie between " + winnerlist;
            else
                return winnerlist;
        }

        public IEnumerable<string> GetPlayerNames()
        {
            return players[0].GetCardNames();
        }

        public string DescribePlayerHands()
        {
            string description = "";
            for (int i =0; i < players.Count; i++)
            {
                description += players[i].Name + " has " + players[i].CardCount;
                if (players[i].CardCount == 1)
                    description += " card." + Environment.NewLine;
                else
                    description += " cards" + Environment.NewLine;
            }
            description += "The stock has " + stock.Count + " cards left";
            return description;
        }

    } 
}