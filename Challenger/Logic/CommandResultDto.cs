using System;

namespace Challenger.Logic
{
    public class CommandResultDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string ResultObjectJson { get; set; }

        public Type ResultObjectType { get; set; }
    }
}
