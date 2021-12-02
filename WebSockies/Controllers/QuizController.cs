using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSockies.Containers;
using WebSockies.Data;
using WebSockies.Data.Models;
using System.Json;

namespace WebSockies
{
    public class QuizController
    {
        private LobbyContainer _lobbyContainer;
        private UserContainer _userContainer;
        private QuestionContainer _questionContainer;
        private Random _random;

        public QuizController(LobbyContainer lobbyContainer, UserContainer userContainer, QuestionContainer questionContainer) {
            _userContainer = userContainer;
            _lobbyContainer = lobbyContainer;
            _questionContainer = questionContainer;
            _random = new Random();
        }

        public void SubmitAnswer(User user, string[] answerString)
        {
            Answer answer = JsonSerializer.Deserialize<Answer>(answerString[0]);
            Lobby lobby = _lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode);
            
            if (lobby.Settings.LobbyType == LobbySettings.LobbyTypeEnum.Standard && !lobby.HasAnswered.Contains(user))
            {
                lobby.HasAnswered.Add(user);

                if (lobby.HasAnswered.Count == _userContainer.users.FindAll(p => p.LobbyInviteCode == user.LobbyInviteCode).Count)
                {
                    var correctAnswer = lobby.Quiz.Questions[lobby.CurrentQuestion].Answers.Find(a => a.IsCorrect);
                    if (correctAnswer == answer)
                    {
                        user.Score = +CalcScore(user.LobbyInviteCode, answer);
                    }

                    GetLobbyScore(user.LobbyInviteCode);

                    NextQuestion(lobby);

                }
            }
            else if (lobby.Settings.LobbyType == LobbySettings.LobbyTypeEnum.Traditional && !lobby.HasAnswered.Contains(user))
            {
                lobby.HasAnswered.Add(user);

                if (lobby.HasAnswered.Count <= 1)
                {
                    user.Score += 500;
                    NextQuestion(lobby);
                }
            }
        }

        public void StartQuiz(User user)
        {
            Lobby lobby = _lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode);
            if (user.Id == lobby.OwnerId)
            {
                _lobbyContainer.Lobbies[_lobbyContainer.Lobbies.IndexOf(lobby)].IsOpen = false;
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "OK", "Quiz started")));
                NextQuestion(lobby);
            }
        }
        
        public void NextQuestion(Lobby lobby)
        {
            if (lobby != null)
            {
                lobby.HasAnswered.Clear();
                User NextQuestionUser = SelectRandomUserFromLobby(lobby.InviteCode);
                lobby.Quiz.Questions[lobby.CurrentQuestion].Answered = true;
                SendQuestion(NextQuestionUser, lobby.Quiz.Questions[lobby.CurrentQuestion]);
            }
        }
        public User SelectRandomUserFromLobby(string lobbyInviteCode)
        {
            List<User> userList = _userContainer.users.FindAll(t => t.LobbyInviteCode == lobbyInviteCode);
            int random = _random.Next(0, userList.Count);
            return userList[random];
        }
        public int CalcScore(string lobbyInviteCode, Answer answer) {
            TimeSpan TimeToAnswer = DateTime.Now - _questionContainer.Questions.Find(h => h.Id == answer.QuestionId).TimeStarted;
            int timetoanswer = (int)Math.Round(TimeToAnswer.TotalMilliseconds);
            int SettingsTimePerQuestion = _lobbyContainer.Lobbies.Find(f => f.InviteCode == lobbyInviteCode).Settings.TimePerQuestion * 1000;
            int basescore = 1000;
            double ScoreDecayPerMs = (basescore / SettingsTimePerQuestion);
            return (int)Math.Round(basescore - (timetoanswer * ScoreDecayPerMs));
        }

        public void GetLobbyScore(string lobbyInviteCode) {
            List<User> userList = _userContainer.users.FindAll(u => u.LobbyInviteCode == lobbyInviteCode);
            foreach (User user in userList)
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("SplashScreen","OK", JsonSerializer.Serialize(userList))));
            }
        
        }
        public void SendQuestion(User user, Question question) {
            user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("Question", "OK", JsonSerializer.Serialize(question) )));


        }
        public void SplashScreenScore(string lobbyCode) {
            List<User> userList = _userContainer.GetUserByLobbyID(lobbyCode);
            Dictionary<string, int> userScores = new Dictionary<string, int>();
            //dit werkt mogelijk niet
            //Pieter zegt dat het werkt
            foreach (User user in userList)
            {
                userScores.Add(user.Username, user.Score);
            }

            foreach (User user in userList)
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("SplashScreen" , "OK", JsonSerializer.Serialize(userScores))));
            }
        }
    }
}