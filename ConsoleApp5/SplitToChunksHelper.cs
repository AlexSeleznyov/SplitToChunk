// Written by Alex Seleznyov aseleznyov@gmail.com
//
using System;
using System.Collections.Generic;

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
    }
}