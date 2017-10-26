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

        /**
         * Is empty is time complexity of O(1) because it is a comparison.  There is no space complexity. 
         **/
        public override bool isEmpty()
        {
            return count == 0;
        }

        /**
         * This is for debugging purposes and is of time complexity O(|V|) because it itterates through all
         * the nodes in the distanceHeap array. There is no space complexity.
         **/
        public override void printQueue()
        {
            Console.Write("Contents of Heap are: ");

            for (int i = 1; i < capacity; i++)
            {
                if (distancesHeap[i] != -1) Console.Write(distancesHeap[i] + ", ");
            }
            Console.WriteLine();
        }

        /**
         * Here both time and space complexity is O(|V|). Time is such because it itterates through the
         * nodes to assign values in increasing number.  Space is of the same order because there are 
         * two arrays that are allocated to the size V.
         **/
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

        /**
         * Time complexity here is O(|logV|) because we  know the minimum node and then we just have to 
         * bubble up and sort the tree to accomadate for deleting the root. The worse case scenario 
         * would be the height of the tree which is |logV|.  There is no space complexity because there 
         * are no new space allocations
         **/
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

        /**
         * Time complexity is O(|logV|) because it finds the new distance to update, switches it
         * and then updates the tree like in delete min.  This gives the worse case to be the height
         * of the tree, |logV|. There is no space complexity because there is no new space allocations.
         **/
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


        /**
         * Insert has a time complexity of O(|logV|) because it inserts the given node
         * and updates the tree by bubbling up.  |log V| is the height of the tree so 
         * that would be the worse case scenario.  There is no space complexity because
         * there are no new space alloations.
         **/
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
