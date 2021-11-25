using System;
using System.Collections.Generic;
using WebSocketSharp;
using System.Text.Json;
using System.Json;

namespace WebSockies.Specs.Drivers
{
    public class WebSocketDriver: IDisposable
    {
        private bool _isDisposed;
        private List<JsonValue> _queuedMessages = new List<JsonValue>();
        public string Username;

        private WebSocket _ws;

        public WebSocketDriver(string username)
        {
            Username = username;
        }

        public void CreateConnection(string url, string username)
        {
            _ws = new WebSocket( "ws://"+ url + "?name=" + username);
            _ws.OnMessage += (sender, e) =>
            {
                _queuedMessages.Add(JsonValue.Parse(e.Data));
            };
            _ws.Connect();
        }

        public void SendMessage(string message)
        {
            _ws.Send(message);
        }

        public JsonValue FindMessage(Predicate<JsonValue> match)
        {
            var toBeReturned = _queuedMessages.Find(match);
            _queuedMessages.Remove(toBeReturned);
            return toBeReturned;
        }
        
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_ws.IsAlive)
            {
                _ws.Close();
            }

            _isDisposed = true;
        }
    }
}