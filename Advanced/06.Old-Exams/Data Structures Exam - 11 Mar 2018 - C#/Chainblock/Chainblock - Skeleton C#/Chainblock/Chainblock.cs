using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Chainblock : IChainblock
{
    public Dictionary<int, Transaction> byId;
    public Dictionary<TransactionStatus, HashSet<Transaction>> byStatus;
    public HashSet<Transaction> transactions;

    public Chainblock()
    {
        this.byId = new Dictionary<int, Transaction>();
        this.byStatus = new Dictionary<TransactionStatus, HashSet<Transaction>>();
        this.transactions = new HashSet<Transaction>();
    }

    public int Count => byId.Count;

    public void Add(Transaction tx)
    {
        byId[tx.Id] = tx;
        AddByStatus(tx);
        transactions.Add(tx);
    }

    private void AddByStatus(Transaction tx)
    {
        if (!byStatus.ContainsKey(tx.Status))
        {
            byStatus[tx.Status] = new HashSet<Transaction>();
        }
        byStatus[tx.Status].Add(tx);
    }

    public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
    {
        if (!byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        byId[id].Status = newStatus;
    }

    public bool Contains(Transaction tx)
    {
        return byId.ContainsKey(tx.Id);
    }

    public bool Contains(int id)
    {
        return byId.ContainsKey(id);
    }

    public IEnumerable<Transaction> GetAllInAmountRange(double lo, double hi)
    {
        return transactions.Where(x => x.Amount >= lo && x.Amount <= hi);
    }

    public IEnumerable<Transaction> GetAllOrderedByAmountDescendingThenById()
    {
        return byId.Values.OrderByDescending(x => x.Amount).ThenBy(x => x.Id);
    }

    public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
    {
        if (!byStatus.ContainsKey(status) || byStatus[status].Count == 0)
        {
            throw new InvalidOperationException();
        }
        return byStatus[status].OrderByDescending(x => x.Amount).Select(x => x.To);
    }

    public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
    {
        if (!byStatus.ContainsKey(status) || byStatus[status].Count == 0)
        {
            throw new InvalidOperationException();
        }
        return byStatus[status].OrderByDescending(x => x.Amount).Select(x => x.From);
    }

    public Transaction GetById(int id)
    {
        if (!byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }
        return byId[id];
    }

    public IEnumerable<Transaction> GetByReceiverAndAmountRange(string receiver, double lo, double hi)
    {
        var result = byId.Values.Where(x => x.To == receiver && x.Amount >= lo && x.Amount < hi)
            .OrderByDescending(x => x.Amount).ThenBy(x => x.Id);

        if (result.Count() == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    public IEnumerable<Transaction> GetByReceiverOrderedByAmountThenById(string receiver)
    {
        var result = byId.Values.Where(x => x.To == receiver);
        if (result.Count() == 0)
        {
            throw new InvalidOperationException();
        }
        return result.OrderByDescending(x => x.Amount).ThenBy(x => x.Id);
    }

    public IEnumerable<Transaction> GetBySenderAndMinimumAmountDescending(string sender, double amount)
    {
        var result = byId.Values.Where(x => x.From == sender && x.Amount > amount).OrderByDescending(a => a.Amount); ;

        if (result.Count() == 0)
        {
            throw new InvalidOperationException();
        }
        return result;
    }

    public IEnumerable<Transaction> GetBySenderOrderedByAmountDescending(string sender)
    {
        var result = this.byId
           .Values
           .Where(a => a.From == sender)
           .OrderByDescending(a => a.Amount);
        if (result.Count() == 0)
        {
            throw new InvalidOperationException();
        }
        return result;
    }

    public IEnumerable<Transaction> GetByTransactionStatus(TransactionStatus status)
    {
        if (!byStatus.ContainsKey(status))
        {
            throw new InvalidOperationException();
        }
        return byStatus[status].OrderByDescending(x => x.Amount);
    }

    public IEnumerable<Transaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, double amount)
    {
        if (!this.byStatus.ContainsKey(status))
        {
            return Enumerable.Empty<Transaction>();
        }
        return byStatus[status].Where(x => x.Amount <= amount).OrderByDescending(x => x.Amount);
    }

    public IEnumerator<Transaction> GetEnumerator()
    {
        foreach (var transaction in this.transactions)
        {
            yield return transaction;
        }
    }

    public void RemoveTransactionById(int id)
    {
        if (!byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }

        var transaction = byId[id];
        byId.Remove(id);
        byStatus[transaction.Status].Remove(transaction);
        transactions.Remove(transaction);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

