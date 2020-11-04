namespace _01.RoyaleArena
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class RoyaleArena : IArena
    {
        private Dictionary<int, BattleCard> byId;
        private Dictionary<string, OrderedBag<BattleCard>> byName;
        private OrderedBag<BattleCard> bySwag;

        public RoyaleArena()
        {
            this.byId = new Dictionary<int, BattleCard>();
            this.byName = new Dictionary<string, OrderedBag<BattleCard>>();
            this.bySwag = new OrderedBag<BattleCard>((x, y) => x.Swag.CompareTo(y.Swag));
        }

        public int Count => this.byId.Count;

        public void Add(BattleCard card)
        {
            if (!this.Contains(card))
            {
                this.byId.Add(card.Id, card);

                if (!this.byName.ContainsKey(card.Name))
                {
                    this.byName.Add(card.Name, new OrderedBag<BattleCard>((x, y)
                        => y.Swag.CompareTo(x.Swag)));
                }

                this.byName[card.Name].Add(card);
                this.bySwag.Add(card);
            }
        }

        public void ChangeCardType(int id, CardType type)
        {
            if (!this.byId.ContainsKey(id))
            {
                throw new ArgumentException();
            }

            this.byId[id].Type = type;
        }

        public bool Contains(BattleCard card)
        {
            int key = card.Id;
            return this.byId.ContainsKey(key);
        }

        public IEnumerable<BattleCard> FindFirstLeastSwag(int n)
        {
            if (n > this.Count)
            {
                throw new InvalidOperationException();
            }

            return this.bySwag.Take(n).OrderBy(x => x.Id);
        }

        public IEnumerable<BattleCard> GetAllByNameAndSwag()
        {
            if (this.byId.Count == 0)
            {
                yield break;
            }

            foreach (var kvp in this.byName)
            {
                yield return kvp.Value.First();
            }
        }

        public IEnumerable<BattleCard> GetAllInSwagRange(double lo, double hi)
        {
            return this.bySwag.Where(x => x.Swag >= lo && x.Swag <= hi);
        }

        public IEnumerable<BattleCard> GetByCardType(CardType type)
        {
            if (!this.byId.Values.Where(x => x.Type == type).Any())
            {
                throw new InvalidOperationException();
            }

            return this.byId.Values.Where(x => x.Type == type).OrderBy(x => x);
        }

        public IEnumerable<BattleCard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
        {
            if (!this.byId.Values.Where(x => x.Type == type).Any())
            {
                throw new InvalidOperationException();
            }

            return this.byId.Values
                .Where(x => x.Type == type && x.Damage <= damage)
                .OrderBy(x => x);
        }

        public BattleCard GetById(int id)
        {
            if (!this.byId.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            return this.byId[id];
        }

        public IEnumerable<BattleCard> GetByNameAndSwagRange(string name, double lo, double hi)
        {
            if (!this.byId.Values.Where(x => x.Name == name && x.Swag >= lo
            && x.Swag < hi).Any())
            {
                throw new InvalidOperationException();
            }

            return this.byId.Values
                .Where(x => x.Name == name && x.Swag >= lo && x.Swag < hi)
                .OrderByDescending(x => x.Swag)
                .ThenBy(x => x.Id);
        }

        public IEnumerable<BattleCard> GetByNameOrderedBySwagDescending(string name)
        {
            if (!this.byId.Values.Where(x => x.Name == name).Any())
            {
                throw new InvalidOperationException();
            }

            return this.byId.Values.Where(x => x.Name == name).OrderByDescending(x => x.Swag)
                .ThenBy(x => x.Id);
        }

        public IEnumerable<BattleCard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
        {
            if (!this.byId.Values.Where(x => x.Type == type).Any())
            {
                throw new InvalidOperationException();
            }

            return this.byId.Values
                .Where(x => x.Type == type && x.Damage <= hi && x.Damage > lo)
                .OrderBy(x => x);
        }

        public IEnumerator<BattleCard> GetEnumerator()
        {
            foreach (var kvp in this.byId)
            {
                yield return kvp.Value;
            }
        }

        public void RemoveById(int id)
        {
            if (!this.byId.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            BattleCard cardToRemove = this.GetById(id);
            this.byId.Remove(id);
            this.byName[cardToRemove.Name].Remove(cardToRemove);
            this.bySwag.Remove(cardToRemove);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}