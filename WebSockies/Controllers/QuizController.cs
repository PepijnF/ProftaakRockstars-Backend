using System;
using System.Collections.Generic;
using WebSockies.Containers;
using WebSockies.Data;

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

        public void submitAnswer(User user, Answer answer)
        {
            
            if (!_lobbyContainer.Lobbies.Find(l => l.InviteCode == user.RoomNumber).HasAnswered.Contains(user))
            {
                _lobbyContainer.Lobbies.Find(k => k.InviteCode == user.RoomNumber).HasAnswered.Add(user);

                if (_lobbyContainer.Lobbies.Find(l => l.InviteCode == user.RoomNumber).HasAnswered.Count == _userContainer.users.FindAll(p => p.RoomNumber == user.RoomNumber).Count)
                {
                    TimeSpan test = DateTime.Now - _questionContainer.Questions.Find(h => h.Id == answer.QuestionId).TimeStarted;
                    int timetoanswer = (int)Math.Round(test.TotalMilliseconds);
                    int SettingsTimePerQuestion = _lobbyContainer.Lobbies.Find(f => f.InviteCode == user.RoomNumber).Settings.TimePerQuestion * 1000;
                    int basescore = 1000;
                    double ScoreDecayPerMs = (basescore / SettingsTimePerQuestion);
                    user.Score = (int)Math.Round(basescore - (timetoanswer * ScoreDecayPerMs));
                    Console.WriteLine(user.Score);
                    NextQuestion(user);
                }

            }

        }
        public void NextQuestion(User user)
        {
            _lobbyContainer.Lobbies.Find(o => o.InviteCode == user.RoomNumber).HasAnswered.Clear();


        }
    }
}