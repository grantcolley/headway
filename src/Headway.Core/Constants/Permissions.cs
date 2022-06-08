namespace Headway.Core.Constants
{
    public static class Permissions
    {
        // Default
        public const string ADMIN = "Admin";
        public const string USER = "User";
        public const string DEVELOPER = "Developer";

        // RemediatR
        public const string REDRESS_READ = "Redress Read";
        public const string REDRESS_WRITE = "Redress Write";
        public const string REDRESS_TRANSITION = "Redress Transition";
        public const string COMMUNICATION_DISPATCH = "Communication Dispatch Transition";
        public const string AWAITING_REPONSE_TRANSITION = "Awaiting Response Transition";
        public const string REDRESS_REVIEW_TRANSITION = "Redress Review Transition";
        public const string REDRESS_COMPLETE = "Redress Complete";
        public const string REFUND_CACULATION = "Refund Calculation Complete";
        public const string REFUND_VERIFICATION_COMPLETE = "Refund Varification Complete";
        public const string REFUND_REVIEW_TRANSITION = "Refund Review Transition";
    }
}
