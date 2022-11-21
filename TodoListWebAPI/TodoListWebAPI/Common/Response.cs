namespace TodoListWebAPI.Common
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Response(object data)
        {
            StatusCode = 200;
            Message = "Success";
            Data = data;
        }

        public Response(int statusCode, string message, object data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}
