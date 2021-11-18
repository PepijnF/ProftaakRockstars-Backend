﻿using System;
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
            if (lobby != null && lobby.HasAnswered.Contains(user))
            {
                lobby.HasAnswered.Add(user);

                if (lobby.HasAnswered.Count == _userContainer.users.FindAll(p => p.LobbyInviteCode == user.LobbyInviteCode).Count)
                {
                    var correctAnswer = lobby.Quiz.Questions[lobby.CurrentQuestion].Answers.Find(a => a.IsCorrect);
                    if (correctAnswer == answer)
                    {
                        user.Score = CalcScore(user, answer);
                    }
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
            User NextQuestionUser = SelectRandomUser(user);
        }
        public User SelectRandomUser(User user)
        {
            List<User> userList = _userContainer.users.FindAll(t => t.LobbyInviteCode == user.LobbyInviteCode);
            int random = _random.Next(0, userList.Count);
            return userList[random];
        }
        public int CalcScore(User user, Answer answer) {
            TimeSpan TimeToAnswer = DateTime.Now - _questionContainer.Questions.Find(h => h.Id == answer.QuestionId).TimeStarted;
            int timetoanswer = (int)Math.Round(TimeToAnswer.TotalMilliseconds);
            int SettingsTimePerQuestion = _lobbyContainer.Lobbies.Find(f => f.InviteCode == user.LobbyInviteCode).Settings.TimePerQuestion * 1000;
            int basescore = 1000;
            double ScoreDecayPerMs = (basescore / SettingsTimePerQuestion);
            return user.Score + (int)Math.Round(basescore - (timetoanswer * ScoreDecayPerMs));
        }
    }
}