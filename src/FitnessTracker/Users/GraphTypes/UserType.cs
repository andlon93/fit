using FitnessTracker.Users.DTOs;
using GraphQL.Types;
using System;

namespace FitnessTracker.Users.GraphTypes
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Field<GuidGraphType, Guid>().Name(nameof(User.Id)).Description("Id of the user");
            Field<StringGraphType, string?>().Name(nameof(User.Email));
            Field<StringGraphType, string?>().Name(nameof(User.FirstName));
            Field<StringGraphType, string?>().Name(nameof(User.LastName));
            Field<StringGraphType, string?>().Name(nameof(User.Country));
            Field<IntGraphType, int?>().Name(nameof(User.Height));
        }
    }
}
