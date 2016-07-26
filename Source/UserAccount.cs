namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAccount
    {
        public int Rid { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserComment { get; set; }
        public byte[] EncLmHash { get; set; }
        public byte[] EncNtHash { get; set; }
        public string LmHash { get; set; }
        public string NtHash { get; set; }
        public string LastLoginDate { get; set; }
        public string PasswordResetDate { get; set; }
        public string AccountExpiryDate { get; set; }
        public string LoginFailedDate { get; set; }
        public long LoginCount { get; set; }
        public long FailedLogins { get; set; }
        public string ProfilePath { get; set; }
        public string Groups { get; set; }
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserAccount()
        {
            LoginName = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            UserComment = string.Empty;
            LmHash = string.Empty;
            NtHash = string.Empty;
            LastLoginDate = string.Empty;
            PasswordResetDate = string.Empty;
            AccountExpiryDate = string.Empty;
            LoginFailedDate = string.Empty;
            ProfilePath = string.Empty;
            Groups = string.Empty;

            EncLmHash = new byte[0];
            EncNtHash = new byte[0];
        }
    }
}
