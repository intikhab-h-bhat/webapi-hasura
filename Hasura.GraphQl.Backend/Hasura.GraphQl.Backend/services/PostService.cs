using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Hasura.GraphQl.Backend.Dtos;



namespace Hasura.GraphQl.Backend.services
{
    public class PostService
    {
        private readonly GraphQLHttpClient _client;

        public PostService(IConfiguration config)
        {
            var graphqlUrl = config["Hasura:GraphQLUrl"];
            var adminSecret = config["Hasura:AdminSecret"];

            _client = new GraphQLHttpClient(graphqlUrl, new NewtonsoftJsonSerializer());

            // Add Admin Secret for authentication
            _client.HttpClient.DefaultRequestHeaders.Add("x-hasura-admin-secret", adminSecret);
        }


        public async Task<List<Posts>> GetAllPostsAsync()
        {
            var query = new GraphQLRequest
            {
                Query = @"
               query {
                 posts {
                    id
                    title
                    content
                     user_id
                     user {
                        id
                        name
                        gender
                         age
                            }
                           }
                    }",

            };

            var response = await _client.SendQueryAsync<UserPostResponse>(query);

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

            return response.Data.Posts;
        }

        // Add a Post
        public async Task<AddPost> AddPostAsync(string title,string content, int userId)
        {
            var mutation = new GraphQLRequest
            {
                Query = @"
                        mutation ($title: String!,$content:String!, $userId: Int!)
                        {
                        insert_posts(objects: {title: $title,content:$content, user_id: $userId})
                            {
                            returning { 
                                    id
                                    title 
                                    }
                        }
                    }",
                Variables = new { title,content ,userId }
            };

            var response = await _client.SendMutationAsync<UserPostResponse>(mutation);
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


            return response.Data.insert_posts.returning.FirstOrDefault();
        }


        public async Task<string> DeletePostAsync(int id)
        {
            var mutation = new GraphQLRequest()
            {
                Query= @"
                mutation ($id: Int!) {
                delete_posts_by_pk(id: $id) {
                                        title
                                    }
                               }"
                ,Variables= new {id}
            };

            var response = await _client.SendMutationAsync<UserPostResponse>(mutation);
          
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


            return response.Data?.delete_posts_by_pk?.title ?? "Post Deleted";

        }


        private class UserPostResponse
        {
            public List<Posts> Posts { get; set; }
            public InsertPostsResult insert_posts { get; set; }
            public Posts posts_by_pk { get; set; }
            public PostDelete delete_posts_by_pk { get; set; }

        }

        public class PostDelete
        {
            public string title { get; set; }
        }
        public class InsertPostsResult
        {
            public AddPost[] returning { get; set; }
        }
    }
}
