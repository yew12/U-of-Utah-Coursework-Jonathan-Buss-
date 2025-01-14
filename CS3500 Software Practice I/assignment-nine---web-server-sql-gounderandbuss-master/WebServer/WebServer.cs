﻿
using Communications;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;

namespace StarterCode
{
    /// <summary>
    /// Author:   H. James de St. Germain
    /// Date:     Spring 2020
    /// Updated:  Spring 2022
    /// 
    /// Code for a simple web server
    /// </summary>
    class WebServer
    {
        /// <summary>
        /// keep track of how many requests have come in.  Just used
        /// for display purposes.
        /// </summary>
        static private int counter = 1;

        public static Networking web_server;

        /// <summary>
        ///  Test several connections and print the output to the console
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // create networking object
            web_server = new Networking(NullLogger.Instance, OnClientConnect, OnDisconnect, onMessage, '\n');

            Console.WriteLine("I am a webserver");

            // start the server - builds
            web_server.WaitForClients(11001, true);

            Console.Read();
            // connect to database

            // send http response to client
        }

        /// <summary>
        /// Basic connect handler - i.e., a browser has connected!
        /// Print an information message
        /// </summary>
        /// <param name="channel"> the Networking connection</param>
        internal static void OnClientConnect(Networking channel)
        {
            //throw new NotImplementedException("Print something about a connection happening");
            Console.WriteLine($"{channel.ID} has connected!");
        }

        /// <summary>
        /// Create the HTTP response header, containing items such as
        /// the "HTTP/1.1 200 OK" line.
        /// 
        /// See: https://www.tutorialspoint.com/http/http_responses.htm
        /// 
        /// Warning, don't forget that there have to be new lines at the
        /// end of this message!
        /// </summary>
        /// <param name="length"> how big a message are we sending</param>
        /// <param name="type"> usually html, but could be css</param>
        /// <returns>returns a string with the response header</returns>
        private static string BuildHTTPResponseHeader(int length, string type = "text/html")
        {
            return $@"
HTTP / 1.1 {length} OK
Connection: close
Content-Type: {type}; charset=UTF-8
 ";
        }

        /// <summary>
        ///   Create a web page!  The body of the returned message is the web page
        ///   "code" itself. Usually this would start with the doctype tag followed by the HTML element.  Take a look at:
        ///   https://www.sitepoint.com/a-basic-html5-template/
        ///   
        /// creates html code and returns it
        /// </summary>
        /// <returns> A string the represents a web page.</returns>
        private static string BuildHTTPBody()
        {
            // want to include info from database

            // maybe create placeholders for database information

            // FIXME: this should be a complete web page.
            return $@"
<h1>hello world</h1>
<a href='localhost:11000'>Reload</a> 
<br/>how are you...";
        }

        /// <summary>
        /// Create a response message string to send back to the connecting
        /// program (i.e., the web browser).  The string is of the form:
        /// 
        ///   HTTP Header
        ///   [new line]
        ///   HTTP Body
        ///  
        ///  The Header must follow the header protocol.
        ///  The body should follow the HTML doc protocol.
        /// </summary>
        /// <returns> the complete HTTP response</returns>
        private static string BuildMainPage()
        {
            string message = BuildHTTPBody();
            string header = BuildHTTPResponseHeader(message.Length);

            return header + message;
        }


        /// <summary>
        ///   <para>
        ///     When a request comes in (from a browser) this method will
        ///     be called by the Networking code.  Each line of the HTTP request
        ///     will come as a separate message.  The "line" we are interested in
        ///     is a PUT or GET request.  
        ///   </para>
        ///   <para>
        ///     The following messages are actionable:
        ///   </para>
        ///   <para>
        ///      get highscore - respond with a highscore page
        ///   </para>
        ///   <para>
        ///      get favicon - don't do anything (we don't support this)
        ///   </para>
        ///   <para>
        ///      get scores - along with a name, respond with a list of scores for the particular user
        ///   </para>
        ///   <para>
        ///     create - contact the DB and create the required tables and seed them with some dummy data
        ///   </para>
        ///   <para>
        ///     get index (or "", or "/") - send a happy home page back
        ///   </para>
        ///   <para>
        ///     get css/styles.css?v=1.0  - send your sites css file data back
        ///   </para>
        ///   <para>
        ///     otherwise send a page not found error
        ///   </para>
        ///   <para>
        ///     Warning: when you send a response, the web browser is going to expect the message to
        ///     be line by line (new line separated) but we use new line as a special character in our
        ///     networking object.  Thus, you have to send _every line of your response_ as a new Send message.
        ///   </para>
        /// </summary>
        /// <param name="network_message_state"> provided by the Networking code, contains socket and message</param>
        internal static void onMessage(Networking channel, string message)
        {
            if (message.StartsWith("GET"))
            {
            string responseHtml = $@"
<!DOCTYPE html>
<html>
<head>
<link rel='stylesheet' href='style.css'/>
</head>
  <body>

    <p>Welcome to the website!</p>
</body>
</html>
";
            int length = responseHtml.Length;
            string header = $@"HTTP/1.1 200 OK Content-Length:{length} Content-Type:text/html Connection:Closed".Replace("\n", " ") + "\n";
            string res = header + responseHtml;
                foreach (var line in res.Split())
                    channel.Send(line);
                channel.Send("\n\n");
                channel.Disconnect();
            }
            //channel.Send(header + message);

            //channel.Send(BuildHTTPResponseHeader(message.Length, "text/html"));

            //// if the user is just joining the web server
            //if (message.StartsWith("GET"))
            //{
            //    string response = "....\n....\n...\n...\n";
            //    foreach (var line in response.Split())
            //    {
            //        channel.Send(line);
            //    }
            //    channel.Send("HTTP / 1.1 200 OK");
            //    channel.Send("Connection: close");
            //    channel.Send("Content-Type: text/html; charset=UTF-8");
            //    channel.Send("");
            //    channel.Send("<html>");
            //    channel.Send("<h1>hi Gage</h1>");
            //    channel.Send("</html>");
            //}

            //channel.Disconnect();

        }

        /// <summary>
        /// Handle some CSS to make our pages beautiful
        /// </summary>
        /// <returns>HTTP Response Header with CSS file contents added</returns>
        private static string SendCSSResponse()
        {
            throw new NotSupportedException("read the css file from the solution folder, build an http response, and return this string");
            //Note: for starters, simply return a static hand written css string from right here (don't do file reading)
        }


        /// <summary>
        ///    (1) Instruct the DB to seed itself (build tables, add data)
        ///    (2) Report to the web browser on the success
        /// </summary>
        /// <returns> the HTTP response header followed by some informative information</returns>
        private static string CreateDBTablesPage()
        {
            throw new NotImplementedException("create the database tables by 'talking' with the DB server and then return an informative web page");
        }

        internal static void OnDisconnect(Networking channel)
        {
            Debug.WriteLine($"Goodbye {channel.RemoteAddressPort}");
        }

    }
}

