namespace Problem03.Queue
{
    public class Node<T>
    {
        public T Value { get; set; }

        public Node<T> Next { get; set; }

        public Node(T value)
        {
            this.Value = value;
        }

        public Node(T value, Node<T> next)
        {
            this.Value = value;
            this.Next = next;
        }
    }
}