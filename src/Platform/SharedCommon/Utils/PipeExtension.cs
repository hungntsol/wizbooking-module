namespace SharedCommon.Utils;

/// <summary>
/// </summary>
public static class PipeExtension
{
    public static TOut Pipe<T, TOut>(this T @this, Func<T, TOut> next)
    {
        return next(@this);
    }

    public static T Pipe<T>(this T @this, Action<T> action)
    {
        action(@this);

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