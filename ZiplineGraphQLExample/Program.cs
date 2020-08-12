using System;
using System.IO;
using System.Net;
using System.Text;

namespace Examples.System.Net
{
    public class WebRequestPostExample
    {
        public static void Main()
        {
            // Create a request using a URL that can receive a post.
            WebRequest request = WebRequest.Create("https://zipline.retailzipline.com/api/graphql");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Set the access token in an Authorization Header
            request.Headers.Set("Authorization", "Bearer b50a4e313dd7fd1e28bce649db2053532a9463e5dd850e144c9104352b186316");

            // Build the GraphQL query in to a string object
            string query = @"
            # This is a mutation
            mutation {
                # That creates a communication
                createCommunication(
                    # Set your attribute values here. Double quotes need to be escaped twice
                    # so that they are still escaped in the postData
                    headline: \""Test from C Sharp With Formatting\""
                ) {
                    # Specify the fields you want back here
                    communication { id }
                    errors
                }
            }
            ";

            // Create POST data and convert it to a byte array.
            // The data needs to be in JSON format with a "query" property.
            string postData = $"{{\"query\":\"{query}\"}}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
            }

            // Close the response.
            response.Close();
        }
    }
}
