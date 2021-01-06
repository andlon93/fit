using Microsoft.AspNetCore.Mvc;
using GraphQL.SystemTextJson;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using GraphQL.Types;
using GraphQL;
using System;
using GraphQL.DataLoader;
using GraphQL.Validation;
using FitnessTracker.Authentication;

namespace FitnessTracker.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;
        private readonly ISchema _schema;
        private readonly DataLoaderDocumentListener _listener;
        public GraphQLController(
            FitnessTrackerSchema schema, 
            IDocumentExecuter executer, 
            IDocumentWriter writer,
            DataLoaderDocumentListener listener)
        {
            _schema = schema;
            _executer = executer;
            _writer = writer;
            _listener = listener;
        }

        [HttpPost]
        public async Task PostAsync()
        {
            var request = await JsonSerializer.DeserializeAsync<GraphQLRequest>
            (
                HttpContext.Request.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (request == null)
            {
                throw new ArgumentException("No request provided");
            }

            var result = await _executer.ExecuteAsync(options =>
            {
                options.Schema = _schema;
                options.Query = request.Query;
                options.OperationName = request.OperationName;
                options.Inputs = request.Variables.ToInputs();
                options.Listeners.Add(_listener);
                options.ValidationRules = DocumentValidator.CoreRules;

                if (HttpContext.Items.TryGetValue(AuthorizationConstants.GoogleIdContextTitle, out var googleId) && googleId != null)
                {
                    options.UserContext.Add(AuthorizationConstants.GoogleIdContextTitle, googleId);
                }
                if (HttpContext.Items.TryGetValue(AuthorizationConstants.AuthorIdContextTitle, out var authorId) && authorId != null)
                {
                    options.UserContext.Add(AuthorizationConstants.AuthorIdContextTitle, authorId);
                }
            });

            HttpContext.Response.ContentType = "application/json";
            HttpContext.Response.StatusCode = 200; // OK

            await _writer.WriteAsync(HttpContext.Response.Body, result);
        }
    }

    public class GraphQLRequest
    {
        public string? OperationName { get; set; }
        public string? Query { get; set; }
        [JsonConverter(typeof(ObjectDictionaryConverter))]
        public Dictionary<string, object>? Variables { get; set; }
    }
}