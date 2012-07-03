namespace TestData.Profiles
{
    public interface ICompleteMemberData : IMemberData
    {
        object GetValue(object instance, DataConfiguration dataConfiguration);
        IValueCreator ValueCreator { get; }
    }
}