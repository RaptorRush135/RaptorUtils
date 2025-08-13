namespace RaptorUtils.CodeAnalysis;

/// <summary>
/// Indicates that a code element is performance sensitive.
/// </summary>
[AttributeUsage(
    AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field,
    AllowMultiple = true,
    Inherited = false)]
public sealed class PerformanceSensitiveAttribute() : Attribute;
