using System.Reflection;

public static class ReflectionUtils
{
    public static T? GetFieldOrPropertyValueOrDefault<T>(this Type type, string name, BindingFlags flags, object? obj = null)
        => TryGetFieldOrPropertyValue<T>(type, name, flags, obj, out var value) ? value : default;

    public static bool TryGetFieldOrPropertyValue<T>(this Type type, string name, BindingFlags flags, out T? value) => TryGetFieldOrPropertyValue<T>(type, name, flags, null, out value);
    public static bool TryGetFieldOrPropertyValue<T>(this Type type, string name, BindingFlags flags, object? obj, out T? value)
    {
        value = default;
        var field = type.GetField(name, flags);
        var property = type.GetProperty(name, flags);
        if (field == null && property == null)
            return false;
        if (field != null && property != null)
            return false;
        var memberReturnValue = field != null ? field.GetValue(obj) : property!.GetValue(obj);
        if (memberReturnValue is not T) return false;
        value = (T)memberReturnValue;
        return true;
    }

    public static Type? GetBaseTypeImplementingInterface<I>(this Type? type)
    {
        if (!typeof(I).IsInterface) throw new Exception($"Invalid generic parameter 'I'. Type '{typeof(I).Name}' is not an interface!");

        while (type?.BaseType != null && type.BaseType.GetInterfaces().Contains(typeof(I)))
            type = type.BaseType;
        return type;
    }
}