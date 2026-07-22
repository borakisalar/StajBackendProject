using System.Text.Json.Serialization;

namespace StajBackendProject.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Users> Users { get; set; }
    }
}
