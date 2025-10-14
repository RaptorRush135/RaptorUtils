namespace RaptorUtils.Collections.Extensions;

using RaptorUtils.CodeAnalysis;

/// <summary>
/// Provides extension methods for arrays.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Determines whether any element of the specified array satisfies a condition.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to check against the condition.</param>
    /// <param name="predicate">
    /// A function to test each element for a condition. The element will satisfy the condition if
    /// this function returns <see langword="true"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if any elements in the list match the specified condition;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [MustUseReturnValue]
    public static bool Any<T>(this T[] array, Func<T, bool> predicate)
    {
        return Array.Exists(array, new Predicate<T>(predicate));
    }
}
