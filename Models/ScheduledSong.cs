
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDR_Worship.Models
{
    public class ScheduledSong
    {
        private DateTime _created;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _updated;

        // Primary Key
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

     
        public string? Description { get; set; }


        // Propiedades para fechas
        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                _created = value.ToUniversalTime();
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value.ToUniversalTime();
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value.ToUniversalTime();
            }
        }


        public DateTime? Updated
        {
            get => _updated;

            set
            {
                if (value.HasValue)
                {
                    _updated = value.Value.ToUniversalTime();
                }
              
            }
        }


        public int? ChordId { get; set; }
        public int? MemberId { get; set; }

        public int? LeadSingerId { get; set; }
        public int? BackingVocalistId { get; set; }
        public int? BackingVocalistTwoId { get; set; }
        public int? LeadGuitaristId { get; set; }
        public int? SecondGuitaristId { get; set; }
        public int? BassistId { get; set; }
        public int? DrummerId { get; set; }




        // Navigation properties
        public virtual Chord? Chord { get; set; }
        public virtual Member? Member { get; set; }

        // Propiedades de navegación para los roles de los miembros
     
        public virtual Member? LeadSinger { get; set; }

       
        public virtual Member? BackingVocalist { get; set; }

      
        public virtual Member? BackingVocalistTwo { get; set; }

        public virtual Member? LeadGuitarist { get; set; }

      
        public virtual Member? SecondGuitarist { get; set; }

 
        public virtual Member? Bassist { get; set; }

    
        public virtual Member? Drummer { get; set; }


        //public virtual ICollection<SongComment> Comments { get; set; } = new HashSet<SongComment>();

        public virtual ICollection<SongDocument> Songs { get; set; } = new HashSet<SongDocument>();

        public virtual ICollection<ChordDocument> ChordDocuments { get; set; } = new HashSet<ChordDocument>();

       
    }

}

