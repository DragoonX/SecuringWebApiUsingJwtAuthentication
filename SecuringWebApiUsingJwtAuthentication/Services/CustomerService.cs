using SecuringWebApiUsingJwtAuthentication.Entities;
using SecuringWebApiUsingJwtAuthentication.Helpers;
using SecuringWebApiUsingJwtAuthentication.Interfaces;
using SecuringWebApiUsingJwtAuthentication.Requests;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringWebApiUsingJwtAuthentication.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomersDbContext _customersDbContext;

        public CustomerService(CustomersDbContext customersDbContext)
        {
            _customersDbContext = customersDbContext;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var customer = _customersDbContext.Customers.SingleOrDefault(customer => customer.Active && customer.Username == loginRequest.Username);
            if (customer == null)
            {
                return null;
            }

            var passwordHash = HashingHelper.HashUsingPbkdf2(loginRequest.Password, customer.PasswordSalt);
            if (customer.Password != passwordHash)
            {
                return null;
            }

            var token = await Task.Run(() => TokenHelper.GenerateToken(customer));

            return new LoginResponse { Username = customer.Username, FirstName = customer.FirstName, LastName = customer.LastName, Token = token };
        }
    }
}
