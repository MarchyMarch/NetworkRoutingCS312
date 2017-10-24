using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    /**
     * Abstract class that is implemented by HeapPriorityQueue and ArrayPriorityQueue
     **/

    abstract class PriorityQueue
    {
        public PriorityQueue() { }
        public abstract void makeQueue(int nodeNum);
        public virtual int deleteMin() { return 0; }
        public virtual int deleteMin(ref List<double> distances) { return 0; }
        public virtual void decreaseKey(int index, double key) { }
        public virtual void decreaseKey(ref List<double> distances, int index) { }
        public virtual void insert(int index, double value) { }
        public virtual void insert(ref List<double> distances, int index) { }
        public abstract void printQueue();
        public abstract bool isEmpty();
    }
}
