using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using TriviaBotDSharp.Core.Services.AnswersServices.Contracts;

namespace TriviaBotDSharp.Core.Services.AnswersServices
{
    public class AnswersService : IAnswersService
    {
        private Random rng = new Random();
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
        public List<T> Shuffle<T>(List<T> list)
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
        public string FormatTriviaOptionsMultipleChoice(List<string> answers)
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
