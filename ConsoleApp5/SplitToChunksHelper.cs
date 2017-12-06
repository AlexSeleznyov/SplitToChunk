// Written by Alex Seleznyov aseleznyov@gmail.com
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp5
{
    public static class SplitToChunksHelper
    {
        /// <summary>
        /// Splits <paramref name="source"/> into chunks of size not greater than <paramref name="chunkMaxSize"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Array to be split</param>
        /// <param name="chunkMaxSize">Max size of chunk</param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="Array"/> of <typeparam name="T"/></returns>
        public static IEnumerable<T[]> AsChunks<T>(this T[] source, int chunkMaxSize)
        {
            var pos = 0;
            var sourceLength = source.Length;
            do
            {
                var len = Math.Min(pos + chunkMaxSize, sourceLength) - pos;
                if (len == 0)
                {
                    yield break;;
                }
                var arr = new T[len];
                Array.Copy(source, pos, arr, 0, len);
                pos += len;
                yield return arr;
            } while (pos < sourceLength);
        }

        /// <summary>
        /// Splits <paramref name="source"/> into chunks of size not greater than <paramref name="chunkMaxSize"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/> to be split</param>
        /// <param name="chunkMaxSize">Max size of chunk</param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="Array"/> of <typeparam name="T"/></returns>
        public static IEnumerable<T[]> AsChunks<T>(this IEnumerable<T> source, int chunkMaxSize)
        {
            var arr = new T[chunkMaxSize];
            var pos = 0;
            foreach (var item in source)
            {
                arr[pos++] = item;
                if (pos == chunkMaxSize)
                {
                    yield return arr;
                    arr = new T[chunkMaxSize];
                    pos = 0;
                }
            }
            if (pos > 0)
            {
                Array.Resize(ref arr, pos);
                yield return arr;
            }
        }

        /// <summary>
        /// StackOverflow implementation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            if (batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchElements(enumerator, batchSize - 1);
                }
            }
        }

        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (var i = 0; i < batchSize && source.MoveNext(); i++)
            {
                yield return source.Current;
            }
        }

        public static IEnumerable<T[]> AsChunks_ByInbetween<T>(
            this T[] source, int chunkMaxSize)
        {
            var chunks = source.Length / chunkMaxSize;
            var leftOver = source.Length % chunkMaxSize;
            var result = new List<T[]>(chunks + 1);
            var offset = 0;

            for (var i = 0; i < chunks; i++)
            {
                result.Add(new ArraySegment<T>(source,
                    offset,
                    chunkMaxSize).ToArray());
                offset += chunkMaxSize;
            }

            if (leftOver > 0)
            {
                result.Add(new ArraySegment<T>(source,
                    offset,
                    leftOver).ToArray());
            }

            return result;
        }

        public static IEnumerable<IList<T>> AsChunks_ByInbetween2<T>(
            this T[] source, int chunkMaxSize)
        {
            var chunks = source.Length / chunkMaxSize;
            var leftOver = source.Length % chunkMaxSize;
            var result = new List<IList<T>>(chunks + 1);
            var offset = 0;

            for (var i = 0; i < chunks; i++)
            {
                result.Add(new ArraySegment<T>(source,
                    offset,
                    chunkMaxSize));
                offset += chunkMaxSize;
            }

            if (leftOver > 0)
            {
                result.Add(new ArraySegment<T>(source,
                    offset,
                    leftOver));
            }

            return result;
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> items, int partitionSize)
        {
            List<T> partition = new List<T>(partitionSize);
            foreach (T item in items)
            {
                partition.Add(item);
                if (partition.Count == partitionSize)
                {
                    yield return partition;
                    partition = new List<T>(partitionSize);
                }
            }
            // Cope with items.Count % partitionSize != 0
            if (partition.Count > 0) yield return partition;
        }
    }
}