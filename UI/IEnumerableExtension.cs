using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IEnumerableExtension
	{
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> enumerable, int size)
		{
			while (enumerable.Any())
			{
				yield return enumerable.Take(size);
				enumerable = enumerable.Skip(size);
			}
		}
	}
}