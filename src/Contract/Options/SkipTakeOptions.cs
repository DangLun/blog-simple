namespace Contract.Options
{
    public class SkipTakeOptions
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public SkipTakeOptions()
        {
        }

        public SkipTakeOptions(int? skip, int? take)
        {
            Skip = skip < 0 ? null : skip;
            Take = take < 0 ? null : take;
        }
    }
}
