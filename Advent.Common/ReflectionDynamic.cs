using System.Reflection;

namespace System.Dynamic;

// Allows to invoke private methods
public class ReflectionDynamic(object target) : DynamicObject
{
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var type = target.GetType();

        var method = type.GetMethod(binder.Name,
                                    BindingFlags.Instance |
                                    BindingFlags.Public |
                                    BindingFlags.NonPublic);

        if (method is null)
        {
            result = null;
            return false;
        }

        try
        {
            var rawResult = method.Invoke(target, args);

            if (rawResult is null)
            {
                result = null;
                return true;
            }

            var resultType = rawResult.GetType();

            if (resultType.IsPrimitive || resultType == typeof(string) || resultType.IsEnum)
                result = rawResult;
            else
                result = new ReflectionDynamic(rawResult);

            return true;
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException ?? ex;
        }
    }

    public override bool TryConvert(ConvertBinder binder, out object? result)
    {
        try
        {
            result = Convert.ChangeType(target, binder.Type);
            return true;
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch
#pragma warning restore CA1031 // Do not catch general exception types
        {
            result = null;
            return false;
        }
    }

    public object Unwrap() => target;
}
