namespace RaptorUtils.Collections.Extensions;

/// <summary>
/// Provides extension methods for lists.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Determines whether any element of the specified list satisfies a condition.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to check against the condition.</param>
    /// <param name="predicate">
    /// A function to test each element for a condition. The element will satisfy the condition if
    /// this function returns <see langword="true"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if any elements in the list match the specified condition;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Any<T>(this List<T> list, Func<T, bool> predicate)
    {
        return list.Exists(new Predicate<T>(predicate));
    }
}
