using System.Net;
using System.Net.Sockets;
using System.Text;

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
    try
    {
        var buffer = new byte[1024];
        while (true)
        {
            var bytesRead = await socket.ReceiveAsync(buffer, SocketFlags.None);
            if (bytesRead == 0)
                break;
            
            var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if (request.StartsWith("*1\r\n$4\r\nPING\r\n"))
            {
                // RESP-compliant response for PING
                var response = new ArraySegment<byte>("+PONG\r\n"u8.ToArray());
                await socket.SendAsync(response, SocketFlags.None);
            }
            else
            {
                // RESP error for unknown commands
                var errorResponse = new ArraySegment<byte>("-ERR unknown command\r\n"u8.ToArray());
                await socket.SendAsync(errorResponse, SocketFlags.None);
            }
            
        }

    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    finally
    {
        socket.Close();
    }
    
}


