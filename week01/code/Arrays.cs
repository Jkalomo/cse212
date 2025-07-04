using System;
using System.Collections.Generic;

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  
    /// For example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  
    /// Assume that length is a positive integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // PLAN:
        // Step 1: Create a new array of doubles with the size equal to 'length'
        // Step 2: Loop from 0 to length - 1
        // Step 3: In each iteration, set the array at index i to number * (i + 1)
        //         (This will give us number, 2*number, 3*number, ..., length*number)
        // Step 4: Return the filled array

        double[] result = new double[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = number * (i + 1);
        }

        return result;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  
    /// For example, if the data is List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and amount is 3 then the list after the function runs 
    /// should be List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  
    /// The value of amount will be in the range of 1 to data.Count, inclusive.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // PLAN:
        // Step 1: Determine the number of elements in the list for (data.Count)
        // Step 2: Make use of  GetRange to get the last 'amount' elements from the list
        // Step 3: Make use of GetRange once more to get the first 'data.Count - amount' elements
        // Step 4: Clear the original list
        // Step 5: Add the 'last amount' elements first (to rotate them to front)
        // Step 6: Add the remaining elements to the list to ensure update

        int count = data.Count;

        // Get the last 'amount' elements
        List<int> tail = data.GetRange(count - amount, amount);
        // Get the first part of the list (everything before the tail)
        List<int> head = data.GetRange(0, count - amount);

        // Clear the original list and reassemble it in rotated order
        data.Clear();
        data.AddRange(tail);
        data.AddRange(head);
    }
}
