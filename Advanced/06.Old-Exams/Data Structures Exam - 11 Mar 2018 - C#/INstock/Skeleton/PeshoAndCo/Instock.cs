using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Instock : IProductStock
{
    private Dictionary<string, Product> byLabel;
    private Dictionary<int, Product> byIndex;
    private Dictionary<int, HashSet<Product>> byQuantity;
    private SortedSet<Product> sortedLabels;

    public Instock()
    {
        this.byLabel = new Dictionary<string, Product>();
        this.byIndex = new Dictionary<int, Product>();
        this.byQuantity = new Dictionary<int, HashSet<Product>>();
        this.sortedLabels = new SortedSet<Product>();
    }

    private int index;

    public int Count => this.byLabel.Count;

    public void Add(Product product)
    {
        this.byLabel[product.Label] = product;
        this.byIndex[index++] = product;
        this.AddByQuantity(product);
        this.sortedLabels.Add(product);
    }

    public void ChangeQuantity(string product, int quantity)
    {
        if (!this.byLabel.ContainsKey(product))
        {
            throw new ArgumentException();
        }

        var currentProduct = this.byLabel[product];
        this.byQuantity[currentProduct.Quantity].Remove(currentProduct);
        currentProduct.Quantity = quantity;
        this.AddByQuantity(currentProduct);
    }

    public bool Contains(Product product)
    {
        return this.byLabel.ContainsKey(product.Label);
    }

    public Product Find(int index)
    {
        if (!this.byIndex.ContainsKey(index))
        {
            throw new IndexOutOfRangeException();
        }

        return this.byIndex[index];
    }

    public IEnumerable<Product> FindAllByPrice(double price)
    {
        return this.byLabel.Values.Where(a => a.Price == price);
    }

    public IEnumerable<Product> FindAllByQuantity(int quantity)
    {
        if (!this.byQuantity.ContainsKey(quantity))
        {
            return Enumerable.Empty<Product>();
        }
        return this.byQuantity[quantity];
    }

    public IEnumerable<Product> FindAllInRange(double lo, double hi)
    {
        return this.byLabel
            .Values.Where(a => a.Price > lo && a.Price <= hi)
            .OrderByDescending(a => a.Price);
    }

    public Product FindByLabel(string label)
    {
        if (!this.byLabel.ContainsKey(label))
        {
            throw new ArgumentException();
        }

        return this.byLabel[label];
    }

    public IEnumerable<Product> FindFirstByAlphabeticalOrder(int count)
    {
        if (count < 0 || count > this.Count)
        {
            throw new ArgumentException();
        }

        return this.sortedLabels.Take(count);
    }

    public IEnumerable<Product> FindFirstMostExpensiveProducts(int count)
    {
        if (count < 0 || count > this.Count)
        {
            throw new ArgumentException();
        }

        return this.byLabel.Values.OrderByDescending(a => a.Price).Take(count);
    }

    public IEnumerator<Product> GetEnumerator()
    {
        foreach (var product in this.byLabel)
        {
            yield return product.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private void AddByQuantity(Product product)
    {
        if (!this.byQuantity.ContainsKey(product.Quantity))
        {
            this.byQuantity[product.Quantity] = new HashSet<Product>();
        }

        this.byQuantity[product.Quantity].Add(product);
    }
}