namespace RemediatR.Core.Constants
{
    public static class RemediatRAuthorisation
    {
        // Permissions
        public const string CUSTOMER_READ = "Customer Read";
        public const string CUSTOMER_WRITE = "Customer Write";
        public const string REDRESS_READ = "Redress Read";
        public const string REDRESS_CASE_OWNER_WRITE = "Redress Case Owner Write";
        public const string REDRESS_REVIEWER_WRITE = "Redress Reviewer Write";
        public const string REFUND_ASSESSOR_WRITE = "Refund Assessor Write";
        public const string REFUND_REVIEWER_WRITE = "Refund Reviewer Write";

        // Roles
        public const string REDRESS_CASE_OWNER = "Redress Case Owner";
        public const string REDRESS_REVIEWER = "Redress Reviewer";
        public const string REFUND_ASSESSOR = "Refund Assessor";
        public const string REFUND_REVIEWER = "Refund Reviewer";
    }
}
