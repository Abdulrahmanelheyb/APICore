namespace APICore
{
    public static class Message
    {
        #region Private Fields
        private static readonly string[] Bases = { "Failed !", "Complete." };
        #endregion

        #region Publics
        // CRUD Messages
        public static readonly string[] GetAll = { $"Get All Data {Bases[0]}", $"Get All Data {Bases[1]}"};
        public static readonly string[] Get = { $"Get {Bases[0]}", $"Get {Bases[1]}"};
        public static readonly string[] Add = { $"Insert {Bases[0]}", $"Insert {Bases[1]}"};
        public static readonly string[] Update = { $"Update {Bases[0]}", $"Update {Bases[1]}"};
        public static readonly string[] Delete = { $"Delete {Bases[0]}", $"Delete {Bases[1]}"};

        public const string Expection = "Error ! \n";

        #endregion

    }
}