namespace SharedCommon.Utils;

/// <summary>
/// </summary>
public static class PipeExtension
{
    public static TOut Pipe<T, TOut>(this T @this, Func<T, TOut> next)
    {
        return next(@this);
    }

    public static T Pipe<T>(this T @this, Action<T> next)
    {
        next(@this);

        return @this;
    }

    public static T PipeIf<T>(this T @this, bool condition, Action<T> next)
    {
        if (condition)
        {
            next(@this);
        }

        return @this;
    }

    public static T Pipe<T>(this T @this, Action next)
    {
        next();
        return @this;
    }

    public static T PipeIf<T>(this T @this, bool condition, Action next)
    {
        if (condition)
        {
            next();
        }

        return @this;
    }

    public static TOut PipeIf<T, TOut>(
        this T @this,
        bool condition,
        Func<T, TOut> next) where T : TOut
    {
        return condition ? next(@this) : @this;
    }

    public static TOut PipeIfNotNull<T, TOut>(
        this T @this,
        Func<T, TOut> next) where T : TOut
    {
        return @this is not null ? next(@this) : @this;
    }
}
