namespace Fortune_Teller_UI.Services
{
    public static class HttpClients
    {
        public static string WithRetry = "fortunesWithHystrixRetry";

        public static string WithUserCommand = "fortunesWithUserCommand";

        public static string WithInlineCommand = "fortunesWithInlineCommand";

        public static string WithoutHystrix = "fortunesWithoutHystrixHandler";
    }
}
