using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WebSockies.Specs.Drivers;

namespace WebSockies.Specs.Hooks
{
    [Binding]
    public class Hooks
    {
        [BeforeScenario()]
        public void SetupTestLobby(ScenarioContext scenarioContext)
        {
            var ws = new WebSocketDriver("lobbyOwner");
            ws.CreateConnection("127.0.0.1:8001", "lobbyOwner");
            ws.SendMessage("{\"Controller\" : \"LobbyController\",\"Method\": \"CreateLobby\",\"Parameters\": []}");
            Task.Delay(3000).GetAwaiter().GetResult();
            var message = ws.FindMessage(m => m["Type"] == "InviteCode");
            scenarioContext["drivers"] = new List<WebSocketDriver>() { ws };
            scenarioContext["InviteCode"] = message["Content"];
        }
    }
}