using System;

namespace WebSockies
{
    public class HelloWorldModel: MessageModel
    {
        public void HelloWorld()
        {
            Console.WriteLine("Hello World");
        }

        public HelloWorldModel(UserContainer userContainer)
        {
            
        }
    }
}