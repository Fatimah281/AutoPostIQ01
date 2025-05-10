using AutoPostIQ.Services.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;


namespace AutoPostIQ.Services.Implementation
{
	public class ContentGeneration : IContentGeneration
	{
        private readonly OpenAIService _openAIService;
        private readonly IConfiguration _configuration;

        public ContentGeneration(IConfiguration configuration)
        {
            _configuration = configuration;
            _openAIService = new OpenAIService(new OpenAI.GPT3.OpenAIOptions()
            {
                ApiKey = _configuration["OpenAIServiceOptions:ApiKey"]
            });
        }

        public async Task<string> GenerateContent(string topic)
        {
            var completionResult = await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest()
            { Prompt = $"Write a short and clear social media post about: {topic}",
                Model = Models.TextDavinciV3, MaxTokens = 100 });
            return completionResult.Choices[0].Text.Trim();
        }
	}
}
