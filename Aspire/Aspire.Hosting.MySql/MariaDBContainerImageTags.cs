namespace RaptorUtils.Aspire.Hosting.MySql;

/// <summary>
/// Contains constant values related to the MariaDB Docker container image.
/// </summary>
internal static class MariaDBContainerImageTags
{
    /// <summary>docker.io.</summary>
    public const string Registry = "docker.io";

    /// <summary>library/mariadb.</summary>
    public const string Image = "library/mariadb";

    /// <summary>11.5.2.</summary>
    public const string Tag = "11.5.2";
}
