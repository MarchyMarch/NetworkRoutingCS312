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

        /**
         * Time Complexity is O(1). There is no space complexity.
         * */
        public override bool isEmpty()
        {
            return count == 0;
        }

        /**
         * makeQueue time and space complexity is O(|V|) because it itterates through all the nodes.
         * it also makes a new array the same size of the number of nodes.
         * @Param the number of nodes generated
         * */
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
         * If it needs to be included because the instructions say so it is of time complexity O(N)
         * where N is the length of the queue
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

        /**
         * Time Complexity is O(1) because it just puts the key in the queue at the given index.
         * There is no space complexity.
         **/
        public override void decreaseKey(int index, double key)
        {
            queue[index] = key;
        }

        /**
         * The time complexity is O(|V|) because it itterates through all the nodes in the system.
         * There is no space complexity because there is no new arrays made.
         **/
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

        /**
         * The time complexity of the insert method is O(1) because it directly inserts into the
         * array at the given index.  There is no space complexity because there are no new 
         * allocations.
         **/
        public override void insert(int index, double value)
        {
            queue[index] = value;
            count++;
        }
    }
}
