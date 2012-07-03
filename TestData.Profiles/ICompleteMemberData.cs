namespace TestData.Profiles
{
    public interface ICompleteMemberData : IMemberData
    {
        object GetValue(object instance);
    }

    public interface ICompleteMemberData<TType, TProperty> : ICompleteMemberData
    {
        //TProperty GetValue(TType data);
    }
}