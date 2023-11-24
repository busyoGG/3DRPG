using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortUtils
{
    /// <summary>
    /// Ã°ÅÝÅÅÐò
    /// </summary>
    /// <param name="nums">Êý×é</param>
    /// <param name="desc">ÊÇ·ñ½µÐò</param>
    public static void BubbleSort(ref int[] nums, bool desc = false)
    {
        int temp;
        int length = nums.Length;
        if (desc)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (nums[i] < nums[j])
                    {
                        temp = nums[i];
                        nums[i] = nums[j];
                        nums[j] = temp;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (nums[i] > nums[j])
                    {
                        temp = nums[i];
                        nums[i] = nums[j];
                        nums[j] = temp;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ¿ìËÙÅÅÐò
    /// </summary>
    /// <param name="nums"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="desc"></param>
    public static void QuickSort(ref int[] nums, int left, int right, bool desc = false)
    {
        if (left >= right) return;

        int index = QuickSortUnit(ref nums, left, right, desc);
        QuickSort(ref nums, left, index - 1, desc);
        QuickSort(ref nums, index + 1, right, desc);
    }

    private static int QuickSortUnit(ref int[] nums, int left, int right, bool desc)
    {
        int key = nums[left];
        if (desc)
        {
            while (left < right)
            {
                while (nums[right] < key && left < right)
                {
                    right--;
                }
                nums[left] = nums[right];

                while (nums[left] > key && left < right)
                {
                    left++;
                }
                nums[right] = nums[left];
            }
        }
        else
        {
            while (left < right)
            {
                while (nums[right] > key && left < right)
                {
                    right--;
                }
                nums[left] = nums[right];

                while (nums[left] < key && left < right)
                {
                    left++;
                }
                nums[right] = nums[left];
            }
        }
        nums[left] = key;
        return right;
    }
}
