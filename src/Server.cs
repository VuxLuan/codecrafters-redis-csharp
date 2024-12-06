using System.Net;
using System.Net.Sockets;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

// Uncomment this block to pass the first stage
var server = new TcpListener(IPAddress.Any, 6379);
server.Start();
while (true) {
    try {
        Socket clientSocket = await server.AcceptSocketAsync();
        _ = Task.Run(() => HandleRequestAsync(clientSocket));
    } catch (Exception ex) {
        Console.WriteLine(ex);
        throw;
    }
}

async Task HandleRequestAsync(Socket socket)
{
   //var buffer = new byte[1024];
    //var bytesRead = await socket.ReceiveAsync(buffer, SocketFlags.None);
    await socket.SendAsync(new ArraySegment<byte>("+PONG\r\n"u8.ToArray()), SocketFlags.None);
}


