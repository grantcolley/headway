namespace RemediatR.Core.Constants
{
    public static class RemediatRAuthorisation
    {
        // Permissions
        public const string CUSTOMER_READ = "Customer Read";
        public const string CUSTOMER_WRITE = "Customer Write";
        public const string REDRESS_READ = "Redress Read";
        public const string REDRESS_WRITE = "Redress Write";
        public const string REDRESS_TRANSITION = "Redress Transition";
        public const string COMMUNICATION_DISPATCH_TRANSITION = "Communication Dispatch Transition";
        public const string AWAITING_REPONSE_TRANSITION = "Awaiting Response Transition";
        public const string REDRESS_REVIEW_TRANSITION = "Redress Review Transition";
        public const string REDRESS_COMPLETE = "Redress Complete";
        public const string REFUND_READ = "Refund Read";
        public const string REFUND_WRITE = "Refund Write";
        public const string REFUND_CACULATION_COMPLETE = "Refund Calculation Complete";
        public const string REFUND_VERIFICATION_COMPLETE = "Refund Varification Complete";
        public const string REFUND_REVIEW_TRANSITION = "Refund Review Transition";

        // Roles
        public const string REDRESS_CASE_OWNER = "Redress Case Owner";
        public const string REDRESS_REVIEWER = "Redress Reviewer";
        public const string REFUND_ASSESSOR = "Refund Assessor";
        public const string REFUND_REVIEWER = "Refund Reviewer";
    }
}
