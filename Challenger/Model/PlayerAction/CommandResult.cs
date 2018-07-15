namespace Challenger.Model.PlayerAction
{
    public class CommandResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public dynamic ResultObject { get; set; }
    }
}
