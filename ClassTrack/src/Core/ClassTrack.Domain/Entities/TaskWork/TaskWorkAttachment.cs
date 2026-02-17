using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Entities
{
    public class TaskWorkAttachment:BaseAccountableEntity
    {
        public string FileUrl { get; set; }
        public string PublicId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        //relations

        public long TaskWorkId { get; set; }
        public TaskWork TaskWork { get; set; }
    }
}
