using Challenger.Model;
using ChallengeModel.PlayerAction;
using Newtonsoft.Json;
using System.Text;

namespace Challenger.Logic
{
    public class ChallengeManager
    {
        private ShipManager _shipManager;
        private StateManager _stateManager;

        public ChallengeManager(ChallengeState state, string playerName)
        {
            _shipManager = new ShipManager(state, playerName);
            _stateManager = new StateManager(state, playerName);
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
                case "State": return _stateManager.ProcessCommand(command);
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