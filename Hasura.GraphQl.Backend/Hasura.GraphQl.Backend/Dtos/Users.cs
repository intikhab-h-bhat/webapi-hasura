namespace Hasura.GraphQl.Backend.Dtos
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        ICollection<Posts> Posts { get; set; }
    }
}
