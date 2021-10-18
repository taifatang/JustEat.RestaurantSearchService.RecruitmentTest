using System;

namespace RestaurantSearchService.Domain.Exceptions
{
    public class UnexpectedResponseException : Exception
    {
        public UnexpectedResponseException(string service) : base($"Unexpected response from {service}")
        {

        }
    }
}
