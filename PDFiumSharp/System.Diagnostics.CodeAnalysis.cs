namespace System.Diagnostics.CodeAnalysis
{
    //
    // Summary:
    //     Specifies that when a method returns System.Diagnostics.CodeAnalysis.NotNullWhenAttribute.ReturnValue,
    //     the parameter will not be null even if the corresponding type allows it.
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    sealed class NotNullWhenAttribute : Attribute
    {
        //
        // Summary:
        //     Initializes the attribute with the specified return value condition.
        //
        // Parameters:
        //   returnValue:
        //     The return value condition. If the method returns this value, the associated
        //     parameter will not be null.
        public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

        //
        // Summary:
        //     Gets the return value condition.
        //
        // Returns:
        //     The return value condition. If the method returns this value, the associated
        //     parameter will not be null.
        public bool ReturnValue { get; }
    }

    //
    // Summary:
    //     Specifies that when a method returns System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute.ReturnValue,
    //     the parameter may be null even if the corresponding type disallows it.
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    sealed class MaybeNullWhenAttribute : Attribute
    {
        //
        // Summary:
        //     Initializes the attribute with the specified return value condition.
        //
        // Parameters:
        //   returnValue:
        //     The return value condition. If the method returns this value, the associated
        //     parameter may be null.
        public MaybeNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

        //
        // Summary:
        //     Gets the return value condition.
        //
        // Returns:
        //     The return value condition. If the method returns this value, the associated
        //     parameter may be null.
        public bool ReturnValue { get; }
    }

    //
    // Summary:
    //     Specifies that the method or property will ensure that the listed field and property
    //     members have values that aren't null.
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class MemberNotNullAttribute : Attribute
    {
        //
        // Summary:
        //     Initializes the attribute with a field or property member.
        //
        // Parameters:
        //   member:
        //     The field or property member that is promised to be non-null.
        public MemberNotNullAttribute(string member) : this(new[] { member }) { }
        //
        // Summary:
        //     Initializes the attribute with the list of field and property members.
        //
        // Parameters:
        //   members:
        //     The list of field and property members that are promised to be non-null.
        public MemberNotNullAttribute(params string[] members) => Members = members;

        //
        // Summary:
        //     Gets field or property member names.
        public string[] Members { get; }
    }

    //
    // Summary:
    //     Specifies that the method or property will ensure that the listed field and property
    //     members have non-null values when returning with the specified return value condition.
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class MemberNotNullWhenAttribute : Attribute
    {
        //
        // Summary:
        //     Initializes the attribute with the specified return value condition and a field
        //     or property member.
        //
        // Parameters:
        //   returnValue:
        //     The return value condition. If the method returns this value, the associated
        //     parameter will not be null.
        //
        //   member:
        //     The field or property member that is promised to be non-null.
        public MemberNotNullWhenAttribute(bool returnValue, string member)
            : this(returnValue, new[] { member }) { }
        //
        // Summary:
        //     Initializes the attribute with the specified return value condition and list
        //     of field and property members.
        //
        // Parameters:
        //   returnValue:
        //     The return value condition. If the method returns this value, the associated
        //     parameter will not be null.
        //
        //   members:
        //     The list of field and property members that are promised to be non-null.
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
        {
            ReturnValue = returnValue;
            Members = members;
        }

        //
        // Summary:
        //     Gets field or property member names.
        public string[] Members { get; }
        //
        // Summary:
        //     Gets the return value condition.
        public bool ReturnValue { get; }
    }
}