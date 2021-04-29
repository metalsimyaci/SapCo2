using System;
using System.Collections.Generic;
using System.Linq;

namespace SapCo2.Samples.Web.Extensions
{
    public static class ListExtensions
    {
        public static List<T>[] Partition<T>(this IEnumerable<T> list, int count)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (!list.Any())
                return new List<T>[0];

            var toList = list.ToList();

            if (count < 1)
                count = 1;

            int partCount = 1;
            int listCount = list.Count();

            if (listCount > count)
                partCount = listCount % count == 0 ? listCount / count : listCount / count + 1;

            if (listCount <= partCount)
            {
                var parts = new List<T>[2];
                int maxSize = (int)Math.Ceiling(listCount / 2.0);
                int k = 0;

                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = new List<T>();

                    for (int j = k; j < k + maxSize; j++)
                    {
                        if (j >= listCount)
                            break;
                        parts[i].Add(toList[j]);
                    }

                    k += maxSize;
                }

                return listCount == 1 ? new[] { parts[0] } : parts;
            }
            else
            {
                var partitions = new List<T>[partCount];
                int maxSize = (int)Math.Ceiling(listCount / (double)partCount);
                int k = 0;

                for (int i = 0; i < partitions.Length; i++)
                {
                    partitions[i] = new List<T>();

                    for (int j = k; j < k + maxSize; j++)
                    {
                        if (j >= listCount)
                            break;
                        partitions[i].Add(toList[j]);
                    }

                    k += maxSize;
                }

                return partitions;
            }
        }
    }
}
