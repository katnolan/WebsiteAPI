namespace ApiReadRoutes.Models
{
    public class Club
    {
        public int clubid { get; set; }
        public string clubname { get; set; }
        public bool isActive { get; set; }
        public string location { get; set; }
        public int language { get; set; }


        public Club()
        {

        }

        public Club(int id, string name, int cid, int gid, string city, bool act, int lang)
        {

            clubid = id;
            clubname = name;
            isActive = act;
            location = city;
            language = lang;

        }
    }
}
