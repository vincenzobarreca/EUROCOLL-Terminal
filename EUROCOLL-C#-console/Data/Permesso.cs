namespace EUROCOLL.Data
{
    public class Permesso
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public override string ToString()
        {
            return Nome;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (!(obj is Permesso)) return false;
            Permesso other = (Permesso)obj;
            return Id.Equals(other.Id);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
