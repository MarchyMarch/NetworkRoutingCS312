using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class HeapPriorityQueue : PriorityQueue
    {
        private int capacity;
        private int count;
        private int lastElement;
        private int[] distances;
        private int[] pointers;

        public HeapPriorityQueue() { }

        public override void decreaseKey(ref List<double> distances, int index)
        {
            base.decreaseKey(ref distances, index);
        }

        public override int deleteMin(ref List<double> distances)
        {
            return base.deleteMin(ref distances);
        }

        public override void insert(ref List<double> distances, int index)
        {
            base.insert(ref distances, index);
        }

        public override bool isEmpty()
        {
            throw new NotImplementedException();
        }

        public override void makeQueue(int nodeNum)
        {
            throw new NotImplementedException();
        }

        public override void printQueue()
        {
            throw new NotImplementedException();
        }
    }
}
