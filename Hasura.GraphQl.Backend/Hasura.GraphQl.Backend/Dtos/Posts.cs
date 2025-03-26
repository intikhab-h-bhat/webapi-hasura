namespace Hasura.GraphQl.Backend.Dtos
{
    public class Posts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int User_Id { get; set; }

        public Users Users { get; set; }

        ICollection<Comments> Comments { get; set; }

    }
}
