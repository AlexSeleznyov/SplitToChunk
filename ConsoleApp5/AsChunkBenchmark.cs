﻿using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace ConsoleApp5
{
    public class AsChunkBenchmark
    {
        private const int ChunkSize = 1000;

        private static int[] GetTestData()
        {
            var arr = new int[10005];
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }
            return arr;
        }

        [Benchmark]
        public int TestArray()
        {
            var arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.AsChunks(ChunkSize))
            {
                Array.ForEach(chunk, _ => sum += _);
            }
            return sum;
        }

        [Benchmark]
        public int TestIEnumerable()
        {
            IEnumerable<int> arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.AsChunks(ChunkSize))
            {
                Array.ForEach(chunk, _ => sum += _);
            }
            return sum;
        }

        [Benchmark]
        public int TestIEnumerableLinqGroupBy()
        {
            IEnumerable<int> arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.GroupBy(idx => idx / ChunkSize).Select(_ => _.ToArray()))
            {
                Array.ForEach(chunk, _ => sum += _);
            }
            return sum;
        }

        [Benchmark]
        public int TestLinqGroupBy()
        {
            var arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.GroupBy(idx => idx / ChunkSize).Select(_ => _.ToArray()))
            {
                Array.ForEach(chunk, _ => sum += _);
            }
            return sum;
        }
        [Benchmark]
        public int TestSO_Markus()
        {
            var arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.Batch(ChunkSize))
            {
                var array = chunk.ToArray();
                Array.ForEach(array, _ => sum += _);
            }
            return sum;
        }

        [Benchmark]
        public int TestSO_mjwills()
        {
            var arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.Partition(ChunkSize))
            {
                var array = chunk.ToArray();
                Array.ForEach(array, _ => sum += _);
            }
            return sum;
        }

        [Benchmark]
        public int TestSO_InBetween()
        {
            var arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.AsChunks_ByInbetween(ChunkSize))
            {
                Array.ForEach(chunk, _ => sum += _);
            }
            return sum;
        }

        [Benchmark]
        public int TestSO_InBetween2()
        {
            var arr = GetTestData();

            int sum = 0;

            foreach (var chunk in arr.AsChunks_ByInbetween2(ChunkSize))
            {
                for (int i = 0; i < chunk.Count; i++)
                {
                    sum += chunk[i];
                }
            }
            return sum;
        }
    }
}