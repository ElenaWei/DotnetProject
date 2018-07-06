using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDotnetProject.Models
{
    [Table("Models")]
    public class Model
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        //navigation property
        public Make Make{ get; set; }
        //foreign key mapping
        public int MakeId { get; set; }
    }
}