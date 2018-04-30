using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace Server
{
    class AsyncUserToken
    {
        Socket m_socket;

        public AsyncUserToken() : this(null) { }

        public AsyncUserToken(Socket socket)
        {
            m_socket = socket;
        }

        public Socket Socket
        {
            get { return m_socket; }
            set { m_socket = value; }
        }
    }
    class Connector
    {
        const int receiveBufferSize = 1000;
        const int port = 20184;
        const int maxConnetions = 10;
        int _numConnectedSockets;
        byte[] _buffer;
        Semaphore _maxNumberAcceptedClients;
        Socket listenSocket;
        Stack<SocketAsyncEventArgs> _pool;




        public Connector()
        {
            _numConnectedSockets = 0;
            _pool = new Stack<SocketAsyncEventArgs>(maxConnetions);
            _maxNumberAcceptedClients = new Semaphore(maxConnetions, maxConnetions);
            _buffer = new byte[receiveBufferSize * maxConnetions * 2];
            SocketAsyncEventArgs readWriteEventArg;
            for (int i = 0; i < maxConnetions; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();   //Pre-allocate a set of reusable SockeyAsyncEventArgs
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();
                readWriteEventArg.SetBuffer(_buffer, i * receiveBufferSize, receiveBufferSize);  // Assign a byte buffer from the buffer pool
                Push(readWriteEventArg);
            }

        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgPoll cannot be null");
            }
            lock (_pool)
            {
                _pool.Push(item);
            }
            Debug.WriteLine("Push an item in the event argument pool");
        }
        public SocketAsyncEventArgs Pop()
        {
            lock (_pool)
            {
                return _pool.Pop();
            }
            Debug.WriteLine("Pop an event argument from the pool");
        }

        public void Start()
        {
            //Build listener and start listening
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(10);
            StartAccept(null);    //post accepts on the listening socket
            Thread.Sleep(-1);    // Never suspend the thread
        }

        private void StartAccept(SocketAsyncEventArgs acceptEvenArg)
        {
            if (acceptEvenArg == null)
            {
                acceptEvenArg = new SocketAsyncEventArgs();
                acceptEvenArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                acceptEvenArg.AcceptSocket = null;     //listen socket must be cleared for the next new connecting request from client
            }
            _maxNumberAcceptedClients.WaitOne();
            if (!listenSocket.AcceptAsync(acceptEvenArg))   //I/O completed synchronously, no event raised, must handle now.
            {
                ProcessAccept(acceptEvenArg);
            }
        }

        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Interlocked.Increment(ref _numConnectedSockets);
            Debug.WriteLine("Number of Connected Sockets is {0}", _numConnectedSockets);

            SocketAsyncEventArgs readEventArgs = Pop();    // Get the socket for the accepted connection 
            ((AsyncUserToken)readEventArgs.UserToken).Socket = e.AcceptSocket;    // put the socket into the readEventArg object user token
            //As soon as the client is connected, start an asynchronous request to reveive, so that it can be ready for accepting a new one and not loosing the accepted one
            readEventArgs.SetBuffer(readEventArgs.Offset, receiveBufferSize);
            if (!e.AcceptSocket.ReceiveAsync(readEventArgs))   // I/O completed asynchronously, no event raised, must handle now.
            {
                ProcessReceive(e);
            }
            StartAccept(e);   // Accept the next connection request
        }

        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            //determin which type of operation just completed and call the associated handle
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");

            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
            {
                CloseClientSocket(e);
                return;
            }

            //Read the data sent from client
            String receives = Encoding.ASCII.GetString(e.Buffer, e.Offset, e.BytesTransferred);
            Console.WriteLine(receives);


            String response = "Hi, dear";   //////////////////Process message here, it will be re-edited later//////////////////
            byte[] resp = Encoding.ASCII.GetBytes(response);
            Buffer.BlockCopy(resp, 0, e.Buffer, e.Offset, resp.Length);  // Copy into the transmitt buffer
            e.SetBuffer(e.Offset, resp.Length);
            // Start an asynchronous request to send
            if (!token.Socket.SendAsync(e))     //I/O completed synchronously, no event raised, must handle now
            {
                ProcessSend(e);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;    //Done transmitting
                e.SetBuffer(e.Offset, receiveBufferSize);    //Restore buffer size for the receiver
                //Start an asynchronous request to receive
                if (!token.Socket.ReceiveAsync(e))      //I/O completed synchronously, no event raised, must handle now
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;

            // Close the socket associated with the client
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error shuttin down socket: Source {0}, Message: {1}", ex.Source, ex.Message);
            }
            token.Socket.Close();

            _maxNumberAcceptedClients.Release();
            Interlocked.Decrement(ref _numConnectedSockets);
            Debug.WriteLine("Connecttion dropped, {0} remaining.", _numConnectedSockets);
            Push(e);    //Free the SocketAsyncEventArg, so that they can be reused by another client
        }

    }
}
