using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class ArrayPriorityQueue : PriorityQueue
    {
        private double[] queue;
        private int count;

        public ArrayPriorityQueue() { }

        public override bool isEmpty()
        {
            return count == 0;
        }

        public override void makeQueue(int nodeNum)
        {
            queue = new double[nodeNum];
            count = nodeNum;

            for(int i = 0; i < nodeNum; i++)
            {
                queue[i] = double.MaxValue;
            }
        }

        /**
         * Print queue method for debugging
         **/
        public override void printQueue()
        {
            Console.WriteLine("Queue: ");
            for(int i = 0; i < queue.Length; i++)
            {
                Console.Write(i + ": " + queue[i] + ", ");
            }
            Console.WriteLine();
        }

        public override void decreaseKey(int index, double key)
        {
            queue[index] = key;
        }

        public override int deleteMin()
        {
            double minValue = double.MaxValue;
            int minIndex = 0;

            for(int i = 0; i < queue.Count(); i++)
            {
                if(queue[i] < minValue)
                {
                    minValue = queue[i];
                    minIndex = i;
                }
            }
            count--;
            queue[minIndex] = double.MaxValue;

            return minIndex;
        }

        public override void insert(int index, double value)
        {
            queue[index] = value;
            count++;
        }
    }
}
