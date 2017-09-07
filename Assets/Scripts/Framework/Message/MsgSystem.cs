using System.CodeDom;
using System.Collections.Generic;


namespace Lite
{
    public delegate void CmdCallback(Message msg);


    public enum MsgLevel
    {
        Append,
        Insert,
        Exec
    }


    public class MsgSystem
    {
        public static MsgSystem Inst { get; private set; }
        protected readonly object SyncMsgMap = new object();
        protected Dictionary<int, CmdCallback> MsgMap = new Dictionary<int, CmdCallback>();
        protected readonly object SyncMsgQ = new object();
        protected Queue<Message> MsgQueue = new Queue<Message>();

        public MsgSystem()
        {
            Inst = this;
        }

        public void Init()
        {
            
        }

        public void Tick()
        {
            lock (SyncMsgQ)
            {
                while (MsgQueue.Count > 0)
                {
                    Execute(MsgQueue.Dequeue());
                }
            }
        }

        public void Push(Message msg)
        {
            if (msg.Level == MsgLevel.Exec)
            {
                Execute(msg);
                return;
            }
            lock (SyncMsgQ)
            {
                MsgQueue.Enqueue(msg);
            }
        }

        public virtual void Execute(Message msg)
        {
            lock (SyncMsgMap)
            {
                if (MsgMap.ContainsKey(msg.Type))
                {
                    MsgMap[msg.Type].Invoke(msg);
                }
            }
        }

        public virtual void Register(int commandName, CmdCallback command)
        {
            lock (SyncMsgMap)
            {
                if (!MsgMap.ContainsKey(commandName))
                {
                    MsgMap[commandName] = new CmdCallback(command);
                }
                else
                {
                    MsgMap[commandName] += new CmdCallback(command);
                }
            }
        }

        public virtual void Unregister(int commandName, CmdCallback commandType)
        {
            lock (SyncMsgMap)
            {
                if (MsgMap.ContainsKey(commandName))
                {
                    // ReSharper disable once DelegateSubtraction
                    MsgMap[commandName] -= commandType;
                }
            }
        }

    }
}