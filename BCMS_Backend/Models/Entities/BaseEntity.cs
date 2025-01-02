namespace BCMS_Backend.Entities
{
    public  class BaseEntity
    {
        public int Id { get; set; }

        protected bool Equals(BaseEntity other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseEntity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
