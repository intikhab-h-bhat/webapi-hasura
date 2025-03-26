namespace Hasura.GraphQl.Backend.Dtos
{
    public class Comments
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public int User_Id { get; set; }
        public int Post_Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public Users Users { get; set; }
        public Posts Posts { get; set; }



    }
}
