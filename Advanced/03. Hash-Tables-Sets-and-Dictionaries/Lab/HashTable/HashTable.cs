using HashTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HashTable<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
{
    private LinkedList<KeyValue<TKey, TValue>>[] slots;
    private const int InitialCapacity = 16;
    private const float LoadFactor = 0.75f;
    private int maxElements;

    public int Count { get; private set; }

    public int Capacity => this.slots.Length;

    public HashTable(int capacity = InitialCapacity)
    {
        this.slots = new LinkedList<KeyValue<TKey, TValue>>[capacity];
        this.maxElements = (int)(capacity * LoadFactor);
        this.Count = 0;
    }

    public void Add(TKey key, TValue value)
    {
        this.GrowIfNeeded();

        int slotNumber = FindSlotNumber(key);

        if (this.slots[slotNumber] == null)
        {
            this.slots[slotNumber] = new LinkedList<KeyValue<TKey, TValue>>();
        }

        foreach (var keyValue in this.slots[slotNumber])
        {
            if (keyValue.Key.Equals(key))
            {
                throw new ArgumentException();
            }
        }

        this.slots[slotNumber].AddLast(new KeyValue<TKey, TValue>(key, value));

        this.Count++;
    }

    private void Growth()
    {
        var newTable = new HashTable<TKey, TValue>(this.Capacity * 2);

        foreach (var linkedList in this.slots)
        {
            if (linkedList == null)
            {
                continue;
            }

            foreach (var keyValue in linkedList)
            {
                newTable.Add(keyValue.Key, keyValue.Value);
            }
        }

        this.slots = newTable.slots;

        this.Count = newTable.Count;
    }

    public bool AddOrReplace(TKey key, TValue value)
    {
        this.GrowIfNeeded();

        int hash = FindSlotNumber(key);

        if (this.slots[hash] == null)
        {
            this.slots[hash] = new LinkedList<KeyValue<TKey, TValue>>();
        }

        foreach (var keyValue in this.slots[hash])
        {
            if (keyValue.Key.Equals(key))
            {
                keyValue.Value = value;
                return true;
            }
        }

        this.slots[hash].AddLast(new KeyValue<TKey, TValue>(key, value));

        this.Count++;

        return false;
    }

    public TValue Get(TKey key)
    {
        var kvp = this.Find(key);

        if (kvp == null)
        {
            throw new KeyNotFoundException();
        }

        return kvp.Value;
    }

    public TValue this[TKey key]
    {
        get => this.Get(key);
        set => this.AddOrReplace(key, value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        var kvp = this.Find(key);

        if (kvp != null)
        {
            value = kvp.Value;
            return true;
        }

        value = default(TValue);
        return false;
    }

    public KeyValue<TKey, TValue> Find(TKey key)
    {
        int hash = this.FindSlotNumber(key);

        if (this.slots[hash] != null)
        {
            foreach (var element in this.slots[hash])
            {
                if (key.Equals(element.Key))
                {
                    return element;
                }
            }
        }

        return null;
    }

    public bool ContainsKey(TKey key)
    {
        return this.Find(key) != null;
    }

    public bool Remove(TKey key)
    {
        var elements = this.Find(key);

        if (elements != null)
        {
            int hash = FindSlotNumber(key);

            this.slots[hash].Remove(elements);

            this.Count--;

            return true;
        }

        return false;
    }

    public void Clear()
    {
        this.slots = new LinkedList<KeyValue<TKey, TValue>>[InitialCapacity];
        this.maxElements = (int)(this.Capacity * LoadFactor);
        this.Count = 0;
    }

    public IEnumerable<TKey> Keys => this.slots
        .Where(list => list != null)
        .SelectMany(x => x.Select(y => y.Key));

    public IEnumerable<TValue> Values => this.slots
        .Where(list => list != null)
        .SelectMany(x => x.Select(y => y.Value));

    public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
    {
        foreach (var linkedList in this.slots)
        {
            if (linkedList != null)
            {
                foreach (var keyValue in linkedList)
                {
                    yield return keyValue;
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private int FindSlotNumber(TKey key)
    {
        return Math.Abs(key.GetHashCode()) % this.Capacity;
    }

    private void GrowIfNeeded()
    {
        if (this.Count < maxElements)
        {
            return;
        }

        this.Growth();

        maxElements = (int)(this.Capacity * LoadFactor);
    }
}