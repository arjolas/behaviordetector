public static class EventType
{
    public const string Click = "click";
    public const string Scroll = "scroll";
    public const string Keypress = "keypress";

    public static readonly HashSet<string> ValidTypes = new()
    {
        Click,
        Scroll,
        Keypress
    };
}
