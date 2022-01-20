namespace Dio.MiniRPG.Exceptions
{
    [System.Serializable]
    public class InvalidTargetsException : System.Exception
    {
        public InvalidTargetsException() : base("Invalid target group for CharacterAction") { }
        public InvalidTargetsException(string message) : base(message) { }
        public InvalidTargetsException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidTargetsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}