using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Primitives;

namespace Shimakaze.MinimalApi.Plus;

#if !NET9_0_OR_GREATER
[RequiresUnreferencedCode("It cannot supported AOT because it used TypeDescriptor.GetConverter.")]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ApiEndpointsHelper
{
    public static T ConvertTo<T>(string? value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        object obj = TypeDescriptor
                .GetConverterFromRegisteredType(typeof(T))
                .ConvertFromInvariantString(value)
                ?? throw new InvalidCastException();

        return (T)obj;
    }

    public static T ConvertTo<T>(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        return (T)obj;
    }

    public static T ConvertTo<T>(StringValues value)
        => ConvertTo<T>(value.FirstOrDefault());
    public static IEnumerable<T> ConvertEnumerableTo<T>(StringValues value)
        => value.Select(ConvertTo<T>);

#if !NET9_0_OR_GREATER
    extension(TypeDescriptor)
    {
        [RequiresUnreferencedCode("It cannot supported AOT because it used TypeDescriptor.GetConverter.")]
        private static TypeConverter GetConverterFromRegisteredType(Type type)
            => TypeDescriptor.GetConverter(type);
    }
#endif
}
