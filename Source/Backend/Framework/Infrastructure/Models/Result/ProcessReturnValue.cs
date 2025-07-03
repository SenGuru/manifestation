namespace Framework.Infrastructure.Models.Result
{
    public class ProcessReturnValue
    {
        public bool Result { get; set; }

        public string OutputStreamSting { get; set; }

        public string ErrorStreamSting { get; set; }

        public int ExitCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
