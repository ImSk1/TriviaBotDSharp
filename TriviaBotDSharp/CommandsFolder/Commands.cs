using DiscordBotTutorial.Bots.Handlers.Dialogue.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.DAL;
using TriviaBotDSharp.DAL.Models;
using TriviaBotDSharp.Handlers;
using TriviaController;

namespace TriviaBotDSharp
{

    public class Commands : BaseCommandModule
    {

        private static Random rng = new Random();        
        private readonly IProfileService _profileService;
        public Commands(IProfileService profileService)
        {
            _profileService = profileService;
        }
                
        [Command("trivia")]

        public async Task Trivia(CommandContext ctx)
        {                        
            var apiClient = new APIClient(@"https://opentdb.com/api.php?amount=10&type=multiple");
            var triviaQuestion = apiClient.GetDataAsync<TriviaAPIModel>();
            var answers = ProcessAnswers(triviaQuestion.Results[0].IncorrectAnswers, triviaQuestion.Results[0].CorrectAnswer);
            var question = HttpUtility.HtmlDecode(triviaQuestion.Results[0].Question);


            var correctStep = new TextStep("-", $":white_check_mark: Congratulations! \"{triviaQuestion.Results[0].CorrectAnswer}\" was the correct answer.", null);
            var incorrectStep = new TextStep("-", $":x: Incorrect! \"{triviaQuestion.Results[0].CorrectAnswer}\" was the correct answer.", null);

            

            var triviaStep = new TriviaStep(question,
                FormatTriviaOptionsMultipleChoice(answers),
                triviaQuestion.Results[0].Category, triviaQuestion.Results[0].Difficulty,
            new Dictionary<DiscordEmoji, TriviaStepData>
            {
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_a:"), new TriviaStepData{Content = "a", NextStep = answers[0] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[0] == triviaQuestion.Results[0].CorrectAnswer} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_b:"), new TriviaStepData{Content = "b", NextStep = answers[1] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[1] == triviaQuestion.Results[0].CorrectAnswer} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_c:"), new TriviaStepData{Content = "c", NextStep = answers[2] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[2] == triviaQuestion.Results[0].CorrectAnswer} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_d:"), new TriviaStepData{Content = "d", NextStep = answers[3] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[3] == triviaQuestion.Results[0].CorrectAnswer} },
            }, _profileService);

            var userChannel = ctx.Channel;
            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, triviaStep);
            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeeded) return;

        }
        public List<string> ProcessAnswers(List<string> incorrectAnnswers, string correctAnswer)
        {
            StringBuilder sb = new StringBuilder();
            incorrectAnnswers.Add(correctAnswer);
            incorrectAnnswers = Shuffle<string>(incorrectAnnswers);            
            for (int i = 0; i < 4; i++)
            {
                HttpUtility.HtmlDecode(incorrectAnnswers[i]);                
            }            
            
            return incorrectAnnswers;
        }
        public static List<T> Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        public static string FormatTriviaOptionsMultipleChoice(List<string> answers)
        {
            StringBuilder sb = new StringBuilder();
            string[] aToD = new string[4] { "A)", "B)", "C)", "D)" };
            for (int i = 0; i < 4; i++)
            {
                sb.AppendLine(aToD[i] + " " + answers[i]);
            }
            return sb.ToString();
        }
    }
}
