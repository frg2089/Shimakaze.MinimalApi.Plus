namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

[Flags]
internal enum ParameterFrom
{
    None = 0,
    FromBody = 1 << 0,
    FromForm = 1 << 1,
    FromHeader = 1 << 2,
    FromQuery = 1 << 3,
    FromRoute = 1 << 4,
    FromServices = 1 << 5,
    FromKeyedServices = 1 << 6,
}
