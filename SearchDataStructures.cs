using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithms; 

public interface IFrontier<T> {
    public void Add(T x);
    public T Remove();
    public bool IsEmpty();
}

public class FrontierStack<T>: IFrontier<T> {
    private Stack<T> stack;

    public FrontierStack() {
        stack = new Stack<T>();
    }

    public void Add(T x) {
        stack.Push(x);
    }

    public T Remove() {
        return stack.Pop();
    }

    public bool IsEmpty() {
        return stack.Count == 0;
    }
}

public class FrontierQueue<T>: IFrontier<T> {
    private Queue<T> queue;

    public FrontierQueue() {
        queue = new Queue<T>();
    }

    public void Add(T x) {
        queue.Enqueue(x);
    }

    public T Remove() {
        return queue.Dequeue();
    }

    public bool IsEmpty() {
        return queue.Count == 0;
    }
}

public class FrontierPriorityQueue<T> : IFrontier<T> {
    private List<T> pQueue;
    private Heuristic calcPriority;
    public delegate int Heuristic(T tile);

    public FrontierPriorityQueue(Heuristic calcPriority) {
        //pQueue = new PriorityQueue<T, int>();
        pQueue = new List<T>();
        this.calcPriority = calcPriority;
    }

    public void Add(T x) {
        // Add new item to list (after which will be potentiall unsorted)
        pQueue.Add(x);
    }

    public T Remove() {
        // Need to sort before dequeueing
        pQueue.Sort((x1, x2) => {
            return  calcPriority(x1).CompareTo(calcPriority(x2));
        });

        var ret = pQueue.First();

        pQueue.RemoveAt(0);
        return ret;
    }

    public bool IsEmpty() {
        return pQueue.Count == 0;
    }
}