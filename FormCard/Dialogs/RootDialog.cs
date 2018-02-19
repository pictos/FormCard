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
            //
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.Contains("Valor X") || activity.Text.Contains("Valor A"))
            {
                await context.PostAsync("Fim do formulário <br/> Digite algo se quiser reiniciar ");
                
                return;
            }

            var resposta = activity.CreateReply();
            

            string[] opcoes = { "opcao1", "opcao2", "opcao3" };
            var card = CriarFormButton(opcoes);
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
                string[] teste = { "Valor X", "Valor Y", "Valor z" };
                return CriarFormButton( teste);
            }
            if(text.Equals("opcao2", StringComparison.InvariantCultureIgnoreCase))
            {
                string[] teste = { "Valor A", "Valor B", "Valor C" };
                return CriarFormButton(teste);
            }
            else
            {
                return null;
            }
        }


        private Attachment CriarFormButton(string [] text)
        {
            var Buttons = new List<CardAction>();
            foreach (var item in text)
            {
                Buttons.Add(new CardAction
                {
                    Title = item,
                    Type = ActionTypes.PostBack,
                    Value = item
                });
            }

            var heroCard = new HeroCard
            {
                Buttons = Buttons
            };

            return heroCard.ToAttachment();
        }


        #region Obsoleto
        //private Attachment CriaFormulario(string text)
        //{
        //    var heroCard = new HeroCard
        //    {
        //        Buttons = new List<CardAction>
        //    {
        //        new CardAction
        //        {
        //          Title = text,
        //          Type = ActionTypes.PostBack,
        //          Value =Opcao.opcao1.ToString()

        //        },
        //        new CardAction
        //        {
        //            Title = "Titulo2",
        //            Type = ActionTypes.PostBack,
        //            Value = Opcao.opcao2.ToString()
        //        }
        //    }
        //    };

        //    return heroCard.ToAttachment();
        //}

        //enum Opcao
        //{
        //    opcao1 = 1,
        //    opcao2
        //}
        #endregion
    }
}