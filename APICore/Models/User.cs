
namespace APICore.Models
{
    public class User: Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
    }
}