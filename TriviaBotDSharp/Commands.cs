using DiscordBotTutorial.Bots.Handlers.Dialogue.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TriviaBotDSharp.Handlers;
using TriviaController;

namespace TriviaBotDSharp
{
    public class Commands : BaseCommandModule
    {

        [Command("trivia")]
        public async Task Trivia(CommandContext ctx)
        {
            var apiClient = new APIClient(@"https://opentdb.com/api.php?amount=10&type=multiple");
            var triviaQuestion = apiClient.GetDataAsync<TriviaAPIModel>();
            var answers = new List<string>(triviaQuestion.Results[0].IncorrectAnswers);

            answers.Add(triviaQuestion.Results[0].CorrectAnswer);

            CommandsUtillityMethods.Shuffle(answers);
            StringBuilder sb = new StringBuilder();
            string[] aToD = new string[4] {"A)", "B)", "C)", "D)"};
            for (int i = 0; i < 4; i++)
            {
                sb.AppendLine(aToD[i] + " " + answers[i]);
            }
            
            var correctStep = new TextStep("Correct!", ":D", null);
            var incorrectStep = new TextStep("Incorrect!", ":(", null);
            var decodedString = HttpUtility.HtmlDecode(triviaQuestion.Results[0].Question);

            var triviaStep = new TriviaStep(decodedString, sb.ToString(), new Dictionary<DiscordEmoji, TriviaStepData>
            {
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_a:"), new TriviaStepData{Content = "a", NextStep = answers[0] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_b:"), new TriviaStepData{Content = "b", NextStep = answers[1] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_c:"), new TriviaStepData{Content = "c", NextStep = answers[2] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_d:"), new TriviaStepData{Content = "d", NextStep = answers[3] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep} },
            });
            //var triviaStep = new TriviaStep("a", "B", new Dictionary<DiscordEmoji, TriviaStepData>
            //{
            //    { DiscordEmoji.FromName(ctx.Client, ":a:"), new TriviaStepData{Content = "a", NextStep = correctStep} },
            //    { DiscordEmoji.FromName(ctx.Client, ":b:"), new TriviaStepData{Content = "b", NextStep = null} }
            //});
            //var userDmChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);
            var userChannel = ctx.Channel;
            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, triviaStep);
            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeeded) return;

        }
    }
}
