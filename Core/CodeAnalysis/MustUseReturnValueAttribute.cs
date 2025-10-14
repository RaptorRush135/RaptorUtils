namespace RaptorUtils.CodeAnalysis;

/// <summary>
/// Indicates that the return value must be used.
/// Ignoring the return value may indicate a logic error.
/// </summary>
[AttributeUsage(
    AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class MustUseReturnValueAttribute : Attribute;
