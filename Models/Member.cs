using System.ComponentModel.DataAnnotations.Schema;

namespace CDR_Worship.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string? MemberName { get; set; }
        public string? Role { get; set; }



    }
}
