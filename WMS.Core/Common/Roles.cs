namespace WMS.Core.Common
{
    public static class Roles
    {
        public const string Manager = "Manager";
        public const string Warehouseman = "Warehouseman";
        public const string Analyst = "Analyst";
        public const string User = "User";

        public static readonly string[] All = [Manager,Warehouseman, Analyst, User];
    }
}
