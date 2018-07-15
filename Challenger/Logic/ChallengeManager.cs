using Challenger.Model;
using Challenger.Model.PlayerAction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenger.Logic
{
    public class ChallengeManager
    {
        private ChallengeConfiguration _config;
        private ShipManager _shipManager;

        public ChallengeManager(ChallengeConfiguration config)
        {
            _config = config;
            _shipManager = new ShipManager(config);
        }

        public byte[] ProcessCommand(byte[] command)
        {
            var cmd = JsonConvert.DeserializeObject<Command>(Encoding.UTF8.GetString(command));
            var dto = GetCommandResult(cmd);
            var result = ParseCommandResult(dto);
            return result;
        }

        private CommandResultDto GetCommandResult(Command command)
        {
            switch (command.Type)
            {
                case "Ship": return _shipManager.ProcessCommand(command);
                default: return new CommandResultDto
                {
                    Success = false,
                    Message = "Invalid Command Type.",
                    ResultObjectJson = null,
                    ResultObjectType = typeof(object)
                };
            }
        }

        private byte[] ParseCommandResult(CommandResultDto dto)
        {
            var resultObject = JsonConvert.DeserializeObject(dto.ResultObjectJson, dto.ResultObjectType);
            var commandResult = new CommandResult()
            {
                Success = dto.Success,
                Message = dto.Message,
                ResultObject = resultObject
            };

            var json = JsonConvert.SerializeObject(commandResult);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}