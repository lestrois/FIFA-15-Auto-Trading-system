using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_JOB.Models
{
    public class Player
    {
        public int Id { get; set; }
        public long resourceID { get; set; }
        public string Name { get; set; }
        public string PreferredPosition { get; set; }
        public byte Rating { get; set; }
        public byte RareFlag { get; set; }
        public int PAC { get; set; }
        public int SHO { get; set; }
        public int PAS { get; set; }
        public int DRI { get; set; }
        public int DEF { get; set; }
        public int PHY { get; set; }
        public string FaceImgUrl { get; set; }
    }
}
