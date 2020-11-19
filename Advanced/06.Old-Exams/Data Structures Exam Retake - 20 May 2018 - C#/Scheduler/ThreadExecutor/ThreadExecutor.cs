using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

/// <summary>
/// The ThreadExecutor is the concrete implementation of the IScheduler.
/// You can send any class to the judge system as long as it implements
/// the IScheduler interface. The Tests do not contain any <e>Reflection</e>!
/// </summary>
public class ThreadExecutor : IScheduler
{
    public List<Task> tasks;
    public Dictionary<int, Task> byId;
    public OrderedDictionary<int, HashSet<Task>> byConsumption;

    public ThreadExecutor()
    {
        byId = new Dictionary<int, Task>();
        tasks = new List<Task>();
        byConsumption = new OrderedDictionary<int, HashSet<Task>>();
    }

    private int lowCycles;
    private int highCycles;
    public int Count => byId.Count;

    int IScheduler.Count => Count;

    public void ChangePriority(int id, Priority newPriority)
    {
        if (!byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        byId[id].TaskPriority = newPriority;
    }

    public bool Contains(Task task)
    {
        return byId.ContainsKey(task.Id);
    }

    public int Cycle(int cycles)
    {
        if (byId.Count == 0)
        {
            throw new InvalidOperationException();
        }

        highCycles += cycles;
        var range = byConsumption.Range(lowCycles, true, highCycles, true);
        lowCycles = highCycles + 1;

        return RemoveTasks(range);
    }

    private int RemoveTasks(OrderedDictionary<int, HashSet<Task>>.View range)
    {
        var count = 0;
        foreach (var kvp in range)
        {
            foreach (var task in kvp.Value)
            {
                byId.Remove(task.Id);
                count++;
            }
        }
        return count;
    }

    private int GetUpdatedConsumtion(Task task)
    {
        return task.Consumption - highCycles;
    }

    public void Execute(Task task)
    {
        if (byId.ContainsKey(task.Id))
        {
            throw new ArgumentException();
        }
        byId[task.Id] = task;
        tasks.Add(task);
        AddByConsumtion(task);
    }
    private void AddByConsumtion(Task task)
    {
        if (!byConsumption.ContainsKey(task.Consumption))
        {
            byConsumption[task.Consumption] = new HashSet<Task>();
        }
        byConsumption[task.Consumption].Add(task);
    }

    public IEnumerable<Task> GetByConsumptionRange(int lo, int hi, bool inclusive)
    {
        IEnumerable<Task> result = null;

        if (inclusive)
        {
            result = byId
                .Values
                .Where(a => GetUpdatedConsumtion(a) >= lo && GetUpdatedConsumtion(a) <= hi);
        }
        else
        {
            result = byId
                .Values
                .Where(a => GetUpdatedConsumtion(a) > lo && GetUpdatedConsumtion(a) < hi);
        }

        return result.OrderBy(a => GetUpdatedConsumtion(a))
                .ThenByDescending(a => a.TaskPriority);
    }

    public Task GetById(int id)
    {
        if (!byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        return byId[id];
    }

    public Task GetByIndex(int index)
    {
        if (index < 0 || index > Count - 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        while (!byId.ContainsKey(tasks[index].Id))
        {
            index++;
        }

        return tasks[index];
    }

    public IEnumerable<Task> GetByPriority(Priority type)
    {
        return byId.Values.Where(x => x.TaskPriority == type).OrderByDescending(a => a.Id);
    }

    public IEnumerable<Task> GetByPriorityAndMinimumConsumption(Priority priority, int lo)
    {
        return byId
            .Values
            .Where(a => a.TaskPriority == priority && a.Consumption >= lo)
            .OrderByDescending(a => a.Id);
    }

    public IEnumerator<Task> GetEnumerator()
    {
        foreach (var task in tasks)
        {
            if (byId.ContainsKey(task.Id))
            {
                yield return task;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
