using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace FormCard.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;           

            var resposta = activity.CreateReply();
            //resposta.Attachments.Clear();
            var card =  CriaFormulario("Titulo1");
            resposta.Attachments.Add(card);

            

            //Aqui vou analisar o retorno
            var retorno =  VerificaRetorno(activity.Text);

            if (retorno != null)
            {
                resposta.Attachments.Clear();
                resposta.Attachments.Add(retorno);

            }
            await context.PostAsync(resposta);
        }

        Attachment VerificaRetorno(string text)
        {
            if(text.Equals("opcao1", StringComparison.InvariantCultureIgnoreCase))
            {
                return CriaFormulario("retorno");
            }
            if(text.Equals("opcao2", StringComparison.InvariantCultureIgnoreCase))
            {
                return CriaFormulario("retorno");
            }
            else
            {
                return null;
            }
        }

        private Attachment CriaFormulario(string text)
        {
            var heroCard = new HeroCard
            {
                Buttons = new List<CardAction>
            {
                new CardAction
                {
                  Title = text,
                  Type = ActionTypes.PostBack,
                  Value = Opcao.opcao1.ToString()

                },
                new CardAction
                {
                    Title = "Titulo2",
                    Type = ActionTypes.PostBack,
                    Value = Opcao.opcao2.ToString()
                }
            }
            };

            return heroCard.ToAttachment();
        }

        enum Opcao
        {
            opcao1 = 1,
            opcao2
        }
    }
}