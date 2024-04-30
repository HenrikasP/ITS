using System.ComponentModel.DataAnnotations;

namespace Claims.Options;

public record RabbitMQOptions
{
    public const string ConfigurationKey = "RabbitMQ";

    [Required]
    public string Prefix { get; init; } = default!;

    [Required]
    public string Host { get; init; } = default!;

    [Required]
    public string VirtualHost { get; init; } = "/";

    [Required]
    public string Username { get; init; } = default!;

    [Required]
    public string Password { get; init; } = default!;
}
