using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public class CommandQueue
    {
        public Queue<string> CurrentCommands { get; set; }
        public virtual void OnCommandReceived(CommandReceivedEventArgs e)
        {
             
        }
    }
    public class CommandReceivedEventArgs : EventArgs
    {
        public string Command { get; set; }
    }
    
}
