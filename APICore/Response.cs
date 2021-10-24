namespace APICore
{
    /// <summary>
    /// This response class created with included variables is status,
    /// message and data, for returns response Info with requested data
    /// </summary>
    /// <typeparam name="T">Response data type</typeparam>
    public struct Response<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
