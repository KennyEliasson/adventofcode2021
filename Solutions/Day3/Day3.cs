using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day3
    {
        private readonly ITestOutputHelper _output;

        public Day3(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Day3_Part1()
        {
            var (bitArrays, rows, rowLength) = ParseInputToBitArrays("day3/day3input.txt");
            
            var gamma = new BitArray(rowLength);
            for (int i = 0; i < rowLength; i++)
            {
                var howManySet = bitArrays.Count(x => x.Get(i));
                if (howManySet > rows / 2)
                {
                    gamma.Set(i, true);
                }
            }

            // BitArray uses different endian ....
            gamma = gamma.Reverse();
            var epsilon = new BitArray(gamma).Not();

            _output.WriteLine($"Answer is {gamma.ToInt()*epsilon.ToInt()}");
        }
        
        [Fact]
        public void Day3_Part2()
        {
            var (bitArrays, _, _) = ParseInputToBitArrays("day3/day3input.txt");

            var oxygenGeneratorRating = FindLastBitArray(bitArrays, 0, true).Reverse();
            var co2ScrubberRating = FindLastBitArray(bitArrays, 0, false).Reverse();
            
            _output.WriteLine($"Answer is {oxygenGeneratorRating.ToInt()*co2ScrubberRating.ToInt()}");
        }

        private static (List<BitArray> bitArrays, int rows, int rowLength) ParseInputToBitArrays(string fileName)
        {
            var input = File.ReadAllLines(fileName).ToList();
            var rowLength = input[0].Length;
            var rows = input.Count;

            var bitArrayList = input.Select(row => new BitArray(row.Select(bit => bit == '1').ToArray())).ToList();
            return (bitArrayList, rows, rowLength);
        }

        private BitArray FindLastBitArray(List<BitArray> bitArrays, int index, bool findMostCommon)
        {
            if (bitArrays.Count == 1)
                return bitArrays[0];
            
            var bitSet = bitArrays.Where(x => x.Get(index)).ToList();
            var bitNotSet = bitArrays.Where(x => !x.Get(index)).ToList();

            if (findMostCommon)
            {
                if (bitSet.Count >= bitNotSet.Count)
                {
                    return FindLastBitArray(bitSet, index + 1, findMostCommon);
                }

                return FindLastBitArray(bitNotSet, index + 1, findMostCommon);
            }

            if (bitNotSet.Count <= bitSet.Count)
            {
                return FindLastBitArray(bitNotSet, index + 1, findMostCommon);
            }

            return FindLastBitArray(bitSet, index + 1, findMostCommon);
        }
    }

    public static class BitExtensions
    {
        public static int ToInt(this BitArray bitArray)
        {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        public static BitArray Reverse(this BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                (array[i], array[length - i - 1]) = (array[length - i - 1], array[i]);
            }

            return new BitArray(array);
        }
    }
}

