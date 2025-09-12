namespace RaptorUtils.AspNet.EntityFramework.Extensions;

using Microsoft.EntityFrameworkCore;

using RaptorUtils.EntityFramework.Metadata.Extensions;
using RaptorUtils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ModelBuilder"/>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Singularizes the table names of ASP.NET Identity entities by removing a trailing 's' from tables
    /// whose names start with "AspNet".
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance to apply the configuration to.</param>
    public static void SingularizeAspNetIdentityTables(this ModelBuilder builder)
    {
        builder.Model.GetEntityTypes()
            .Where(t => t.GetTableName()?.StartsWith("AspNet") ?? false)
                .Configure(
                    e => e.SetTableName(
                        e.GetTableName()!.RemoveRequiredSuffix("s", StringComparison.InvariantCulture)));
    }
}
