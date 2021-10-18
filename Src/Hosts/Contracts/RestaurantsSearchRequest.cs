using System.ComponentModel.DataAnnotations;

namespace Hosts.Contracts
{
    public class RestaurantsSearchRequest
    {
        // TODO: Make validation more stringent
        //
        //Reference: https://stackoverflow.com/questions/23679586/regex-for-uk-postcode
        //[RegularExpression(@"[A-Za-z]{1,2}[0-9Rr][0-9A-Za-z]? [0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}",
        //    ErrorMessage = "OutCode is invalid.")]
        
        [Required]
        public string OutCode { get; set; }
    }
}
