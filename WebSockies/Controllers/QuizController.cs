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

        public QuizController(UserContainer userContainer, LobbyContainer lobbyContainer, QuestionContainer questionContainer) {
            _userContainer = userContainer;
            _lobbyContainer = lobbyContainer;
            _questionContainer = questionContainer;
            _random = new Random();
        }

        public void SubmitAnswer(User user, object[] answers)
        {
            Answer answer = JsonSerializer.Deserialize<Answer>(answers[0].ToString());
            Lobby lobby = _lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode);
            
            if (lobby.Settings.LobbyType == LobbySettings.LobbyTypeEnum.Standard && !lobby.HasAnswered.Contains(user))
            {
                lobby.HasAnswered.Add(user);

                // + 1 because there is one person with the question and no way to answer
                if (lobby.HasAnswered.Count + 1 == _userContainer.users.FindAll(p => p.LobbyInviteCode == user.LobbyInviteCode).Count)
                {
                    var correctAnswer = lobby.Quiz.Questions[lobby.CurrentQuestion].Answers.Find(a => a.IsCorrect);
                    if (correctAnswer.AnswerString == answer.AnswerString)
                    {
                        user.Score = +CalcScore(lobby, answer);
                    }

                    GetLobbyScore(user.LobbyInviteCode);

                    NextQuestion(user.LobbyInviteCode);

                }
            }
            else if (lobby.Settings.LobbyType == LobbySettings.LobbyTypeEnum.Traditional && !lobby.HasAnswered.Contains(user))
            {
                lobby.HasAnswered.Add(user);

                if (lobby.HasAnswered.Count <= 1)
                {
                    user.Score += 500;
                    NextQuestion(user.LobbyInviteCode);
                }
            }
        }

        public void StartQuiz(User user)
        {
            Lobby lobby = _lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode);
            if (user.Id == lobby.OwnerId)
            {
                _lobbyContainer.Lobbies[_lobbyContainer.Lobbies.IndexOf(lobby)].IsOpen = false;
                user.SocketConnection.Send(
                    JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "OK", "Quiz started")));
                Console.WriteLine("Quiz has been started");
                NextQuestion(user.LobbyInviteCode);
            }
        }
        
        public void NextQuestion(string LobbyCode)
        {
            Lobby lobby = _lobbyContainer.Lobbies.Find(l => l.InviteCode == LobbyCode);
            if (lobby != null)
            {
                lobby.HasAnswered.Clear();
                User NextQuestionUser = SelectRandomUserFromLobby(lobby.InviteCode);
                SendQuestion(NextQuestionUser, lobby.Quiz.Questions[lobby.CurrentQuestion], lobby);
                lobby.Quiz.Questions[lobby.CurrentQuestion].Answered = true;
            }
        }
        
        
        public User SelectRandomUserFromLobby(string lobbyInviteCode)
        {
            List<User> userList = _userContainer.users.FindAll(t => t.LobbyInviteCode == lobbyInviteCode);
            int random = _random.Next(0, userList.Count);
            return userList[random];
        }
        public int CalcScore(Lobby lobby, Answer answer)
        {
            Question question = lobby.Quiz.Questions[lobby.CurrentQuestion];
            
            TimeSpan TimeToAnswer = DateTime.Now - question.TimeStarted;
            int timetoanswer = (int)Math.Round(TimeToAnswer.TotalMilliseconds);
            int SettingsTimePerQuestion =lobby.Settings.TimePerQuestion * 1000;
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
        public void SendQuestion(User questionUser, Question question, Lobby lobby) {
            //questionUser.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("QuestionString", "OK", JsonSerializer.Serialize(question.QuestionString) )));
            List<User> users = lobby.Users;
            List<string> QuestionStrings = new List<string>();
            foreach (Answer answer in question.Answers)
            {
                QuestionStrings.Add(answer.AnswerString);
            }
            foreach (var user in users)
            {
                //if (user.Id != questionUser.Id)
                //{
                    user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("Answers", "OK",
                        QuestionStrings)));
                //}
            }
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