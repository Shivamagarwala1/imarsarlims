using Razorpay.Api;
namespace iMARSARLIMS
{
    public class RazorpayService
    {
        private readonly string _key;
        private readonly string _secret;

        public RazorpayService(IConfiguration configuration)
        {
            _key = configuration["Razorpay:Key"];
            _secret = configuration["Razorpay:Secret"];
        }

        public RazorpayClient CreateRazorpayClient()
        {
            return new RazorpayClient(_key, _secret);
        }

    }
}
