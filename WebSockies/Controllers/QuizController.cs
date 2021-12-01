using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSockies.Containers;
using WebSockies.Data;
using WebSockies.Data.Models;

namespace WebSockies
{
    public class QuizController
    {
        private LobbyContainer _lobbyContainer;
        private UserContainer _userContainer;
        private QuestionContainer _questionContainer;

        public QuizController(LobbyContainer lobbyContainer, UserContainer userContainer, QuestionContainer questionContainer) {
            _userContainer = userContainer;
            _lobbyContainer = lobbyContainer;
            _questionContainer = questionContainer;
        }

        public void SubmitAnswer(User user, string[] answerString)
        {
            Answer answer = JsonSerializer.Deserialize<Answer>(answerString[0]);

            if (_lobbyContainer.Lobbies.Find(test => test.InviteCode == user.LobbyInviteCode).LobbyType.Equals(0) && !_lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode).HasAnswered.Contains(user))
            {
                _lobbyContainer.Lobbies.Find(k => k.InviteCode == user.LobbyInviteCode).HasAnswered.Add(user);

                if (_lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode).HasAnswered.Count == _userContainer.users.FindAll(p => p.LobbyInviteCode == user.LobbyInviteCode).Count)
                {
                    TimeSpan test = DateTime.Now - _questionContainer.Questions.Find(h => h.Id == answer.QuestionId).TimeStarted;
                    int timeToAnswer = (int)Math.Round(test.TotalMilliseconds);
                    int settingsTimePerQuestion = _lobbyContainer.Lobbies.Find(f => f.InviteCode == user.LobbyInviteCode).Settings.TimePerQuestion * 1000;
                    int baseScore = 1000;
                    double scoreDecayPerMs = (baseScore / settingsTimePerQuestion);
                    user.Score = (int)Math.Round(baseScore - (timeToAnswer * scoreDecayPerMs));
                    Console.WriteLine(user.Score);
                    NextQuestion(user);
                }
            }
            else if (_lobbyContainer.Lobbies.Find(test => test.InviteCode == user.LobbyInviteCode).LobbyType.Equals(1))
            {
                SubmitAnswerTradQuiz(user, answerString);
            }
        }

        public void SubmitAnswerTradQuiz(User user, string[] answerString)
        {
            Answer answer = JsonSerializer.Deserialize<Answer>(answerString[0]);

            if (!_lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode).HasAnswered.Contains(user))
            {
                _lobbyContainer.Lobbies.Find(k => k.InviteCode == user.LobbyInviteCode).HasAnswered.Add(user);

                if (_lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode).HasAnswered.Count <= 1)
                {
                    user.Score = 500;
                    Console.WriteLine(user.Score);
                    NextQuestion(user);
                }
                else if (_lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode).HasAnswered.Count > 1)
                {
                    user.Score = 0;
                    Console.WriteLine(user.Score);
                    NextQuestion(user);
                }
            }
        }

        public void StartQuiz(User user)
        {
            var lobby = _lobbyContainer.Lobbies.Find(l => l.InviteCode == user.LobbyInviteCode);
            if (user.Id == lobby.OwnerId)
            {
                _lobbyContainer.Lobbies[_lobbyContainer.Lobbies.IndexOf(lobby)].IsOpen = false;
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "OK", "Quiz started")));
                NextQuestion(user);
            }
        }
        
        public void NextQuestion(User user)
        {
            _lobbyContainer.Lobbies.Find(o => o.InviteCode == user.LobbyInviteCode).HasAnswered.Clear();

        }

        public void CalcScore(User user, Answer answer) {
            TimeSpan TimeToAnswer = DateTime.Now - _questionContainer.Questions.Find(h => h.Id == answer.QuestionId).TimeStarted;
            int timetoanswer = (int)Math.Round(TimeToAnswer.TotalMilliseconds);
            int SettingsTimePerQuestion = _lobbyContainer.Lobbies.Find(f => f.InviteCode == user.LobbyInviteCode).Settings.TimePerQuestion * 1000;
            int basescore = 1000;
            double ScoreDecayPerMs = (basescore / SettingsTimePerQuestion);
            user.Score = user.Score + (int)Math.Round(basescore - (timetoanswer * ScoreDecayPerMs));
            Console.WriteLine(user.Score);
        }
    }
}