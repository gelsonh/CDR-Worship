using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDR_Worship.Models
{
    public class CommentReply
    {

        public int Id { get; set; }
        public int ParentCommentId { get; set; }
        public string? ReplyText { get; set; }
        public int ScheduledSongId { get; set; }
    }
}