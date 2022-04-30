using System;
using System.Collections.Generic;
using System.Text;

namespace TriviaBotDSharp.Core.Services.AnswersServices.Contracts
{
    public  interface IAnswersService
    {
        List<string> ProcessAnswers(List<string> incorrectAnnswers, string correctAnswer);
        List<T> Shuffle<T>(List<T> list);
        string FormatTriviaOptionsMultipleChoice(List<string> answers);
    }
}
