namespace Booker
{
    public class EntityBase
    {
        public EntityBase()
        {
            Enabled = true;
            Modified = DateTime.UtcNow;
        }

        public bool Enabled { get; set; }
        public DateTime? Modified { get; set; }
    }
}