using System;
using System.Json;
using System.Text.Json;
using Fleck;
using NUnit.Framework;
using Moq;
using WebSockies;
using WebSockies.Containers;
using WebSockies.Data;
using WebSockies.Data.Models;

namespace TestProjecie
{
    public class Tests
    {
        private LobbyContainer _lobbyContainer;
        private QuestionContainer _questionContainer;
        private UserContainer _userContainer;
        private string _sendCallback;
        
        
        [SetUp]
        public void Setup()
        {
            _lobbyContainer = new LobbyContainer();
            _questionContainer = new QuestionContainer();
            _userContainer = new UserContainer();
        }

        private User _createUser(string username)
        {
            var webSocketConnection = new Mock<IWebSocketConnection>();
            webSocketConnection.Setup(w => w.ConnectionInfo.Id).Returns(Guid.NewGuid);
            webSocketConnection.Setup(w => w.ConnectionInfo.Path).Returns("?name=Pepijn");
            webSocketConnection.Setup(w => w.Send(It.IsAny<string>())).Callback((string message) => _sendCallback = message);
            User user = new User(webSocketConnection.Object);

            _userContainer.users.Add(user);
            
            return user;
        }

        [Test]
        public void CreateLobby()
        {
            // Arrange
            User user = _createUser("Pepijn");
            
            LobbyController lobbyController = new LobbyController(_userContainer, _lobbyContainer);
            
            // Act
            lobbyController.CreateLobby(user);
            
            // Assert
            Assert.True(_lobbyContainer.Lobbies.Exists(l => l.OwnerId == user.Id));
            
        }

        [Test]
        public void JoinLobby()
        {
            // Arrange
            LobbyController lobbyController = new LobbyController(_userContainer, _lobbyContainer);
            User owner = _createUser("owner");
            lobbyController.CreateLobby(owner);
            owner = _userContainer.users.Find(u => u.Id == owner.Id);
            User user = _createUser("test");

            // Act
            lobbyController.JoinLobby(user, new []{owner.LobbyInviteCode});

            // Assert
            Assert.True(_lobbyContainer.Lobbies.Exists(l => l.Users.Exists(u => u.Id == user.Id)));
        }

        [Test]
        public void StartQuiz()
        {
            // Arrange
            LobbyController lobbyController = new LobbyController(_userContainer, _lobbyContainer);
            QuizController quizController = new QuizController(_userContainer, _lobbyContainer, _questionContainer);
            // Setup lobby
            User owner = _createUser("owner");
            lobbyController.CreateLobby(owner);
            owner = _userContainer.users.Find(u => u.Id == owner.Id);
            User user = _createUser("test");

            // Act
            // Join lobby
            lobbyController.JoinLobby(user, new []{owner.LobbyInviteCode});
            
            // Start quiz
            quizController.StartQuiz(owner);
            
            // Assert
            var response = JsonSerializer.Deserialize<ResponseModel>(_sendCallback);
            Assert.True(response.Content.Contains("Ja"));
        }
    }
}