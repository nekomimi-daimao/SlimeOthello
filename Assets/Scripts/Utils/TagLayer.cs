namespace Utils
{
    public static class TagLayer
    {
        public enum Tag
        {
            NavMeshTarget,
        }

        public static string Name(this Tag tag)
        {
            return tag.ToString();
        }
    }
}
