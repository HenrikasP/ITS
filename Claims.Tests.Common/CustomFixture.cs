using AutoFixture;
using AutoFixture.Xunit2;

namespace Claims.Tests.Common;

public class CustomFixture
{
    public static Fixture Create()
    {
        var fixture = new Fixture();
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        return fixture;
    }
}

public class CustomAutoDataAttribute : AutoDataAttribute
{
    public CustomAutoDataAttribute() : base(CustomFixture.Create)
    {
    }
}

public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object?[]? objects) : base(new CustomAutoDataAttribute(), objects) { }
}