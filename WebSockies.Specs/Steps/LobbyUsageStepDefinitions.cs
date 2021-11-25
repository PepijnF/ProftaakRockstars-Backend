using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WebSockies.Specs.Drivers;
using Xunit;

namespace WebSockies.Specs.Steps
{
    [Binding]
    public sealed class LobbyUsageStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly List<WebSocketDriver> _webSocketDrivers;
        private readonly ScenarioContext _scenarioContext;
        private int currentUserInt;

        public LobbyUsageStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _webSocketDrivers = (List<WebSocketDriver>)_scenarioContext["drivers"];
        }


        [Given(@"the username of the player (.*)")]
        public void GivenTheUsernameOfThePlayerTestUser(string username)
        {
            var ws = new WebSocketDriver(username);
            ws.CreateConnection("127.0.0.1:8001", username);
            _webSocketDrivers.Add(ws);
        }

        [Then(@"(.*) creates a new lobby")]
        public void ThenThePlayerCreatesANewLobby(string username)
        {
            _webSocketDrivers.Find(u => u.Username == username).SendMessage("{\"Controller\" : \"LobbyController\",\"Method\": \"CreateLobby\",\"Parameters\": []}");
        }

        [Then(@"a new lobby will be created")]
        public async Task ThenANewLobbyWillBeCreated()
        {
            await Task.Delay(3000);
        }

        [Then(@"a lobby code will be returned by the server to (.*)")]
        public void ThenALobbyCodeWillBeReturnedByTheServer(string username)
        {
            var m = _webSocketDrivers.Find(u => u.Username == username).FindMessage(f => f["Type"] == "InviteCode");
            
            Assert.True(m["Status"] == "OK");

            if (m["Status"] == "OK")
            {
                _scenarioContext["InviteCode"] = m["Content"];
                Assert.True(true);
            }
            else
            {
                Assert.False(false);
            }
        }

        [Then(@"(.*) joins the lobby")]
        public void ThenThePlayerJoinsTheLobby(string username)
        {
            var ws = new WebSocketDriver(username);
            ws.CreateConnection("127.0.0.1:8001", username);
            ws.SendMessage("{\"Controller\" : \"LobbyController\",\"Method\": \"JoinLobby\",\"Parameters\": ["+ _scenarioContext["InviteCode"] + "]}");
            _webSocketDrivers.Add(ws);
            Task.Delay(5000).GetAwaiter().GetResult();
        }

        [Then(@"a list of players is returned to (.*)")]
        public void ThenAListOfPlayersIsReturnedToLobbyJoiner(string username)
        {
            var message = _webSocketDrivers.Find(u => u.Username == username).FindMessage(m => m["Type"] == "UserList");
            Assert.True(message["Status"] == "OK");
            
        }
    }
}