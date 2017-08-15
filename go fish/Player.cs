using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace go_fish
{
    class Player
    {
        private string name;
        public string Name { get { return name; } }
        private Random random;
        private Deck cards;
        private TextBox textBoxOnForm;
        

        public Player (String name, Random random, TextBox textBoxOnForm)
        {
            this.name = name;
            this.random = random;
            this.cards = new Deck(new Card[] { });
            this.textBoxOnForm = textBoxOnForm;
            textBoxOnForm.Text += name + " has just joined the game" + Environment.NewLine;
        }

        public IEnumerable<Values> PullOutOfBooks() {
            List<Values> books = new List<Values>();

            for(int i = 1; i<13; i++)
            {
                Values value = (Values)i;
                int howMany = 0;
                for (int card = 0; card < cards.Count; card++)
                    if (cards.Peek(card).Value == value)
                        howMany++;
                if (howMany == 4)
                {
                    books.Add(value);
                    for (int card = cards.Count - 1; card >= 0; card--)
                        cards.Deal(card);
                }
            }
            return books;
        }

        public Values GetRandomValue()
        {
            Card randomCard = cards.Peek(random.Next(cards.Count));
            return randomCard.Value;    
        }

        public Deck DoYouHaveAny(Values value)
        {
            Deck cardsIHave = cards.PullOutValues(value);
            textBoxOnForm.Text += Name + " has " + cardsIHave.Count + " " + Card.Plural(value) + Environment.NewLine;
            return cardsIHave;
        }

        public void AskForACard (List<Player> players, int myindex, Deck stock)
        {
            Values randomvalue = GetRandomValue();
            AskForACard(players, myindex, stock, randomvalue);
        }

        public void AskForACard (List<Player> players, int myindex, Deck stock, Values value)
        {
            textBoxOnForm.Text += Name + " asks if anyone have a " + value + Environment.NewLine;
            int totalcardsgiven = 0;

            for (int i =0; i < players.Count; i++)
            {
                if (i != myindex)
                {
                    Player player = players[i];
                    Deck CardsGiven = player.DoYouHaveAny(value);
                    totalcardsgiven += CardsGiven.Count;
                    while (CardsGiven.Count > 0)
                        cards.Add(CardsGiven.Deal());
                }
            }
            if (totalcardsgiven == 0)
            {
                textBoxOnForm.Text += Name + " must draw from the stock." + Environment.NewLine;
                cards.Add(stock.Deal());
            }
        }

        public int CardCount { get { return cards.Count; } }
        public void TakeCard(Card card) { cards.Add(card); }
        public IEnumerable<string> GetCardNames() { return cards.GetCardsNames(); }
        public Card Peek(int CardNumber) { return cards.Peek(CardNumber); }
        public void SortHand() { cards.SortByValue(); }
    }
}
