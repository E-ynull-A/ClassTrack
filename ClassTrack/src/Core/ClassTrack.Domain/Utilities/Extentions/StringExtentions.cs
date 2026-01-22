namespace ClassTrack.Domain.Utilities
{
    public static class StringExtentions
    {
        public static ICollection<E> TrimAll<E>(this ICollection<E> items) where E : class, new()
        {
            ICollection<E> trimeds = [];
            foreach (E item in items)
            {
                trimeds.Add(item);
            }
            return trimeds;
        }
    }
}
