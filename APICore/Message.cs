using System;

namespace APICore
{
    public enum MessageTypes
    {
        GetAll,
        Get,
        Add,
        Update,
        Delete,
        Login,
        Logout,
        Exception
    }
    
    public static class Message
    {
        #region Private Fields
        private static readonly string[] Bases = { "Failed !", "Complete." };
        #endregion
        
        public static string GetMessage(MessageTypes messageType, bool messageValue = false)
        {
            var messageValueType = messageValue ? Bases[1] : Bases[0];
            
            return messageType switch
            {
                MessageTypes.GetAll => $"Get All Data {messageValueType}",
                MessageTypes.Get => $"Get {messageValueType}",
                MessageTypes.Add => $"Insert {messageValueType}",
                MessageTypes.Update => $"Update {messageValueType}",
                MessageTypes.Delete => $"Delete {messageValueType}",
                MessageTypes.Login => $"Authentication {messageValueType}",
                MessageTypes.Logout => $"Logout {messageValueType}",
                MessageTypes.Exception => $"System error !",
                _ => "No message !"
            };
        }

    }
}