using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Hasura.GraphQl.Backend.Dtos;



namespace Hasura.GraphQl.Backend.services
{
    public class HasuraService
    {
        private readonly GraphQLHttpClient _client;

        public HasuraService(IConfiguration config)
        {
            var graphqlUrl = config["Hasura:GraphQLUrl"];
            var adminSecret = config["Hasura:AdminSecret"];

            _client = new GraphQLHttpClient(graphqlUrl, new NewtonsoftJsonSerializer());

            // Add Admin Secret for authentication
            _client.HttpClient.DefaultRequestHeaders.Add("x-hasura-admin-secret", adminSecret);
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            var query = new GraphQLRequest
            {
                Query = @"
            query ($id: Int!) {
              users_by_pk(id: $id) {
                id
                name
                age
              }
            }",
                Variables = new { id }
            };

            var response = await _client.SendQueryAsync<UserResponse>(query);
            if (response.Errors != null && response.Errors.Any())
            {
                Console.WriteLine("GraphQL Error: " + response.Errors[0].Message);
                return null;
            }

            if (response.Data == null)
            {
                Console.WriteLine("Response Data is null");
                return null;
            }
            return response.Data.users_by_pk;
        }


        public async Task<List<Users>> GetAllUsersAsync()
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query{
                 users{
                    id
                    name
                    gender
                     age
                   }
                }",
               
            };

            var response = await _client.SendQueryAsync<UserResponse>(query);

            if (response.Errors != null && response.Errors.Any())
            {
                Console.WriteLine("GraphQL Error: " + response.Errors[0].Message);
                return null;
            }

            if (response.Data == null)
            {
                Console.WriteLine("Response Data is null");
                return null;
            }

            return response.Data.Users;
        }

        private class UserResponse
        {
            public List<Users> Users { get; set; }
            public Users users_by_pk { get; set; }

        }
       
    }
}
