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
        private int[] distancesHeap;
        private int[] pointers;

        public HeapPriorityQueue() { }

        public override bool isEmpty()
        {
            return count == 0;
        }

        public override void printQueue()
        {
            Console.Write("Contents of Heap are: ");

            for (int i = 1; i < capacity; i++)
            {
                if (distancesHeap[i] != -1) Console.Write(distancesHeap[i] + ", ");
            }
            Console.WriteLine();
        }

        public override void makeQueue(int nodeNum)
        {
            distancesHeap = new int[nodeNum + 1];
            pointers = new int[nodeNum];

            for (int i = 1; i < nodeNum + 1; i++)
            {
                distancesHeap[i] = i - 1;
                pointers[i - 1] = i;
            }

            // distancesHeap[0] = -1;
            capacity = nodeNum;
            count = 0;
            lastElement = capacity;
        }

        public override int deleteMin(ref List<double> distancesArray)
        {
            int minValue = distancesHeap[1];
            count--;

            if (lastElement == -1) return minValue;

            distancesHeap[1] = distancesHeap[lastElement];
            pointers[distancesHeap[1]] = 1;
            lastElement--;

            int whileIndex = 1;
            while (whileIndex <= lastElement)
            {
                int leftChildIndex = whileIndex * 2;
                if (leftChildIndex > lastElement) break;

                if (leftChildIndex + 1 <= lastElement && distancesArray[distancesHeap[leftChildIndex + 1]] < distancesArray[distancesHeap[leftChildIndex]])
                {
                    leftChildIndex++;
                }

                if (distancesArray[distancesHeap[whileIndex]] > distancesArray[distancesHeap[leftChildIndex]])
                {
                    int temp = distancesHeap[leftChildIndex];
                    distancesHeap[leftChildIndex] = distancesHeap[whileIndex];
                    distancesHeap[whileIndex] = temp;

                    pointers[distancesHeap[leftChildIndex]] = leftChildIndex;
                    pointers[distancesHeap[whileIndex]] = whileIndex;
                }

                whileIndex = leftChildIndex;
            }

            return minValue;
        }

        public override void decreaseKey(ref List<double> distancesArray, int index)
        {
            int heapIndex = pointers[index];
            count++;

            int whileIndex = heapIndex;
            while (whileIndex > 1 && distancesArray[distancesHeap[whileIndex / 2]] > distancesArray[distancesHeap[whileIndex]])
            {
                int temp = distancesHeap[whileIndex / 2];
                distancesHeap[whileIndex / 2] = distancesHeap[whileIndex];
                distancesHeap[whileIndex] = temp;


                pointers[distancesHeap[whileIndex / 2]] = whileIndex / 2;
                pointers[distancesHeap[whileIndex]] = whileIndex;

                whileIndex = whileIndex / 2;
            }
        }



        public override void insert(ref List<double> distances, int pointerIndex)
        {
            count++;

            int whileIndex = pointers[pointerIndex];
            while (whileIndex > 1 && distances[distancesHeap[whileIndex / 2]] > distances[distancesHeap[whileIndex]])
            {
                int temp = distancesHeap[whileIndex / 2];
                distancesHeap[whileIndex /2] = distancesHeap[whileIndex];
                distancesHeap[whileIndex] = temp;

                pointers[distancesHeap[whileIndex / 2]] = whileIndex / 2;
                pointers[distancesHeap[whileIndex]] = whileIndex;

                whileIndex = whileIndex / 2;
            }
        }
    }
}
