namespace NovelGame.Master.Scripts.Editor
{
    public static class LogStringUtility
    {
        public static string WarningString(this string message) => $"<color=yellow>{message}</color>";
        public static string ErrorString(this string message) => $"<color=red>{message}</color>";
    }
}
