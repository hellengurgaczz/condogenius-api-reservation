using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using System;
using System.IO;

namespace Utils 
{
    public class PubSubUtils 
    {
        public void SendMessageToPubSub(string projectId, string topicId, string message)
        {
            TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);
            string credentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "credentials.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

            PublisherServiceApiClient publisher = PublisherServiceApiClient.Create();
            
            ByteString data = ByteString.CopyFromUtf8(message);

            PubsubMessage pubsubMessage = new PubsubMessage
            {
                Data = data
            };

            Console.WriteLine("Enviando mensagem para pub/sub: " + topicId);
            try {
                var result = publisher.Publish(topicName, new[] { pubsubMessage });
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar mensagem para o Pub/Sub: " + ex.Message);
            }
            
        }
    }
}
