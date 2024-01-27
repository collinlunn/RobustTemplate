using System;
using System.Collections;

namespace Content.Shared.Helpers
{
    public static class FunctionalHelpers
    {
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var element in enumerable)
				action.Invoke(element);
		}
    }
}
