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
using TriviaBotDSharp.Core.Services.AnswersServices.Contracts;
using TriviaBotDSharp.Core.Services.APIServices.Contracts;
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
        private readonly IAnswersService _answersService;
        private readonly IAPIService _apiService;
        public Commands(IProfileService profileService, IAnswersService answersServie, IAPIService apiService)
        {
            _profileService = profileService;
            _answersService = answersServie;
            _apiService = apiService;
        }
                
        [Command("trivia")]

        public async Task Trivia(CommandContext ctx)
        {
            
            var triviaQuestion = await _apiService.GetDataAsync<TriviaAPIModel>(@"https://opentdb.com/api.php?amount=10&type=multiple");
            List<string> answers = _answersService.ProcessAnswers(triviaQuestion.Results[0].IncorrectAnswers, triviaQuestion.Results[0].CorrectAnswer);

            var question = HttpUtility.HtmlDecode(triviaQuestion.Results[0].Question);


            var correctStep = new TextStep("-", $":white_check_mark: Congratulations! \"{HttpUtility.HtmlDecode(triviaQuestion.Results[0].CorrectAnswer)}\" was the correct answer.", null);
            var incorrectStep = new TextStep("-", $":x: Incorrect! \"{HttpUtility.HtmlDecode(triviaQuestion.Results[0].CorrectAnswer)}\" was the correct answer.", null);

            

            var triviaStep = new TriviaStep(
            question,
            _answersService.FormatTriviaOptionsMultipleChoice(answers),
            triviaQuestion.Results[0].Category, 
            triviaQuestion.Results[0].Difficulty,
            new Dictionary<DiscordEmoji, TriviaStepData>
            {
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_a:"), new TriviaStepData{Content = "a", NextStep = answers[0] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[0] == triviaQuestion.Results[0].CorrectAnswer} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_b:"), new TriviaStepData{Content = "b", NextStep = answers[1] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[1] == triviaQuestion.Results[0].CorrectAnswer} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_c:"), new TriviaStepData{Content = "c", NextStep = answers[2] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[2] == triviaQuestion.Results[0].CorrectAnswer} },
                { DiscordEmoji.FromName(ctx.Client, ":regional_indicator_d:"), new TriviaStepData{Content = "d", NextStep = answers[3] == triviaQuestion.Results[0].CorrectAnswer ? correctStep : incorrectStep, AnsweredCorrectly = answers[3] == triviaQuestion.Results[0].CorrectAnswer} },
            }, 
            _profileService);

            var userChannel = ctx.Channel;
            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, triviaStep);
            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeeded) return;

        }
        
    }
}
