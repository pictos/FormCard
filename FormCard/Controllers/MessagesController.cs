using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FormCard.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System;

namespace FormCard
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    await Conversation.SendAsync(activity, () => new RootDialog());
                    break;
                case ActivityTypes.ConversationUpdate:
                    if(activity.MembersAdded.Any(c=>c.Id == activity.Recipient.Id))
                    {
                        var resposta = activity.CreateReply();
                        resposta.Text = "Digite algo para iniciar";
                        await connector.Conversations.ReplyToActivityAsync(resposta);                        
                    }
                    break;                
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

       
    }
}